# 2024 Software Engineering 2 Polimi Course Project

## Build commands

###### Commands are to be run from the root folder

- Build project
```
    dotnet build app/client/client.csproj
    dotnet build app/server/server.csproj
```
- Run project
```
    dotnet run --project app/client/client.csproj
    dotnet run --project app/server/server.csproj
```
- Build optimized releases
```
    dotnet build --configuration release
```
- Clean from old build files
```
    dotnet clean
```

## Document redaction

###### Command is to be run from the document folder

To build the pdf documents, run `python3 ../makepdf.py`.

In order to build LaTeX documents install `texlive`.
- Windows: `winget install texlive.texlive`
- MacOS: `brew install --cask mactex`
- Debian Linux: `sudo apt install texlive-latex-base`

- - -

TODO:
- [ ] translate makepdf bash script to python
- [ ] configure docker for both client and server

###### C# code written by Francesco Ostidich, Matteo Salari, Francesco Rivitti
