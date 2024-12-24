<p align="center">
  <img src="assets/SC-logo.png" alt="S&C logo" width="250">
</p>

# Students & Companies

Students&Companies (S&C) is a platform designed to connect university students with companies offering internships.
It simplifies the internship searches of students and the projects advertisement for companies.

The platform employs recommendation mechanisms to match students and companies based on experience, skills, and project requirements.
S&C also supports the selection process by managing interviews and collecting feedbacks.
Additionally, it provides suggestions for improving CVs and project descriptions.

- - -

## Usage

To build and run the project apps, you will need to install Docker.
All commands are meant to be run from the [project root folder](.).
On Windows, Docker Desktop must be turned on before launching the apps from the terminal line.

Firstly, pull the docker images from Docker Hub.

```
docker pull fostidich/sc-web-server:latest
docker pull fostidich/sc-application-server:latest
```

Then, run the images.

```
docker run -d --rm --name sc-web-server -p 80:80 fostidich/sc-web-server
docker run -d --rm --name sc-application-server -p 4673:4673 fostidich/sc-application-server
```

Once concluded, stop the servers executions.

```
docker stop sc-web-server
docker stop sc-application-server
```

- - -

## Locations

The latest version of the project documents can be found [here](delivery).

- [RASD](delivery/RASDv1.0.pdf)
- [DD](delivery/DDv1.0.pdf)
- [ITD](delivery/ITDv1.0.pdf)
- [ATD](delivery/ATDv1.0.pdf)

The applications source code can be found [here](apps).

- [Web server](apps/web-server)
- [Application server](apps/application-server)

- - -

## TODO

- [x] Set up code environment
- [ ] Start coding

###### C# code written by Francesco Ostidich, Matteo Salari, Francesco Rivitti
