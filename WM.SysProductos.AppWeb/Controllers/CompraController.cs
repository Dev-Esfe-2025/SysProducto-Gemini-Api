using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using WM.SysProductos.BL;
using WM.SysProductos.DAL;
using WM.SysProductos.EN;
using WM.SysProductos.EN.Filtros;
using static WM.SysProductos.EN.Compra;
using static WM.SysProductos.EN.Filtros.CompraFiltros;

namespace WM.SysProductos.AppWeb.Controllers
{
    public class CompraController : Controller
    {
        readonly ProveedorBL proveedorBL;
        readonly CompraBL compraBL;
        readonly ProductoBL productoBL;

        public CompraController(ProveedorBL pProveedorBL, CompraBL pCompraBL, ProductoBL pProductoBL)
        {
            proveedorBL = pProveedorBL;
            compraBL = pCompraBL;
            productoBL = pProductoBL;
        }
        // GET: CompraController
        public async Task<IActionResult> Index(byte? estado)
        {
            var compras = await compraBL.ObtenerPorEstadoAsync(estado ?? 0); 

            var estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Todos" },
                new SelectListItem { Value = "1", Text = "Activa" },
                new SelectListItem { Value = "2", Text = "Anulada" }
            };

            ViewBag.Estados = new SelectList(estados, "Value", "Text", estado?.ToString());

            return View(compras);
        }


        // GET: CompraController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompraController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Proveedores = new SelectList(await proveedorBL.ObtenerTodosAsync(), "Id", "Nombre");
            ViewBag.Productos = await productoBL.ObtenerTodosAsync(); 

            return View();
        }
       

        // POST: CompraController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Compra compra)
        {
            try
            {
                compra.Estado = (byte)EnumEstadoCompra.Activa;
                compra.FechaCompra = DateTime.Now;
                await compraBL.CrearAsync(compra);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompraController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompraController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompraController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompraController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Anular(int id)
        {
            var compra = await compraBL.ObtenerPorIdAsync(id); 
            if (compra == null)
            {
                return NotFound();
            }
            await compraBL.AnularAsync(id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReporteComprasExcel(List<Compra> compras)
        {
            using (var package = new ExcelPackage())
            {
                var hojaExcel = package.Workbook.Worksheets.Add("Reporte Compras");

                // Encabezados
                hojaExcel.Cells["A1"].Value = "Fecha de Compra";
                hojaExcel.Cells["B1"].Value = "Cliente";
                hojaExcel.Cells["C1"].Value = "Producto";
                hojaExcel.Cells["D1"].Value = "Cantidad";
                hojaExcel.Cells["E1"].Value = "Subtotal";
                hojaExcel.Cells["F1"].Value = "Total de la Compra";

                int row = 2;
                int totalCantidad = 0;
                decimal totalSubTotal = 0;
                decimal totalGeneral = 0;

                foreach (var compra in compras)
                {
                    foreach (var detalle in compra.DetalleCompras)
                    {
                        hojaExcel.Cells[row, 1].Value = compra.FechaCompra.ToString("yyyy-MM-dd");
                        hojaExcel.Cells[row, 2].Value = compra.Proveedor?.Nombre ?? "N/A";
                        hojaExcel.Cells[row, 3].Value = detalle.Producto?.Nombre ?? "N/A";
                        hojaExcel.Cells[row, 4].Value = detalle.Cantidad;
                        hojaExcel.Cells[row, 5].Value = detalle.SubTotal;
                        hojaExcel.Cells[row, 6].Value = compra.Total;

                        // Acumular totales
                        totalCantidad += detalle.Cantidad;
                        totalSubTotal += detalle.SubTotal;
                        totalGeneral += compra.Total;

                        row++;
                    }
                }
                // Fila de totales
                hojaExcel.Cells[row, 3].Value = "Totales:";
                hojaExcel.Cells[row, 4].Value = totalCantidad;
                hojaExcel.Cells[row, 5].Value = totalSubTotal;
                hojaExcel.Cells[row, 6].Value = totalGeneral;

                // Negrita para la fila de totales
                hojaExcel.Cells[row, 3, row, 6].Style.Font.Bold = true;

                hojaExcel.Cells["A:F"].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteComprasExcel.xlsx");
            }
        }


        [HttpGet]
		public async Task<IActionResult> DescargarReporte(CompraFiltros filtro)
		{
			var compras = await compraBL.ObtenerReporteComprasAsync(filtro);

			if (filtro.TipoReporte == (byte)EnumTipoReporte.PDF)
			{
				return new ViewAsPdf("rpCompras", compras);
			}
			else if (filtro.TipoReporte == (byte)EnumTipoReporte.Excel)
			{
				return await ReporteComprasExcel(compras);
			}

			return BadRequest("Formato no válido");
		}


		[HttpGet]
		public IActionResult ReporteCompras()
		{
			return View();
		}


	}
}
