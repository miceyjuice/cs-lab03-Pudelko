using System;
using System.Collections.Generic;
using static PudelkoLibrary.Pudelko.UnitOfMeasure;
using static PudelkoLibrary.Kompresja;

namespace PudelkoLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Pudelko pudeleczko1 = new Pudelko(2, 4, 1, Meter);
            
            Console.WriteLine(pudeleczko1.ToString("m"));
            Console.WriteLine(pudeleczko1.ToString("cm"));
            Console.WriteLine(pudeleczko1.ToString("mm"));
            Console.WriteLine("---");
            Console.WriteLine($"Objetosc: {pudeleczko1.Objetosc}");
            Console.WriteLine($"Pole: {pudeleczko1.Pole}");
            Console.WriteLine("---");
            
            Pudelko pudeleczko2 = new Pudelko(2550, 3050,1000, Millimeter);
            Pudelko pudeleczko3 = new Pudelko(2.333, 1.4444,1.15523);
            
            Console.WriteLine($"Czy dlugosci bokow [{pudeleczko1.ToString()}] sa takie same jak w [{pudeleczko2.ToString()}] : {pudeleczko1.Equals(pudeleczko2)}");
            Console.WriteLine(String.Join(" | ",(double[])pudeleczko2));
            Console.WriteLine(pudeleczko1[0]);
            Console.WriteLine("---");
            
            foreach (var d in pudeleczko1)
            {
                Console.WriteLine(d);
            }
            Console.WriteLine(Pudelko.Parse("312 cm × 305 cm × 100 cm").ToString());
            Console.WriteLine(new Pudelko(2.5, 9.321, 0.1) == Pudelko.Parse("2.500 m × 9.321 m × 0.100 m"));
            Console.WriteLine("---");

            var listaPudelek = new List<Pudelko> {pudeleczko1, pudeleczko2, pudeleczko3};
            Console.WriteLine("---");
            Console.WriteLine("Skompresowane pudelko:");
            Console.WriteLine("---");
            
            Console.WriteLine(Kompresuj(pudeleczko1));
            
            Console.WriteLine("---");
            Console.WriteLine("Nieposortowana lista:");
            Console.WriteLine("---");

            foreach (var i in listaPudelek)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("---");
            Console.WriteLine("Posortowana lista:");
            Console.WriteLine("---");
            listaPudelek.Sort(Pudelko.Comparison);
            foreach (var i in listaPudelek)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("---");
            Console.WriteLine("Tworzenie pudelka przez '+':");
            Console.WriteLine("---");
            Console.WriteLine(pudeleczko1+pudeleczko2);
        }
    }
}