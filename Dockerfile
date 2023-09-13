FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /.

# Copy everything
COPY . ./
# Restore as distinct layers
WORKDIR /rinha-de-compiler-csharp
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /rinha-de-compiler-csharp
COPY --from=build-env /rinha-de-compiler-csharp/out .
COPY --from=build-env /var/rinha ./var/rinha
ENTRYPOINT ["dotnet", "rinha-de-compiler-csharp.dll"]