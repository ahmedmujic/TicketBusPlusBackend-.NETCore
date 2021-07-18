#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NotificationServiceN/NotificationServiceN.csproj", "NotificationServiceN/"]
RUN dotnet restore "NotificationServiceN/NotificationServiceN.csproj"
COPY . .
WORKDIR "/src/NotificationServiceN"
RUN dotnet build "NotificationServiceN.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationServiceN.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationServiceN.dll"]