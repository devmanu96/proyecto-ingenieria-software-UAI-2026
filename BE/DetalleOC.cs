using System;

namespace BE
{
    public class DetalleOC
    {
        public int IdDetalleOC { get; set; }
        public int IdOrden { get; set; }

        public string IdProducto { get; set; }

        public int CantidadSolicitada { get; set; }

        public decimal PrecioAcordado { get; set; }
        public decimal Subtotal
        {
            get { return CantidadSolicitada * PrecioAcordado; }
        }
    }
}