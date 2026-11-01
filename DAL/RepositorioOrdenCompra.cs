using BE;
using System;
using System.Data;

namespace DAL
{
    public class RepositorioOrdenCompra
    {
        public void GuardarOrdenCompra(OrdenCompra nuevaOrden, int idSolicitudReferencia)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();

            DataTable? dtOC = ds.Tables["ORDEN_COMPRA"];
            DataTable? dtDetalle = ds.Tables["DETALLE_OC"];
            DataTable? dtSolicitud = ds.Tables["SOLICITUD_ABASTECIMIENTO"];

            if (dtOC == null || dtDetalle == null || dtSolicitud == null)
                throw new Exception("Faltan tablas del módulo de compras en el DataSet.");

            // 1. Cabecera (ORDEN_COMPRA)
            DataRow rowOC = dtOC.NewRow();
            rowOC["IdProveedor"] = nuevaOrden.IdProveedor;
            rowOC["FechaEmision"] = nuevaOrden.FechaEmision;
            rowOC["Estado"] = nuevaOrden.Estado;
            dtOC.Rows.Add(rowOC);

            int idNuevaOrden = Convert.ToInt32(rowOC["IdOrden"]);

            // 2. Detalles (DETALLE_OC)
            foreach (DetalleOC det in nuevaOrden.Detalles)
            {
                DataRow rowDet = dtDetalle.NewRow();
                rowDet["IdOrden"] = idNuevaOrden;
                rowDet["IdProducto"] = det.IdProducto;
                rowDet["CantidadSolicitada"] = det.CantidadSolicitada;
                rowDet["PrecioAcordado"] = det.PrecioAcordado;
                dtDetalle.Rows.Add(rowDet);
            }

            // 3. Actualizar estado de SOLICITUD_ABASTECIMIENTO
            DataRow[] filasSol = dtSolicitud.Select($"IdSolicitud = {idSolicitudReferencia}");
            if (filasSol.Length > 0)
            {
                filasSol[0]["Estado"] = "Procesada por Compras";
            }

            dao.SubirCambiosBD();
        }

        // =========================================================
        // MÉTODOS PARA RECEPCIÓN DE MERCADERÍA (SOLUCIÓN ERRORES)
        // =========================================================

        public DataView ObtenerOrdenesPendientesDeRecepcion()
        {
            DataTable? dt = DAO.GetInstance.ObtenerDataSet().Tables["ORDEN_COMPRA"];
            if (dt == null) throw new Exception("La tabla ORDEN_COMPRA no está en el DataSet.");

            DataView dv = new DataView(dt);
            // Filtramos las órdenes que ya fueron emitidas pero aún no se reciben físicamente
            dv.RowFilter = "Estado = 'Emitida' OR Estado = 'Pagada'";
            return dv;
        }

        public DataView ObtenerDetallesPorOrden(int idOrden)
        {
            DataTable? dt = DAO.GetInstance.ObtenerDataSet().Tables["DETALLE_OC"];
            if (dt == null) throw new Exception("La tabla DETALLE_OC no está en el DataSet.");

            DataView dv = new DataView(dt);
            dv.RowFilter = $"IdOrden = {idOrden}";
            return dv;
        }
    }
}