import { Add } from "@mui/icons-material";
import { Button } from "@mui/material";
import { Link } from "react-router-dom";
import styles from "./QuizNavbar.module.scss";

export const QuizNavbar = () => {
  return (
    <div className={styles.quizNavbar}>
      <div className={styles.navbarContainer}>
        <div className={styles.text}>Quiz list</div>

        <div>
          <Link to={"/create-quiz"}>
            <Button variant="outlined" startIcon={<Add />}>
              Create quiz
            </Button>
          </Link>
        </div>
      </div>
    </div>
  );
};
