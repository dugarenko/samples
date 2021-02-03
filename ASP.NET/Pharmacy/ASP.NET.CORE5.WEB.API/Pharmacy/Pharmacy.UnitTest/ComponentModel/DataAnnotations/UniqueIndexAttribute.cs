using System;

namespace Pharmacy.UnitTest.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Używany w klasie EntityFramework w celu oznaczenia właściwości, która ma być używana jako unikalny indeks.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class UniqueIndexAttribute : Attribute
    {
        /// <summary>
        /// Tworzy nową instancję klasy.
        /// </summary>
        /// <param name="groupName">Opcjonalna wartość, używana do grupowania wielu właściwości, w jeden połączony unikalny indeks.</param>
        /// <param name="order">Opcjonalna wartość, używana do sortowania właściwości jednostki, która jest częścią połączonego unikalnego indeksu.</param>
        public UniqueIndexAttribute(string groupName = null, int order = 0)
        {
            GroupName = groupName;
            Order = order;
        }

        /// <summary>
        /// Pobiera lub ustawia wartość, która używana jest do grupowania wielu właściwości, w jeden połączony unikalny indeks.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Pobiera lub ustawia wartość, która używana jest do sortowania właściwości jednostki, która jest częścią połączonego unikalnego indeksu.
        /// </summary>
        public int Order { get; set; }
    }
}
