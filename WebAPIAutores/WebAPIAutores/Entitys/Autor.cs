using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Entitys
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido no puede dejarlo vacio") ]
        public string Name { get; set; }
        [Url]
        [NotMapped]
        public string URL{ get; set; }
        public List<Libro> Libros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var primeraLetra = Name[0].ToString();

                if(primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe se mayuscula", new string[] { nameof(Name) });
                }
            }
        }
    }
}