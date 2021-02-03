using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WAPagedList.Models
{
    [MetadataType(typeof(KlientMetadata))]
    public partial class Klient
    { }
    public class KlientMetadata
    {
        [HiddenInput]
        public int IdKlient { get; set; }

        [Required]
        [Display(Name = "Nazwa")]

        [StringLength(250)]
        public string Nazwa { get; set; }

        public virtual ICollection<Adres> Adres { get; set; }
    }

    [MetadataType(typeof(AdresMetadata))]
    public partial class Adres
    { }
    public class AdresMetadata
    {
        [HiddenInput]
        public int IdAdres { get; set; }

        [HiddenInput]
        public int IdKlient { get; set; }

        [Required]
        [Display(Name = "Ulica")]
        [StringLength(250)]
        public string Ulica { get; set; }

        [Display(Name = "Numer budynku")]
        [StringLength(15)]
        public string NumerBudynku { get; set; }

        public virtual Klient Klient { get; set; }
    }
}