using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WM.SysProductos.EN
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de venta es obligatoria.")]
        public DateTime FechaVenta { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El total de la venta es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "El total de la venta es obligatorio.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public byte Estado { get; set; }

        // Relación con cliente
        public virtual Cliente? Cliente { get; set; }

        // Relación con DetalleVenta (Una venta tiene varios detalles)
        public virtual ICollection<DetalleVenta>? DetalleVentas { get; set; }

        public enum EnumEstadoVenta
        {
            Activa = 1,
            Anulada = 2
        }
    }
}
