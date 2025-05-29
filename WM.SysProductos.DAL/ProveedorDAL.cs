using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WM.SysProductos.DAL;
using WM.SysProductos.EN;

namespace WM.SysProveedors.DAL
{
    public class ProveedorDAL
    {

        readonly SysProductosDBContext dbContext;
        public ProveedorDAL(SysProductosDBContext sysProductosDB)
        {
            dbContext = sysProductosDB;
        }

        public async Task<int> CrearAsync(Proveedor pProveedor)
        {
            Proveedor proveedor = new Proveedor()
            {
                Nombre = pProveedor.Nombre,
                NRC = pProveedor.NRC,
                Direccion = pProveedor.Direccion,
                Telefono = pProveedor.Telefono,
                Email = pProveedor.Email
            };
            dbContext.Proveedores.Add(proveedor);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> EliminarAsync(Proveedor pProveedor)
        {
            var proveedor = await dbContext.Proveedores.FirstOrDefaultAsync(s => s.Id == pProveedor.Id);
            if (proveedor != null && proveedor.Id != 0)
            {
                dbContext.Proveedores.Remove(proveedor);
                return await dbContext.SaveChangesAsync();
            }
            else
                return 0;

        }

        public async Task<int> ModificarAsync(Proveedor pProveedor)
        {
            var proveedor = await dbContext.Proveedores.FirstOrDefaultAsync(s => s.Id == pProveedor.Id);
            if (proveedor != null && proveedor.Id != 0)
            {
                proveedor.Nombre = pProveedor.Nombre;
                proveedor.NRC = pProveedor.NRC;
                proveedor.Direccion = pProveedor.Direccion;
                proveedor.Telefono = pProveedor.Telefono;
                proveedor.Email = pProveedor.Email;

                dbContext.Update(proveedor);
                return await dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }

        public async Task<Proveedor> ObtenerPorIdAsync(Proveedor pProveedor)
        {
            var proveedor = await dbContext.Proveedores.FirstOrDefaultAsync(s => s.Id == pProveedor.Id);
            if (proveedor != null && proveedor.Id != 0)
            {
                return new Proveedor
                {
                    Id = proveedor.Id,
                    Nombre = proveedor.Nombre,
                    NRC = proveedor.NRC,
                    Direccion = proveedor.Direccion,
                    Telefono = proveedor.Telefono,
                    Email = proveedor.Email
                };
            }
            else
                return new Proveedor();
        }

        public async Task<List<Proveedor>> ObtenerTodosAsync()
        {
            var proveedores = await dbContext.Proveedores.ToListAsync(); 
            if (proveedores != null && proveedores.Count > 0)
            {
                var list = new List<Proveedor>();
                proveedores.ForEach(p => list.Add(new Proveedor
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    NRC = p.NRC,
                    Direccion = p.Direccion,
                    Telefono = p.Telefono,
                    Email = p.Email
                }));
                return list;
            }
            else
                return new List<Proveedor>();
        }
    }
}
