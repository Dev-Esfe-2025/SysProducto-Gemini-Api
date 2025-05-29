using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WM.SysProductos.EN
{
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre no puede tener más de 255 caracteres.")]
        public string Nombre { get; set; } 

        [StringLength(50, ErrorMessage = "El NRC no puede tener más de 50 caracteres.")]
        public string? NRC { get; set; }

        [StringLength(80, ErrorMessage = "La dirección no puede tener más de 80 caracteres.")]
        public string? Direccion { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres.")]
        public string? Telefono { get; set; }

        [StringLength(100, ErrorMessage = "El correo electrónico no puede tener más de 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string? Email { get; set; }
    }
}
