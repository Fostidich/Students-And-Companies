# 2024 Software Engineering 2 Polimi Course Project

## Application build

###### Commands are to be run from the root folder

- Run project (debug)
```
    dotnet run --project app/client
    dotnet run --project app/server
```
- Run project (release)
```
    docker compose run client
    docker compose run server
```

## Document redaction

###### Command is to be run from the document folder

- Build document
```
    pdflatex main.tex
```

- - -

##### TODO
- [x] configure dotnet for both client and server
- [x] configure docker for both client and server

###### C# code written by Francesco Ostidich, Matteo Salari, Francesco Rivitti
