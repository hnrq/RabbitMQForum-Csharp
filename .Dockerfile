FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR app/
COPY bin/Release/netcoreapp2.2/publish/ app/
ENTRYPOINT ["dotnet", "Backend.dll"]