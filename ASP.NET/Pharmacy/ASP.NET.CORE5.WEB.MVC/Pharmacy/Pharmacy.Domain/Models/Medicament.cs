using Newtonsoft.Json;
using Pharmacy.Annotations.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pharmacy.Domain.Models
{
    [Table("Medicament")]
    [Index(nameof(IdProducer), Name = "IX_Medicament_IdProducer")]
    public partial class Medicament
    {
        [Key]
        [Display(Name = nameof(Resources.IdMedicament), ShortName = "ID", ResourceType = typeof(Resources))]
        public int IdMedicament { get; set; }

        [Required]
        [Display(Name = nameof(Resources.IdProducer), ShortName = "ID", ResourceType = typeof(Resources))]
        public int IdProducer { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = nameof(Resources.MedicamentName), ShortName = "Name", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        [Display(Name = nameof(Resources.Price), ResourceType = typeof(Resources))]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Price { get; set; }

        [StringLength(1500)]
        [Display(Name = nameof(Resources.Description), ResourceType = typeof(Resources))]
        public string Description { get; set; }

        [ForeignKey(nameof(IdProducer))]
        [InverseProperty(nameof(Producer.Medicaments))]
        [Display(Name = nameof(Resources.ProducerName), ShortName = "Name", ResourceType = typeof(Resources))]
        public virtual Producer IdProducerNavigation { get; set; }
    }
}
