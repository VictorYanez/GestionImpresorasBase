using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionImpresoras.ViewModels
{
    public class ImpresoraViewModel
    {
        public Impresora vImpresora { get; set; }

        //-----  Listas para la seleccion de Opciones -------------
        public List<SelectListItem> vListaEstado { get; set; }
        //---------------------------------------------------------
        public List<SelectListItem> vListaMarca { get; set; }
        public List<SelectListItem> vListaModelo { get; set; }
        //---------------------------------------------------------
        public List<SelectListItem> vListaArea { get; set; }
        public List<SelectListItem> vListaUnidad { get; set; }
    }
}
