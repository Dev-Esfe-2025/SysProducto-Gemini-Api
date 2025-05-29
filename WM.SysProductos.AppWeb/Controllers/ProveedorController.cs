using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WM.SysProductos.BL;
using WM.SysProductos.EN;

namespace WM.SysProductos.AppWeb.Controllers
{
    public class ProveedorController : Controller
    {
        readonly ProveedorBL _proveedorBL;
        public ProveedorController(ProveedorBL pProveedorBL)
        {
            _proveedorBL = pProveedorBL;
        }
        // GET: ProveedorController
        public async Task<ActionResult> Index()
        {
            var proveedores = await _proveedorBL.ObtenerTodosAsync();
            return View(proveedores);
        }

        // GET: ProveedorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProveedorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProveedorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Proveedor pProveedor)
        {
            try
            {
                var result = await _proveedorBL.CrearAsync(pProveedor);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProveedorController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var proveedor = await _proveedorBL.ObtenerPorIdAsync(new Proveedor { Id = id });

            return View(proveedor);
        }

        // POST: ProveedorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Proveedor pProveedor)
        {
            try
            {
                var result = await _proveedorBL.ModificarAsync(pProveedor);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProveedorController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var proveedor = await _proveedorBL.ObtenerPorIdAsync(new Proveedor { Id = id });
            return View(proveedor);
        }

        // POST: ProveedorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProveedor(int id)
        {
            try
            {
                var resul = await _proveedorBL.EliminarAsync(new Proveedor { Id = id });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
