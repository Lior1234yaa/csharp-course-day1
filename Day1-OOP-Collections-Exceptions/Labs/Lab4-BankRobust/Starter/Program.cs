// Day1.Lab4.Starter — A "robust" bank: exceptions, validation, debugging
// The program compiles and runs — but contains at least 8 bugs. Find and fix them (see the README).
//
// Commands (one command per line, or a file via stdin):
//   open <id> <owner>      deposit <id> <amount>     withdraw <id> <amount>
//   transfer <from> <to> <amount>     show <id>     list     quit
using Day1.Lab4;

var log = new TransactionLog("transactions.log");   // BUG? When is the file closed and flushed to disk?
var bank = new Bank(log);
int processed = 0, failed = 0;

Console.WriteLine("Bank ready. Type 'help' for commands.");
while (true)
{
    Console.Write("bank> ");
    var line = Console.ReadLine();
    if (line is null || line.Trim() == "quit") break;
    if (string.IsNullOrWhiteSpace(line)) continue;

    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var cmd = parts[0].ToLowerInvariant();

    try
    {
        switch (cmd)
        {
            case "help":
                Console.WriteLine("open <id> <owner> | deposit <id> <amount> | withdraw <id> <amount> | transfer <from> <to> <amount> | show <id> | list | quit");
                break;
            case "open":
                var acc = bank.Open(parts[1], parts[2]);
                Console.WriteLine($"opened {acc.Id} for {acc.Owner}");
                break;
            case "deposit":
                // TODO B: Replace decimal.Parse with TryParse and print a friendly message for non-numeric input
                bank.Deposit(parts[1], decimal.Parse(parts[2]));
                Console.WriteLine($"ok, balance = {bank.Get(parts[1]).Balance:N2}");
                break;
            case "withdraw":
                bank.Withdraw(parts[1], decimal.Parse(parts[2]));
                Console.WriteLine($"ok, balance = {bank.Get(parts[1]).Balance:N2}");
                break;
            case "transfer":
                bank.Transfer(parts[1], parts[2], decimal.Parse(parts[3]));
                Console.WriteLine("ok");
                break;
            case "show":
                var a = bank.Get(parts[1]);
                Console.WriteLine($"{a.Id} ({a.Owner}): {a.Balance:N2}");
                foreach (var h in a.RecentHistory(3)) Console.WriteLine("   " + h);
                break;
            case "list":
                foreach (var x in bank.Accounts) Console.WriteLine($"{x.Id} ({x.Owner}): {x.Balance:N2}");
                break;
            default:
                Console.WriteLine($"unknown command '{cmd}'");
                break;
        }
        processed++;
    }
    catch (BankException ex)
    {
        Console.WriteLine($"error: {ex.Message}");
        failed++;
    }
    // TODO C: What happens with IndexOutOfRangeException when a parameter is missing? And with FormatException? Add handling.
}

Console.WriteLine($"processed={processed}, failed={failed}, log lines={log.Lines}");
