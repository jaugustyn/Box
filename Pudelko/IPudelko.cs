namespace PudelkoLib;

public interface IPudelko
{
    enum UnitOfMeasure
    {
        Meter = 1,
        Centimeter = 100,
        Millimeter = 1000
    }

    double A { get; }
    double C { get; }
    double B { get; }

    static double UnitConverter(UnitOfMeasure from, UnitOfMeasure to, double value)
    {
        return (from, to) switch
        {
            (UnitOfMeasure.Millimeter, UnitOfMeasure.Centimeter) => Math.Round(value * 0.1, 1),
            (UnitOfMeasure.Millimeter, UnitOfMeasure.Meter) => Math.Round(value * 0.001, 3),
            (UnitOfMeasure.Centimeter, UnitOfMeasure.Millimeter) => Math.Round(value * 10, 0),
            (UnitOfMeasure.Centimeter, UnitOfMeasure.Meter) => Math.Round(value * 0.01, 3),
            (UnitOfMeasure.Meter, UnitOfMeasure.Millimeter) => Math.Round(value * 1000, 0),
            (UnitOfMeasure.Meter, UnitOfMeasure.Centimeter) => Math.Round(value * 100, 1),
            _ => value
        };
    }

    static UnitOfMeasure ParseUnitName(string str)
    {
        return str switch
        {
            "m" => UnitOfMeasure.Meter,
            "cm" => UnitOfMeasure.Centimeter,
            "mm" => UnitOfMeasure.Millimeter,
            null => UnitOfMeasure.Meter,
            _ => throw new FormatException("Incorrect format!")
        };
    }
}