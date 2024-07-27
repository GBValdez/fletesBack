using System.ComponentModel.DataAnnotations;

namespace project.utils.catalogues.dto
{
    public class catalogueCreationDto
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        public string name { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(255, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        public string description { get; set; }
    }
}
