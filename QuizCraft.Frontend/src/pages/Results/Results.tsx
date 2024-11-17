import { useLocation, useNavigate } from "react-router-dom";
import { Button, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import { Navbar } from "../../components/Navbar/Navbar";
import styles from "./Results.module.scss";

export const Results = () => {
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    if(!sessionStorage.getItem("token"))
      navigate("/signin?redirect=/results");  
  }, [navigate])

  const { correctAnswers, totalQuestions } = location.state;
  const [comment, setComment] = useState<string>("");

  const handleCommentChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setComment(e.target.value);
  };

  const handleSubmitComment = () => {
    console.log("Comment submitted: ", comment);
    // You can save the comment to a backend or local storage here
  };

  return (
    <>
      <Navbar />
      <div className={styles.resultsContainer}>
        <div className={styles.resultsSection}>
          <h1>Quiz Results</h1>
          <p className={styles.resultsText}>
            You answered {correctAnswers} out of {totalQuestions} questions
            correctly!
          </p>

          <div className={styles.resultsButtons}>
            <Button variant="contained" onClick={() => navigate("/quizzes")}>
              Go to Quizzes
            </Button>
            <Button
              variant="contained"
              onClick={() => navigate("/")}
              style={{ marginLeft: "10px" }}
            >
              Go to Home
            </Button>
          </div>
        </div>

        <div className={styles.commentsSection}>
          <h3>Leave a comment about the quiz:</h3>
          <TextField
            label="Comment"
            multiline
            rows={4}
            value={comment}
            onChange={handleCommentChange}
            variant="outlined"
            style={{ width: "100%" }}
          />
          <Button
            variant="contained"
            onClick={handleSubmitComment}
            style={{ marginTop: "10px" }}
          >
            Submit Comment
          </Button>
        </div>
      </div>
    </>
  );
};
