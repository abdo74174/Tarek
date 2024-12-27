using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grand_Hall.Models
{
    [Table("Halls", Schema = "EventHall")]
    public class Hall
    {
        [Key]
        [Display(Name = "Halls ID")]
        public int HallsID { get; set; }

        [Required]
        [Display(Name = "Hall Name")]
        [Column(TypeName = "varchar(200)")]
        public string HallsName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Required]
        [Display(Name = "Price per Day")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Description")]
        [Column(TypeName = "text")]
        public string? Description { get; set; }

        [Display(Name = "Image Path")]
        [Column(TypeName = "varchar(500)")]
        public string? ImagePath { get; set; } // For storing image file paths

        // One-to-many relationship: A Hall can have many Reservations.
        //public ICollection<Reservation>? Reservations { get; set; }
    }
}
