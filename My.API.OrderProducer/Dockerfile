# 基础运行时镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 构建阶段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["My.API.OrderProducer.csproj", "."]
RUN dotnet restore "My.API.OrderProducer.csproj"

COPY . .

RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build My.API.OrderProducer.csproj

# 发布阶段（必须重新声明 ARG）
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish My.API.OrderProducer.csproj /p:UseAppHost=false

# 最终镜像
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "My.API.OrderProducer.dll"]
