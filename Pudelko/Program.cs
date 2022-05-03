// See https://aka.ms/new-console-template for more information

using PudelkoLib;

var test = new Pudelko(3.8, 2.5, 2, IPudelko.UnitOfMeasure.Centimeter);
var test2 = new Pudelko(3.8, 2.5, 2 );

Console.WriteLine(test2.Pole.ToString("F10"));
Console.WriteLine(test2.Objetosc.ToString("F10"));

Console.WriteLine(test.ToString());
Console.WriteLine(new Pudelko(3.8, 2.5, 2, IPudelko.UnitOfMeasure.Centimeter) == Pudelko.Parse(test.ToString("cm")));
Console.WriteLine(test2.ToString("cm"));
Console.WriteLine(Pudelko.Parse(test2.ToString("cm")));
