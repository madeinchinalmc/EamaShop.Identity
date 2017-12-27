FROM microsoft/aspnetcore:2.0.3 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY src/Service/EamaShop.Identity.API/EamaShop.Identity.API.csproj src/Service/EamaShop.Identity.API/
COPY src/Service/EamaShop.Identity.Services/EamaShop.Identity.Services.csproj src/Service/EamaShop.Identity.Services/
RUN dotnet restore
COPY . .
WORKDIR /src/src/Service/EamaShop.Identity.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EamaShop.Identity.API.dll"]