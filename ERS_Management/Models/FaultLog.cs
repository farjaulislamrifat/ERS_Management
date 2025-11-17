using ERS_Management.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ERS_Management.Models
{
    public class FaultLog
    {
        [Key]
        public int Id { get; set; }

        public int FaultId { get; set; }
        public DateTime EntryTime { get; set; }
        public string EnteredBy { get; set; } = string.Empty;
        public LogAction logAction { get; set; }


    }

}
