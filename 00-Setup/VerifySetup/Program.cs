// Setup.VerifySetup — checks your work environment for the C# / .NET course
// Run: dotnet run
//
// The program checks that the computer is ready for the course: .NET version, operating system,
// installed SDK, internet access (NuGet), and two small sanity checks (JSON and async).

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

// On Windows, the console window doesn't always display Unicode/emoji by default
try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { /* not critical */ }

var results = new List<CheckResult>();

Console.WriteLine("=====================================================");
Console.WriteLine("  Environment check — C# / .NET course (4 days)");
Console.WriteLine("=====================================================");
Console.WriteLine();

// ---------- 1. .NET version ----------
Console.WriteLine("[1] .NET version");
Console.WriteLine($"    Environment.Version      : {Environment.Version}");
Console.WriteLine($"    FrameworkDescription     : {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"    RuntimeIdentifier        : {RuntimeInformation.RuntimeIdentifier}");
bool runtimeOk = Environment.Version.Major >= 10;
results.Add(new(".NET 10+ runtime", runtimeOk,
    runtimeOk ? $"Running on .NET {Environment.Version.Major}" : $"Found version {Environment.Version} — 10 or later is required"));
Console.WriteLine();

// ---------- 2. Operating system ----------
Console.WriteLine("[2] Operating system");
Console.WriteLine($"    OSDescription            : {RuntimeInformation.OSDescription}");
Console.WriteLine($"    OSArchitecture           : {RuntimeInformation.OSArchitecture}");
bool isWindows = OperatingSystem.IsWindows();
Console.WriteLine($"    Windows?                 : {(isWindows ? "Yes" : "No")}");
results.Add(new("Operating system detected", true, RuntimeInformation.OSDescription));
results.Add(new("Windows (required for WPF on Day 3)", isWindows,
    isWindows
        ? "WPF and WinForms can run"
        : "Days 1, 2 and 4 will work; Day 3 (WPF) needs Windows or a virtual machine",
    Mandatory: false));
Console.WriteLine();

// ---------- 3. dotnet SDK installed ----------
Console.WriteLine("[3] dotnet SDK (dotnet --list-sdks)");
var (sdkOk, sdkDetail) = CheckDotnetSdk();
Console.WriteLine($"    {sdkDetail}");
results.Add(new("dotnet SDK 10.x installed", sdkOk, sdkDetail));
Console.WriteLine();

// ---------- 4. Internet access (NuGet) ----------
Console.WriteLine("[4] Access to api.nuget.org (5-second timeout)");
var (netOk, netDetail) = await CheckNuGetAsync();
Console.WriteLine($"    {netDetail}");
results.Add(new("Access to NuGet (for downloading packages)", netOk, netDetail, Mandatory: false));
Console.WriteLine();

// ---------- 5. System.Text.Json ----------
Console.WriteLine("[5] System.Text.Json check (serialize -> deserialize)");
var (jsonOk, jsonDetail) = CheckJsonRoundTrip();
Console.WriteLine($"    {jsonDetail}");
results.Add(new("System.Text.Json works", jsonOk, jsonDetail));
Console.WriteLine();

// ---------- 6. async/await ----------
Console.WriteLine("[6] async/await check (Task.WhenAll)");
var (asyncOk, asyncDetail) = await CheckAsyncAsync();
Console.WriteLine($"    {asyncDetail}");
results.Add(new("async/await works", asyncOk, asyncDetail));
Console.WriteLine();

// ---------- Summary ----------
Console.WriteLine("=====================================================");
Console.WriteLine("  Summary");
Console.WriteLine("=====================================================");
foreach (var r in results)
{
    string icon = r.Passed ? "✅" : r.Mandatory ? "❌" : "⚠️";
    Console.WriteLine($"  {icon} {r.Name}");
    if (!r.Passed)
        Console.WriteLine($"       -> {r.Detail}");
}
Console.WriteLine();

// "Mandatory" checks: runtime + SDK + JSON + async. Internet and Windows are warnings only.
bool[] mandatory = [runtimeOk, sdkOk, jsonOk, asyncOk];
int failedMandatory = mandatory.Count(ok => !ok);

if (failedMandatory == 0 && netOk && isWindows)
{
    Console.WriteLine("✅ All set! Your environment is ready for all four days of the course.");
}
else if (failedMandatory == 0)
{
    Console.WriteLine("✅ The basics are OK. Note the warnings above:");
    if (!netOk) Console.WriteLine("   - Without access to NuGet you won't be able to add packages (see INSTALL.md, Troubleshooting section).");
    if (!isWindows) Console.WriteLine("   - Day 3 (WPF) requires Windows.");
}
else
{
    Console.WriteLine($"❌ {failedMandatory} mandatory checks failed. Go through INSTALL.md and fix them before the course starts.");
}

return failedMandatory == 0 ? 0 : 1;

// =====================================================================
// Helper functions
// =====================================================================

static (bool ok, string detail) CheckDotnetSdk()
{
    try
    {
        var psi = new ProcessStartInfo("dotnet", "--list-sdks")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        using var process = Process.Start(psi);
        if (process is null)
            return (false, "Could not run the dotnet command — is it in the PATH?");

        string output = process.StandardOutput.ReadToEnd();
        if (!process.WaitForExit(10_000))
        {
            try { process.Kill(); } catch { }
            return (false, "The command dotnet --list-sdks did not finish in time");
        }

        var sdks = output
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        if (sdks.Count == 0)
            return (false, "No installed SDKs found (Runtime only?). Install the .NET 10 SDK.");

        bool has10 = sdks.Any(s => s.StartsWith("10.", StringComparison.Ordinal));
        string list = string.Join(", ", sdks.Select(s => s.Split(' ')[0]));
        return has10
            ? (true, $"SDK 10 found (installed: {list})")
            : (false, $"SDK 10 not found (installed: {list})");
    }
    catch (Exception ex)
    {
        return (false, $"Error running dotnet: {ex.GetType().Name} — {ex.Message}");
    }
}

static async Task<(bool ok, string detail)> CheckNuGetAsync()
{
    try
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
        var sw = Stopwatch.StartNew();
        using var response = await http.GetAsync("https://api.nuget.org/v3/index.json");
        sw.Stop();
        return response.IsSuccessStatusCode
            ? (true, $"HTTP {(int)response.StatusCode} in {sw.ElapsedMilliseconds} ms")
            : (false, $"Unexpected response: HTTP {(int)response.StatusCode}");
    }
    catch (TaskCanceledException)
    {
        return (false, "timeout — no response within 5 seconds (no internet? corporate proxy?)");
    }
    catch (HttpRequestException ex)
    {
        return (false, $"Network error: {ex.Message}");
    }
    catch (Exception ex)
    {
        return (false, $"Unexpected error: {ex.GetType().Name} — {ex.Message}");
    }
}

static (bool ok, string detail) CheckJsonRoundTrip()
{
    try
    {
        var original = new Student("Dana", 3, ["OOP", "Async", "WPF", "AI"]);
        var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        string json = JsonSerializer.Serialize(original, options);
        var copy = JsonSerializer.Deserialize<Student>(json, options);

        bool ok = copy is not null
                  && copy.Name == original.Name
                  && copy.Day == original.Day
                  && copy.Topics.SequenceEqual(original.Topics);

        return ok
            ? (true, $"round-trip OK: {json}")
            : (false, "The object after deserialize differs from the original");
    }
    catch (Exception ex)
    {
        return (false, $"Error: {ex.GetType().Name} — {ex.Message}");
    }
}

static async Task<(bool ok, string detail)> CheckAsyncAsync()
{
    try
    {
        var sw = Stopwatch.StartNew();
        // Three "concurrent" tasks of 100ms — they should finish together in about 100ms, not 300ms
        var tasks = Enumerable.Range(1, 3)
            .Select(async i => { await Task.Delay(100); return i * 10; });
        int[] values = await Task.WhenAll(tasks);
        sw.Stop();

        int sum = values.Sum();
        bool ok = sum == 60 && sw.ElapsedMilliseconds < 1000;
        return ok
            ? (true, $"3 tasks finished concurrently in {sw.ElapsedMilliseconds} ms (sum = {sum})")
            : (false, $"Unexpected result: sum = {sum}, time = {sw.ElapsedMilliseconds} ms");
    }
    catch (Exception ex)
    {
        return (false, $"Error: {ex.GetType().Name} — {ex.Message}");
    }
}

// =====================================================================
// Types
// =====================================================================

record CheckResult(string Name, bool Passed, string Detail, bool Mandatory = true);

record Student(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("day")] int Day,
    [property: JsonPropertyName("topics")] string[] Topics);
