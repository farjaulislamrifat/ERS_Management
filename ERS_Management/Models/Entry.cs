using ERS_Management.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ERS_Management.Models
{
    public class Entries
    {
        [Key]
        public int SerialNumber { get; set; } // S/N

        [Required]
        [StringLength(50)]
        public string Area { get; set; } // AREA (e.g., Janjira, Mawa)

        [Required]
        [StringLength(100)]
        public string Location { get; set; } // LOCATION (e.g., Admin Room, ITS Room)

        [StringLength(50)]
        public string Brand { get; set; } // BRAND (e.g., LG, Cisco)

        [StringLength(50)]
        public string Model { get; set; } // MODEL (e.g., 27MP400, SF95-24)

        [Required]
        [StringLength(100)]
        public string ItemType { get; set; } // ITEM TYPE (e.g., Monitor, Optical Ethernet Switch)

        [StringLength(200)]
        public string UserPurpose { get; set; } // USER/USING PURPOSE (e.g., Universal Use (VMS))

        [StringLength(50)]
        public string AssetNo { get; set; } // Asset NO (e.g., PA-JJ-AD-PHOTOCOPIER-01)

        [StringLength(50)]
        public string SerialNo { get; set; } // S/L NO. (e.g., 302NTBK8L869)

        [Required]
        public int Quantity { get; set; } // QTY

        [Required]
        public EntryStatus Status { get; set; } // STATUS (e.g., Running, Damaged, Standby)

        public string Remarks { get; set; } // REMARKS (optional)

        public string Username { get; set; } // USERNAME (optional)

        public DateTime CreatedAt { get; set; } // CREATED AT

        public DateTime? LastUpdate { get; set; } // LAST UPDATE (optional, as some are empty)
    }
}
