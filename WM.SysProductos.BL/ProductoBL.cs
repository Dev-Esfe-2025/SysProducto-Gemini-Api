using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WM.SysProductos.DAL;
using WM.SysProductos.EN;

namespace WM.SysProductos.BL
{
    public class ProductoBL
    {
        readonly ProductoDAL productoDAL;
        public ProductoBL(ProductoDAL pProductoDAL)
        {
            productoDAL = pProductoDAL;
        }

        public async Task<int> CrearAsync(Producto pProducto)
        {
            return await productoDAL.CrearAsync(pProducto);
        }

        public async Task<int> ModificarAsync(Producto pProducto)
        {
            return await productoDAL.ModificarAsync(pProducto);
        }

        public async Task<int> EliminarAsync(Producto pProducto)
        {
            return await productoDAL.EliminarAsync(pProducto);
        }

        public async Task<Producto> ObtenerPorIdAsync(Producto pProducto)
        {
            return await productoDAL.ObtenerPorIdAsync(pProducto);
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await productoDAL.ObtenerTodosAsync();
        }
        public Task AgregarTodosAsync(List<Producto> pProductos)
        {
            return productoDAL.AgregarTodosAsync(pProductos);

        }
    
    }
}
