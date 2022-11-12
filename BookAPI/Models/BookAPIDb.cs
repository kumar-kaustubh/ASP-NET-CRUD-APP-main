using System.ComponentModel.DataAnnotations;

namespace BookAPI.Models
{
    public class BookAPIDb
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter the Title.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Add Description.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please Add Salary.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please add Images.")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Please Add Author.")]
        public string Author { get; set; }

        public DateTime createdTime = DateTime.Now;
    }
}
