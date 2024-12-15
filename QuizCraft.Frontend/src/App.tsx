import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Home } from "./pages/Home/Home";
import { SignIn } from "./pages/SignIn/SignIn";
import { Quizzes } from "./pages/Quizzes/Quizzes";
import { CreateQuiz } from "./pages/CreateQuiz/CreateQuiz";
import { Quiz } from "./pages/SelectedQuiz/Quiz";
import { Results } from "./pages/Results/Results";
import { GoogleOAuthProvider } from "@react-oauth/google";

// 1. Application can be interacted with using *some* sort of interface
export const App = () => {
  return (
    <GoogleOAuthProvider clientId="138852874630-e6qnkeeocq735jgllo85ff50u2ejj57c.apps.googleusercontent.com">
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
    </GoogleOAuthProvider>
  );
};