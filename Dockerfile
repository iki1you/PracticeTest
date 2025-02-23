FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /PracticeTest

ENV ASPNETCORE_ENVIRONMENT="Development"
ENV ASPNETCORE_URLS="http://+:8080"
COPY . ./
WORKDIR WebApi
RUN dotnet restore
RUN dotnet publish -c release -o app
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /PracticeTest
COPY --from=build /PracticeTest/WebApi/app .
 
ENTRYPOINT ["dotnet", "WebApi.dll"]