FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
RUN apt-get install -y libxss1

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY . .
RUN dotnet restore "EasyNow.Job.Smzdm/EasyNow.Job.Smzdm.csproj"
WORKDIR "/src/EasyNow.Job.Smzdm"

FROM build AS publish
RUN dotnet publish "EasyNow.Job.Smzdm.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EasyNow.Job.Smzdm.dll"]
