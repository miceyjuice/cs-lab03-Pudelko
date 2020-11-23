using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace PudelkoLibrary
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        #region Constructor

        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.Meter)
        {
            _unit = unit;

            var minimum = CalculateMinimum();

            this.a = a.GetValueOrDefault(CalculateDefaults());
            this.b = b.GetValueOrDefault(CalculateDefaults());
            this.c = c.GetValueOrDefault(CalculateDefaults());

            if (this.a < minimum || this.b < minimum || this.c < minimum)
                throw new ArgumentOutOfRangeException("Niepoprawna wartosc!");

            indexer = new[] {this.a, this.b, this.c};

            if (A > 10 || B > 10 || C > 10) throw new ArgumentOutOfRangeException("Niepoprawna wartosc!");
        }

        #endregion

        public IEnumerator<double> GetEnumerator() => new PudelkoEnum(indexer);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Equals(Pudelko other)
        {
            if (other is null) return false;

            var numbers = new List<double> {A, B, C};
            if (numbers.Contains(other.A)) numbers.Remove(other.A);
            if (numbers.Contains(other.B)) numbers.Remove(other.B);
            if (numbers.Contains(other.C)) numbers.Remove(other.C);

            return numbers.Count == 0;
        }

        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            return format switch
            {
                "cm" => $"{A * 100:0.0} cm × {B * 100:0.0} cm × {C * 100:0.0} cm",
                "mm" => $"{A * 1000} mm \u00D7 {B * 1000} mm \u00D7 {C * 1000} mm",
                "m" => ToString(),
                null => ToString(),
                _ => throw new FormatException("Niepoprawna jednostka!")
            };
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() + b.GetHashCode() + c.GetHashCode();
        }

        private static double GetValueWithProperUnit(double v, UnitOfMeasure unit)
        {
            var temp = unit switch
            {
                UnitOfMeasure.Centimeter => v / 100,
                UnitOfMeasure.Millimeter => v / 1000,
                _ => v
            };

            return GetRoundedNumber(temp);
        }
        public override bool Equals(object? obj) => Equals((Pudelko) obj);
        public static bool operator ==(Pudelko p1, Pudelko p2) => p1.Equals(p2);
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);

        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            var a = p1.A + p2.A;
            var b = p1.B > p2.B ? p1.B : p2.B;
            var c = p1.C > p2.C ? p1.C : p2.C;

            var p3 = new Pudelko(a, b, c);

            return p3;
        }

        public static explicit operator double[](Pudelko p) => new[] {p.A, p.B, p.C};

        public static implicit operator Pudelko(ValueTuple<int, int, int> v) =>
            new Pudelko(v.Item1, v.Item2, v.Item3, UnitOfMeasure.Millimeter);

        public override string ToString() => $"{A:0.000} m × {B:0.000} m × {C:0.000} m";

        private static double GetRoundedNumber(double number)
        {
            number *= 1000;
            number = (int) number;
            number /= 1000;
            return number;
        }

        public static int Comparison(Pudelko p1, Pudelko p2)
        {
            var p1Sum = p1.A + p1.B + p1.C;
            var p2Sum = p2.A + p2.B + p2.C;
            if (p1.Objetosc < p2.Objetosc) return -1;
            if (p1.Objetosc > p2.Objetosc) return 1;
            if (p1.Pole < p2.Pole) return -1;
            if (p1.Pole > p2.Pole) return 1;
            if (p1Sum < p2Sum) return -1;
            if (p1Sum > p2Sum) return 1;
            return 0;
        }


        public static Pudelko Parse(string name)
        {
            var provider = new NumberFormatInfo {NumberDecimalSeparator = "."};
            var numbers = new double[3];
            string u = null;
            var strings = name.Split(" \u00D7 ");
            if (strings.Length != 3) throw new ArgumentException("Given string is not correct!");
            for (var i = 0; i < 3; i++)
            {
                var stringsy = strings[i].Split(' ');
                numbers[i] = Convert.ToDouble(stringsy[0], provider);
                u = u == null ? stringsy[1] :
                    u != stringsy[1] ? throw new ArgumentException("Different units!") : stringsy[1];
            }

            var pudelko = new Pudelko(numbers[0], numbers[1], numbers[2], ParseUnitMeasure(u));
            return pudelko;
        }

        private static UnitOfMeasure ParseUnitMeasure(string unit) =>
            unit switch
            {
                "m" => UnitOfMeasure.Meter,
                "cm" => UnitOfMeasure.Centimeter,
                "mm" => UnitOfMeasure.Millimeter,
                _ => throw new FormatException("Niepoprawny format!")
            };

        #region A,B,C Values

        private double a, b, c;

        public double A => GetValueWithProperUnit(a, _unit);
        public double B => GetValueWithProperUnit(b, _unit);
        public double C => GetValueWithProperUnit(c, _unit);

        #endregion

        #region Objetosc i Pole
        public double Objetosc => Math.Round(A * B * C, 9);
        public double Pole => Math.Round(2 * A * B + 2 * A * C + 2 * B * C, 6);
        #endregion

        #region Enum
        public enum UnitOfMeasure { Millimeter, Centimeter, Meter }
        private readonly UnitOfMeasure _unit;
        #endregion

        #region Indexer

        private readonly double[] indexer;
        public object this[int index] => indexer[index];

        #endregion

        #region CalculateMethods

        private double CalculateDefaults() =>
            _unit switch
            {
                UnitOfMeasure.Meter => 0.1,
                UnitOfMeasure.Centimeter => 10,
                UnitOfMeasure.Millimeter => 100,
                _ => throw new NotImplementedException("Nieobslugiwane")
            };

        private double CalculateMinimum() =>
            _unit switch
            {
                UnitOfMeasure.Meter => 0.001,
                UnitOfMeasure.Centimeter => 0.1,
                UnitOfMeasure.Millimeter => 1,
                _ => throw new NotImplementedException("Nieobslugiwane")
            };

        #endregion
    }

    public sealed class PudelkoEnum : IEnumerator<double>
    {
        private double[] _values;
        private int _position = -1;

        public PudelkoEnum(double[] values) => _values = values;
        public bool MoveNext() => (++_position < _values.Length);

        public void Reset() => _position = -1;

            object IEnumerator.Current => Current;

        public double Current
        {
            get
            {
                try
                {
                    return _values[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose() => _values = null;
    }
}