FROM mcr.microsof.com/dotnet/sdk:6.0 AS build

WORKDIR /PlainOldStoreApp.Ui/

COPY . .

RUN dotnet publish -c Release

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /PlainOldStoreApp.Ui/

COPY --from=build /PlainOldStoreApp.Ui/bin/Release/net6.0/publish/ .

ENTRYPOINT [ "dotnet", "PlainOldStoreApp.Ui.dll" ]