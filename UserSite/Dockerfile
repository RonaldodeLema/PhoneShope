FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UserSite/UserSite.csproj", "UserSite/"]
COPY ["AdminSite/AdminSite.csproj", "AdminSite/"]
COPY ["Internals/Internals.csproj", "Internals/"]
RUN dotnet restore "UserSite/UserSite.csproj"
COPY . .
WORKDIR "/src/UserSite"
RUN dotnet build "UserSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserSite.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserSite.dll"]
