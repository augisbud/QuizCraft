import { useState, ChangeEvent, FormEvent } from 'react';
import styles from './App.module.scss';
import { QuizDto } from './types';

// 1. Application can be interacted with using *some* sort of interface
export const App = () => {
    const [file, setFile] = useState<File | null>(null);
    const [quiz, setQuiz] = useState<QuizDto | null>(null);

    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.files) {
            setFile(event.target.files[0]);
        }
    };

    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!file) {
            alert('Please select a file first.');
            return;
        }

        const formData = new FormData();
        formData.append('file', file);

        try {
            const quizzesResponse = await fetch('https://localhost:8080/quizzes', {
                method: 'POST',
                body: formData,
            });

            if (quizzesResponse.ok) {
                const quizData: QuizDto = await quizzesResponse.json();
                setQuiz(quizData);
            } else {
                const errorText = await quizzesResponse.text();
                alert(`Quiz generation failed: ${errorText}`);
            }
        } catch (error) {
            console.error('Error during file upload:', error);
            alert('An error occurred.');
        }
    };

    return (
        <div className={styles.wrapper}>
            <h1 className={styles.title}>Upload a File</h1>
            <form onSubmit={handleSubmit}>
                <input type="file" onChange={handleFileChange} />
                <button className={styles.button} type="submit">Upload</button>
            </form>

            {quiz && (
                <div className={styles.quiz}>
                    <h2>{quiz.questions[0].text}</h2> {}
                    <ul>
                        {quiz.questions[0].answers.map((answer, index) => (
                            <li key={index}>{answer.text}</li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default App;
