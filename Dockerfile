FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY src .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
# ENV ASPNETCORE_URLS=http://+:8000
EXPOSE 8000
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "kuberApi.dll"]
