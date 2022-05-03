using PudelkoLib;

namespace PudelkoApp
{
    public static class Extension
    {
        public static Pudelko Kompresuj(Pudelko p)
        {
            var side = Math.Pow(p.Objetosc, (double)1/3);
            return new Pudelko(side, side, side);
        }
    }
}
