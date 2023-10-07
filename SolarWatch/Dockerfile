
FROM mcr.microsoft.com/dotnet/aspnet:latest AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src
COPY ["SolarWatch.csproj", ""]
RUN dotnet restore "SolarWatch.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "SolarWatch.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SolarWatch.csproj" -c Release -o /app/publish



# Use a runtime image for the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SolarWatch.dll"]
