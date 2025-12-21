# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copier les fichiers csproj et restaurer les dépendances
COPY *.csproj ./
RUN dotnet restore

# Copier le reste et publier
COPY . ./
RUN dotnet publish -c Release -o out

# Étape finale : Image de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Render utilise la variable d'environnement PORT, on force l'écoute sur 0.0.0.0
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "BrasilBurger.Web.dll"]