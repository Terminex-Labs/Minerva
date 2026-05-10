FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["dotnet/src/Minerva/Minerva.csproj", "dotnet/src/Minerva/"]

RUN dotnet restore "dotnet/src/Minerva/Minerva.csproj"

COPY . .

WORKDIR "/src/dotnet/src/Minerva"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Minerva.dll"]