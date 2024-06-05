FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ARG TARGETARCH
WORKDIR /app
COPY src .
RUN dotnet restore -a $TARGETARCH
RUN dotnet publish -a $TARGETARCH  -c Release -o out

# Build runtime image mcr.microsoft.com/dotnet/runtime:8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0
# ENV ASPNETCORE_URLS=http://+:8000
EXPOSE 8000
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "kuberApi.dll"]
