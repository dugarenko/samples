using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SportsStore.Domain.Models
{
    [MetadataType(typeof(AddressMetadata)), Table("Address", Schema = "dbo")]
    public partial class Address
    { }
    public partial class AddressMetadata
    {
        [Key, HiddenInput]
        public int IdAddress { get; set; }
        public int IdCustomer { get; set; }

        [Required, StringLength(50)]
        public string Street { get; set; }
        [Required, StringLength(15)]
        public string PostalCode { get; set; }
        [Required, StringLength(50)]
        public string City { get; set; }
    }

    [MetadataType(typeof(CategoryMetadata)), Table("Category", Schema = "dbo")]
    public partial class Category
    { }
    public partial class CategoryMetadata
    {
        public int IdCategory { get; set; }

        [Required, StringLength(50), Display(Name = "Nazwa kategorii", ShortName = "Kategoria")]
        public string Name { get; set; }
    }

    [MetadataType(typeof(CustomerMetadata)), Table("Customer", Schema = "dbo")]
    public partial class Customer
    { }
    public partial class CustomerMetadata
    {
        [Key, HiddenInput]
        public int IdCustomer { get; set; }

        [Required, StringLength(250)]
        public string Nazwa { get; set; }
    }

    [MetadataType(typeof(OrderMetadata)), Table("Order", Schema = "dbo")]
    public partial class Order
    { }
    public partial class OrderMetadata
    {
        [Key, HiddenInput]
        public int IdOrder { get; set; }
        public int IdCustomer { get; set; }

        [Required, DataType(DataType.Date), Display(Name = "Data zamówienia", ShortName = "Data")]
        public DateTime OrderDate { get; set; }
    }

    [MetadataType(typeof(OrderDetailMetadata)), Table("OrderDetail", Schema = "dbo")]
    public partial class OrderDetail
    { }
    public partial class OrderDetailMetadata
    {
        [Key, HiddenInput]
        public int IdOrderDetail { get; set; }
        public int IdOrder { get; set; }
        public int IdProduct { get; set; }

        [Required, Display(Name = "Cena jednostkowa", ShortName = "Cena jdn.")]
        public decimal UnitPrice { get; set; }
        [Required, Display(Name = "Iloœæ")]
        public short Quantity { get; set; }
    }

    [MetadataType(typeof(ProductMetadata)), Table("Product", Schema = "dbo")]
    public partial class Product
    { }
    public partial class ProductMetadata
    {
        [Key, HiddenInput]
        public int IdProduct { get; set; }
        public int IdCategory { get; set; }

        [Required, StringLength(250), Display(Name = "Nazwa produktu", ShortName = "Produkt")]
        public string Name { get; set; }
        [Required, Display(Name = "Cena jednostkowa", ShortName = "Cena jdn.")]
        public decimal UnitPrice { get; set; }
    }
}
