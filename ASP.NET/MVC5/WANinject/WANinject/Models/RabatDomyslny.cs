namespace WANinject.Models
{
    public class RabatDomyslny : IRabat
    {
        private const decimal WYSOKOSC_RABATU = 10m;

        public decimal Oblicz(decimal wartosc)
        {
            return (wartosc - (WYSOKOSC_RABATU / 100m * wartosc));
        }
    }
}