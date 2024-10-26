.PHONY: help clean client client-build client-run client-docker server server-build server-run server-docker rasd dd itd atd

help:
ifeq ($(OS),Windows_NT)
	@cmd /c "echo."
	@echo    make help                Print help panel
	@cmd /c "echo."
	@echo    make clean               Clean project tree from build files
	@cmd /c "echo."
	@echo    make client              Launch a clean client docker run
	@echo    make client-build        Build client app
	@echo    make client-run          Run client app
	@echo    make client-docker       Dockerize client release app
	@cmd /c "echo."
	@echo    make server              Launch a clean server docker run
	@echo    make server-build        Build server app
	@echo    make server-run          Run server app
	@echo    make server-docker       Dockerize server release app
	@cmd /c "echo."
	@echo    make rasd                Compile RASD.pdf from LaTeX
	@echo    make dd                  Compile DD.pdf from LaTeX
	@echo    make itd                 Compile ITD.pdf from LaTeX
	@echo    make atd                 Compile ATD.pdf from LaTeX
	@cmd /c "echo."
	@echo    make rasd-verbose        Run pdflatex verbosely for RASD
	@echo    make dd-verbose          Run pdflatex verbosely for DD
	@echo    make itd-verbose         Run pdflatex verbosely for ITD
	@echo    make atd-verbose         Run pdflatex verbosely for ATD
	@cmd /c "echo."
else
	@echo ""
	@echo "    make help                Print help panel"
	@echo ""
	@echo "    make clean               Clean project tree from build files"
	@echo ""
	@echo "    make client              Launch a clean client docker run"
	@echo "    make client-build        Build client app"
	@echo "    make client-run          Run client app"
	@echo "    make client-docker       Dockerize client release app"
	@echo ""
	@echo "    make server              Launch a clean server docker run"
	@echo "    make server-build        Build server app"
	@echo "    make server-run          Run server app"
	@echo "    make server-docker       Dockerize server release app"
	@echo ""
	@echo "    make rasd                Compile RASD.pdf from LaTeX"
	@echo "    make dd                  Compile DD.pdf from LaTeX"
	@echo "    make itd                 Compile ITD.pdf from LaTeX"
	@echo "    make atd                 Compile ATD.pdf from LaTeX"
	@echo ""
	@echo "    make rasd-verbose        Run pdflatex verbosely for RASD"
	@echo "    make dd-verbose          Run pdflatex verbosely for DD"
	@echo "    make itd-verbose         Run pdflatex verbosely for ITD"
	@echo "    make atd-verbose         Run pdflatex verbosely for ATD"
	@echo ""
endif

clean:
ifeq ($(OS),Windows_NT)
	del /Q /F RASD\*.aux RASD\*.log RASD\*.out RASD\*.toc RASD\*.fls
	del /Q /F DD\*.aux DD\*.log DD\*.out DD\*.toc DD\*.fls
	del /Q /F ITD\*.aux ITD\*.log ITD\*.out ITD\*.toc ITD\*.fls
	del /Q /F ATD\*.aux ATD\*.log ATD\*.out ATD\*.toc ATD\*.fls
	rmdir /S /Q apps\client\bin || rem
	rmdir /S /Q apps\client\obj || rem
	rmdir /S /Q apps\server\bin || rem
	rmdir /S /Q apps\server\obj || rem
	docker container prune -f || rem
	docker image prune -f || rem
	docker rmi sc-client -f || rem
	docker rmi sc-server -f || rem
else
	rm -f RASD/*.aux RASD/*.log RASD/*.out RASD/*.toc RASD/*.fls
	rm -f DD/*.aux DD/*.log DD/*.out DD/*.toc DD/*.fls
	rm -f ITD/*.aux ITD/*.log ITD/*.out ITD/*.toc ITD/*.fls
	rm -f ATD/*.aux ATD/*.log ATD/*.out ATD/*.toc ATD/*.fls
	rm -rf apps/client/bin
	rm -rf apps/-f client/obj
	rm -rf apps/server/bin
	rm -rf apps/server/obj
	docker container prune -f
	docker image prune -f
	docker rmi sc-client -f
	docker rmi sc-server -f
endif

client:
ifeq ($(OS),Windows_NT)
	@docker build -t sc-client ./apps/client > NUL 2>&1
	@docker run --rm -it -p 4674:80 sc-client
	@docker image prune -f > NUL
else
	@docker build -t sc-client ./apps/client > /dev/null 2>&1
	@docker run --rm -it -p 4674:80 sc-client
	@docker image prune -f > /dev/null
endif

client-build:
	dotnet build apps/client

client-run:
	@dotnet run --project apps/client

client-docker:
	docker build -t sc-client ./apps/client
	docker run --rm -it -p 4674:80 sc-client
	docker image prune -f

server:
ifeq ($(OS),Windows_NT)
	@docker build -t sc-server ./apps/server > NUL 2>&1
	@docker run --rm -it -p 4673:80 sc-server
	@docker image prune -f > NUL
else
	@docker build -t sc-server ./apps/server > /dev/null 2>&1
	@docker run --rm -it -p 4673:80 sc-server
	@docker image prune -f > /dev/null
endif

server-build:
	dotnet build apps/server

server-run:
	@dotnet run --project apps/server

server-docker:
	docker build -t sc-server ./apps/server
	docker run --rm -it -p 4673:80 sc-server
	docker image prune -f

rasd:
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=RASD RASD/main.tex > NUL
	pdflatex -output-directory=RASD RASD/main.tex > NUL
	del /Q /F RASD\*.aux RASD\*.log RASD\*.out RASD\*.toc RASD\*.fls
	del RASD\RASD.pdf
	cd RASD && rename main.pdf RASD.pdf
else
	pdflatex -output-directory=RASD RASD/main.tex > /dev/null
	pdflatex -output-directory=RASD RASD/main.tex > /dev/null
	rm -f RASD/*.aux RASD/*.log RASD/*.out RASD/*.toc RASD/*.fls
	mv RASD/main.pdf RASD/RASD.pdf
endif

dd:
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=DD DD/main.tex > NUL
	pdflatex -output-directory=DD DD/main.tex > NUL
	del /Q /F DD\*.aux DD\*.log DD\*.out DD\*.toc DD\*.fls
	del DD\DD.pdf
	cd DD && rename main.pdf DD.pdf
else
	pdflatex -output-directory=DD DD/main.tex > /dev/null
	pdflatex -output-directory=DD DD/main.tex > /dev/null
	rm -f DD/*.aux DD/*.log DD/*.out DD/*.toc DD/*.fls
	mv DD/main.pdf DD/DD.pdf
endif

itd:
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=ITD ITD/main.tex > NUL
	pdflatex -output-directory=ITD ITD/main.tex > NUL
	del /Q /F ITD\*.aux ITD\*.log ITD\*.out ITD\*.toc ITD\*.fls
	del ITD\ITD.pdf
	cd ITD && rename main.pdf ITD.pdf
else
	pdflatex -output-directory=ITD ITD/main.tex > /dev/null
	pdflatex -output-directory=ITD ITD/main.tex > /dev/null
	rm -f ITD/*.aux ITD/*.log ITD/*.out ITD/*.toc ITD/*.fls
	mv ITD/main.pdf ITD/ITD.pdf
endif

atd:
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=ATD ATD/main.tex > NUL
	pdflatex -output-directory=ATD ATD/main.tex > NUL
	del /Q /F ATD\*.aux ATD\*.log ATD\*.out ATD\*.toc ATD\*.fls
	del ATD\ATD.pdf
	cd ATD && rename main.pdf ATD.pdf
else
	pdflatex -output-directory=ATD ATD/main.tex > /dev/null
	pdflatex -output-directory=ATD ATD/main.tex > /dev/null
	rm -f ATD/*.aux ATD/*.log ATD/*.out ATD/*.toc ATD/*.fls
	mv ATD/main.pdf ATD/ATD.pdf
endif

rasd-verbose:
	pdflatex -output-directory=RASD RASD/main.tex


dd-verbose:
	pdflatex -output-directory=DD DD/main.tex


itd-verbose:
	pdflatex -output-directory=ITD ITD/main.tex


atd-verbose:
	pdflatex -output-directory=ATD ATD/main.tex
