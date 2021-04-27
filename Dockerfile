#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8032

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SampleCommonInfo/SampleCommonInfo.csproj", "SampleCommonInfo/"]
RUN dotnet restore "SampleCommonInfo/SampleCommonInfo.csproj"
COPY . .
WORKDIR "/src/SampleCommonInfo"
RUN dotnet build "SampleCommonInfo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleCommonInfo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleCommonInfo.dll"]