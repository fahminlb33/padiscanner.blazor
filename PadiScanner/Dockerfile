#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PadiScanner/PadiScanner.csproj", "PadiScanner/"]
RUN dotnet restore "PadiScanner/PadiScanner.csproj"
COPY . .
WORKDIR "/src/PadiScanner"
RUN dotnet build "PadiScanner.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PadiScanner.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PadiScanner.dll"]