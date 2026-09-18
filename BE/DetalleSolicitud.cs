using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class DetalleSolicitud
    {
        public Guid IdDetalle { get; set; }
        public Producto Producto { get; set; } // Relación con el producto
        public int CantidadSolicitada { get; set; }
        
    }

}
