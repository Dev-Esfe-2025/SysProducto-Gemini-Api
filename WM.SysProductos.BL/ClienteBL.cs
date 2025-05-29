using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WM.SysProductos.DAL;
using WM.SysProductos.EN;

namespace WM.SysProductos.BL
{
    public class ClienteBL
    {
        readonly ClienteDAL clienteDAL;

        public ClienteBL(ClienteDAL pClienteDAL)
        {
            clienteDAL = pClienteDAL;
        }

        public async Task<int> CrearAsync(Cliente pCliente)
        {
            return await clienteDAL.CrearAsync(pCliente);
        }

        public async Task<int> ModificarAsync(Cliente pCliente)
        {
            return await clienteDAL.ModificarAsync(pCliente);
        }

        public async Task<int> EliminarAsync(Cliente pCliente)
        {
            return await clienteDAL.EliminarAsync(pCliente);
        }

        public async Task<Cliente> ObtenerPorIdAsync(Cliente pCliente)
        {
            return await clienteDAL.ObtenerPorIdAsync(pCliente);
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await clienteDAL.ObtenerTodosAsync();
        }

        public Task AgregarTodosAsync(List<Cliente> pClientes)
        {
            return clienteDAL.AgregarTodosAsync(pClientes);
        }
    }
}
