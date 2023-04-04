FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DripChip_API/DripChip_API.csproj", "DripChip_API/"]
COPY ["DripChip_API.Domain/DripChip_API.Domain.csproj", "DripChip_API.Domain/"]
COPY ["DripChip_API.Service/DripChip_API.Service.csproj", "DripChip_API.Service/"]
COPY ["DripChip_API.DAL/DripChip_API.DAL.csproj", "DripChip_API.DAL/"]
RUN dotnet restore "DripChip_API/DripChip_API.csproj"
COPY . .
WORKDIR "/src/DripChip_API"
RUN dotnet build "DripChip_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DripChip_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DripChip_API.dll"]
