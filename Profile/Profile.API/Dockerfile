#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

COPY ./certs/RootCA.crt /usr/local/share/ca-certificates/RootCA.crt
RUN update-ca-certificates

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Profile/Profile.API/Profile.API.csproj", "Profile/Profile.API/"]
RUN dotnet restore "Profile/Profile.API/Profile.API.csproj"
COPY . .
WORKDIR "/src/Profile/Profile.API"
RUN dotnet build "Profile.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Profile.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Profile.API.dll"]