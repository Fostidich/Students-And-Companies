help:
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

clean:
ifeq ($(OS),Windows_NT)
	del /Q /F RASD\*.aux RASD\*.log RASD\*.out RASD\*.toc
	del /Q /F DD\*.aux DD\*.log DD\*.out DD\*.toc
	del /Q /F ITD\*.aux ITD\*.log ITD\*.out ITD\*.toc
	del /Q /F ATD\*.aux ATD\*.log ATD\*.out ATD\*.toc
	rmdir /S /Q apps\client\bin
	rmdir /S /Q apps\client\obj
	rmdir /S /Q apps\server\bin
	rmdir /S /Q apps\server\obj
	docker container prune -f
	docker image prune -f
else
	rm -f RASD/*.aux RASD/*.log RASD/*.out RASD/*.toc
	rm -f DD/*.aux DD/*.log DD/*.out DD/*.toc
	rm -f ITD/*.aux ITD/*.log ITD/*.out ITD/*.toc
	rm -f ATD/*.aux ATD/*.log ATD/*.out ATD/*.toc
	rm -rf apps/client/bin
	rm -rf apps/client/obj
	rm -rf apps/server/bin
	rm -rf apps/server/obj
	docker container prune -f
	docker image prune -f
endif

client:
ifeq ($(OS),Windows_NT)
	@docker build -t sc-client ./apps/client > NUL 2>&1
	@docker run --rm -it -p 5001:80 sc-client
	@docker image prune -f > NUL
else
	@docker build -t sc-client ./apps/client > /dev/null 2>&1
	@docker run --rm -it -p 5001:80 sc-client
	@docker image prune -f > /dev/null
endif

client-build:
	dotnet build apps/client

client-run:
	@dotnet run --project apps/client

client-docker:
	docker build -t sc-client ./apps/client
	docker run --rm -it -p 5001:80 sc-client
	docker image prune -f

server:
ifeq ($(OS),Windows_NT)
	@docker build -t sc-server ./apps/server > NUL 2>&1
	@docker run --rm -it -p 5000:80 sc-server
	@docker image prune -f > NUL
else
	@docker build -t sc-server ./apps/server > /dev/null 2>&1
	@docker run --rm -it -p 5000:80 sc-server
	@docker image prune -f > /dev/null
endif

server-build:
	dotnet build apps/server

server-run:
	@dotnet run --project apps/server

server-docker:
	docker build -t sc-server ./apps/server
	docker run --rm -it -p 5000:80 sc-server
	docker image prune -f

rasd:
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=RASD RASD/main.tex > NUL
	pdflatex -output-directory=RASD RASD/main.tex > NUL
	del /Q RASD\*.aux RASD\*.log RASD\*.out RASD\*.toc
	move RASD\main.pdf RASD\RASD.pdf
else
	pdflatex -output-directory=RASD RASD/main.tex > /dev/null
	pdflatex -output-directory=RASD RASD/main.tex > /dev/null
	rm RASD/*.aux RASD/*.log RASD/*.out RASD/*.toc
	mv RASD/main.pdf RASD/RASD.pdf
endif

dd:
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=DD DD/main.tex > NUL
	pdflatex -output-directory=DD DD/main.tex > NUL
	del /Q DD\*.aux DD\*.log DD\*.out DD\*.toc
	move DD\main.pdf DD\DD.pdf
else
	pdflatex -output-directory=DD DD/main.tex > /dev/null
	pdflatex -output-directory=DD DD/main.tex > /dev/null
	rm DD/*.aux DD/*.log DD/*.out DD/*.toc
	mv DD/main.pdf DD/DD.pdf
endif

itd:
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=ITD ITD/main.tex > NUL
	pdflatex -output-directory=ITD ITD/main.tex > NUL
	del /Q ITD\*.aux ITD\*.log ITD\*.out ITD\*.toc
	move ITD\main.pdf ITD\ITD.pdf
else
	pdflatex -output-directory=ITD ITD/main.tex > /dev/null
	pdflatex -output-directory=ITD ITD/main.tex > /dev/null
	rm ITD/*.aux ITD/*.log ITD/*.out ITD/*.toc
	mv ITD/main.pdf ITD/ITD.pdf
endif

atd:
ifeq ($(OS),Windows_NT)
	pdflatex -output-directory=ATD ATD/main.tex > NUL
	pdflatex -output-directory=ATD ATD/main.tex > NUL
	del /Q ATD\*.aux ATD\*.log ATD\*.out ATD\*.toc
	move ATD\main.pdf ATD\ATD.pdf
else
	pdflatex -output-directory=ATD ATD/main.tex > /dev/null
	pdflatex -output-directory=ATD ATD/main.tex > /dev/null
	rm ATD/*.aux ATD/*.log ATD/*.out ATD/*.toc
	mv ATD/main.pdf ATD/ATD.pdf
endif