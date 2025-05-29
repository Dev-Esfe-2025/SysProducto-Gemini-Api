using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WM.SysProductos.EN;

namespace WM.SysProductos.DAL
{
    public class ClienteDAL
    {
        readonly SysProductosDBContext dbContext;

        public ClienteDAL(SysProductosDBContext productoDB)
        {
            dbContext = productoDB;
        }

        public async Task<int> CrearAsync(Cliente pCliente)
        {
            dbContext.Clientes.Add(pCliente);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> EliminarAsync(Cliente pCliente)
        {
            var cliente = await dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                dbContext.Clientes.Remove(cliente);
                return await dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }

        public async Task<int> ModificarAsync(Cliente pCliente)
        {
            var cliente = await dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                cliente.Nombre = pCliente.Nombre;
                cliente.Direccion = pCliente.Direccion;
                cliente.Telefono = pCliente.Telefono;
                cliente.Email = pCliente.Email;

                dbContext.Update(cliente);
                return await dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }

        public async Task<Cliente> ObtenerPorIdAsync(Cliente pCliente)
        {
            var cliente = await dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                return new Cliente
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Direccion = cliente.Direccion,
                    Telefono = cliente.Telefono,
                    Email = cliente.Email
                };
            }
            else
                return new Cliente();
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            var clientes = await dbContext.Clientes.ToListAsync();
            if (clientes != null && clientes.Count > 0)
            {
                var list = new List<Cliente>();
                clientes.ForEach(c => list.Add(new Cliente
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Direccion = c.Direccion,
                    Telefono = c.Telefono,
                    Email = c.Email
                }));
                return list;
            }
            else
                return new List<Cliente>();
        }

        public async Task AgregarTodosAsync(List<Cliente> pClientes)
        {
            await dbContext.Clientes.AddRangeAsync(pClientes);
            await dbContext.SaveChangesAsync();
        }
    }
}
