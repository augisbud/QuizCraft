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
import { Analytics, PlayCircleOutline } from "@mui/icons-material";
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
                  <TableCell align="right">{row.questionCount}</TableCell>
                  <TableCell align="center">
                    <Link to={`/quizzes/${row.id}`} style={{ textDecoration: "none" }}>
                      <PlayCircleOutline fontSize="large" />
                    </Link>
                    <span onClick={() => individualAnalyticsDialogVar.set(row.id)}>
                      <Analytics fontSize="large" />
                    </span>
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
