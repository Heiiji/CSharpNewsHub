# News RSS Hub

## Description
Ce projet est une application web en C# conçue à des fin de test. Elle cherche des info depuis des flux RSS et stock les resultats formaté pour pouvoir les redistribuer dans un flux unique.

## Prérequis
- .NET Core 3.1 ou supérieur
- Un IDE comme Visual Studio ou VSCode
- MongoDB

## Installation
1. Clonez le dépôt
2. Ouvrez la solution dans votre IDE.
3. Restaurez les packages NuGet.
4. Configurez `appsettings.json`.
```json
{
    "NewsStoreDatabase": {
        "ConnectionString": "mongodb://example:PORT",
        "DatabaseName": string,
        "NewsCollectionName": string
    },
        "SourcesStoreDatabase": {
        "ConnectionString": "mongodb://example:PORT",
        "DatabaseName": string,
        "SourcesCollectionName": string
    }
}
```
5. Lancez l'application.

## Utilisation
Le job CRON fetch les news une fois par heure. Vous pouvez tester en ajoutant/supprimant et modifiant les sources en utilisant l'api `/api/source`
