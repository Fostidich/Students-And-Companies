# Flags:
#   -v  run pdflatex verbosely
#   -l  leave trailing pdflatex files

import os
import subprocess
import sys

latex_files = ["*.aux", "*.log", "*.out", "*.toc"]
latex_main_file = "main.tex"
latex_body_file = "body.tex"

def check_installations(program):
    try:
        subprocess.run([program, "--version"], check=True, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    except subprocess.CalledProcessError:
        print(program + " not installed")
        sys.exit(1)

def main():
    # Check that pdflatex is installed in the system
    check_installations("pdflatex")

    # Check main and body presence

    # Run verbosely (always leave trailing pdflatex files)
    # Rename pdf to dir name
    # Exit

    # Run pdflatex for first time

    # Check if successful

    # Run pdflatex for second time

    # Delete trailing files

    # Rename pdf to dir name



if __name__ == '__main__':
    main()
