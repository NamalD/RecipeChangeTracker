FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /api

# Copy and publish API
COPY . ./ 
RUN dotnet restore RecipeChangeTracker.API/RecipeChangeTracker.API.fsproj
RUN dotnet publish -c Release -o out RecipeChangeTracker.API/RecipeChangeTracker.API.fsproj

# TODO: Run tests

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /api
COPY --from=build-env /api/out .
ENTRYPOINT ["dotnet", "RecipeChangeTracker.API.App.dll"]
