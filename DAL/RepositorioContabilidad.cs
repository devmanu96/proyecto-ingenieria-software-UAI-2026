using BE;
using System;
using System.Data;

namespace DAL
{
    public class RepositorioContabilidad
    {
        public DataView ObtenerOrdenesPendientesPago()
        {
            DataTable? dt = DAO.GetInstance.ObtenerDataSet().Tables["ORDEN_COMPRA"];
            if (dt == null) throw new Exception("La tabla ORDEN_COMPRA no está mapeada en el DAO.");

            DataView dv = new DataView(dt);
            dv.RowFilter = "Estado = 'Emitida'";
            return dv;
        }

        public DataView ObtenerDetallesPorOrden(int idOrden)
        {
            DataTable? dt = DAO.GetInstance.ObtenerDataSet().Tables["DETALLE_OC"];
            if (dt == null) throw new Exception("La tabla DETALLE_OC no está mapeada en el DAO.");

            DataView dv = new DataView(dt);
            dv.RowFilter = $"IdOrden = {idOrden}";
            return dv;
        }

        public void RegistrarPago(PagoEmitido pago)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();

            DataTable? dtPago = ds.Tables["PAGO_EMITIDO"];
            DataTable? dtOrden = ds.Tables["ORDEN_COMPRA"];

            if (dtPago == null || dtOrden == null)
                throw new Exception("Faltan tablas contables en el DataSet.");

            // 1. Insertar el comprobante de pago
            DataRow nuevoPago = dtPago.NewRow();
            nuevoPago["IdOrden"] = pago.IdOrden;
            nuevoPago["FechaPago"] = pago.FechaPago;
            nuevoPago["MontoTransferido"] = pago.MontoTransferido;
            nuevoPago["NumeroComprobante"] = pago.NumeroComprobante;
            dtPago.Rows.Add(nuevoPago);

            // 2. Actualizar el estado de la Orden de Compra a 'Pagada'
            DataRow[] filasOrden = dtOrden.Select($"IdOrden = {pago.IdOrden}");
            if (filasOrden.Length > 0)
            {
                filasOrden[0]["Estado"] = "Pagada";
            }

            dao.SubirCambiosBD();
        }
    }
}