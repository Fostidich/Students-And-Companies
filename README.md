# 2024 Software Engineering 2 Polimi Course Project

## Usage

To build and run the project apps, you will need to install [docker](#docker-run), or otherwise the [dotnet](#dotnet-run) SDK.

All commands are meant to be run from the [project root folder](.).

For ease of use, we recommend installing `make`, as it allows useful commands to be launched directly via the `Makefile`.
To list all available commands, run `make help`.


### Docker run

On Windows, Docker Desktop must be turned on before launching the apps from the terminal line.

If `make` is installed, a clean release `docker` build and run can be launched with

```
make client
```
```
make server
```

otherwise, if `make` does not suit you, the apps can be run directly with `docker` as follows.

```
docker build -t sc-client ./apps/client
docker run --rm -it -p 4674:80 sc-client
```
```
docker build -t sc-server ./apps/server
docker run --rm -it -p 4673:80 sc-server
```

### Dotnet run

To build and run the apps with `dotnet`, use the following commands.

```
dotnet run --project apps/client --configuration Release
```
```
dotnet run --project apps/server --configuration Release
```

- - -

## Locations

The latest version of the project documents can be found [here](Delivery).

- [RASD](Delivery/RASDv0.1.pdf)
- [DD](Delivery/DDv0.1.pdf)
- [ITD](Delivery/ITDv0.1.pdf)
- [ATD](Delivery/ATDv0.1.pdf)

The applications source code can be found [here](apps).

- [Client](apps/client/src)
- [Server](apps/server/src)

- - -

## TODO

### General

- [x] Configure dotnet for both client and server
- [x] Configure docker for both client and server
- [x] Configure make for ease of use
- [ ] Explain the installing process for dotnet and docker

### RASD

#### Project scope

- [ ] Personal data upload
- [ ] Selction process management
- [ ] Other part information querying
- [ ] Internship management
- [ ] Complaints handling
- [ ] Statistics collection
- [ ] Feedback collection
- [ ] Recommendation system
- [ ] Suggestion system

#### Project descriptors

- [ ] Goals ~5
- [ ] Features ~5
- [ ] World phenomena ~7
- [ ] Shared phenomena ~20
- [ ] Scenarios ~10
- [ ] Requirements ~25
- [ ] Use cases ~15

###### C# code written by Francesco Ostidich, Matteo Salari, Francesco Rivitti
