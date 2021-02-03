using Pharmacy.Annotations.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Pharmacy.Domain.Models
{
    [Table("Producer")]
    public partial class Producer
    {
        public Producer()
        {
            Medicaments = new HashSet<Medicament>();
        }

        [Key]
        [Display(Name = nameof(Resources.IdProducer), ShortName = "ID", ResourceType = typeof(Resources))]
        public int IdProducer { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = nameof(Resources.ProducerName), ShortName = "Name", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        [StringLength(250)]
        [Display(Name = nameof(Resources.Description), ResourceType = typeof(Resources))]
        public string Description { get; set; }

        [InverseProperty(nameof(Medicament.IdProducerNavigation))]
        public virtual ICollection<Medicament> Medicaments { get; set; }
    }
}
