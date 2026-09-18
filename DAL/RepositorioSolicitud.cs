using BE;
using System;
using System.Data;

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
    }
}