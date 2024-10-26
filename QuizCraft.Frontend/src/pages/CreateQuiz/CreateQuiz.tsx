import { ChangeEvent, useState } from "react";
import { Navbar } from "../../components/Navbar/Navbar";
import { Button, styled } from "@mui/material";
import { CloudUpload } from "@mui/icons-material";
import styles from "./CreateQuiz.module.scss";
import { useNavigate } from "react-router-dom";
import { QuizDto } from "../../utils/QuizCraftAPIClient";

export const CreateQuiz = () => {
  const navigate = useNavigate();

  const [file, setFile] = useState<File | null>(null);

  const handleFileChange = async (event: ChangeEvent<HTMLInputElement>) => {
    if (event.target.files) {
      const selectedFile = event.target.files[0];
      setFile(selectedFile);

      await handleSubmit(selectedFile);
    }
  };

  const handleSubmit = async (fileToUpload: File) => {
    if (!fileToUpload) {
      alert("Please select a file first.");
      return;
    }

    const formData = new FormData();
    formData.append("file", fileToUpload);

    try {
      const response = await fetch("https://localhost:8080/quizzes", {
        method: "POST",
        body: formData,
      });

      if (response.ok) {
        const quiz = (await response.json()) as QuizDto;

        navigate(`/quizzes/${quiz.id}`);
      } else {
        alert("File upload failed.");
      }
    } catch (error) {
      console.error("Error during file upload:", error);
      alert("An error occurred.");
    }
  };

  const VisuallyHiddenInput = styled("input")({
    clip: "rect(0 0 0 0)",
    clipPath: "inset(50%)",
    height: 1,
    overflow: "hidden",
    position: "absolute",
    bottom: 0,
    left: 0,
    whiteSpace: "nowrap",
    width: 1,
  });

  return (
    <>
      <Navbar />
      <div className={styles.mainContainer}>
        <div className={styles.quizUploaderContainer}>
          <div className={styles.uploadSection}>
            <h3>Upload File</h3>
            <Button
              component="label"
              role={undefined}
              variant="contained"
              tabIndex={-1}
              startIcon={<CloudUpload />}
            >
              Upload Quiz
              <VisuallyHiddenInput type="file" onChange={handleFileChange} />
            </Button>
            {file && <p>Selected File: {file.name}</p>}
          </div>
        </div>
      </div>
    </>
  );
};
