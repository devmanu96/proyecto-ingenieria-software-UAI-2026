using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class GestorCompras
    {
        private RepositorioOrdenCompra repoOrden;
        private RepositorioCompras repoGenericoCompras;
        private RepositorioSolicitud repoSolicitud;

        public GestorCompras()
        {
            repoOrden = new RepositorioOrdenCompra();
            repoGenericoCompras = new RepositorioCompras();
            repoSolicitud = new RepositorioSolicitud();
        }

        // --- MÉTODOS DE BANDEJA DE SOLICITUDES ---
        public DataView ObtenerSolicitudesPendientes()
        {
            return repoSolicitud.ObtenerSolicitudesPendientes();
        }

        public DataView ObtenerDetallesPorSolicitud(int idSolicitud)
        {
            return repoSolicitud.ObtenerDetallesPorSolicitud(idSolicitud);
        }

        // --- MÉTODOS DE CATÁLOGO B2B ---
        public DataView ObtenerCatalogoB2B()
        {
            return repoGenericoCompras.ObtenerCatalogoB2B();
        }

        public Dictionary<int, int> ObtenerMapaProveedoresTab()
        {
            return repoGenericoCompras.ObtenerMapaProveedoresTab();
        }

        // --- MÉTODOS DE ORDEN DE COMPRA ---
        public void EmitirOrdenCompra(OrdenCompra ordenActual, int idSolicitudReferencia)
        {
            if (ordenActual.Detalles == null || ordenActual.Detalles.Count == 0)
                throw new Exception("No se puede emitir una orden de compra vacía.");

            if (ordenActual.IdProveedor <= 0)
                throw new Exception("Proveedor no válido.");

            repoOrden.GuardarOrdenCompra(ordenActual, idSolicitudReferencia);
        }

        // --- MÉTODOS PARA RECEPCIÓN (SOLUCIONA CS1061) ---
        public DataView ObtenerOrdenesPendientesDeRecepcion()
        {
            return repoOrden.ObtenerOrdenesPendientesDeRecepcion();
        }

        public DataView ObtenerDetallesPorOrden(int idOrden)
        {
            if (idOrden <= 0) throw new ArgumentException("ID de Orden inválido.");
            return repoOrden.ObtenerDetallesPorOrden(idOrden);
        }
    }
}