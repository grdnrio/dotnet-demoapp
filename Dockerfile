 # Stage 1
 FROM microsoft/aspnetcore-build:2.0 AS builder
 WORKDIR /source

 # caches restore result by copying csproj file separately
 COPY dotnet-demoapp.csproj .
 RUN dotnet restore

 # copies the rest of your code
 COPY . .
 RUN dotnet publish --output /app/ --configuration Release

 # Stage 2
 FROM microsoft/aspnetcore:2.0
 WORKDIR /app
 COPY --from=builder /app .
 EXPOSE 80
 ENTRYPOINT ["dotnet", "dotnet-demoapp.dll"]