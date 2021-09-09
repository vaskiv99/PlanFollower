FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5001/tcp

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Planner/Planner.Web/Planner.Web.csproj", "Services/Planner/Planner.Web/"]
COPY ["BuldingBlocks/BuildingBlocks.Host/BuildingBlocks.Web.csproj", "BuldingBlocks/BuildingBlocks.Host/"]
COPY ["BuldingBlocks/BuildingBlocks.Common/BuildingBlocks.Common.csproj", "BuldingBlocks/BuildingBlocks.Common/"]
COPY ["BuldingBlocks/BuildingBlocks.Logging/BuildingBlocks.Logging.csproj", "BuldingBlocks/BuildingBlocks.Logging/"]
COPY ["Services/Planner/Planner.Persistent/Planner.Persistent.csproj", "Services/Planner/Planner.Persistent/"]
COPY ["Services/Planner/Planner.Application/Planner.Application.csproj", "Services/Planner/Planner.Application/"]
COPY ["BuldingBlocks/BuildingBlocks.Scheduler.Quartz/BuildingBlocks.Scheduler.Quartz.csproj", "BuldingBlocks/BuildingBlocks.Scheduler.Quartz/"]
COPY ["Services/Planner/Planner.Domain/Planner.Domain.csproj", "Services/Planner/Planner.Domain/"]
COPY ["BuldingBlocks/BuldingBlocks.Domain/BuildingBlocks.Domain.csproj", "BuldingBlocks/BuldingBlocks.Domain/"]
COPY ["BuldingBlocks/BuildingBlocks.Database/BuildingBlocks.Database.csproj", "BuldingBlocks/BuildingBlocks.Database/"]
RUN dotnet restore "Services/Planner/Planner.Web/Planner.Web.csproj"
COPY . .
WORKDIR "/src/Services/Planner/Planner.Web"
RUN dotnet build "Planner.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planner.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planner.Web.dll"]