import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Home } from "./pages/Home";
import { SignIn } from "./pages/SignIn/SignIn";
import { Quizzes } from "./pages/Quizzes/Quizzes";
import { CreateQuiz } from "./pages/CreateQuiz/CreateQuiz";
import { Quiz } from "./pages/SelectedQuiz/Quiz";
import { Results } from "./pages/Results/Results";

export const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/signin" element={<SignIn />} />
        <Route path="/quizzes" element={<Quizzes />} />
        <Route path="/create-quiz" element={<CreateQuiz />} />
        <Route path="/quizzes/:quizId" element={<Quiz />} />
        <Route path="/results" element={<Results />} />
      </Routes>
    </BrowserRouter>
  );
};
