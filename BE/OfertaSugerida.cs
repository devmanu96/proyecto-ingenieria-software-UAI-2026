using System;

namespace BE
{
    public class OfertaSugerida
    {
        public int IdOferta { get; set; }
        public int IdLote { get; set; } // El lote específico que está en riesgo

        // Nullable porque la oferta puede ser solo para el producto por vencer, 
        // o un combo cruzado con un "Top Seller" (Cross-selling).
        public string? IdProductoAsociado { get; set; }

        public string TituloOferta { get; set; }
        public decimal DescuentoSugerido { get; set; } // Ej: 15.50 (%)
        public string Estado { get; set; }
        public DateTime FechaGeneracion { get; set; }

        public OfertaSugerida()
        {
            // Valores por defecto al ser generada por el motor analítico
            Estado = "Pendiente";
            FechaGeneracion = DateTime.Now;
        }
    }
}