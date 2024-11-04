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
import { QuestionDto, QuizDto, AnswerValidationInputDto, AnswerDto } from "../../utils/QuizCraftAPIClient";
import { client } from "../../utils/Client";

export const Quiz = () => {
  const { quizId } = useParams<{ quizId: string }>();

  const navigate = useNavigate();

  useEffect(() => {
    if(!sessionStorage.getItem("token"))
      navigate(`/signin?redirect=/quizzes/${quizId}`);  
  }, [navigate, quizId])

  const [quiz, setQuiz] = useState<QuizDto>();
  const [questions, setQuestions] = useState<QuestionDto[]>([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState<number>(0);
  const [answers, setAnswers] = useState<{ [key: string]: string }>({});
  const [loading, setLoading] = useState<boolean>(true);
  const [isAnswerCorrect, setIsAnswerCorrect] = useState<boolean | null>(null);
  const [correctAnswer, setCorrectAnswer] = useState<AnswerDto | null>(null);
  const [persistedAnswers, setPersistedAnswers] = useState<{ [key: string]: string }>({});


  useEffect(() => {
      const fetchQuiz = async () => {
        if (quizId === undefined) return;

        const quizData = await client.quizzesGET(quizId);
        setQuiz(quizData);

        if (quizData?.nextUnansweredQuestionIndex !== undefined) {
            setCurrentQuestionIndex(quizData.nextUnansweredQuestionIndex);
        }

        // Retrieve saved answers from localStorage or API
        const savedAnswers = JSON.parse(localStorage.getItem(`quiz_${quizId}_answers`) || "{}");
        setPersistedAnswers(savedAnswers);
        setAnswers(savedAnswers); // Set initial answers based on saved answers
      };


    if(sessionStorage.getItem("token"))
      fetchQuiz();
  }, [quizId]);

  useEffect(() => {
    const fetchQuestions = async () => {
      if (quizId === undefined) return;
      if(!sessionStorage.getItem("token")) return;
      
      const questionsData = await client.questionsAll(quizId);

      setQuestions(questionsData);
      setLoading(false);
    };

    fetchQuestions();
  }, [quizId]);

  if (loading) return <div>Loading...</div>;

  const currentQuestion = questions[currentQuestionIndex];

  const handleAnswerChange = (questionId: string, answer: string) => {
    setAnswers((prevAnswers) => {
      const updatedAnswers = { ...prevAnswers, [questionId]: answer };
    
      // Persist the updated answers
      localStorage.setItem(`quiz_${quizId}_answers`, JSON.stringify(updatedAnswers));
    
      return updatedAnswers;
  });
};


  const handleSubmitValidation = async () => {
    const questionId = currentQuestion.id!;
    const answer = answers[questionId];
    const body: AnswerValidationInputDto = { text: answer!, init: () => {}, toJSON: () => ({ text: answer! }) };

    try {
      const response = await client.questions(quizId!, questionId, body);
      setIsAnswerCorrect(response.isCorrect!);
      setCorrectAnswer(response.correctAnswer!);
    } catch (error) {
      console.error("Error validating answer:", error);
    }
  };

  const handleNextQuestion = () => {
    setIsAnswerCorrect(null);
    setCorrectAnswer(null);
    setCurrentQuestionIndex((prevIndex) => prevIndex + 1);
  };

  const handleSubmit = () => {
    console.log("Submitted answers:", answers);
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
            value={answers[currentQuestion.id!] || ""}
            onChange={(e) => handleAnswerChange(currentQuestion.id!, e.target.value)}
          >
            {currentQuestion.answers!.map((e) => (
              <FormControlLabel
                key={e}
                style={{
                  color: "black",
                  backgroundColor: correctAnswer?.text === e ? "lightgreen" : "transparent",
                }}
                value={e}
                control={<Radio />}
                label={e}
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
              disabled={isAnswerCorrect === null || currentQuestionIndex === questions.length - 1}
            >
              Next
            </Button>
          </div>
          {currentQuestionIndex === questions.length - 1 && isAnswerCorrect !== null && (
            <Button
              variant="contained"
              onClick={handleSubmit}
              style={{ display: "flex", marginLeft: "10px" }}
            >
              Submit Quiz
            </Button>
          )}
        </div>
          <p style={{ color: "black" }}>
            Answered Questions: {Object.keys({ ...persistedAnswers, ...answers }).length}/{quiz?.questionCount || questions.length}
          </p>
      </div>
    </>
  );
};