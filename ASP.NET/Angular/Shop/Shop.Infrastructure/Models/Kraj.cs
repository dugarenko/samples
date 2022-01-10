using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Shop.Infrastructure.Models
{
    [Table("Kraj")]
    [Index(nameof(KodKrajuISO2), Name = "UIX_Kraj__KodKrajuISO2", IsUnique = true)]
    [Index(nameof(KodKrajuISO3), Name = "UIX_Kraj__KodKrajuISO3", IsUnique = true)]
    public partial class Kraj
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Timestamp]
        [ConcurrencyCheck]
        public byte[] Znacznik { get; set; }
        [Required]
        [StringLength(100)]
        public string NazwaPolska { get; set; }
        [Required]
        [StringLength(100)]
        public string NazwaAngielska { get; set; }
        public bool UE { get; set; }
        [Required]
        [StringLength(3)]
        public string KodKrajuISO2 { get; set; }
        [Required]
        [StringLength(3)]
        public string KodKrajuISO3 { get; set; }
        [Required]
        [StringLength(3)]
        public string KodWalutyISO { get; set; }
    }
}
