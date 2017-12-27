FROM microsoft/aspnetcore:2.0.3 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY src/Service/EamaShop.Identity.API/EamaShop.Identity.API.csproj src/Service/EamaShop.Identity.API/
COPY src/Service/EamaShop.Identity.Services/EamaShop.Identity.Services.csproj src/Service/EamaShop.Identity.Services/
COPY test/EamaShop.Respository.Tests/EamaShop.Respository.Tests.csproj test/EamaShop.Respository.Tests
COPY test/EamaShop.Service.Tests/EamaShop.Service.Tests.csproj test/EamaShop.Service.Tests
RUN dotnet restore
COPY . .
WORKDIR /src/test/EamaShop.Service.Tests
RUN dotnet test 
WORKDIR /src/test/EamaShop.Respository.Tests
RUN dotnet test
WORKDIR /src/src/Service/EamaShop.Identity.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EamaShop.Identity.API.dll"]
