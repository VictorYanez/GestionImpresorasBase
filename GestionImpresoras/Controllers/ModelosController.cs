using GestionImpresoras.Data;
using GestionImpresoras.Models;
using GestionImpresoras.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionImpresoras.Controllers
{
    public class ModelosController : Controller
    {
        private readonly ApplicationDBContext _contexto;

        public ModelosController(ApplicationDBContext contexto)
        {
            this._contexto = contexto;
        }

        //Endpoint para la visualización del Listado 
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lista = await _contexto.Modelos.Include(x => x.Marca).ToListAsync();
            return View(lista);
        }

        //Endpoints para la creación (Insert) de registro 
        [HttpGet]
        public IActionResult Crear()
        {
            {
                ViewBag.MarcaId = _contexto.Marcas.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
                ViewBag.ModeloId = _contexto.Modelos.Where(m => m.MarcaId == 0).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
                return View();
            }
        }

            [HttpPost]
            [ValidateAntiForgeryToken]  //Para validar ataques 
            public async Task<IActionResult> Crear(Modelo modelo)
            {
                if (ModelState.IsValid)
                {
                    _contexto.Modelos.Add(modelo);
                    await _contexto.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View();
            }

            // Este codigo es el que funciona 
            public JsonResult GetModelos(int MarcaId)
            {
                var modelos = _contexto.Modelos.Where(m => m.MarcaId == MarcaId).Select(m => new { id = m.Id, nombre = m.Nombre }).ToList();
                return Json(modelos);
            }

        // Endpoint para la Edición de un registro
        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var modeloDisplay = await _contexto.Modelos.FindAsync(id);
            if (modeloDisplay == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            ViewBag.MarcaId = new SelectList(_contexto.Marcas, "Id", "Nombre", modeloDisplay.MarcaId);

            return View(modeloDisplay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,MarcaId,Nombre,Descripcion")] Modelo modelo)
        {
            if (id != modelo.Id)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contexto.Update(modelo);
                    await _contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModeloExists(modelo.Id))
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

            ViewBag.MarcaId = new SelectList(_contexto.Marcas, "Id", "Nombre", modelo.MarcaId);

            return View(modelo);
        }

        private bool ModeloExists(int id)
        {
            return _contexto.Modelos.Any(e => e.Id == id);
        }
        //Endpoints para el borrado (Delete) de registros
        [HttpGet]
        public async Task<IActionResult> Borrar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var modeloDisplay = await _contexto.Modelos.Include(m => m.Marca).FirstOrDefaultAsync(m => m.Id == id); 
            if (modeloDisplay == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            return View(modeloDisplay);
        }

        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]  //Para validar ataques 
        public async Task<IActionResult> BorrarModelo(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var modelo = await _contexto.Modelos.FindAsync(id);
            if (modelo == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            // Borrado de registro
            _contexto.Modelos.Remove(modelo);
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Endpoint para el desplegar (Details) de registros
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            var modeloDisplay = await _contexto.Modelos.Include(m => m.Marca).FirstOrDefaultAsync(m => m.Id == id);
            if (modeloDisplay == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            return View(modeloDisplay);
        }

    }
}
