FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS restore
WORKDIR /app
COPY ./src/Core/Core.csproj ./src/Core/Core.csproj
COPY ./src/Infra/Infra.csproj ./src/Infra/Infra.csproj
COPY ./src/Web/Web.csproj ./src/Web/Web.csproj
RUN dotnet restore ./src/Web/Web.csproj

FROM restore as publish
COPY ./src ./src
RUN dotnet publish ./src/Web/Web.csproj -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS final
EXPOSE 80
WORKDIR /app
COPY --from=publish /app/out ./
ENTRYPOINT ["dotnet", "Web.dll"]