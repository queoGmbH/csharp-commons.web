#Cake Template - .NET

Template für .NET  Anwendungen inklusive Asciidoc Dokumentation und Erstellen von Nuget Paketen.

## Branches

Main: Ist fertig und kann verwendet werden

Develop: Ist im Allgemeinen funktionsfähig, kann aber noch Fehler enthalten.

## Orderstruktur

- `root`: Solution, Azure Pipelines, Versionsinformationen, Nuget Konfiguration und Cake Startskript sowie diese ReadMe
- `build`: Cake Pipeline mit ihren Targets, Flows und Tasks
- `doc`: Wurzelverzeichnis für Asciidoc Dokumentation
- `src`: Die einzelnen Projekte der Solution, welche die finale Anwendung abbilden
- `tests`: Die Testprojekte

## Setup

- Inhalt als Zip herunterladen und an geeigneter Stelle entpacken
- `build.ps1` über Powershell ausführen, um Funktionalität zu prüfen
- Eigene Solution und (Test)Projekte analog den vorhandenen `ExampleAPI` Dateien in die Ordnerstruktur einfügen.
  - Die `ExampleAPI` Dateien können dabei gelöscht werden.
  - Bei einem bereits bestehenden Projekt können folgende Bestandteile in das bestehende Projekt übernommen werden:
    - `build`
    - `azure-pipelines-*.*`
    - `build.ps1`
    - Bei der Übernahme von `.gitignore` und `.editorconfig` müssen diese mit evtl. bereits vorhandenen Dateien zusammengeführt werden.
- `build/Build/Context.cs` mit einem geeigneten Editor öffnen und folgende Werte anpassen:
  - `General.Name`: Der Name, der z.B: in Artefakten verwendet werden soll. Kann abweichend des tatsächelichen Anwendungsnamen sein.
  - `ProjectSpecifics.SolutionName`: Der Name der Solution Datei inklusive Dateiendung (.sln).
  - `ProjectSpecifics.BuildConfig`: Die Buildkonfiguration. Standardmäßig `Release`
  - `ProjectSpecifics.MainProject`: Das zu bauende Projekt (.csproj Datei).
  - `Tests.TestProjectName`: Der Name des Testprojekts.
  - `Tests.TestProject`: Die Projektdatei (.csproj) des Testprojekts.
    - Es können weitere Testprojekte mit Namen und Projektdatei definiert werden, welche anschließend im `RunTestsAndPublishResults.cs` Task eingebunden werden müssen.
  - `Doc.RootFilePath`: Die Startdatei der Asciidoc Dokumentation. Aktuell ist hier nur eine Dokumentation vorgesehen. Weitere Dokumentationen müssen nachträglich in die Cake Pipeline integriert werden.
  - `Doc.GeneratedPath`: Der Pfad, in dem die generierte Dokumentation abgelegt wird.
  - `DocOnly.AssemlyVersion`: Die Versionsnummer die in der Dokumentation angezeigt wird, wenn die Dokumentation ohne Build erstellt wird. Anderenfalls wird die Versionsnummer aus der Assembly Version des Hauptprojekts abgeleitet.

## Targets

|Name|Shell Parameter|
|----|---------------|
|Default||
|BuildAndTest|--target=BuildAndTest|
|BuildPackage|--target=BuildPackage|

## Flows

Verschiedene Flows für Standardabläufe sind bereits in der `build/Build/Flows` Datei definiert, diese können angepasst und erweitert werden.
