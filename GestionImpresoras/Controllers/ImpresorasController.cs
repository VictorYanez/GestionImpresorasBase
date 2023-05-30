using System.Web;

using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc;
using GestionImpresoras.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using GestionImpresoras.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionImpresoras.Controllers
{
    public class ImpresorasController : Controller
    {
        private readonly ApplicationDBContext _contexto;

        public ImpresorasController(ApplicationDBContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lista = await _contexto.Impresoras
                .Include(x => x.Marca)
                .Include(x => x.Modelo)
                .Include(x => x.Estado).ToListAsync();
            return View(lista);
        }

        // GET: Impresoras/Creater
        public IActionResult Crear()
        {
            ViewBag.EstadoId = _contexto.Estados.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre }).ToList();
            ViewBag.MarcaId = _contexto.Marcas.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            ViewBag.ModeloId = _contexto.Modelos.Where(m => m.MarcaId == 0).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            ///<!----------------------  Segundo Grupo de SelectListItems --------------------------->
            ViewBag.InstitucionId = _contexto.Instituciones.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre }).ToList();
            ViewBag.AreaId = _contexto.Areas.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nombre }).ToList();
            ViewBag.UnidadId = _contexto.Unidades.Where(a => a.AreaId == 0).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nombre }).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                _contexto.Update(impresora);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  //  return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  //Para validar ataques 
        public async Task<IActionResult> Creater([Bind("Id,CodigoActivoFijo,MarcaId,ModeloId,EstadoId,DireccionIP")] Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                _contexto.Impresoras.Add(impresora);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }

        // Devolver listados de Modelos correpondientes a dicha marca
        public JsonResult GetModelos(int marcaId)
        {
            var modelos = _contexto.Modelos.Where(m => m.MarcaId == marcaId).Select(m => new { id = m.Id, nombre = m.Nombre }).ToList();
            return Json(modelos);
        }

        // Devolver listados de unidades correpondientes a dicha área
        public JsonResult GetUnidades(int areaId)
        {
            var unidades = _contexto.Unidades.Where(a => a.AreaId == areaId).Select(a => new { id = a.Id, nombre = a.Nombre }).ToList();
            return Json(unidades);
        }

        [HttpGet]
        public IActionResult CrearViewModel()
        {
            ImpresoraViewModel impresoraViewModel = new()
            {
                vImpresora = new Impresora(),
                vListaEstado = _contexto.Estados.Select(estado => new SelectListItem()
                {
                    Text = estado.Nombre,
                    Value = estado.Id.ToString()
                }).ToList(),
                vListaMarca = _contexto.Marcas.Select(marca => new SelectListItem()
                {
                    Text = marca.Nombre,
                    Value = marca.Id.ToString()
                }).ToList(),
                vListaModelo = _contexto.Modelos.Select(modelo => new SelectListItem()
                {
                    Text = modelo.Nombre,
                    Value = modelo.Id.ToString()
                }).ToList(),
                vListaArea = _contexto.Areas.Select(area => new SelectListItem()
                {
                    Text = area.Nombre,
                    Value = area.Id.ToString()
                }).ToList(),
                vListaUnidad = _contexto.Unidades.Select(unidad => new SelectListItem()
                {
                    Text = unidad.Nombre,
                    Value = unidad.Id.ToString()
                }).ToList()
            };

            return View(impresoraViewModel);
        }

        // Codigo Video Hector de Leon 
        [HttpGet]
        public ActionResult<List<SelectListItem>> ListaModelos(int marcaId)
        {
            List<SelectListItem> listaMarcas = new List<SelectListItem>();

            if (marcaId != 0)
            {
                listaMarcas = (from m in _contexto.Modelos
                               where m.MarcaId == marcaId
                               select new SelectListItem
                               {
                                   Text = m.Nombre,
                                   Value = m.Id.ToString(),

                               }).ToList();
            }

            return View(listaMarcas);
        }

        // Codigo Video Codigo Oldest & FG
        [HttpGet("{marcaId}/modelo")]
        // Codigo FG  para listado de Modelos en vistas Blazor
        public async Task<List<Modelo>> WhenMarcaChanged2(int marcaId)
        {
            List<SelectListItem> listaMarcas = new List<SelectListItem>();

            {
                return await _contexto.Modelos.Where(m => m.MarcaId == marcaId).OrderBy(m => m.Nombre).ToListAsync();
            }
        }

        // GET: Impresoras/Editar
        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var impresora = await _contexto.Impresoras.FindAsync(id);
            if (impresora == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            //  Campos de marcas y Modelos 
            ViewData["MarcaId"] = new SelectList(_contexto.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewData["ModeloId"] = new SelectList(_contexto.Modelos, "Id", "Nombre", impresora.ModeloId);
            //  Campos de Area y Unidad 
            ViewData["AreaId"] = new SelectList(_contexto.Areas, "Id", "Nombre", impresora.AreaId);
            ViewData["UnidadId"] = new SelectList(_contexto.Unidades, "Id", "Nombre", impresora.UnidadId);
            //  Campos de Estado e Institución  
            ViewData["EstadoId"] = new SelectList(_contexto.Estados, "Id", "Nombre", impresora.EstadoId);
            ViewData["InstitucionId"] = new SelectList(_contexto.Instituciones, "Id", "Nombre", impresora.InstitucionId);

            return View(impresora);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                _contexto.Update(impresora);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  //  return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Editar2(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            var existe = _contexto.Impresoras.Any(e => e.Id == id);      
            if (!existe)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            var impresoraEdit = await _contexto.Impresoras.FindAsync(id);
            ViewBag.MarcaId = new SelectList(_contexto.Marcas, "Id", "Nombre", impresoraEdit.MarcaId);
            ViewBag.ModeloId = new SelectList(_contexto.Modelos.Where(m => m.MarcaId == impresoraEdit.MarcaId), "Id", "Nombre", impresoraEdit.ModeloId);
            ViewBag.EstadoId = new SelectList(_contexto.Estados, "Id", "Nombre", impresoraEdit.EstadoId);
           
            ViewBag.AreaId = new SelectList(_contexto.Areas, "Id", "Nombre", impresoraEdit.AreaId);
            ViewBag.UnidadId = new SelectList(_contexto.Unidades.Where(a => a.AreaId == impresoraEdit.AreaId), "Id", "Nombre", impresoraEdit.UnidadId);
            ViewBag.InstitucionId = new SelectList(_contexto.Instituciones, "Id", "Nombre", impresoraEdit.InstitucionId);
            return View(impresoraEdit);
        }

        // Acción para mostrar el formulario de creación codigo Copiloto 
        // Acción para mostrar el formulario de edición
        public IActionResult EditCop(int id)
        {
            var impresora = _contexto.Impresoras.Find(id);
            if (impresora == null)
            {
                return NotFound();
            }

            ViewBag.MarcaId = _contexto.Marcas.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            ViewBag.ModeloId = _contexto.Modelos.Where(m => m.MarcaId == 0).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            ///<!----------------------  Segundo Grupo de SelectListItems --------------------------->

            ViewBag.AreaId = _contexto.Areas.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nombre }).ToList();
            ViewBag.UnidadId = _contexto.Unidades.Where(a => a.AreaId == 0).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Nombre }).ToList();
            ///<!----------------------  Segundo Grupo de SelectListItems --------------------------->
            ViewBag.EstadoId = _contexto.Estados.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre }).ToList();
            ViewBag.InstitucionId = _contexto.Instituciones.Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nombre }).ToList();

            ViewBag.Marcas = new SelectList(_contexto.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewBag.Modelos = new SelectList(_contexto.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            ViewBag.Estados = new SelectList(_contexto.Estados, "Id", "Nombre", impresora.EstadoId);

            return View(impresora);
        }

        //<!----------------    (IA)   --------------------------->
        // GET: Impresoras/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_contexto.Estados, "Id", "Nombre");
            ViewData["MarcaId"] = new SelectList(_contexto.Marcas, "Id", "Nombre");
            ViewData["ModeloId"] = new SelectList(_contexto.Modelos.Where(m => m.MarcaId == 0), "Id", "Nombre");
            return View();
        }

        // POST: Impresoras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodigoActivoFijo,MarcaId,ModeloId,EstadoId,DireccionIP")] Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                _contexto.Add(impresora);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_contexto.Estados, "Id", "Nombre", impresora.EstadoId);
            ViewData["MarcaId"] = new SelectList(_contexto.Marcas, "Id", "Nombre", impresora.MarcaId);
            ViewData["ModeloId"] = new SelectList(_contexto.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            ViewBag.ModeloId = new SelectList(_contexto.Modelos.Where(m => m.MarcaId == impresora.MarcaId), "Id", "Nombre", impresora.ModeloId);
            return View(impresora);
        }

        //Endpoints para el borrado (Delete) de registros
        [HttpGet]
        public async Task<IActionResult> Borrar(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            var impresoraDisplay = await _contexto.Impresoras
                .Include(ma => ma.Marca)
                .Include(mo => mo.Modelo)
                .Include(a => a.Area)
                .Include(u => u.Unidad)
                .Include(e => e.Estado)
                .Include(i => i.Institucion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (impresoraDisplay == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            return View(impresoraDisplay);
        }

        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]  //Para validar ataques 
        public async Task<IActionResult> BorrarImpresora(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }

            var impresora = await _contexto.Impresoras.FindAsync(id);
            if (impresora == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            // Borrado de registro
            _contexto.Impresoras.Remove(impresora);
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
            var impresoraDisplay = await _contexto.Impresoras
                .Include(ma => ma.Marca)
                .Include(mo => mo.Modelo)
                .Include(a => a.Area)
                .Include(u => u.Unidad)
                .Include(e => e.Estado)
                .Include(i => i.Institucion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (impresoraDisplay == null)
            {
                return RedirectToAction("Noencontrado", "Home");
            }
            return View(impresoraDisplay);
        }

        // Falla, podría ser el codigo JavaScript 
        [HttpGet]
        public JsonResult GetModelosByMarcaId(int marcaId)
        {
            var modelos = _contexto.Modelos.Where(m => m.MarcaId == marcaId).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            return Json(modelos);
        }

        //<!----------------    Copiloto --------------------------->
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}