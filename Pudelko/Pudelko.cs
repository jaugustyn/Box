using System.Collections;
using System.Globalization;

namespace PudelkoLib;

public class Pudelko : IPudelko, IFormattable, IEquatable<Pudelko>, IEnumerable
{
    public delegate int Comparison<in T>(T x, T y);

    private readonly double[] _values;
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }
    public object this[int index] => _values[index];
    public double Objetosc => Math.Round(A * B * C, 9);
    public double Pole => Math.Round(2 * A * B + 2 * B * C + 2 * A * C, 6);

    public Pudelko(double? a = null, double? b = null, double? c = null, IPudelko.UnitOfMeasure unit = IPudelko.UnitOfMeasure.Meter)
    {
        a = a == null ? 0.1 : IPudelko.UnitConverter(unit, IPudelko.UnitOfMeasure.Meter, Convert.ToDouble(a));
        b = b == null ? 0.1 : IPudelko.UnitConverter(unit, IPudelko.UnitOfMeasure.Meter, Convert.ToDouble(b));
        c = c == null ? 0.1 : IPudelko.UnitConverter(unit, IPudelko.UnitOfMeasure.Meter, Convert.ToDouble(c));

        if (a < 0.001 || b < 0.001 || c < 0.001) throw new ArgumentOutOfRangeException();
        if (a > 10 || b > 10 || c > 10) throw new ArgumentOutOfRangeException();

        A = (double) a;
        B = (double) b;
        C = (double) c;

        _values = new[] {A, B, C};
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<double> GetEnumerator() => new PudelkoEnum(_values);

    public bool Equals(Pudelko? other)
    {
        if (other is null) return false;
        if (GetType() != other.GetType()) return false;

        var list = new List<double> {A, B, C};

        if (list.Contains(other.A)) list.Remove(other.A);
        if (list.Contains(other.B)) list.Remove(other.B);
        if (list.Contains(other.C)) list.Remove(other.C);

        return list.Count == 0;
    }


    public override string ToString() => ToString("m", CultureInfo.CurrentCulture);

    public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? provider)
    {
        if (string.IsNullOrEmpty(format)) format = "m";
        provider ??= CultureInfo.CurrentCulture;

        const string sign = "\u00D7";
        var unit = IPudelko.ParseUnitName(format.ToLower());
        var round = unit switch
        {
            IPudelko.UnitOfMeasure.Millimeter => "F0",
            IPudelko.UnitOfMeasure.Centimeter => "F1",
            _ => "F3"
        };

        var tempA = IPudelko.UnitConverter(IPudelko.UnitOfMeasure.Meter, unit, A).ToString(round, provider);
        var tempB = IPudelko.UnitConverter(IPudelko.UnitOfMeasure.Meter, unit, B).ToString(round, provider);
        var tempC = IPudelko.UnitConverter(IPudelko.UnitOfMeasure.Meter, unit, C).ToString(round, provider);

        return unit switch
        {
            IPudelko.UnitOfMeasure.Millimeter => $"{tempA} mm {sign} {tempB} mm {sign} {tempC} mm",
            IPudelko.UnitOfMeasure.Centimeter => $"{tempA} cm {sign} {tempB} cm {sign} {tempC} cm",
            IPudelko.UnitOfMeasure.Meter => $"{tempA} m {sign} {tempB} m {sign} {tempC} m",
            _ => throw new FormatException()
        };
    }

    public override bool Equals(object? obj) => Equals(obj as Pudelko);

    public override int GetHashCode() => A.GetHashCode() + B.GetHashCode() + C.GetHashCode();

    public static bool operator ==(Pudelko p1, Pudelko p2) => p1.Equals(p2);

    public static bool operator !=(Pudelko p1, Pudelko p2) => !p1.Equals(p2);

    public static Pudelko operator +(Pudelko p1, Pudelko p2)
    {
        var a = p1.A + p2.A;
        var b = p1.B > p2.B ? p1.B : p2.B;
        var c = p1.C > p2.C ? p1.C : p2.C;

        return new Pudelko(a, b, c);
    }

    public static explicit operator double[](Pudelko p) => new[] {p.A, p.B, p.C};

    public static implicit operator Pudelko(ValueTuple<int, int, int> vt) => new(vt.Item1, vt.Item2, vt.Item3, IPudelko.UnitOfMeasure.Millimeter);

    public static Pudelko Parse(string str)
    {
        var numbers = new double[3];
        var strings = str.Split(" \u00D7 ");
        var unit = "m";

        if (strings.Length != 3) throw new ArgumentException("Incorrect argument.");
        
        for (var i = 0; i < 3; i++)
        {
            var moreStrings = strings[i].Split(" ");
            numbers[i] = double.Parse(moreStrings[0]);
            unit = moreStrings[1];
        }

        return new Pudelko(numbers[0], numbers[1], numbers[2], IPudelko.ParseUnitName(unit));
    }

    public static int Compare(Pudelko p1, Pudelko p2)
    {
        if (p1.Objetosc > p2.Objetosc) return 1;
        if (p1.Objetosc < p2.Objetosc) return -1;
        if (p1.Pole > p2.Pole) return 1;
        if (p1.Pole < p2.Pole) return -1;
        if (p1.A + p1.B + p1.C > p2.A + p2.B + p2.C) return 1;
        if (p1.A + p1.B + p1.C < p2.A + p2.B + p2.C) return -1;
        return 0;
    }

    public class PudelkoEnum : IEnumerator<double>
    {
        private readonly double[] _values;
        private int _index;

        public PudelkoEnum(double[] values)
        {
            _values = values;
        }
        public double Current
        {
            get
            {
                try
                {
                    return _values[_index++];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
        object IEnumerator.Current { get; } = new();
        public bool MoveNext() => _index < 3;
        public void Reset() => _index = 0;
        public void Dispose(){}
    }
}