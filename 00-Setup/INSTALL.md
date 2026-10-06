# What to Install — Setup Guide for the C# / .NET Course

This document takes you from zero to a working environment ready for all four days of the course. The guide is written **Windows-first** because on Day 3 we build WPF applications (which run only on Windows), but on Days 1, 2 and 4 you can also work on macOS or Linux — there are relevant notes along the way.

We recommend finishing the installation **before** Day 1 and running the verification program (section 10). If you're coming with a company laptop, check installation permissions and proxy settings in advance (see "Troubleshooting").

## 1. Quick overview — what each day needs

| Day | Topic | Required | Recommended |
|-----|------|------|-------|
| 1 | OOP, Collections, Exceptions | .NET 10 SDK, Visual Studio or VS Code, Git | Windows Terminal, LINQPad |
| 2 | Multithreading, Async, REST + JSON | All of the above | Postman / REST Client, `curl` |
| 3 | WPF (and comparison with WinForms) | **Windows + Visual Studio** with the .NET desktop workload | — |
| 4 | AI-assisted development, unit tests and maintainable code (DI) | Accounts for AI tools (Copilot / Claude / ChatGPT), Node.js LTS | Cursor |

## 2. .NET 10 SDK (LTS)

The SDK is the foundation for everything: the compiler, the CLI (`dotnet`) and the runtime.

1. Go to https://dotnet.microsoft.com/download
2. Choose **.NET 10** (an LTS release — long-term support) and download the **SDK** (not just the Runtime!) for your operating system.
   - Windows: x64 Installer (on most computers). If your computer has an ARM processor, choose Arm64.
   - macOS: choose Arm64 for Apple Silicon computers (M1 and later) or x64 for Intel computers.
   - Linux: preferably install through your distribution's package manager (instructions per distribution are on the download page).
3. Run the installer, then **open a new terminal** (a terminal that was open before the installation doesn't know the updated PATH).

Verification:

```bash
dotnet --version
dotnet --info
```

The output of `dotnet --version` should start with `10.` (for example `10.0.100`). `dotnet --info` shows all installed SDKs and runtimes and the operating system — very useful for troubleshooting.

Note: if you install Visual Studio (section 3) with the .NET workload, the SDK is installed along with it. It's still a good idea to run `dotnet --version` and make sure the version is 10.

## 3. Visual Studio Community (Windows)

Visual Studio is Microsoft's full IDE for Windows, and it is **required for Day 3** because of the WPF designer (XAML Designer). The Community edition is free for personal use, learning and small businesses.

1. Download the installer from https://visualstudio.microsoft.com/ — choose **Community** and the **latest** version available (2022 or newer; this guide doesn't commit to an exact version number).
2. Run the **Visual Studio Installer**. On the "Workloads" screen, select:
   - **.NET desktop development** — required. Includes WPF, Windows Forms, console, and the .NET SDK.
   - **ASP.NET and web development** — optional. Useful for Day 2 if you want to run a local Web API of your own.
3. On the **Individual components** tab, make sure ".NET 10 SDK" is selected (it's usually selected automatically with the workload). If the 10 SDK doesn't appear in the installer, install it separately as in section 2; Visual Studio detects an SDK that was installed separately.
4. Click Install. The installation takes several GB and some time — don't leave it for the day of the course.

How do you choose workloads? A workload is a "capability bundle" — a set of components installed together. You can always add or remove workloads later via Visual Studio Installer -> Modify. There's no need to install everything; the two workloads above are enough for the course.

Where to find what we'll need on Day 3:
- **XAML Designer**: opening a `.xaml` file in a WPF project shows a split window — a visual view on top and XAML below (Split View). Use View -> Toolbox to drag controls.
- **XAML Hot Reload**: works automatically when you run with Debug (F5) — changes to the XAML update in the running window without restarting. If it doesn't work, check Tools -> Options -> Debugging -> Hot Reload.
- **Live Visual Tree / Live Property Explorer**: Debug -> Windows — explore the control tree at runtime.

If you work on macOS: Visual Studio for Mac has been discontinued, and there is no replacement for the WPF Designer outside Windows. For Day 3 you'll need a Windows machine (physical, a VM such as Parallels/VMware/VirtualBox, or Windows in the cloud). Days 1, 2 and 4 work great with VS Code.

## 4. Alternative: Visual Studio Code

VS Code is a lightweight, free, cross-platform editor (Windows / macOS / Linux). It is **enough for Days 1, 2 and 4** (console projects, tests, HTTP, AI tools). It is **not enough for Day 3** — it has no visual WPF designer, and WPF itself runs only on Windows.

1. Download from https://code.visualstudio.com/
2. Install the extensions (Extensions, shortcut `Ctrl+Shift+X`):
   - **C# Dev Kit** (by Microsoft) — a full development experience: Solution Explorer, running tests, project templates. It also automatically installs the **C#** extension (language service, IntelliSense, debugging).
   - **REST Client** (by Huachao Mao) — for Day 2, sending HTTP requests from a `.http` file (see section 6).
3. Open a project folder (File -> Open Folder) and press F5 to run with the debugger. The first time, VS Code will ask which project to use.

Tip: even if you install Visual Studio, it's worth installing VS Code as well. On Day 4 we'll work with GitHub Copilot and Claude Code, and both integrate well with VS Code.

## 5. Git and GitHub

1. **Git**: on Windows, download **Git for Windows** from https://git-scm.com/download/win and install with the defaults (including Git Bash). On macOS: `xcode-select --install` or via Homebrew (`brew install git`). On Linux: `sudo apt install git` (or your distribution's equivalent).
2. **GitHub account**: create a free account at https://github.com/ — we'll also need it for GitHub Copilot on Day 4.
3. One-time identity setup:

```bash
git config --global user.name "Your Name"
git config --global user.email "you@example.com"
```

4. Clone the course repository — the exact link will be given by the instructor:

```bash
git clone <course-repo-url>
cd <course-repo-folder>
```

Verification: `git --version` prints a version.

## 6. HTTP tools for Day 2

On Day 2 we'll call REST APIs and work with JSON. You need a tool for sending requests and inspecting responses:

- **Postman** (https://www.postman.com/downloads/) — a full graphical interface. The free account is enough.
- **REST Client** (a VS Code extension) — you write requests in a text file and click "Send Request". Light, fast, and saved in Git together with the code. Example `requests.http` file:

```text
GET https://api.github.com/repos/dotnet/runtime
Accept: application/json
```

- **curl** — a command-line tool. Built into Windows 10/11, macOS and Linux:

```bash
curl -s https://api.github.com/repos/dotnet/runtime
```

Note: in PowerShell, `curl` is sometimes an alias for `Invoke-WebRequest`, which has a different syntax. Type `curl.exe` to run the real curl.

- **Windows Terminal** (recommended) — a modern terminal with tabs and good UTF-8 and Unicode support. Installed by default on Windows 11; on Windows 10, install it from the Microsoft Store.

## 7. AI tools for Day 4 — create accounts in advance

On Day 4 we'll work hands-on with AI assistants. Creating accounts and verifying payment methods takes time — do it **before** the course. Some tools are paid or have a trial period; **check the current pricing on each tool's website**, it changes.

| Tool | What it is | How to install |
|-----|-------|-------------|
| **GitHub Copilot** | Code completion and chat inside the IDE | Requires a GitHub account with a Copilot subscription (there's a limited free plan and paid plans — see https://github.com/features/copilot). In Visual Studio: built into recent versions; sign in with your GitHub account. In VS Code: the "GitHub Copilot" + "GitHub Copilot Chat" extensions. |
| **Claude** | Chat with Anthropic's model in the browser | An account at https://claude.ai |
| **Claude Code** | A coding agent in the terminal that works on your repository | Requires Node.js (section 8). Install: `npm install -g @anthropic-ai/claude-code`, then run `claude` in the project folder. Up-to-date info: https://docs.anthropic.com/en/docs/claude-code |
| **ChatGPT** | OpenAI's chat in the browser | An account at https://chatgpt.com |
| **Cursor** (optional) | A VS Code–based code editor with built-in AI | https://cursor.com — has a trial period; not required for the course |

Verification: sign in to each tool once and send a short question, to make sure the account is active.

## 8. Node.js LTS

In this course, Node.js is needed to install **Claude Code** (distributed via npm), on Day 4.

1. Download the **LTS** version from https://nodejs.org/ and install with the defaults.
2. Open a new terminal and check:

```bash
node --version
npm --version
```

3. Install Claude Code:

```bash
npm install -g @anthropic-ai/claude-code
claude --version
```

## 9. Optional but useful

### LINQPad
https://www.linqpad.net/ — a "notebook" for C#: write an expression or a code snippet and see the result immediately, without creating a project. Great for experimenting with LINQ (Day 1) and async (Day 2). A free edition is available (Windows only).

### dotnet tools
Global tools installed through the CLI. Useful for Day 2 (monitoring threads and the GC):

```bash
dotnet tool install --global dotnet-counters
dotnet-counters --version
```

### .NET CLI — commands worth knowing

```bash
dotnet new list                       # list available templates
dotnet new console -n MyApp           # new console project in folder MyApp
dotnet new xunit -n MyApp.Tests       # test project
dotnet run                            # build + run the project in the current folder
dotnet run -- arg1 arg2               # pass arguments to the program
dotnet build                          # build only
dotnet test                           # run tests
dotnet add package Newtonsoft.Json    # add a NuGet package
dotnet add reference ../Lib/Lib.csproj # reference another project
dotnet format                         # format code according to .editorconfig
dotnet --list-sdks                    # which SDKs are installed
dotnet --list-runtimes                # which runtimes are installed
```

## 10. Verifying the installation

### The verification program
The `00-Setup/VerifySetup` folder contains a small console project that checks your environment. Run:

```bash
cd 00-Setup/VerifySetup
dotnet run
```

The program prints the .NET version, the operating system, whether SDK 10 is installed, whether NuGet is reachable, and runs a JSON check and an async check. At the end there's a summary with ✅ / ⚠️ / ❌. A ⚠️ on Windows or NuGet is only a warning (for example on macOS); ❌ means something needs fixing.

### Checklist

| Tool | Command to check | Expected result |
|-----|--------------|-------------|
| .NET SDK | `dotnet --version` | `10.x.y` |
| Installed SDKs | `dotnet --list-sdks` | A line starting with `10.` |
| Create and run | `dotnet new console -n Hello && cd Hello && dotnet run` | `Hello, World!` |
| Visual Studio | Open VS -> Create a new project -> search for "WPF Application" | The template appears (Windows only) |
| VS Code + C# Dev Kit | Open a project folder, F5 | The program runs with the debugger |
| Git | `git --version` | `git version 2.x` |
| GitHub | Sign in at https://github.com | Your account's home page |
| curl | `curl.exe --version` (Windows) / `curl --version` | A version is printed |
| Postman / REST Client | Send `GET https://api.github.com` | A JSON response with status 200 |
| Node.js | `node --version` | `v22.x` or a newer LTS |
| npm | `npm --version` | A version number |
| Claude Code | `claude --version` | A version number |
| GitHub Copilot | Open a `.cs` file in the IDE | Gray completion suggestions |
| Claude / ChatGPT | Sign in in the browser | An active chat |
| Verification program | `cd 00-Setup/VerifySetup && dotnet run` | "✅ All set" (or only ⚠️) |

## 11. Troubleshooting

### `dotnet` is not recognized as a command ('dotnet' is not recognized)
- Close and reopen the terminal (and VS Code / Visual Studio) — environment variables are only loaded into new processes.
- On Windows, check that `C:\Program Files\dotnet\` is in the PATH: Settings -> System -> About -> Advanced system settings -> Environment Variables.
- On macOS/Linux, make sure `/usr/local/share/dotnet` (or `/usr/share/dotnet`, depending on the installation) is in the PATH, for example in `~/.zshrc` or `~/.bashrc`.

### Several SDKs are installed and the project uses the wrong version
The CLI picks the **newest** SDK unless there's a `global.json` file in the folder or in one of its parent folders. Check with `dotnet --version` inside the project folder. To pin a version (for example if you also have an 11 preview):

```json
{
  "sdk": {
    "version": "10.0.100",
    "rollForward": "latestFeature"
  }
}
```

You can create it automatically with `dotnet new globaljson --sdk-version 10.0.100`. If you get the error "The specified SDK version could not be found", either delete `global.json` or install the version it requires.

### NuGet restore fails behind a corporate proxy
Symptoms: `error NU1301: Unable to load the service index for source https://api.nuget.org/v3/index.json`.
- Set the proxy as environment variables: `HTTPS_PROXY=http://proxy.company.com:8080` (and `HTTP_PROXY` too if needed). NuGet and the CLI respect them.
- Alternatively, in the `NuGet.Config` file (on Windows: `%AppData%\NuGet\NuGet.Config`) add:

```xml
<configuration>
  <config>
    <add key="http_proxy" value="http://proxy.company.com:8080" />
  </config>
</configuration>
```

- Many organizations replace TLS certificates (SSL inspection). If you get a "certificate" error, ask IT for the corporate CA certificate and install it in the system certificate store.
- If you have an internal NuGet feed (Artifactory / Azure Artifacts), ask IT for the URL and add it: `dotnet nuget add source <url> -n corp`.

### `EnableWindowsTargeting` — building a WPF project on a non-Windows machine
A project with `<TargetFramework>net10.0-windows</TargetFramework>` fails on macOS/Linux with NETSDK1100. Adding `<EnableWindowsTargeting>true</EnableWindowsTargeting>` to the `.csproj` lets you **build** (compile) it there too — but not run it; WPF runs only on Windows. The course's WPF projects already include this setting, so you can at least read and compile them on any system.

### HTTPS dev certificate — security warnings when running a local Web API
If on Day 2 you run a local Web API (`dotnet new webapi`) and the browser/client complains about an untrusted certificate:

```bash
dotnet dev-certs https --trust
```

On Windows and macOS a confirmation prompt appears. On Linux the command creates the certificate but doesn't always add it to the trusted store — see the instructions in the output. Alternatively, work against the `http://` address listed in `launchSettings.json`.

### PowerShell: "running scripts is disabled on this system"
This happens when you run a `.ps1` script (for example an installation script or `dotnet-install.ps1`). A fix for the current user only (doesn't require admin):

```text
Set-ExecutionPolicy -Scope CurrentUser RemoteSigned
```

Alternatively, run the single script with `powershell -ExecutionPolicy Bypass -File script.ps1`.

### Non-English text or emoji show up as question marks in the console
Use Windows Terminal (not the old cmd), and choose a font that supports Unicode (for example Cascadia Code). In code: `Console.OutputEncoding = Encoding.UTF8;` (the verification program already does this).

### Visual Studio doesn't offer the "WPF Application" template
The ".NET desktop development" workload isn't installed. Open Visual Studio Installer -> Modify -> select the workload -> Modify.

### Still stuck?
- Run `dotnet --info` and send the output to the instructor (in the course group or by email) together with the full error message.
- Arrive 20 minutes early on Day 1 — we'll use that time to solve installation problems.
