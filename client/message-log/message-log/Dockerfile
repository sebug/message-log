FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY message-log/message-log.csproj message-log/
RUN dotnet restore message-log/message-log.csproj
COPY . .
WORKDIR /src/message-log
RUN dotnet build message-log.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish message-log.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "message-log.dll"]
