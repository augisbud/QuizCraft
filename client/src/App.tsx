import React, { useState, ChangeEvent, FormEvent } from 'react';

function App() {
    // Update state to specify that file can be `File` or `null`
    const [file, setFile] = useState<File | null>(null);

    // Type the event as `ChangeEvent<HTMLInputElement>`
    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        if (event.target.files) {
            setFile(event.target.files[0]);  // Ensure file is not null
        }
    };

    // Type the submit event as `FormEvent<HTMLFormElement>`
    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!file) {
            alert('Please select a file first.');
            return;
        }

        const formData = new FormData();
        formData.append('file', file);

        try {
            const response = await fetch('https://localhost:8080/upload', {
                method: 'POST',
                body: formData,
            });

            if (response.ok) {
                const message = await response.text();
                alert(message);
            } else {
                alert('File upload failed.');
            }
        } catch (error) {
            console.error('Error during file upload:', error);
            alert('An error occurred.');
        }
    };

    return (
        <div>
            <h1>Upload a File</h1>
            <form onSubmit={handleSubmit}>
                <input type="file" onChange={handleFileChange} />
                <button type="submit">Upload</button>
            </form>
        </div>
    );
}

export default App;
