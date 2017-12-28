本来就是表驱动设计啊，不要随便改这个站点的设计好不啦。
ps：简约简单，不要过度抽象






有关dockerfile文件的解释 这不是防止您看不懂嘛~~

#定义该镜像基于的镜像 基于aspnetcore2.0.3 别名为base
FROM microsoft/aspnetcore:2.0.3 AS base
#设置构建上下文，将当前的运行目录设置为/app
WORKDIR /app
#对外开放容器80端口
EXPOSE 80

#拉取build镜像
FROM microsoft/aspnetcore-build:2.0 AS build
#设置当前构建运行者的工作目录为 /src 即上面的设置工作目录的代码会无效 但是设置可以创建一个目录
WORKDIR /src
#获取相对当前工作目录的*.sln统配文件，复制到当前目录 即无操作
COPY *.sln ./
#复制当前工作目录下的src下的。。。。。csproj并复制
COPY src/Service/EamaShop.Identity.API/EamaShop.Identity.API.csproj src/Service/EamaShop.Identity.API/
COPY src/Service/EamaShop.Identity.Services/EamaShop.Identity.Services.csproj src/Service/EamaShop.Identity.Services/
#执行dotnet restore命令
RUN dotnet restore
#无操作
COPY . .
#切换到web的项目目录
WORKDIR /src/src/Service/EamaShop.Identity.API
#执行编译命令 输出到绝对路径 /app下
RUN dotnet build -c Release -o /app

#修改当前构建运行者运行的基于的镜像
FROM build AS publish
#发布 并替换文件
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EamaShop.Identity.API.dll"]
 