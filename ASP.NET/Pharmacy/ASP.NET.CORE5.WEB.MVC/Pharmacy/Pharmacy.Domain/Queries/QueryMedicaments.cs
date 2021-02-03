namespace Pharmacy.Domain.Queries
{
    /// <summary>
    /// Reprezentuje parametry zapytania Medicaments.
    /// </summary>
    public class QueryMedicaments
    {
        /// <summary>
        /// Pobiera lub ustawia identyfikator producenta.
        /// </summary>
        public int? IdProducer { get; set; }
        /// <summary>
        /// Pobiera lub ustawia cenę produktu.
        /// </summary>
        public decimal? Price { get; set; }
    }
}