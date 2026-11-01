using System;
using System.Collections.Generic;

namespace BE
{
    public class Recepcion
    {
        public int IdRecepcion { get; set; }

        // ID de la Orden de Compra original para mantener la trazabilidad
        public int IdOrden { get; set; }

        // El número del documento físico que trae el camionero
        public string NumeroRemito { get; set; }

        public DateTime FechaIngreso { get; set; }

        // Relación 1 a N: Una recepción contiene varios lotes de bebidas
        public List<LoteBebida> LotesRecibidos { get; set; }

        public Recepcion()
        {
            // Inicializamos la lista para evitar errores de NullReference
            LotesRecibidos = new List<LoteBebida>();
        }
    }
}