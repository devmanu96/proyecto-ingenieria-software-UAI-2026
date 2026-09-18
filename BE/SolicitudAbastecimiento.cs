using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class SolicitudAbastecimiento
    {
        public Guid IdSolicitud { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string Estado { get; set; } // Ej: "Pendiente", "Procesada por Compras"

        // Una cabecera contiene una lista de detalles
        public List<DetalleSolicitud> Detalles { get; set; }

        public SolicitudAbastecimiento()
        {
            Detalles = new List<DetalleSolicitud>();
        }
    }
}
