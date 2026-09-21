using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class OrdenCompra
    {
        public int IdOrden { get; set; }
        public int IdProveedor { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Estado { get; set; }

        public List<DetalleOC> Detalles { get; set; }

        public OrdenCompra()
        {
            Detalles = new List<DetalleOC>();
        }

        public decimal MontoTotal
        {
            get
            {
                if (Detalles == null || Detalles.Count == 0) return 0;
                return Detalles.Sum(d => d.Subtotal);
            }
        }
    }
}