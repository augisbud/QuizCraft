import React, { useState } from 'react';

function App() {
    const [file, setFile] = useState(null);

    const handleFileChange = (event) => {
        setFile(event.target.files[0]);
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        const formData = new FormData();
        formData.append('file', file);

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
