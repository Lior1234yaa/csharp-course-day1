namespace Day1.Lab2;

// ============ Part A — Shapes (warm-up, ~15 minutes) ============

// TODO 1: Make Shape an abstract class with:
//   - an abstract property Name (string)
//   - abstract methods Area() and Perimeter() that return double
//   - an override of ToString that returns, for example: "Circle: area=3.14, perimeter=6.28"
public class Shape
{
}

// TODO 2: Circle(radius), Rectangle(width, height), Triangle(a, b, c) — inherit from Shape.
//   Triangle: check in the constructor that the sides satisfy the triangle inequality, otherwise throw ArgumentException.
//   Triangle area using Heron's formula: s=(a+b+c)/2, area=sqrt(s(s-a)(s-b)(s-c)).
// TODO 3: Square inherits from Rectangle (a single side), and is marked sealed.
