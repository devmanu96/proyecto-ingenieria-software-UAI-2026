using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PagoEmitido
    {
        public int IdPago { get; set; }
        public int IdOrden { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoTransferido { get; set; }
        public string NumeroComprobante { get; set; }
    }
}
