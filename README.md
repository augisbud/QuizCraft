# QuizCraft

### Deployment on a Local Machine
Checkout the Github Repository, then run the following commands
Using .NET 8 SDK:
```
cd QuizCraft.Domain.API
dotnet run
```

or using Docker
```
cd QuizCraft.Domain.API
docker build -t quicraft:latest .
docker run -p 8080:8080 quizcraft:latest
```