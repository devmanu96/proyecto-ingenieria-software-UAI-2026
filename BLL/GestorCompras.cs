using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class GestorCompras : ISujeto
    {
        private RepositorioCatalogoProveedor repoCatalogo;
        private RepositorioOrdenCompra repoOrdenCompra;
        private RepositorioSolicitud repoSolicitud;
        private List<IObserver> ObserversAttached = new List<IObserver>();

        public GestorCompras()
        {
            repoCatalogo = new RepositorioCatalogoProveedor();
            repoOrdenCompra = new RepositorioOrdenCompra();
            repoSolicitud = new RepositorioSolicitud();

            Attach(GestorBitacora.GetInstance);
        }

        public DataView ObtenerSolicitudesPendientes()
        {
            // Delegamos la responsabilidad al repositorio correspondiente
            return repoSolicitud.ObtenerSolicitudesPendientes();
        }

        public DataView ObtenerDetallesPorSolicitud(int idSolicitud)
        {
            // Delegamos la responsabilidad al repositorio correspondiente
            return repoSolicitud.ObtenerDetallesPorSolicitud(idSolicitud);
        }

        public DataView ObtenerCatalogoB2B()
        {
            return repoCatalogo.ObtenerVistaCatalogo();
        }

        public Dictionary<int, int> ObtenerMapaProveedoresTab()
        {
            return repoCatalogo.MapearTabsConIdsProveedores();
        }

        public void EmitirOrdenCompra(OrdenCompra nuevaOrden, int idSolicitudReferencia)
        {
            if (nuevaOrden.Detalles.Count == 0)
                throw new Exception("La orden de compra debe contener al menos un pallet.");

            repoOrdenCompra.GuardarOrdenCompra(nuevaOrden, idSolicitudReferencia);

            Usuario? usrActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usrActivo != null)
            {
                Notificar(usrActivo.Username, "LOG_EMITIO_OC");
            }
        }

        // ==========================================
        //  Implementación de ISujeto
        // ==========================================
        public void Attach(IObserver observer)
        {
            if (!ObserversAttached.Contains(observer)) ObserversAttached.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            ObserversAttached.Remove(observer);
        }

        public void Notificar(string username, string accion)
        {
            foreach (IObserver item in ObserversAttached)
            {
                item.Update(username, accion);
            }
        }
    }
}