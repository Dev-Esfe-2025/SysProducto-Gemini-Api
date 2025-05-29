using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WM.SysProductos.EN;

namespace WM.SysProductos.DAL
{
    public class ProductoDAL
    {
        readonly SysProductosDBContext dbContext;
        public ProductoDAL(SysProductosDBContext sysProductosDB) 
        {
            dbContext = sysProductosDB;
        }

        public async Task<int> CrearAsync(Producto pProducto)
        {
            Producto producto = new Producto()
            {
                Nombre = pProducto.Nombre,
                Precio = pProducto.Precio,
                CantidadDisponible = pProducto.CantidadDisponible,
                FechaCreacion = DateTime.UtcNow
            };
            dbContext.Productos.Add(producto); 
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> EliminarAsync(Producto pProducto)
        {
            var producto = await dbContext.Productos.FirstOrDefaultAsync(s => s.Id == pProducto.Id);
            if (producto != null && producto.Id != 0)
            {
                dbContext.Productos.Remove(producto); 
                return await dbContext.SaveChangesAsync(); 
            }
            else
                return 0;

        }

        public async Task<int> ModificarAsync(Producto pProducto)
        {
            var producto = await dbContext.Productos.FirstOrDefaultAsync(s => s.Id == pProducto.Id);
            if (producto != null && producto.Id != 0)
            {
                producto.Nombre = pProducto.Nombre;
                producto.Precio = pProducto.Precio;
                producto.CantidadDisponible = producto.CantidadDisponible;
                dbContext.Update(producto);
                return await dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }

        public async Task<Producto> ObtenerPorIdAsync(Producto pProducto)
        {
            var producto = await dbContext.Productos.FirstOrDefaultAsync(s => s.Id == pProducto.Id);
            if (producto != null && producto.Id != 0)
            {
                return new Producto
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    CantidadDisponible = producto.CantidadDisponible,
                    FechaCreacion = producto.FechaCreacion
                };
            }
            else
                return new Producto();
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            var productos = await dbContext.Productos.ToListAsync(); //select 
            if (productos != null && productos.Count > 0)
            {
                var list = new List<Producto>();
                productos.ForEach(p => list.Add(new Producto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    CantidadDisponible = p.CantidadDisponible,
                    FechaCreacion = p.FechaCreacion
                }));
                return list;
            }
            else
                return new List<Producto>();
        }
        public  async Task AgregarTodosAsync(List<Producto> pProductos)
        {
            await dbContext.Productos.AddRangeAsync(pProductos);
            await dbContext.SaveChangesAsync();
        }

       
    }
}

