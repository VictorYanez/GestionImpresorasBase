using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImpresoras.Models
{
    [Table("Estado")]
    public class Estado
    {
        [Key]       
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public String Nombre { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }
    }
}
