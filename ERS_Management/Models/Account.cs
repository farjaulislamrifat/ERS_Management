using System.ComponentModel.DataAnnotations;

namespace ERS_Management.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Admin,
        User
    }
}
