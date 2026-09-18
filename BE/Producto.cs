using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Producto
    {
        public string CodigoBarra { get; set; }

        public string Nombre { get; set; }

        // Cantidad física actual en el almacén
        public int StockActual { get; set; }

        // El límite constante que indica cuándo es necesario pedir más mercadería
        public int PuntoPedido { get; set; }

        // Propiedad calculada de solo lectura para facilitar el filtrado visual en la UI
        public bool RequiereAbastecimiento
        {
            get { return StockActual <= PuntoPedido; }
        }
    }
}
