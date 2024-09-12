FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

EXPOSE 8080

WORKDIR /app

COPY ./bin/Release/net8.0/publish/ . 

ENTRYPOINT ["dotnet", "QuizCraft.Domain.API.dll"]