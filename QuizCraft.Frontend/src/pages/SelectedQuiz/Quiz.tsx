import { useEffect, useState } from "react";
import {
  Radio,
  RadioGroup,
  FormControlLabel,
  FormControl,
  FormLabel,
  Button,
} from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { Navbar } from "../../components/Navbar/Navbar";
import styles from "./Quiz.module.scss";
import { AnswerAttemptDto, AnswerDto, DetailedQuizDto } from "../../utils/QuizCraftAPIClient";
import { client } from "../../utils/Client";

export const Quiz = () => {
  const { quizId } = useParams<{ quizId: string }>();

  const navigate = useNavigate();

  useEffect(() => {
    if (quizId === undefined)
      navigate("/quizzes");

    if (!sessionStorage.getItem("token"))
      navigate(`/signin?redirect=/quizzes/${quizId}`);
  }, [navigate, quizId])

  const [loading, setLoading] = useState<boolean>(true);
  const [quiz, setQuiz] = useState<DetailedQuizDto>();
  const [selectedAnswers, setSelectedAnswers] = useState<{ [key: string]: string }>({});
  const [isAnswerCorrect, setIsAnswerCorrect] = useState<boolean | null>(null);
  const [correctAnswer, setCorrectAnswer] = useState<AnswerDto | null>(null);

  useEffect(() => {
    const fetchDetailedData = async () => {
      const detailedData = await client.questionsGET(quizId!);

      setQuiz(detailedData);
      setLoading(false);
    };

    fetchDetailedData();
  }, [quizId]);

  if (loading) return <div>Loading...</div>;
  if(quiz!.currentQuestionId === "00000000-0000-0000-0000-000000000000") return <div>Question not found</div>;

  const currentQuestion = quiz!.questions.find((value) => value.id === quiz!.currentQuestionId)!;
  const currentQuestionIndex = quiz!.questions.findIndex((value) => value.id === quiz!.currentQuestionId) + 1;
  
  const handleSubmitValidation = async () => {
    const body: AnswerAttemptDto = {
      answerId: selectedAnswers[currentQuestion.id!],
      init: () => { },
      toJSON: () => ({ answerId: selectedAnswers[currentQuestion.id!] })
    };

    try {
      const response = await client.questionsPOST(quizId!, currentQuestion.id, body);

      setIsAnswerCorrect(response.selectedAnswer.answerId === response.correctAnswer.id);
      setCorrectAnswer(response.correctAnswer);
    } catch (error) {
      console.error("Error validating answer:", error);
    }
  };

  const handleNextQuestion = () => {
    setIsAnswerCorrect(null);
    setCorrectAnswer(null);

    const currentIndex = quiz!.questions.findIndex((question) => question.id === currentQuestion.id);
    const nextIndex = currentIndex + 1;
    if (nextIndex < quiz!.questions.length) {
      const nextQuestion = quiz!.questions[nextIndex];

      setQuiz((prevQuiz) => ({
        ...prevQuiz!,
        currentQuestionId: nextQuestion.id,
        init: prevQuiz!.init,
        toJSON: prevQuiz!.toJSON
      }));
    } else
      handleSubmit();
  };

  const handleSubmit = () => {
    client.quizzesPOST2(quizId!);

    navigate(`/quizzes`);
  };

  return (
    <>
      <Navbar />
      <div className={styles.quizContainer}>
        <h1 className={styles.quizTitle}>Quiz {quiz?.title}</h1>
        <FormControl component="fieldset">
          <FormLabel component="legend" style={{ color: "black" }}>
            {currentQuestion.text}
          </FormLabel>
          <RadioGroup
            value={selectedAnswers[currentQuestion.id!] || ""}
            onChange={
              (e) => {
                setSelectedAnswers((prev) => ({
                  ...prev,
                  [currentQuestion.id!]: e.target.value,
                }))
              }
            }
          >
            {currentQuestion.answers!.map((e) => (
              <FormControlLabel
                key={e.id}
                style={{
                  color: "black",
                  backgroundColor: correctAnswer?.id === e.id ? "lightgreen" : "transparent",
                }}
                value={e.id}
                control={<Radio />}
                label={e.text}
              />
            ))}
          </RadioGroup>
        </FormControl>
        {isAnswerCorrect !== null && (
          <div style={{ color: isAnswerCorrect ? "green" : "red" }}>
            {isAnswerCorrect ? "Correct!" : "Incorrect!"}
          </div>
        )}
        <div className={styles.quizNavigation}>
          <div style={{ display: "flex", gap: "10px" }}>
            <Button
              variant="contained"
              onClick={handleSubmitValidation}
              disabled={isAnswerCorrect !== null}
            >
              Submit Answer
            </Button>
            <Button
              variant="contained"
              onClick={handleNextQuestion}
              disabled={isAnswerCorrect === null}
            >
              {currentQuestionIndex == quiz?.questions.length ? "Finish Quiz" : "Next Question"}
            </Button>
          </div>
        </div>
        <p style={{ color: "black" }}>
          Current Question: {currentQuestionIndex}/{quiz!.questions.length}
        </p>
      </div>
    </>
  );
};