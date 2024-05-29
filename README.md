#Cake Template - .NET

Template f�r .NET  Anwendungen inklusive Asciidoc Dokumentation und Erstellen von Nuget Paketen.

## Branches

Main: Ist fertig und kann verwendet werden

Develop: Ist im Allgemeinen funktionsf�hig, kann aber noch Fehler enthalten.

## Orderstruktur

- `root`: Solution, Azure Pipelines, Versionsinformationen, Nuget Konfiguration und Cake Startskript sowie diese ReadMe
- `build`: Cake Pipeline mit ihren Targets, Flows und Tasks
- `doc`: Wurzelverzeichnis f�r Asciidoc Dokumentation
- `src`: Die einzelnen Projekte der Solution, welche die finale Anwendung abbilden
- `tests`: Die Testprojekte

## Setup

- Inhalt als Zip herunterladen und an geeigneter Stelle entpacken
- `build.ps1` �ber Powershell ausf�hren, um Funktionalit�t zu pr�fen
- Eigene Solution und (Test)Projekte analog den vorhandenen `ExampleAPI` Dateien in die Ordnerstruktur einf�gen.
  - Die `ExampleAPI` Dateien k�nnen dabei gel�scht werden.
  - Bei einem bereits bestehenden Projekt k�nnen folgende Bestandteile in das bestehende Projekt �bernommen werden:
    - `build`
    - `azure-pipelines-*.*`
    - `build.ps1`
    - Bei der �bernahme von `.gitignore` und `.editorconfig` m�ssen diese mit evtl. bereits vorhandenen Dateien zusammengef�hrt werden.
- `build/Build/Context.cs` mit einem geeigneten Editor �ffnen und folgende Werte anpassen:
  - `General.Name`: Der Name, der z.B: in Artefakten verwendet werden soll. Kann abweichend des tats�chelichen Anwendungsnamen sein.
  - `ProjectSpecifics.SolutionName`: Der Name der Solution Datei inklusive Dateiendung (.sln).
  - `ProjectSpecifics.BuildConfig`: Die Buildkonfiguration. Standardm��ig `Release`
  - `ProjectSpecifics.MainProject`: Das zu bauende Projekt (.csproj Datei).
  - `Tests.TestProjectName`: Der Name des Testprojekts.
  - `Tests.TestProject`: Die Projektdatei (.csproj) des Testprojekts.
    - Es k�nnen weitere Testprojekte mit Namen und Projektdatei definiert werden, welche anschlie�end im `RunTestsAndPublishResults.cs` Task eingebunden werden m�ssen.
  - `Doc.RootFilePath`: Die Startdatei der Asciidoc Dokumentation. Aktuell ist hier nur eine Dokumentation vorgesehen. Weitere Dokumentationen m�ssen nachtr�glich in die Cake Pipeline integriert werden.
  - `Doc.GeneratedPath`: Der Pfad, in dem die generierte Dokumentation abgelegt wird.
  - `DocOnly.AssemlyVersion`: Die Versionsnummer die in der Dokumentation angezeigt wird, wenn die Dokumentation ohne Build erstellt wird. Anderenfalls wird die Versionsnummer aus der Assembly Version des Hauptprojekts abgeleitet.

## Targets

|Name|Shell Parameter|
|----|---------------|
|Default||
|BuildAndTest|--target=BuildAndTest|
|BuildPackage|--target=BuildPackage|

## Flows

Verschiedene Flows f�r Standardabl�ufe sind bereits in der `build/Build/Flows` Datei definiert, diese k�nnen angepasst und erweitert werden.
