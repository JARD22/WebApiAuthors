using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validators;

namespace WebApiAutores.Entities
{
    public class Author: IValidatableObject
    {
        public int id { get; set; }

        [Required(ErrorMessage ="Field {0} is required")]
        [StringLength(maximumLength:5, ErrorMessage = "Length should not exceed {1}")]
        [TitleCase]
        public string name{ get; set; }

        [Range(18,100)]
        [NotMapped]
        public int age { get; set; }

        public List<Book> Books { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var firstLetter = name[0].ToString();
                if (firstLetter != firstLetter.ToUpper())
                {
                    yield return new ValidationResult("the first letter must be capitalized",
                        new string[] {nameof(name)});
                }
            }
        }
    }
}
