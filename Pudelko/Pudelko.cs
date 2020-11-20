using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace PudelkoLibrary
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        #region A,B,C Values

        private double a, b, c;

        public double A
        {
            get
            {
                double temp = a;
                if (_unit == UnitOfMeasure.Centimeter)
                    temp = a / 100;
                else if (_unit == UnitOfMeasure.Millimeter)
                    temp = a / 1000;

                return GetRoundedNumber(temp);
            }
        }

        public double B
        {
            get
            {
                double temp = b;
                if (_unit == UnitOfMeasure.Centimeter)
                    temp = b / 100;
                else if (_unit == UnitOfMeasure.Millimeter)
                    temp = b / 1000;

                return GetRoundedNumber(temp);
            }
        }

        public double C
        {
            get
            {
                double temp = c;
                if (_unit == UnitOfMeasure.Centimeter)
                    temp = c / 100;
                else if (_unit == UnitOfMeasure.Millimeter)
                    temp = c / 1000;

                return GetRoundedNumber(temp);
            }
        }

        #endregion

        #region Objetosc i Pole

        public string Objetosc => $"{Math.Round((A * B * C), 9)} m3";

        public string Pole => $"{Math.Round(2 * A * B + 2 * A * C + 2 * B * C, 6)} m2";

        #endregion

        public enum UnitOfMeasure
        {
            Millimeter,
            Centimeter,
            Meter
        }

        private readonly double[] indexer;
        public object this[int index]
        {
            get
            {
                return indexer[index];
            }
        }

        private UnitOfMeasure _unit;

        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.Meter)
        {
            _unit = unit;

            double minimum = CalculateMinimum();

            this.a = a.GetValueOrDefault(CalculateDefaults());
            this.b = b.GetValueOrDefault(CalculateDefaults());
            this.c = c.GetValueOrDefault(CalculateDefaults());

            if (this.a < minimum || this.b < minimum || this.c < minimum)
            {
                throw new ArgumentOutOfRangeException("Niepoprawna wartosc!");
            }

            indexer = new[] { this.a, this.b, this.c };

            if (A > 10 || B > 10 || C > 10)
            {
                throw new ArgumentOutOfRangeException("Niepoprawna wartosc!");
            }
        }

        private double CalculateDefaults()
        {
            switch (_unit)
            {
                case UnitOfMeasure.Meter:
                    return 0.1;
                case UnitOfMeasure.Centimeter:
                    return 10;
                case UnitOfMeasure.Millimeter:
                    return 100;
                default:
                    throw new NotImplementedException("Nieobslugiwane");
            }
        }

        private double CalculateMinimum()
        {
            switch (_unit)
            {
                case UnitOfMeasure.Meter:
                    return 0.001;
                case UnitOfMeasure.Centimeter:
                    return 0.1;
                case UnitOfMeasure.Millimeter:
                    return 1;
                default:
                    throw new NotImplementedException("Nieobslugiwane");
            }
        }

        public IEnumerator<double> GetEnumerator()
        {
            return new PudelkoEnum(indexer);
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() + b.GetHashCode() + c.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return Equals((Pudelko)obj);
        }

        public bool Equals(Pudelko other)
        {
            if (other is null)
            {
                return false;
            }

            List<double> numbers = new List<double>()
            {
                A, B, C
            };
            if (numbers.Contains(other.A)) numbers.Remove(other.A);
            if (numbers.Contains(other.B)) numbers.Remove(other.B);
            if (numbers.Contains(other.C)) numbers.Remove(other.C);

            if (numbers.Count == 0) return true;

            return false;
        }

        public static bool operator ==(Pudelko p1, Pudelko p2) => p1.Equals(p2);
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);
        public static explicit operator double[](Pudelko p) => new double[] { p.A, p.B, p.C };
        public static implicit operator Pudelko(ValueTuple<int, int, int> v) => new Pudelko(v.Item1, v.Item2, v.Item3, UnitOfMeasure.Millimeter);

        public override string ToString()
        {
            return $"{String.Format("{0:0.000}", A)} m \u00D7 {String.Format("{0:0.000}", B)} m \u00D7 {String.Format("{0:0.000}", C)} m";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            switch (format)
            {
                case "cm":
                    return $"{String.Format("{0:0.0}", A * 100)} cm \u00D7 {String.Format("{0:0.0}", B * 100)} cm \u00D7 {String.Format("{0:0.0}", C * 100)} cm";
                case "mm":
                    return $"{A * 1000} mm \u00D7 {B * 1000} mm \u00D7 {C * 1000} mm";
                case "m":
                default:
                    return ToString();
            }
        }

        private static double GetRoundedNumber(double number)
        {
            number *= 1000;
            number = (int)number;
            number /= 1000;
            return number;
        }

        public static Pudelko Parse(string name)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            double[] numbers = new double[3];
            string u = null;
            var strings = name.Split(" \u00D7 ");
            if (strings.Length != 3) throw new ArgumentException("Given string is not correct!");
            for (var i = 0; i < 3; i++)
            {
                var stringsy = strings[i].Split(" ");
                numbers[i] = Convert.ToDouble(stringsy[0], provider);
                u = u == null ? stringsy[1] :
                    u != stringsy[1] ? throw new ArgumentException("Different units!") : stringsy[1];
            }
            Pudelko pudelko = new Pudelko(numbers[0], numbers[1], numbers[2], ParseUnitMeasure(u));
            return pudelko;
        }

        public static UnitOfMeasure ParseUnitMeasure(string unit)
        {
            switch (unit)
            {
                case "m":
                    return UnitOfMeasure.Meter;
                case "cm":
                    return UnitOfMeasure.Centimeter;
                case "mm":
                    return UnitOfMeasure.Millimeter;
                default:
                    throw new FormatException("Niepoprawny format!");
            }
        }
    }

    public sealed class PudelkoEnum : IEnumerator<double>
    {
        private int position = -1;
        private double[] values;

        public PudelkoEnum(double[] values)
        {
            this.values = values;
        }

        public bool MoveNext()
        {
            return (++position < values.Length);
        }

        public void Reset()
        {
            position -= 1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public double Current
        {
            get
            {
                try
                {
                    return values[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose() { }
    }
}