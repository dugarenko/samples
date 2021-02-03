using Pharmacy.Annotations.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Pharmacy.Domain.Models
{
    [Table("City")]
    public partial class City
    {
        [Key]
        [Display(Name = nameof(Resources.IdCity), ShortName = "ID", ResourceType = typeof(Resources))]
        public int IdCity { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = nameof(Resources.CityName), ShortName = "Name", ResourceType = typeof(Resources))]
        public string Name { get; set; }
    }
}
