using GestionImpresoras.Data;
using GestionImpresoras.Models;
using GestionImpresoras.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionImpresoras.Controllers
{
    public class UnidadesController : Controller
    {
        private readonly ApplicationDBContext _contexto;

        public UnidadesController(ApplicationDBContext contexto)
        {
            this._contexto = contexto;
        }

        //Endpoint para la visualización del Listado 
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Uso de carga diligente con Include
            var lista = await _contexto.Unidades.Include(x => x.Area).ToListAsync();
            return View(lista);
        }

        //Endpoints para la creación (Insert) de registro 
        [HttpGet]
        public IActionResult Crear()
        {
            {
                ViewBag.MarcaId = _contexto.Areas.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
                ViewBag.ModeloId = _contexto.Unidades.Where(m => m.AreaId == 0).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  //Para validar ataques 
        public async Task<IActionResult> Crear(Unidad unidad)
        {
            if (ModelState.IsValid)
            {
                _contexto.Unidades.Add(unidad);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        // Este codigo es el que funciona 
        public JsonResult GetUnidades(int AreaId)
        {
            var unidades = _contexto.Unidades.Where(m => m.AreaId == AreaId).Select(m => new { id = m.Id, nombre = m.Nombre }).ToList();
            return Json(unidades);
        }

        // Endpoint para la Edición de un registro
        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var unidadDisplay = await _contexto.Unidades.FindAsync(id);
            if (unidadDisplay == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            ViewBag.MarcaId = new SelectList(_contexto.Areas, "Id", "Nombre", unidadDisplay.AreaId);

            return View(unidadDisplay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,AreaId,Nombre,Descripcion")] Unidad unidad)
        {
            if (id != unidad.Id)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contexto.Update(unidad);
                    await _contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnidadExists(unidad.Id))
                    {
                        return RedirectToAction("Noencontrado", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MarcaId = new SelectList(_contexto.Marcas, "Id", "Nombre", unidad.AreaId);

            return View(unidad);
        }

        private bool UnidadExists(int id)
        {
            return _contexto.Unidades.Any(e => e.Id == id);
        }


        //Endpoints para el borrado (Delete) de registros
        [HttpGet]
        public async Task<IActionResult> Borrar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var unidadDisplay = await _contexto.Unidades.Include(m => m.Area).FirstOrDefaultAsync(m => m.Id == id);
            if (unidadDisplay == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            return View(unidadDisplay);
        }

        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]  //Para validar ataques 
        public async Task<IActionResult> BorrarModelo(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var unidadDisplay = await _contexto.Unidades.FindAsync(id);
            if (unidadDisplay == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            // Borrado de registro
            _contexto.Unidades.Remove(unidadDisplay);
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Endpoints para el desplegar (Details) de registros
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            var unidadDisplay = await _contexto.Unidades.Include(m => m.Area).FirstOrDefaultAsync(m => m.Id == id);
            if (unidadDisplay == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            return View(unidadDisplay);
        }
    }
}
