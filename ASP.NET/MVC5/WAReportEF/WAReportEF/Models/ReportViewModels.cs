using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace WAReportEF.Models
{
    public class ReportView
    {
        public IEnumerable<Report> Reports { get; set; }
        public ReportFilter Filter { get; set; }
        public SelectList Locals { get; set; }
    }

    [Table("Report", Schema = "dbo")]
    public class Report
    {
        [Key]
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Data")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Użytkownik")]
        [StringLength(50)]
        public string User { get; set; }

        [Required]
        [Display(Name = "Lokal")]
        [StringLength(50)]
        public string Local { get; set; }
    }

    public class ReportFilter
    {
        [Display(Name = "Lokal")]
        public string Local { get; set; }

        [Display(Name = "Data od")]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Data do")]
        public DateTime? DateTo { get; set; }
    }
}