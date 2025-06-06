﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WM.SysProductos.EN.Filtros
{
    public class VentaFiltros
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public byte TipoReporte { get; set; }

        public enum EnumTipoReporte
        {
            PDF = 1,
            Excel = 2,
            ResumenIA = 3
        }
    }
}
