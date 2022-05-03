using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PudelkoLib;

namespace PudelkoUnitTests;

[TestClass]
public static class InitializeCulture
{
    [AssemblyInitialize]
    public static void SetEnglishCultureOnAllUnitTest(TestContext context)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
    }
}

// ========================================

[TestClass]
public class UnitTestsPudelkoConstructors
{
    private static readonly double defaultSize = 0.1; // w metrach
    private static readonly double accuracy = 0.001; //dok³adnoœæ 3 miejsca po przecinku

    private void AssertPudelko(Pudelko p, double expectedA, double expectedB, double expectedC)
    {
        Assert.AreEqual(expectedA, p.A, accuracy);
        Assert.AreEqual(expectedB, p.B, accuracy);
        Assert.AreEqual(expectedC, p.C, accuracy);
    }

    #region Constructor tests ================================

    [TestMethod]
    [TestCategory("Constructors")]
    public void Constructor_Default()
    {
        Pudelko p = new Pudelko();

        Assert.AreEqual(defaultSize, p.A, accuracy);
        Assert.AreEqual(defaultSize, p.B, accuracy);
        Assert.AreEqual(defaultSize, p.C, accuracy);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(1.0, 2.543, 3.1,
        1.0, 2.543, 3.1)]
    [DataRow(1.0001, 2.54387, 3.1005,
        1.0, 2.543, 3.1)] // dla metrów licz¹ siê 3 miejsca po przecinku
    public void Constructor_3params_DefaultMeters(double a, double b, double c,
        double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(a, b, c);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(1.0, 2.543, 3.1,
        1.0, 2.543, 3.1)]
    [DataRow(1.0001, 2.54387, 3.1005,
        1.0, 2.543, 3.1)] // dla metrów licz¹ siê 3 miejsca po przecinku
    public void Constructor_3params_InMeters(double a, double b, double c,
        double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(a, b, c, unit: IPudelko.UnitOfMeasure.Meter);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(100.0, 25.5, 3.1,
        1.0, 0.255, 0.031)]
    [DataRow(100.0, 25.58, 3.13,
        1.0, 0.256, 0.031)] // dla centymertów liczy siê tylko 1 miejsce po przecinku
    public void Constructor_3params_InCentimeters(double a, double b, double c,
        double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(a: a, b: b, c: c, unit: IPudelko.UnitOfMeasure.Centimeter);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(100, 255, 3,
        0.1, 0.255, 0.003)]
    [DataRow(100.0, 25.58, 3.13,
        0.1, 0.025, 0.003)] // dla milimetrów nie licz¹ siê miejsca po przecinku
    public void Constructor_3params_InMilimeters(double a, double b, double c,
        double expectedA, double expectedB, double expectedC)
    {
        Pudelko p = new Pudelko(unit: IPudelko.UnitOfMeasure.Millimeter, a: a, b: b, c: c);

        AssertPudelko(p, expectedA, expectedB, expectedC);
    }


    // ----

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(1.0, 2.5, 1.0, 2.5)]
    [DataRow(1.001, 2.599, 1.001, 2.599)]
    [DataRow(1.0019, 2.5999, 1.001, 2.599)]
    public void Constructor_2params_DefaultMeters(double a, double b, double expectedA, double expectedB)
    {
        Pudelko p = new Pudelko(a, b);

        AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(1.0, 2.5, 1.0, 2.5)]
    [DataRow(1.001, 2.599, 1.001, 2.599)]
    [DataRow(1.0019, 2.5999, 1.001, 2.599)]
    public void Constructor_2params_InMeters(double a, double b, double expectedA, double expectedB)
    {
        Pudelko p = new Pudelko(a: a, b: b, unit: IPudelko.UnitOfMeasure.Meter);

        AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(11.0, 2.5, 0.11, 0.025)]
    [DataRow(100.1, 2.599, 1.001, 0.025)]
    [DataRow(2.0019, 0.25999, 0.02, 0.002)]
    public void Constructor_2params_InCentimeters(double a, double b, double expectedA, double expectedB)
    {
        Pudelko p = new Pudelko(unit: IPudelko.UnitOfMeasure.Centimeter, a: a, b: b);

        AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(11, 2.0, 0.011, 0.002)]
    [DataRow(100.1, 2599, 0.1, 2.599)]
    [DataRow(200.19, 2.5999, 0.2, 0.002)]
    public void Constructor_2params_InMilimeters(double a, double b, double expectedA, double expectedB)
    {
        Pudelko p = new Pudelko(unit: IPudelko.UnitOfMeasure.Millimeter, a: a, b: b);

        AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
    }

    // -------

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(2.5)]
    public void Constructor_1param_DefaultMeters(double a)
    {
        Pudelko p = new Pudelko(a);

        Assert.AreEqual(a, p.A);
        Assert.AreEqual(0.1, p.B);
        Assert.AreEqual(0.1, p.C);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(2.5)]
    public void Constructor_1param_InMeters(double a)
    {
        Pudelko p = new Pudelko(a);

        Assert.AreEqual(a, p.A);
        Assert.AreEqual(0.1, p.B);
        Assert.AreEqual(0.1, p.C);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(11.0, 0.11)]
    [DataRow(100.1, 1.001)]
    [DataRow(2.0019, 0.02)]
    public void Constructor_1param_InCentimeters(double a, double expectedA)
    {
        Pudelko p = new Pudelko(unit: IPudelko.UnitOfMeasure.Centimeter, a: a);

        AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(11, 0.011)]
    [DataRow(100.1, 0.1)]
    [DataRow(200.19, 0.2)]
    public void Constructor_1param_InMilimeters(double a, double expectedA)
    {
        Pudelko p = new Pudelko(unit: IPudelko.UnitOfMeasure.Millimeter, a: a);

        AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
    }

    // ---

    public static IEnumerable<object[]> DataSet1Meters_ArgumentOutOfRangeEx => new List<object[]>
    {
        new object[] {-1.0, 2.5, 3.1},
        new object[] {1.0, -2.5, 3.1},
        new object[] {1.0, 2.5, -3.1},
        new object[] {-1.0, -2.5, 3.1},
        new object[] {-1.0, 2.5, -3.1},
        new object[] {1.0, -2.5, -3.1},
        new object[] {-1.0, -2.5, -3.1},
        new object[] {0, 2.5, 3.1},
        new object[] {1.0, 0, 3.1},
        new object[] {1.0, 2.5, 0},
        new object[] {1.0, 0, 0},
        new object[] {0, 2.5, 0},
        new object[] {0, 0, 3.1},
        new object[] {0, 0, 0},
        new object[] {10.1, 2.5, 3.1},
        new object[] {10, 10.1, 3.1},
        new object[] {10, 10, 10.1},
        new object[] {10.1, 10.1, 3.1},
        new object[] {10.1, 10, 10.1},
        new object[] {10, 10.1, 10.1},
        new object[] {10.1, 10.1, 10.1}
    };

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_3params_DefaultMeters_ArgumentOutOfRangeException(double a, double b, double c)
    {
        Pudelko p = new Pudelko(a, b, c);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_3params_InMeters_ArgumentOutOfRangeException(double a, double b, double c)
    {
        Pudelko p = new Pudelko(a, b, c, unit: IPudelko.UnitOfMeasure.Meter);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(-1, 1, 1)]
    [DataRow(1, -1, 1)]
    [DataRow(1, 1, -1)]
    [DataRow(-1, -1, 1)]
    [DataRow(-1, 1, -1)]
    [DataRow(1, -1, -1)]
    [DataRow(-1, -1, -1)]
    [DataRow(0, 1, 1)]
    [DataRow(1, 0, 1)]
    [DataRow(1, 1, 0)]
    [DataRow(0, 0, 1)]
    [DataRow(0, 1, 0)]
    [DataRow(1, 0, 0)]
    [DataRow(0, 0, 0)]
    [DataRow(0.01, 0.1, 1)]
    [DataRow(0.1, 0.01, 1)]
    [DataRow(0.1, 0.1, 0.01)]
    [DataRow(1001, 1, 1)]
    [DataRow(1, 1001, 1)]
    [DataRow(1, 1, 1001)]
    [DataRow(1001, 1, 1001)]
    [DataRow(1, 1001, 1001)]
    [DataRow(1001, 1001, 1)]
    [DataRow(1001, 1001, 1001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_3params_InCentimeters_ArgumentOutOfRangeException(double a, double b, double c)
    {
        Pudelko p = new Pudelko(a, b, c, unit: IPudelko.UnitOfMeasure.Centimeter);
    }


    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(-1, 1, 1)]
    [DataRow(1, -1, 1)]
    [DataRow(1, 1, -1)]
    [DataRow(-1, -1, 1)]
    [DataRow(-1, 1, -1)]
    [DataRow(1, -1, -1)]
    [DataRow(-1, -1, -1)]
    [DataRow(0, 1, 1)]
    [DataRow(1, 0, 1)]
    [DataRow(1, 1, 0)]
    [DataRow(0, 0, 1)]
    [DataRow(0, 1, 0)]
    [DataRow(1, 0, 0)]
    [DataRow(0, 0, 0)]
    [DataRow(0.1, 1, 1)]
    [DataRow(1, 0.1, 1)]
    [DataRow(1, 1, 0.1)]
    [DataRow(10001, 1, 1)]
    [DataRow(1, 10001, 1)]
    [DataRow(1, 1, 10001)]
    [DataRow(10001, 10001, 1)]
    [DataRow(10001, 1, 10001)]
    [DataRow(1, 10001, 10001)]
    [DataRow(10001, 10001, 10001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_3params_InMiliimeters_ArgumentOutOfRangeException(double a, double b, double c)
    {
        Pudelko p = new Pudelko(a, b, c, unit: IPudelko.UnitOfMeasure.Millimeter);
    }


    public static IEnumerable<object[]> DataSet2Meters_ArgumentOutOfRangeEx => new List<object[]>
    {
        new object[] {-1.0, 2.5},
        new object[] {1.0, -2.5},
        new object[] {-1.0, -2.5},
        new object[] {0, 2.5},
        new object[] {1.0, 0},
        new object[] {0, 0},
        new object[] {10.1, 10},
        new object[] {10, 10.1},
        new object[] {10.1, 10.1}
    };

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_2params_DefaultMeters_ArgumentOutOfRangeException(double a, double b)
    {
        Pudelko p = new Pudelko(a, b);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_2params_InMeters_ArgumentOutOfRangeException(double a, double b)
    {
        Pudelko p = new Pudelko(a, b, unit: IPudelko.UnitOfMeasure.Meter);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(-1, 1)]
    [DataRow(1, -1)]
    [DataRow(-1, -1)]
    [DataRow(0, 1)]
    [DataRow(1, 0)]
    [DataRow(0, 0)]
    [DataRow(0.01, 1)]
    [DataRow(1, 0.01)]
    [DataRow(0.01, 0.01)]
    [DataRow(1001, 1)]
    [DataRow(1, 1001)]
    [DataRow(1001, 1001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_2params_InCentimeters_ArgumentOutOfRangeException(double a, double b)
    {
        Pudelko p = new Pudelko(a, b, unit: IPudelko.UnitOfMeasure.Centimeter);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(-1, 1)]
    [DataRow(1, -1)]
    [DataRow(-1, -1)]
    [DataRow(0, 1)]
    [DataRow(1, 0)]
    [DataRow(0, 0)]
    [DataRow(0.1, 1)]
    [DataRow(1, 0.1)]
    [DataRow(0.1, 0.1)]
    [DataRow(10001, 1)]
    [DataRow(1, 10001)]
    [DataRow(10001, 10001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_2params_InMilimeters_ArgumentOutOfRangeException(double a, double b)
    {
        Pudelko p = new Pudelko(a, b, unit: IPudelko.UnitOfMeasure.Millimeter);
    }


    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(-1.0)]
    [DataRow(0)]
    [DataRow(10.1)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_1param_DefaultMeters_ArgumentOutOfRangeException(double a)
    {
        Pudelko p = new Pudelko(a);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(-1.0)]
    [DataRow(0)]
    [DataRow(10.1)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_1param_InMeters_ArgumentOutOfRangeException(double a)
    {
        Pudelko p = new Pudelko(a, unit: IPudelko.UnitOfMeasure.Meter);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(-1.0)]
    [DataRow(0)]
    [DataRow(0.01)]
    [DataRow(1001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_1param_InCentimeters_ArgumentOutOfRangeException(double a)
    {
        Pudelko p = new Pudelko(a, unit: IPudelko.UnitOfMeasure.Centimeter);
    }

    [DataTestMethod]
    [TestCategory("Constructors")]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(0.1)]
    [DataRow(10001)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_1param_InMilimeters_ArgumentOutOfRangeException(double a)
    {
        Pudelko p = new Pudelko(a, unit: IPudelko.UnitOfMeasure.Millimeter);
    }

    #endregion


    #region ToString tests ===================================

    [TestMethod]
    [TestCategory("String representation")]
    public void ToString_Default_Culture_EN()
    {
        var p = new Pudelko(2.5, 9.321);
        var expectedStringEN = "2.500 m × 9.321 m × 0.100 m";

        Assert.AreEqual(expectedStringEN, p.ToString());
    }

    [DataTestMethod]
    [TestCategory("String representation")]
    [DataRow(null, 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
    [DataRow("m", 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
    [DataRow("cm", 2.5, 9.321, 0.1, "250.0 cm × 932.1 cm × 10.0 cm")]
    [DataRow("mm", 2.5, 9.321, 0.1, "2500 mm × 9321 mm × 100 mm")]
    public void ToString_Formattable_Culture_EN(string format, double a, double b, double c,
        string expectedStringRepresentation)
    {
        var p = new Pudelko(a, b, c, unit: IPudelko.UnitOfMeasure.Meter);
        Assert.AreEqual(expectedStringRepresentation, p.ToString(format));
    }

    [TestMethod]
    [TestCategory("String representation")]
    [ExpectedException(typeof(FormatException))]
    public void ToString_Formattable_WrongFormat_FormatException()
    {
        var p = new Pudelko(1);
        var stringformatedrepreentation = p.ToString("wrong code");
    }

    #endregion


    #region Pole, Objêtoœæ ===================================
    
    [DataTestMethod]
    [TestCategory("Area")]
    [DataRow(null, 3, 2, 2, "32")]
    [DataRow("m", 3.8, 2.5, 2, "44.2")]
    [DataRow("cm", 329, 19, 100, "8.2102")]
    [DataRow("mm", 267, 932, 1111, "3.161866")]
    [DataRow(null, 1, 1, 1, "6")]
    public void Area_Pudelko(string format, double a, double b, double c,
        string expectedStringRepresentation)
    {
        var p = new Pudelko(a, b, c, unit: IPudelko.ParseUnitName(format));
        Assert.AreEqual(expectedStringRepresentation, (p.Pole).ToString(CultureInfo.InvariantCulture));
    }

    [DataTestMethod]
    [TestCategory("Volume")]
    [DataRow(null, 3, 2, 2, "12")]
    [DataRow("m", 3.8, 2.5, 2, "19")]
    [DataRow("cm", 329, 19, 100, "0.6251")]
    [DataRow("mm", 267, 932, 1111, "0.276465684")]
    [DataRow(null, 1, 1, 1, "1")]
    public void Volume_Pudelko(string format, double a, double b, double c,
        string expectedStringRepresentation)
    {
        var p = new Pudelko(a, b, c, unit: IPudelko.ParseUnitName(format));
        Assert.AreEqual(expectedStringRepresentation, (p.Objetosc).ToString(CultureInfo.InvariantCulture));
    }
    #endregion

    #region Equals ===========================================

    [DataTestMethod]
    [TestCategory("Equals")]
    [DataRow(null, 3, 2, 2, "m", 2, 2, 3, true)]
    [DataRow("m", 3.8, 2.5, 2, "cm", 3.8, 2.5, 2, false)]
    [DataRow("cm", 329, 19, 100, "cm", 329, 19, 100, true)]
    [DataRow("mm", 267, 932, 1111, "mm", 100, 20, 100, false)]
    [DataRow(null, 6.2, 9.3, 10, null, 6.2, 9.3, 9.3, false)]
    [DataRow("cm", 1, 1, 1, "cm", 1, 1, 1, true)]
    public void Equals_Pudelko(string format, double a, double b, double c, string format2, double d, double e, double f,
        bool expectedBool)
    {
        var p1 = new Pudelko(a, b, c, unit: IPudelko.ParseUnitName(format));
        var p2 = new Pudelko(d, e, f, unit: IPudelko.ParseUnitName(format2));
        Assert.AreEqual(expectedBool, p1.Equals(p2));
    }

    #endregion

    #region Operators overloading ===========================

    [DataTestMethod]
    [TestCategory("Plus_Operator")]
    [DataRow(null, 1, 2, 1, "m", 3, 4, 1, "4.000 m × 4.000 m × 1.000 m")]
    [DataRow("m", 3.8, 2.5, 2, "cm", 3.8, 2.5, 2, "3.838 m × 2.500 m × 2.000 m")]
    [DataRow("cm", 329, 19, 100, "cm", 329, 19, 100, "6.580 m × 0.190 m × 1.000 m")]
    [DataRow("mm", 267, 932, 1111, "mm", 100, 20, 100, "0.367 m × 0.932 m × 1.111 m")]
    [DataRow("cm", 1, 1, 1, "cm", 1, 1, 1, "0.020 m × 0.010 m × 0.010 m")]
    public void Plus_Operator_Overload_Pudelko(string format, double a, double b, double c, string format2, double d, double e, double f,
        string expectedStringRepresentation)
    {
        var p1 = new Pudelko(a, b, c, unit: IPudelko.ParseUnitName(format));
        var p2 = new Pudelko(d, e, f, unit: IPudelko.ParseUnitName(format2));
        Assert.AreEqual(expectedStringRepresentation, (p1 + p2).ToString());
    }

    [DataTestMethod]
    [TestCategory("Plus_Operator")]
    [DataRow(null, 1, 2, 1, "m", 13, 14, 11)]
    [DataRow("m", 0, 2.5, 2, "cm", 3.8, 2.5, 2)]
    [DataRow("cm", 999, 999, 999, "cm", 2, 2, 2)]
    [DataRow("mm", 0, 0, 0, "mm", 1000, 200, 1000)]
    [DataRow("cm", -1, 1, 1, "cm", 1, 1, 1)]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Plus_Operator_Overload_Exception_Pudelko(string format, double a, double b, double c, string format2, double d, double e, double f)
    {
        var p1 = new Pudelko(a, b, c, unit: IPudelko.ParseUnitName(format));
        var p2 = new Pudelko(d, e, f, unit: IPudelko.ParseUnitName(format2));
        var p3 = p1 + p2;
    }

    #endregion

    #region Conversions =====================================

    [TestMethod]
    public void ExplicitConversion_ToDoubleArray_AsMeters()
    {
        var p = new Pudelko(1, 2.1, 3.231);
        var tab = (double[]) p;
        Assert.AreEqual(3, tab.Length);
        Assert.AreEqual(p.A, tab[0]);
        Assert.AreEqual(p.B, tab[1]);
        Assert.AreEqual(p.C, tab[2]);
    }

    [TestMethod]
    public void ImplicitConversion_FromAalueTuple_As_Pudelko_InMilimeters()
    {
        var (a, b, c) = (2500, 9321, 100); // in milimeters, ValueTuple
        Pudelko p = (a, b, c);
        Assert.AreEqual(p.A * 1000, a);
        Assert.AreEqual(p.B * 1000, b);
        Assert.AreEqual(p.C * 1000, c);
    }

    #endregion

    #region Indexer, enumeration ============================

    [TestMethod]
    public void Indexer_ReadFrom()
    {
        var p = new Pudelko(1, 2.1, 3.231);
        Assert.AreEqual(p.A, p[0]);
        Assert.AreEqual(p.B, p[1]);
        Assert.AreEqual(p.C, p[2]);
    }

    [TestMethod]
    public void ForEach_Test()
    {
        var p = new Pudelko(1, 2.1, 3.231);
        var tab = new[] {p.A, p.B, p.C};
        var i = 0;
        foreach (double x in p)
        {
            Assert.AreEqual(x, tab[i]);
            i++;
        }
    }

    #endregion

    #region Parsing =========================================

    #endregion
}