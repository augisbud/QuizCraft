import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Box, IconButton } from "@mui/material";
import { Navbar } from "../../components/Navbar/Navbar";
import Paper from "@mui/material/Paper";
import { Link } from "react-router-dom";
import { QuizNavbar } from "../../components/QuizNavbar/QuizNavbar";
import { client } from "../../utils/Client";
import { useEffect, useState } from "react";
import { QuizDto } from "../../utils/QuizCraftAPIClient";
import { Analytics, DeleteOutline, PlayCircleOutline, Download } from "@mui/icons-material";
import { individualAnalyticsDialogVar } from "../../utils/Cache";
import { IndividualAnalytics } from "../../components/IndividualAnalytics/IndividualAnalytics";

export const Quizzes = () => {
    const [quizzes, setQuizzes] = useState<QuizDto[]>([]);

    useEffect(() => {
        const fetchQuizzes = async () => {
            const quizzesData = await client.quizzesAll();
            setQuizzes(quizzesData);
        };

        fetchQuizzes();
    }, []);


    //TODO: make the file name the name of the quiz. It is passed in the controller through Content Disposition
    const handleDownload = async (quizId: string) => {
        try {
            const fileResponse = await client.exportPdf(quizId);

            const contentDisposition = fileResponse.headers?.['content-disposition'] as string;
            let filename = `quiz_${quizId}.pdf`;

            if (contentDisposition) {
                const matches = /filename\*?=(?:UTF-8'')?([^;]+)/.exec(contentDisposition);
                if (matches && matches[1]) {
                    filename = decodeURIComponent(matches[1]);
                }
            }

            const fileBlob = fileResponse.data;
            const fileURL = URL.createObjectURL(fileBlob);

            const link = document.createElement("a");
            link.href = fileURL;
            link.download = filename;
            link.click();

            URL.revokeObjectURL(fileURL);
        } catch (error) {
            console.error("Error downloading quiz:", error);
        }
    };

    return (
        <>
            <Navbar />
            <QuizNavbar />
            <IndividualAnalytics />
            <Box sx={{ display: "flex", justifyContent: "center", padding: "10px" }}>
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>No.</TableCell>
                                <TableCell>Quiz name</TableCell>
                                <TableCell>Category</TableCell>
                                <TableCell>Questions</TableCell>
                                <TableCell>Completed By</TableCell>
                                <TableCell>Average Score</TableCell>
                                <TableCell>High Score</TableCell>
                                <TableCell align="center">Actions</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {quizzes.map((row, index) => (
                                <TableRow key={row.id} sx={{ "&:last-child td, &:last-child th": { border: 0 } }}>
                                    <TableCell component="th" scope="row" align="right">
                                        {index + 1}
                                    </TableCell>
                                    <TableCell>{row.title}</TableCell>
                                    <TableCell>{row.category}</TableCell>
                                    <TableCell>{row.questionCount}</TableCell>
                                    <TableCell>{row.completedBy}</TableCell>
                                    <TableCell>{row.averageScore ?? 0}/{row.questionCount}</TableCell>
                                    <TableCell>{row.highScore ?? 0}/{row.questionCount}</TableCell>
                                    <TableCell align="center">
                                        <Link to={`/quizzes/${row.id}`} style={{ textDecoration: "none" }}>
                                            <PlayCircleOutline fontSize="large" />
                                        </Link>
                                        <span onClick={() => individualAnalyticsDialogVar.set(row.id)}>
                                            <Analytics fontSize="large" />
                                        </span>
                                        {row.isOwner && (
                                            <span
                                                onClick={() => {
                                                    setQuizzes(quizzes.filter((quiz) => quiz.id !== row.id));
                                                    client.quizzesDELETE(row.id);
                                                }}
                                            >
                                                <DeleteOutline fontSize="large" />
                                            </span>
                                        )}
                                        <IconButton onClick={() => handleDownload(row.id)}>
                                            <Download fontSize="large" />
                                        </IconButton>
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>
        </>
    );
};
