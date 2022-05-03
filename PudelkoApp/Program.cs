// See https://aka.ms/new-console-template for more information

using PudelkoApp;
using PudelkoLib;

var p1 = new Pudelko(20, 20, 30, IPudelko.UnitOfMeasure.Centimeter);
var p2 = new Pudelko(123, 321, null, IPudelko.UnitOfMeasure.Millimeter);
var p3 = new Pudelko();
Console.WriteLine(p1.ToString("mm"));
Console.WriteLine(p2.ToString("CM"));
Console.WriteLine(p3.ToString("m"));

Console.WriteLine($"\nVolume : {p1.Objetosc} m3");
Console.WriteLine($"Area: {p1.Pole} m2");

Console.WriteLine($"Are p1 and p3 equal? p1: {p1} p3: {p3} => {p1.Equals(p2)}\n");
Console.WriteLine("Explicit conversion: " + string.Join(" >< ", (double[])p2));

ValueTuple<int, int, int> vt = (10, 20, 30);
Pudelko p4 = vt;
Console.WriteLine($"Implicit conversion: {p4}\n");

Console.WriteLine("Indexer (p2[0]): " + p2[0]);
Console.WriteLine("Indexer using foreach loop (p1):");
foreach (var p in p1) Console.WriteLine(p);

Console.WriteLine("\nParse: " + (new Pudelko(1, 2, 3) == Pudelko.Parse("1,000 m × 2,000 m × 3,000 m")));

var list = new List<Pudelko>{p1, p2, p3, p4};

Console.WriteLine("List before sort: \n");
foreach (var p in list) Console.WriteLine(p);

Console.WriteLine("\nSorted list: ");
list.Sort(Pudelko.Compare);
foreach (var p in list) Console.WriteLine(p);

Console.WriteLine("\n+ operator overload: ");
var nowePudelko = p1 + p2;
Console.WriteLine(nowePudelko);

Console.WriteLine("\nCompression: ");
Console.WriteLine(Extension.Kompresuj(p4));