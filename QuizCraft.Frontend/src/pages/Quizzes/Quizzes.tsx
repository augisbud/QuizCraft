import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Box
} from "@mui/material";
import { Navbar } from "../../components/Navbar/Navbar";
import Paper from "@mui/material/Paper";
import { Link } from "react-router-dom";
import { QuizNavbar } from "../../components/QuizNavbar/QuizNavbar";
import { client } from "../../utils/Client";
import { useEffect, useState } from "react";
import { QuizDto } from "../../utils/QuizCraftAPIClient";
import { Analytics, DeleteOutline, PlayCircleOutline } from "@mui/icons-material";
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

  return (
    <>
      <Navbar />
      <QuizNavbar />
      <IndividualAnalytics />
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          padding: "10px"
        }}
      >
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
                <TableRow
                  key={row.id}
                  sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                >
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
                    { row.isOwner &&
                       <span onClick={() => {
                          setQuizzes(quizzes.filter(quiz => quiz.id !== row.id))
                          
                          client.quizzesDELETE(row.id)
                       }}>
                          <DeleteOutline fontSize="large" />
                       </span>
                    }
                   
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
