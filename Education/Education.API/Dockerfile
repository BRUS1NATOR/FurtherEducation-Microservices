#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

COPY ./certs/RootCA.crt /usr/local/share/ca-certificates/RootCA.crt
RUN update-ca-certificates

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Education/Education.API/Education.API.csproj", "Education/Education.API/"]
RUN dotnet restore "Education/Education.API/Education.API.csproj"
COPY . .
WORKDIR "/src/Education/Education.API"
RUN dotnet build "Education.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Education.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Education.API.dll"]
