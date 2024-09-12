FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY ./ .

ENTRYPOINT ["dotnet", "QuizCraft.Domain.API.dll"]