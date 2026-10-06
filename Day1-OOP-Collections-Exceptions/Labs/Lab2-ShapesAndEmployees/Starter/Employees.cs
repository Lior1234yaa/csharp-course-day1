namespace Day1.Lab2;

// ============ Part B — Employees and payroll (~45 minutes) ============

// TODO 4: An IPayable interface with:
//   - string PayeeName { get; }
//   - decimal CalculateMonthlyPay();
//   - default member: string PaySlip() => $"{PayeeName,-24} {CalculateMonthlyPay(),12:N2}";
public interface IPayable
{
}

// TODO 5: An abstract Employee class that implements IPayable:
//   - Id (int), Name (string) — read-only, set in the constructor
//   - PayeeName returns Name
//   - CalculateMonthlyPay is abstract
//   - virtual string Describe() => $"#{Id} {Name} ({GetType().Name})"
public abstract class Employee
{
}

// TODO 6: SalariedEmployee(id, name, annualSalary) — monthly pay = annualSalary / 12
public class SalariedEmployee
{
}

// TODO 7: HourlyEmployee(id, name, hourlyRate, hoursWorked):
//   Up to 160 hours — regular rate; every hour beyond that — 1.5x.
//   Describe() overrides and appends "  hours=..." at the end (use base.Describe()).
public class HourlyEmployee
{
}

// TODO 8: Manager inherits from SalariedEmployee and adds:
//   - List<Employee> Reports (direct reports)
//   - Bonus (decimal), a fixed amount per month
//   - CalculateMonthlyPay = base + Bonus
//   - Describe() appends "  manages N"
public class Manager
{
}

// TODO 9: Contractor(name, monthlyFee) — not an employee! Does not inherit from Employee,
//   but implements IPayable directly (a fixed monthly invoice).
public class Contractor
{
}

// TODO 10: Payroll — holds a List<IPayable>; Add(IPayable), TotalMonthly(), PrintSlips()
//   (prints each PaySlip + a summary line). Note: Payroll does not know, and does not need
//   to know, whether it is dealing with an employee or a contractor — that is polymorphism.
public class Payroll
{
    public void Add(IPayable payee) => throw new NotImplementedException();
    public decimal TotalMonthly() => throw new NotImplementedException();
    public void PrintSlips() => throw new NotImplementedException();
}
