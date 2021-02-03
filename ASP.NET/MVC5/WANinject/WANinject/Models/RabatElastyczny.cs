namespace WANinject.Models
{
    public class RabatElastyczny : IRabat
    {
        public decimal WysokoscRabatu { get; set; }

        public decimal Oblicz(decimal wartosc)
        {
            return (wartosc - (WysokoscRabatu / 100m * wartosc));
        }
    }
}