namespace MSMove.Common.Interfaces
{
    public interface IWrapper
    {
        /// <summary>
        /// Umożliwia aplikacji informowanie systemu, że jest w użyciu, zapobiegając w ten sposób
        /// przejściu systemu w stan uśpienia lub wyłączeniu wyświetlacza podczas działania aplikacji.
        /// </summary>
        /// <param name="noSleepOrTurnOff">Wratość 'true' zapobiega przejściu systemu w stan uśpienia lub wyłączeniu wyświetlacza.</param>
        void Display(bool noSleepOrTurnOff);

        bool Move();

        /// <summary>
        /// Nazwa klasy.
        /// </summary>
        string ClassName
        { get; }

        /// <summary>
		/// Podpis okna.
		/// </summary>
		string WindowName
        { get; }

        /// <summary>
        /// Przywraca stany, np. jeśli okno było zminimalizowane to do takiego stanu zostanie przywrócone.
        /// </summary>
        bool RollbackState
        { get; }
    }
}
