.PHONY: help clean client client-build client-run client-docker server server-build server-run server-docker rasd dd itd atd rasd-verbose dd-verbose itd-verbose atd-verbose layout-export

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
    @echo    make layout-export       Copy main LaTeX layout to all documents
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
	@echo "    make layout-export       Copy main LaTeX layout to all documents"
	@echo ""
endif

clean:
ifeq ($(OS),Windows_NT)
	del /Q /F documents\RASD\*.aux documents\RASD\*.log documents\RASD\*.out documents\RASD\*.toc documents\RASD\*.fls
	del /Q /F documents\DD\*.aux documents\DD\*.log documents\DD\*.out documents\DD\*.toc documents\DD\*.fls
	del /Q /F documents\ITD\*.aux documents\ITD\*.log documents\ITD\*.out documents\ITD\*.toc documents\ITD\*.fls
	del /Q /F documents\ATD\*.aux documents\ATD\*.log documents\ATD\*.out documents\ATD\*.toc documents\ATD\*.fls
	rmdir /S /Q apps\client\bin || rem
	rmdir /S /Q apps\client\obj || rem
	rmdir /S /Q apps\server\bin || rem
	rmdir /S /Q apps\server\obj || rem
	docker container prune -f || rem
	docker image prune -f || rem
	docker rmi sc-client -f || rem
	docker rmi sc-server -f || rem
else
	rm -f documents/RASD/*.aux documents/RASD/*.log documents/RASD/*.out documents/RASD/*.toc documents/RASD/*.fls
	rm -f documents/DD/*.aux documents/DD/*.log documents/DD/*.out documents/DD/*.toc documents/DD/*.fls
	rm -f documents/ITD/*.aux documents/ITD/*.log documents/ITD/*.out documents/ITD/*.toc documents/ITD/*.fls
	rm -f documents/ATD/*.aux documents/ATD/*.log documents/ATD/*.out documents/ATD/*.toc documents/ATD/*.fls
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
	@copy assets\layout.tex documents\RASD\main.tex
	@copy assets\layout.tex documents\DD\main.tex
	@copy assets\layout.tex documents\ITD\main.tex
	@copy assets\layout.tex documents\ATD\main.tex
else
	@cp assets/layout.tex documents/RASD/main.tex
	@cp assets/layout.tex documents/DD/main.tex
	@cp assets/layout.tex documents/ITD/main.tex
	@cp assets/layout.tex documents/ATD/main.tex
endif
