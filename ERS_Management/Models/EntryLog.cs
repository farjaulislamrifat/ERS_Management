using ERS_Management.Models.Enums;

namespace ERS_Management.Models
{
    public class EntryLog
    {
        public int Id { get; set; }
        public DateTime EntryTime { get; set; }
        public string EnteredBy { get; set; } = string.Empty;
        public LogAction logAction { get; set; }
        public int FaultId { get; set; }

    }

}
