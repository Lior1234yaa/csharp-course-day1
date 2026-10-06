# C# Programming in the .NET Framework — Day 1

Course materials for the first day of the course: **object-oriented programming, collections and exception handling**, in C# 13 / .NET 10.

## Before Day 1

1. Install the tools following [`00-Setup/INSTALL.md`](00-Setup/INSTALL.md): .NET 10 SDK, Visual Studio 2022 or VS Code with the C# Dev Kit extension, and Git.
2. Clone the repository:

```bash
git clone https://github.com/Lior1234yaa/csharp-course-day1.git
```

3. Check that your environment is ready:

```bash
cd csharp-course-day1/00-Setup/VerifySetup
dotnet run
```

If `dotnet` is not recognized in the VS Code terminal after installation: close VS Code completely (`File` → `Exit`) and open it again.

## What's in the repository

```text
00-Setup/
  INSTALL.md                     Installation guide
  CHEATSHEET.md                  Quick C# reference sheet
  VerifySetup/                   Environment check (dotnet run)
Day1-OOP-Collections-Exceptions/
  README.md                      Schedule, goals, links
  Notes/                         Study materials, one file per module
  Slides/Day1.pptx               The slide deck
  Demos/                         The demos from class (dotnet run)
  Exercises/README.md            14 short exercises
  Labs/LabN-<Name>/              Lab: README.md (instructions) + Starter/ (starter project)
```

Start with [`Day1-OOP-Collections-Exceptions/README.md`](Day1-OOP-Collections-Exceptions/README.md).

## How to work

- **Notes** are the course "textbook". Read them after each module, especially the "Common mistakes" section at the end of each file.
- **Demos** are the code shown in class. You can run and modify them: `cd Demos/<Project>` and then `dotnet run`.
- **Labs**: copy the `Starter` folder to a folder of your own and work on the copy. Detailed instructions are in each lab's README.
