<p align="center">
  <img src="documents/assets/SC-logo.png" alt="S&C logo" width="250">
</p>

# Students & Companies

Students&Companies (S&C) is a platform designed to connect university students with companies offering internships.
It simplifies the internship searches of students and the projects advertisement for companies.

The platform employs recommendation mechanisms to match students and companies based on experience, skills, and project requirements.
S&C also supports the selection process by managing interviews and collecting feedbacks.
Additionally, it provides suggestions for improving CVs and project descriptions.

- - -

## Usage

To launch the project servers, [Docker](https://docker.com) must be installed.

To download the artifacts, pull the images from Docker Hub.

```
docker pull fostidich/sc-web-server:latest
docker pull fostidich/sc-application-server:latest
```

Then, run the images (change the ports if needed).

```
docker run -d --rm --name sc-web-server -p 80:80 fostidich/sc-web-server
docker run -d --rm --name sc-application-server -p 4673:4673 fostidich/sc-application-server
```

You should now find the website up on [localhost:80](http://localhost:80).
Once concluded, stop the servers executions.

```
docker stop sc-web-server
docker stop sc-application-server
```

- - -

## TODO

- [x] Set up code
- [ ] Assign components
- [ ] Dispatch a request

###### C# and JavaScript code written by Francesco Ostidich, Matteo Salari, Francesco Rivitti
