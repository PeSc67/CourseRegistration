using System.ComponentModel.DataAnnotations;

namespace CourseRegistration.Models
{
    public class Student
    {

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Förnamn (First name)")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Efternamn (Last name)")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Stad (City)")]

        public string City { get; set; }

        [StringLength(30)]
        [Display(Name = "Telefon (Phone)")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Epost (Email)")]
        public string Email { get; set; }

        [StringLength(500)]
        [Display(Name = "Övrig (Note)")]
        public string Note { get; set; }






    }
}
