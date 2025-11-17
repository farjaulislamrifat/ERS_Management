using System.ComponentModel.DataAnnotations;

namespace ERS_Management.Models.Enums
{
    public enum EntryStatus
    {
        [Display(Name = "Running")]
        Running = 0,

        [Display(Name = "Damaged")]
        Damaged = 1,

        [Display(Name = "Standby")]
        Standby = 2,

        [Display(Name = "Retired")]
        Retired = 3
    }

    public enum LogAction
    {
        Created,
        Updated,
        Deleted
    }

}
