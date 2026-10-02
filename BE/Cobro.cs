using System;

namespace BE
{
    public class Cobro
    {
        public int IdCobro { get; set; }
        public int IdVenta { get; set; }
        public DateTime FechaCobro { get; set; }
        public decimal MontoCobrado { get; set; }
        public string MetodoPago { get; set; }
        public string? NumeroTarjeta { get; set; }  // Faltaba
        public int Cuotas { get; set; }             // Faltaba
        public string? NumeroComprobante { get; set; }
    }
}