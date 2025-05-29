using WM.SysProductos.EN;

namespace WM.SysProductos.AppWeb.Service
{
    public interface IGeminiService
    {
        Task<string> GenerarAnalisisVentasAsync(List<Venta> ventas);

    }
}
