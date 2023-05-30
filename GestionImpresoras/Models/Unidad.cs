using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImpresoras.Models
{
    [Table("Unidad")]
    public class Unidad
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public String Nombre { get; set; }

        public int AreaId { get; set; }
        [Display(Name = "Area")]
        public virtual Area Area { get; set; } = null!; //Perdonar el nulo? y Ademas propiedad de navegacion 

        [StringLength(200)]
        public string Descripcion { get; set; }

    }

}
