using System;

namespace BE
{
    public class LoteBebida
    {
        public int IdLote { get; set; }
        public int IdRecepcion { get; set; } // Traza de dónde vino físicamente
        public string IdProducto { get; set; } // FK al catálogo de Productos
        public string NumeroLote { get; set; } // Alfanumérico impreso en la caja
        public DateTime FechaVencimiento { get; set; }
        public int CantidadRecibida { get; set; }
        public int StockActual { get; set; }

        // --- Lógica de Negocio Interna (No persiste en BD) ---

        public int DiasParaVencimiento
        {
            get { return (FechaVencimiento - DateTime.Now).Days; }
        }

        // Un lote se considera en riesgo si le quedan menos de X días (por defecto 90)
        public bool EstaPorVencer(int diasUmbral = 90)
        {
            return DiasParaVencimiento <= diasUmbral;
        }

        public bool LoteAgotado
        {
            get { return StockActual <= 0; }
        }
    }
}