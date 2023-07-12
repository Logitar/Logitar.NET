#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Logitar.Demo.Ui/Logitar.Demo.Ui.csproj", "src/Logitar.Demo.Ui/"]
RUN dotnet restore "src/Logitar.Demo.Ui/Logitar.Demo.Ui.csproj"
COPY . .
WORKDIR "/src/src/Logitar.Demo.Ui"
RUN dotnet build "Logitar.Demo.Ui.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Logitar.Demo.Ui.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Logitar.Demo.Ui.dll"]