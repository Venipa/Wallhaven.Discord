@echo off
if "%1" == "-d" (
    docker run --name=wallhaven.discord -d wallhaven.discord
    echo started daemon...
) else (
    docker run --name=wallhaven.discord wallhaven.discord
)
@echo on