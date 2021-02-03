using System.Diagnostics;

namespace WANinject.Models
{
    public class Produkt : IProdukt
    {
        private IRabat _rabat;

        public Produkt(IRabat rabat)
        {
            _rabat = rabat;
            Debug.WriteLine("Utworzono obiekt Produkt.");
        }

        public string Name { get; set; }
        public decimal Wartosc { get; set; }
        public decimal WartoscZRabatem
        {
            get
            {
                return _rabat.Oblicz(Wartosc);
            }
        }
    }
}