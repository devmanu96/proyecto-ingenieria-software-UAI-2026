using BE;
using DAL;
using System;
using System.Data;

namespace BLL
{
    public class GestorContabilidad
    {
        private RepositorioContabilidad repositorio;

        public GestorContabilidad()
        {
            repositorio = new RepositorioContabilidad();
        }

        public DataView ObtenerOrdenesPendientesPago()
        {
            return repositorio.ObtenerOrdenesPendientesPago();
        }

        public DataView ObtenerDetallesPorOrden(int idOrden)
        {
            return repositorio.ObtenerDetallesPorOrden(idOrden);
        }

        public decimal CalcularTotalOrden(int idOrden)
        {
            DataView detalles = ObtenerDetallesPorOrden(idOrden);
            decimal total = 0;

            foreach (DataRowView row in detalles)
            {
                int cantidad = Convert.ToInt32(row["CantidadSolicitada"]);
                decimal precio = Convert.ToDecimal(row["PrecioAcordado"]);
                total += (cantidad * precio);
            }

            return total;
        }

        public void ProcesarPago(int idOrden, decimal monto)
        {
            if (monto <= 0) throw new Exception("El monto a transferir debe ser mayor a cero.");

            PagoEmitido nuevoPago = new PagoEmitido
            {
                IdOrden = idOrden,
                FechaPago = DateTime.Now,
                MontoTransferido = monto,
                // Generamos un número de comprobante alfanumérico simulando comprobante bancario
                NumeroComprobante = "TRF-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            };

            repositorio.RegistrarPago(nuevoPago);
        }
    }
}