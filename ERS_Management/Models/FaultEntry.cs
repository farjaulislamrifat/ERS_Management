using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERS_Management.Models
{
    public class FaultEntry
    {
        // Primary key – auto-incremented by the DB
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }

        // When the fault was reported
        public DateTime? FaultTime { get; set; }

        [Required]
        [StringLength(100)]
        public string ReportedBy { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Site { get; set; } = string.Empty;

        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Resolution { get; set; } = string.Empty;

        [StringLength(100)]
        public string ResolvedBy { get; set; } = string.Empty;

        public DateTime? ResolvedTime { get; set; }


        [NotMapped]
        public double? Duration => ResolvedTime.HasValue && FaultTime.HasValue
            ? (ResolvedTime.Value - FaultTime.Value).TotalHours
            : null;

        [StringLength(2000)]
        public string Remarks { get; set; } = string.Empty;


        public string Username { get; set; } = string.Empty;
    }
}
