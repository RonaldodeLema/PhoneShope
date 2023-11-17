FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AdminSite/AdminSite.csproj", "AdminSite/"]
COPY ["Internals/Internals.csproj", "Internals/"]
RUN dotnet restore "AdminSite/AdminSite.csproj"
COPY . .
WORKDIR "/src/AdminSite"
RUN dotnet build "AdminSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdminSite.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminSite.dll"]
