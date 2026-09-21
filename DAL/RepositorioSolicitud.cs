using BE;
using System;
using System.Data;
using System.Linq;

namespace DAL
{
    public class RepositorioSolicitud
    {
        public void GuardarSolicitud(SolicitudAbastecimiento solicitud)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();

            DataTable? dtSolicitud = ds.Tables["SOLICITUD_ABASTECIMIENTO"];
            DataTable? dtDetalle = ds.Tables["DETALLE_SOLICITUD"];

            if (dtSolicitud == null || dtDetalle == null)
                throw new Exception("Las tablas de solicitud no están mapeadas en el DAO.");

            DataRow filaSolicitud = dtSolicitud.NewRow();
            filaSolicitud["FechaGeneracion"] = solicitud.FechaGeneracion;
            filaSolicitud["Estado"] = solicitud.Estado;
            dtSolicitud.Rows.Add(filaSolicitud);

            int idGenerado = Convert.ToInt32(filaSolicitud["IdSolicitud"]);

            foreach (var detalle in solicitud.Detalles)
            {
                DataRow filaDetalle = dtDetalle.NewRow();
                filaDetalle["IdDetalle"] = detalle.IdDetalle;
                filaDetalle["IdSolicitud"] = idGenerado;
                filaDetalle["IdProducto"] = detalle.Producto.CodigoBarra;
                filaDetalle["CantidadSolicitada"] = detalle.CantidadSolicitada;
                dtDetalle.Rows.Add(filaDetalle);
            }

            dao.SubirCambiosBD();
        }

        public bool ProductoTieneSolicitudPendiente(string codigoBarra)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable? dtSolicitud = ds.Tables["SOLICITUD_ABASTECIMIENTO"];
            DataTable? dtDetalle = ds.Tables["DETALLE_SOLICITUD"];

            if (dtSolicitud == null || dtDetalle == null) return false;

            var idsPendientes = dtSolicitud.AsEnumerable()
                .Where(row => row["Estado"].ToString() == "Pendiente de Compras")
                .Select(row => Convert.ToInt32(row["IdSolicitud"]))
                .ToList();

            if (idsPendientes.Count == 0) return false; // No hay solicitudes pendientes

            bool existeEnPendiente = dtDetalle.AsEnumerable()
                .Any(row => row["IdProducto"].ToString() == codigoBarra &&
                            idsPendientes.Contains(Convert.ToInt32(row["IdSolicitud"])));

            return existeEnPendiente;
        }

        // =========================================================
        // NUEVOS MÉTODOS PARA EL MÓDULO DE COMPRAS (BANDEJA DE ENTRADA)
        // =========================================================

        public DataView ObtenerSolicitudesPendientes()
        {
            DataTable? dt = DAO.GetInstance.ObtenerDataSet().Tables["SOLICITUD_ABASTECIMIENTO"];
            if (dt == null) throw new Exception("La tabla SOLICITUD_ABASTECIMIENTO no está en el DataSet.");

            DataView dv = new DataView(dt);
            dv.RowFilter = "Estado = 'Pendiente de Compras'";
            return dv;
        }

        public DataView ObtenerDetallesPorSolicitud(int idSolicitud)
        {
            DataTable? dt = DAO.GetInstance.ObtenerDataSet().Tables["DETALLE_SOLICITUD"];
            if (dt == null) throw new Exception("La tabla DETALLE_SOLICITUD no está en el DataSet.");

            DataView dv = new DataView(dt);
            dv.RowFilter = $"IdSolicitud = {idSolicitud}";
            return dv;
        }
    }
}