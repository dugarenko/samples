using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Web.Routing;

namespace SportsStore.WebUI.Infrastructure
{
    /// <summary>
    /// Reprezentuje filtr produktów.
    /// </summary>
    public class ProductFilter
    {
        private int? _pageNumber = 1;
        private int _pageSize = 8;

        /// <summary>
        /// Określa sposób sortowania danych.
        /// </summary>
        public SortOrder SortOrder { get; set; }
        /// <summary>
        /// Nazwa kolumny według której nastąpi sortowanie danych.
        /// </summary>
        public string SortColumn { get; set; }
        /// <summary>
        /// Określa numer strony.
        /// </summary>
        public int? PageNumber
        {
            get
            {
                return _pageNumber ?? 1;
            }
            set
            {
                _pageNumber = value;
            }
        }
        /// <summary>
        /// Ilość wyswietlanych elementów na jednej stronie.
        /// </summary>
        internal int PageSize
        {
            get
            {
                return _pageSize < 1 ? 1 : _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        /// <summary>
        /// Nazwa produktu według której nastąpi filtrowanie (wyszukanie) danych.
        /// </summary>
        [Display(Name = "Nazwa produktu")]
        public string ProductName { get; set; }
        /// <summary>
        /// Nazwa kategorii według której nastąpi filtrowanie (wyszukanie) danych.
        /// </summary>
        [Display(Name = "Nazwa katrgorii")]
        public string CategoryName { get; set; }
        /// <summary>
        /// Cena jednostkowa według której nastąpi filtrowanie (wyszukanie) danych.
        /// </summary>
        [Display(Name = "Cena jednostkowa", ShortName = "Cena jdn.")]
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Zwraca wartości trasy.
        /// </summary>
        /// <param name="sortColumn">Kolumna według której nastąpi sortowanie danych.</param>
        /// <param name="invertSortOrder">Określa czy odwrócić wartość sortowania.</param>
        public RouteValueDictionary GetRouteValues<T>(Expression<Func<T>> sortColumn, bool invertSortOrder)
        {
            RouteValueDictionary rvd = new RouteValueDictionary();

            if (invertSortOrder)
            {
                rvd.Add("SortOrder", (SortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending);
            }
            else
            {
                rvd.Add("SortOrder", SortOrder);
            }

            rvd.Add("SortColumn", PropertyInfoEx.GetPropertyName(sortColumn));
            rvd.Add("Page", PageNumber ?? 1);
            rvd.Add("ProductName", ProductName);
            rvd.Add("CategoryName", CategoryName);
            rvd.Add("UnitPrice", UnitPrice);

            return rvd;
        }
    }
}