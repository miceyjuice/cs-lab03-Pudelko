using System;

namespace PudelkoLibrary
{
    public static class Kompresja
    {
        public static Pudelko Kompresuj(Pudelko p)
        {
            var newLength = Math.Pow(p.Objetosc, (1D/3));
            var nowePudelko = new Pudelko(newLength, newLength, newLength);
            
            return nowePudelko;
        }
    }
}