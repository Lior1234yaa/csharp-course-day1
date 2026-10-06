// =====================================================================
// Day1.Demo.Quickstart — C# warm-up
// What the demo shows:
//   * top-level statements (no explicit Main)
//   * value types vs reference types (struct/int vs class/array)
//   * var, strings and interpolation, control flow (if / switch expression / loops)
//   * methods with optional parameters, out and tuples
//   * nullable reference types (string?) and the operators ?. ?? !
// Run:  dotnet run
// =====================================================================

Console.WriteLine("=== 1. Variables, var and interpolation ===");
int age = 30;                    // value type
double price = 19.90;
string name = "Dana";            // reference type (but immutable)
var isActive = true;             // the compiler infers bool
Console.WriteLine($"{name} is {age} years old, price={price:F2}, active={isActive}");
Console.WriteLine($"Rounded: {Math.Round(price)}  |  Upper: {name.ToUpper()}  |  Len: {name.Length}");

Console.WriteLine("\n=== 2. Value vs reference ===");
int a = 5;
int b = a;          // copies the value
b++;
Console.WriteLine($"a={a}, b={b}   (int is a value type: changing b did not affect a)");

int[] arr1 = { 1, 2, 3 };
int[] arr2 = arr1;  // copies the reference — both names point to the same array
arr2[0] = 99;
Console.WriteLine($"arr1[0]={arr1[0]}   (an array is a reference type: the change is visible through both references)");

Point p1 = new(1, 2);
Point p2 = p1;      // struct → copy
p2.X = 100;
Console.WriteLine($"p1.X={p1.X}, p2.X={p2.X}   (struct is copied)");

Console.WriteLine("\n=== 3. Control flow ===");
for (int i = 1; i <= 3; i++) Console.Write($"{i} ");
Console.WriteLine();

string[] days = ["Sun", "Mon", "Tue"];
foreach (var d in days) Console.Write($"{d} ");
Console.WriteLine();

int n = 0;
while (n < 3) { n++; }
Console.WriteLine($"after while: n={n}");

string Grade(int score) => score switch
{
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    _ => "F",
};
Console.WriteLine($"Grade(85) = {Grade(85)}, Grade(50) = {Grade(50)}");

Console.WriteLine("\n=== 4. Methods ===");
Console.WriteLine(Add(2, 3));
Console.WriteLine(Greet("Yossi"));
Console.WriteLine(Greet("Yossi", "Shalom"));
if (TryDivide(10, 2, out double result)) Console.WriteLine($"10/2 = {result}");
if (!TryDivide(10, 0, out _)) Console.WriteLine("10/0 — division by zero is not allowed");
var (min, max) = MinMax([4, 9, 1, 7]);
Console.WriteLine($"min={min}, max={max}");

Console.WriteLine("\n=== 5. Nullable reference types ===");
string? maybeName = FindUser(1);   // can be null
Console.WriteLine($"Length via ?. : {maybeName?.Length}");
Console.WriteLine($"With ?? : {maybeName ?? "(unknown)"}");
string? found = FindUser(0);
Console.WriteLine($"found = {found ?? "null"}");
if (found is not null)
    Console.WriteLine($"found has {found.Length} chars"); // the compiler knows found is not null here

// ---------- Helper methods (file-level local functions) ----------
static int Add(int x, int y) => x + y;

static string Greet(string who, string greeting = "Hello") => $"{greeting}, {who}!";

static bool TryDivide(double x, double y, out double quotient)
{
    if (y == 0) { quotient = 0; return false; }
    quotient = x / y;
    return true;
}

static (int Min, int Max) MinMax(int[] values) => (values.Min(), values.Max());

static string? FindUser(int id) => id == 0 ? "admin" : null;

// struct — a value type we defined ourselves
struct Point
{
    public int X;
    public int Y;
    public Point(int x, int y) { X = x; Y = y; }
}
