using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using WM.SysProductos.BL;
using WM.SysProductos.EN;

namespace WM.SysProductos.AppWeb.Controllers
{
    public class ClienteController : Controller
    {
        readonly ClienteBL _clienteBL;

        public ClienteController(ClienteBL pClienteBL)
        {
            _clienteBL = pClienteBL;
        }
        // GET: ClienteController
        public async Task<ActionResult> Index()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            return View(clientes);
        }

        // GET: ClienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cliente pCliente)
        {
            try
            {
                var result = await _clienteBL.CrearAsync(pCliente);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(new Cliente { Id = id });
            return View(cliente);
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Cliente pCliente)
        {
            try
            {
                var result = await _clienteBL.ModificarAsync(pCliente);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(new Cliente { Id = id });
            return View(cliente);
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EliminarCliente(int id)
        {
            try
            {
                var result = await _clienteBL.EliminarAsync(new Cliente { Id = id });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult> ReporteClientes()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            return new ViewAsPdf("rpCliente", clientes);
        }

        public async Task<IActionResult> GenerarReportePDF()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            return new ViewAsPdf("rpCliente", clientes)
            {
                FileName = "Reporte_Clientes.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--footer-center [page]"
            };
        }

        public async Task<IActionResult> ReporteClientesExcel()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            using (var package = new ExcelPackage())
            {
                var hojaExcel = package.Workbook.Worksheets.Add("Clientes");

                hojaExcel.Cells["A1"].Value = "Nombre";
                hojaExcel.Cells["B1"].Value = "Dirección";
                hojaExcel.Cells["C1"].Value = "Telefono";
                hojaExcel.Cells["D1"].Value = "Email";

                int row = 2;
                foreach (var cliente in clientes)
                {
                    hojaExcel.Cells[row, 1].Value = cliente.Nombre;
                    hojaExcel.Cells[row, 2].Value = cliente.Direccion;
                    hojaExcel.Cells[row, 3].Value = cliente.Telefono;
                    hojaExcel.Cells[row, 4].Value = cliente.Email;

                    row++;
                }

                hojaExcel.Cells["A:D"].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteClientesExcel.xlsx");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubirExcelClientes(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                return RedirectToAction("Index");
            }

            var clientes = new List<Cliente>();

            using (var stream = new MemoryStream())
            {
                await archivoExcel.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var hojaExcel = package.Workbook.Worksheets[0];
                    int rowCount = hojaExcel.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var nombre = hojaExcel.Cells[row, 1].Text;
                        var direccion = hojaExcel.Cells[row, 2].Text;
                        var telefono = hojaExcel.Cells[row, 3].Text;
                        var email = hojaExcel.Cells[row, 4].Text;

                        if (string.IsNullOrEmpty(nombre))
                            continue;

                        clientes.Add(new Cliente
                        {
                            Nombre = nombre,
                            Direccion = direccion,
                            Telefono = telefono,
                            Email = email
                        });
                    }
                }

                if (clientes.Count > 0)
                {
                    await _clienteBL.AgregarTodosAsync(clientes);
                }
                return RedirectToAction("Index");
            }
        }
    }
}
