namespace WANinject.Models
{
    public interface IProdukt
    {
        string Name { get; set; }
        decimal Wartosc { get; set; }
        decimal WartoscZRabatem { get; }
    }
}
