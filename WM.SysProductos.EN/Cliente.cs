using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WM.SysProductos.EN
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres")]
        public string? Direccion { get; set; }

        public string? Telefono { get; set; }
        [StringLength(100, ErrorMessage = "El correo electronico no puede tener mas de 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El correo electronico no tiene un formato valido.")]

        public string? Email { get; set; }
    }
}
