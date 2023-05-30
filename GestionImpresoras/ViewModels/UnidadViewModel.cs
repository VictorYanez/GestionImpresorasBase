using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionImpresoras.ViewModels
{
    public class UnidadViewModel
    {
        public Unidad vUnidad { get; set; }
        public List<SelectListItem> vListaAreas { get; set; }
    }
}
