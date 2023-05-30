using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionImpresoras.Models
{
    [Table("Impresora")]
     public class Impresora
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "No. Activo Fijo")] 
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(22)]
        public string  CodigoActivoFijo { get; set; }

        //Marca y Modelo poseen relación 
        public int MarcaId { get; set; }
        [Display(Name = "Marca")]
        public virtual Marca Marca { get; set; } = null!; //Perdonar el nulo? y Ademas propiedad de navegacion 

        public int ModeloId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Modelo")]
        public virtual Modelo Modelo { get; set; } = null!; 

        public int EstadoId { get; set; }  
        [Display(Name = "Estado")]
        public virtual Estado Estado { get; set; } = null!; 

        [StringLength(15)]
        [Display(Name = "IP")]
        public string DireccionIP { get; set; }

        //Area y Unidad poseen relación 
        public int AreaId { get; set; }
        [Display(Name = "Area")]
        public virtual Area Area { get; set; } = null!; //Perdonar el nulo? y Ademas propiedad de navegacion 

        [Range(1,int.MaxValue, ErrorMessage ="El campo {0} es requerido")]
        public int UnidadId { get; set; }
        [Display(Name = "Unidad")]
        public virtual Unidad Unidad { get; set; } = null!;

        //  --------------------------------
        public int InstitucionId { get; set; }
        [Display(Name = "Institucion")]
        public virtual Institucion Institucion { get; set; } = null!;

        //[StringLength(125)]
        //public String Responsable { get; set; }

        //[Display(Name = "Fecha Reg.")]
        //[Column(TypeName = "Date")]
        //public DateTime? FechaRegistro { get; set; }  //El signo ? en DateTime expresa que puede almacenar nulo 

        [StringLength(200)]
        public string Caracteristicas { get; set; }
    }
}
