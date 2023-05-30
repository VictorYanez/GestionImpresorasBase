using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImpresoras.Models
{
    [Table("Modelo")]
    public class Modelo
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public String Nombre { get; set; }

        public int MarcaId { get; set; }
        [Display(Name = "Marca")]
        public virtual Marca Marca { get; set; } = null!; //Perdonar el nulo? y Ademas propiedad de navegacion 

        //[Display(Name = "Color")]
        //public bool EsdeColor { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

    }
}
