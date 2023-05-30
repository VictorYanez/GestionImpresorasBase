using GestionImpresoras.Data;
using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionImpresoras.Controllers
{
    public class InstitucionesController : Controller
    {
        private readonly ApplicationDBContext _contexto;

        public InstitucionesController(ApplicationDBContext contexto)
        {
            this._contexto = contexto;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listaInstituciones = await _contexto.Instituciones.ToListAsync();
            return View(listaInstituciones);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Institucion institucion)
        {
            if (ModelState.IsValid)
            {
                _contexto.Instituciones.Add(institucion);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");    // RedirectToAction(nameof(Index))
            }
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            else
            {
                var contacto = _contexto.Instituciones.Find(id);
                if (contacto == null)
                {
                    return RedirectToAction("Noencontrado", "Home");
                }
                return View(contacto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Institucion institucion)
        {
            if (ModelState.IsValid)
            {
                _contexto.Instituciones.Update(institucion);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));    //  RedirectToAction("Index")
            }
            return View();
        }

        [HttpGet]
        public IActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            else
            {
                var contacto = _contexto.Instituciones.Find(id);
                if (contacto == null)
                {
                    return RedirectToAction("Noencontrado", "Home");
                }
                return View(contacto);
            }
        }

        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrarInstitucion(int? id)
        {
            var institucion = await _contexto.Instituciones.FindAsync(id);
            if (institucion == null)
            {
                return View();
            }
            else _contexto.Instituciones.Remove(institucion);
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));    //  RedirectToAction("Index")
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            else
            {
                var contacto = _contexto.Instituciones.Find(id);
                if (contacto == null)
                {
                    return RedirectToAction("Noencontrado", "Home");
                }
                return View(contacto);
            }
        }
    }
}
