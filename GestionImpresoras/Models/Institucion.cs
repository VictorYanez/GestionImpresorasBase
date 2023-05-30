using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImpresoras.Models
{
    [Table("Institucion")]
    public class Institucion
    {
        [Key]   
        public int Id { get; set; }
        //[Remote(action: "VerificarExisteInstitucion", controller: "Instituciones", ErrorMessage = "Validacion Remota funionando")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public String Nombre { get; set; }

        public String Descripcion { get; set; }
    }
}
