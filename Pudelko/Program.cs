using System;
using System.Threading.Tasks;
using static PudelkoLibrary.Pudelko.UnitOfMeasure;

namespace PudelkoLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Pudelko pudeleczko1 = new Pudelko(2.5, 9.321);
            
            Console.WriteLine(pudeleczko1.ToString());
            Console.WriteLine(pudeleczko1.ToString("m"));
            Console.WriteLine(pudeleczko1.ToString("cm"));
            Console.WriteLine(pudeleczko1.ToString("mm"));
            Console.WriteLine($"Objetosc: {pudeleczko1.Objetosc}");
            Console.WriteLine($"Pole: {pudeleczko1.Pole}");
            
            Pudelko pudeleczko2 = new Pudelko(2100, 3050,1000, Millimeter);
            
            Console.WriteLine($"Czy dlugosci bokow [{pudeleczko1.ToString()}] sa takie same jak w [{pudeleczko2.ToString()}] : {pudeleczko1.Equals(pudeleczko2)}");
            Console.WriteLine(String.Join(" | ",(double[])pudeleczko2));
            Console.WriteLine(pudeleczko1[0]);
            
            foreach (var d in pudeleczko1)
            {
                Console.WriteLine(d);
            }
            Console.WriteLine(Pudelko.Parse("210 cm × 305 cm × 100 cm").ToString());
            Console.WriteLine(new Pudelko(2.5, 9.321, 0.1) == Pudelko.Parse("2.500 m × 9.321 m × 0.100 m"));
            
            
        }
    }
}