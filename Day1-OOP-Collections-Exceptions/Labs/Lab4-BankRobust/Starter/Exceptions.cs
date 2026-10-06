namespace Day1.Lab4;

// TODO A: A hierarchy of custom exceptions:
//   BankException : Exception                      (base)
//   AccountNotFoundException : BankException       (with an AccountId property)
//   InsufficientFundsException : BankException     (with Requested, Available, Shortfall)
//   InvalidAmountException : BankException         (with Amount)
// Right now there is only one general class — replace it with the hierarchy.
public class BankException(string message) : Exception(message);
