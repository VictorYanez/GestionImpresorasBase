using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImpresoras.Models
{
    [Table("Area")]
    public class Area
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public String Nombre { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }
        public List<Unidad> Unidades { get; set; }
    }
}
