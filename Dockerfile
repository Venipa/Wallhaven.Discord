FROM mcr.microsoft.com/dotnet/core/runtime:3.1

COPY Wallhaven.Discord/bin/Release/netcoreapp3.1/publish/ app/
COPY Wallhaven.Discord.ini app/Wallhaven.Discord.ini

ENTRYPOINT ["dotnet", "app/Wallhaven.Discord.dll"]