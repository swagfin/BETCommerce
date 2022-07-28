#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["BetCommerce.WebClient/BetCommerce.WebClient.csproj", "BetCommerce.WebClient/"]
RUN dotnet restore "BetCommerce.WebClient/BetCommerce.WebClient.csproj"
COPY . .
WORKDIR "/src/BetCommerce.WebClient"
RUN dotnet build "BetCommerce.WebClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BetCommerce.WebClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BetCommerce.WebClient.dll"]