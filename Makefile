.PHONY: help clean test web-server web-server-docker application-server application-server-docker db-migration docker-push rasd dd itd atd rasd-verbose dd-verbose itd-verbose atd-verbose layout-export

help:
ifeq ($(OS),Windows_NT)
	@cmd /c "echo."
	@echo    make help                              Print help panel
	@cmd /c "echo."
	@echo    make clean                             Clean project tree from build files
	@echo    make test                              Launch tests
	@cmd /c "echo."
	@echo    make web-server                        Launch a web server dev run
	@echo    make web-server-docker                 Dockerize web server release app
	@cmd /c "echo."
	@echo    make application-server                Launch an application server dev run
	@echo    make application-server-docker         Dockerize application server release app
	@cmd /c "echo."
	@echo    make db-migration NAME=<name>          Create a migration for updating the DB
	@cmd /c "echo."
	@echo    make docker-push REPO=<repo>           Dockerize servers and push images
	@cmd /c "echo."
	@echo    make rasd                              Compile RASD.pdf from LaTeX
	@echo    make dd                                Compile DD.pdf from LaTeX
	@echo    make itd                               Compile ITD.pdf from LaTeX
	@echo    make atd                               Compile ATD.pdf from LaTeX
	@cmd /c "echo."
	@echo    make rasd-verbose                      Run pdflatex verbosely for RASD
	@echo    make dd-verbose                        Run pdflatex verbosely for DD
	@echo    make itd-verbose                       Run pdflatex verbosely for ITD
	@echo    make atd-verbose                       Run pdflatex verbosely for ATD
	@cmd /c "echo."
	@echo    make layout-export                     Copy main LaTeX layout to all documents
	@cmd /c "echo."
else
	@echo ""
	@echo "    make help                            Print help panel"
	@echo ""
	@echo "    make clean                           Clean project tree from build files"
	@echo "    make test                            Launch tests"
	@echo ""
	@echo "    make web-server                      Launch a web server dev run"
	@echo "    make web-server-docker               Dockerize web server release app"
	@echo ""
	@echo "    make application-server              Launch an application server dev run"
	@echo "    make application-server-docker       Dockerize application server release app"
	@echo ""
	@echo "    make db-migration NAME=<name>        Create a migration for updating the DB"
	@echo ""
	@echo "    make docker-push REPO=<repo>         Dockerize servers and push images"
	@echo ""
	@echo "    make rasd                            Compile RASD.pdf from LaTeX"
	@echo "    make dd                              Compile DD.pdf from LaTeX"
	@echo "    make itd                             Compile ITD.pdf from LaTeX"
	@echo "    make atd                             Compile ATD.pdf from LaTeX"
	@echo ""
	@echo "    make rasd-verbose                    Run pdflatex verbosely for RASD"
	@echo "    make dd-verbose                      Run pdflatex verbosely for DD"
	@echo "    make itd-verbose                     Run pdflatex verbosely for ITD"
	@echo "    make atd-verbose                     Run pdflatex verbosely for ATD"
	@echo ""
	@echo "    make layout-export                   Copy main LaTeX layout to all documents"
	@echo ""
endif

clean:
ifeq ($(OS),Windows_NT)
	del /Q /F documents\RASD\*.aux documents\RASD\*.log documents\RASD\*.out documents\RASD\*.toc documents\RASD\*.fls
	del /Q /F documents\DD\*.aux documents\DD\*.log documents\DD\*.out documents\DD\*.toc documents\DD\*.fls
	del /Q /F documents\ITD\*.aux documents\ITD\*.log documents\ITD\*.out documents\ITD\*.toc documents\ITD\*.fls
	del /Q /F documents\ATD\*.aux documents\ATD\*.log documents\ATD\*.out documents\ATD\*.toc documents\ATD\*.fls
	rmdir /S /Q application-server\bin || rem
	rmdir /S /Q application-server\obj || rem
	rmdir /S /Q application-server\Tests\bin || rem
	rmdir /S /Q application-server\Tests\obj || rem
	rmdir /S /Q application-server\Tests\TestResults || rem
	rmdir /S /Q web-server\node_modules || rem
	docker container prune -f || rem
	docker image prune -f || rem
	docker rmi sc-web-server -f || rem
	docker rmi sc-application-server -f || rem
else
	rm -f documents/RASD/*.aux documents/RASD/*.log documents/RASD/*.out documents/RASD/*.toc documents/RASD/*.fls
	rm -f documents/DD/*.aux documents/DD/*.log documents/DD/*.out documents/DD/*.toc documents/DD/*.fls
	rm -f documents/ITD/*.aux documents/ITD/*.log documents/ITD/*.out documents/ITD/*.toc documents/ITD/*.fls
	rm -f documents/ATD/*.aux documents/ATD/*.log documents/ATD/*.out documents/ATD/*.toc documents/ATD/*.fls
	rm -rf application-server/bin
	rm -rf application-server/obj
	rm -rf application-server/Tests/bin
	rm -rf application-server/Tests/obj
	rm -rf application-server/Tests/TestResults
	rm -rf web-server/node_modules
	docker container prune -f || true
	docker image prune -f || true
	docker rmi sc-web-server -f || true
	docker rmi sc-application-server -f || true
endif

test:
ifeq ($(OS),Windows_NT)
	dotnet clean application-server/Tests > NUL
	dotnet test application-server/Tests --nologo
else
	dotnet clean application-server/Tests > /dev/null
	dotnet test application-server/Tests --nologo
endif

web-server:
	npm --prefix web-server install
	npm --prefix web-server run dev

web-server-docker:
	docker build -t sc_web_server ./web-server
	docker run --rm -it -p 8080:80 sc_web_server
	docker image prune -f

application-server:
	ASPNETCORE_ENVIRONMENT=Development dotnet watch run --project application-server --configuration Debug

application-server-docker:
	docker build -t sc_application_server ./application-server
	docker run --rm -it -p 4673:5000 sc_application_server
	docker image prune -f

db-migration:
	dotnet ef migrations add $(NAME) --project application-server

docker-push:
	docker system prune -af
	docker build -t sc_application_server ./application-server
	docker tag sc_application_server $(REPO)/sc_application_server:latest
	docker push $(REPO)/sc_application_server:latest
	docker build -t sc_web_server ./web-server
	docker tag sc_web_server $(REPO)/sc_web_server:latest
	docker push $(REPO)/sc_web_server:latest

rasd: layout-export
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=documents/RASD documents/RASD/main.tex > NUL
	pdflatex -output-directory=documents/RASD documents/RASD/main.tex > NUL
	del /Q /F documents\RASD\*.aux documents\RASD\*.log documents\RASD\*.out documents\RASD\*.toc documents\RASD\*.fls
	del documents\RASD\RASD.pdf
	cd documents\RASD && rename main.pdf RASD.pdf
else
	pdflatex -output-directory=documents/RASD documents/RASD/main.tex > /dev/null
	pdflatex -output-directory=documents/RASD documents/RASD/main.tex > /dev/null
	rm -f documents/RASD/*.aux documents/RASD/*.log documents/RASD/*.out documents/RASD/*.toc documents/RASD/*.fls
	mv documents/RASD/main.pdf documents/RASD/RASD.pdf
endif

dd: layout-export
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=documents/DD documents/DD/main.tex > NUL
	pdflatex -output-directory=documents/DD documents/DD/main.tex > NUL
	del /Q /F documents\DD\*.aux documents\DD\*.log documents\DD\*.out documents\DD\*.toc documents\DD\*.fls
	del documents\DD\DD.pdf
	cd documents\DD && rename main.pdf DD.pdf
else
	pdflatex -output-directory=documents/DD documents/DD/main.tex > /dev/null
	pdflatex -output-directory=documents/DD documents/DD/main.tex > /dev/null
	rm -f documents/DD/*.aux documents/DD/*.log documents/DD/*.out documents/DD/*.toc documents/DD/*.fls
	mv documents/DD/main.pdf documents/DD/DD.pdf
endif

itd: layout-export
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=documents/ITD documents/ITD/main.tex > NUL
	pdflatex -output-directory=documents/ITD documents/ITD/main.tex > NUL
	del /Q /F documents\ITD\*.aux documents\ITD\*.log documents\ITD\*.out documents\ITD\*.toc documents\ITD\*.fls
	del documents\ITD\ITD.pdf
	cd documents\ITD && rename main.pdf ITD.pdf
else
	pdflatex -output-directory=documents/ITD documents/ITD/main.tex > /dev/null
	pdflatex -output-directory=documents/ITD documents/ITD/main.tex > /dev/null
	rm -f documents/ITD/*.aux documents/ITD/*.log documents/ITD/*.out documents/ITD/*.toc documents/ITD/*.fls
	mv documents/ITD/main.pdf documents/ITD/ITD.pdf
endif

atd: layout-export
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=documents/ATD documents/ATD/main.tex > NUL
	pdflatex -output-directory=documents/ATD documents/ATD/main.tex > NUL
	del /Q /F documents\ATD\*.aux documents\ATD\*.log documents\ATD\*.out documents\ATD\*.toc documents\ATD\*.fls
	del documents\ATD\ATD.pdf
	cd documents\ATD && rename main.pdf ATD.pdf
else
	pdflatex -output-directory=documents/ATD documents/ATD/main.tex > /dev/null
	pdflatex -output-directory=documents/ATD documents/ATD/main.tex > /dev/null
	rm -f documents/ATD/*.aux documents/ATD/*.log documents/ATD/*.out documents/ATD/*.toc documents/ATD/*.fls
	mv documents/ATD/main.pdf documents/ATD/ATD.pdf
endif

rasd-verbose:
	pdflatex -output-directory=documents/RASD documents/RASD/main.tex

dd-verbose:
	pdflatex -output-directory=documents/DD documents/DD/main.tex

itd-verbose:
	pdflatex -output-directory=documents/ITD documents/ITD/main.tex

atd-verbose:
	pdflatex -output-directory=documents/ATD documents/ATD/main.tex

layout-export:
ifeq ($(OS),Windows_NT)
	@copy documents\assets\layout.tex documents\RASD\main.tex
	@copy documents\assets\layout.tex documents\DD\main.tex
	@copy documents\assets\layout.tex documents\ITD\main.tex
	@copy documents\assets\layout.tex documents\ATD\main.tex
else
	@cp documents/assets/layout.tex documents/RASD/main.tex
	@cp documents/assets/layout.tex documents/DD/main.tex
	@cp documents/assets/layout.tex documents/ITD/main.tex
	@cp documents/assets/layout.tex documents/ATD/main.tex
endif

