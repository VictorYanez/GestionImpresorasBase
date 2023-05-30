using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionImpresoras.ViewModels
{
    public class ModeloViewModel
    {
        public Modelo vModelo { get; set; }
        public List<SelectListItem> vListaMarcas { get; set; }
    }
}
