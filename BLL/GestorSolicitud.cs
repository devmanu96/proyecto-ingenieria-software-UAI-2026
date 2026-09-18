using BE;
using DAL;
using System;

namespace BLL
{
    public class GestorSolicitud
    {
        private RepositorioSolicitud repositorio;

        public GestorSolicitud()
        {
            repositorio = new RepositorioSolicitud();
        }

        public void GenerarNuevaSolicitud(SolicitudAbastecimiento nuevaSolicitud)
        {
            if (nuevaSolicitud.Detalles == null || nuevaSolicitud.Detalles.Count == 0)
                throw new Exception("La solicitud debe tener al menos un producto.");

            nuevaSolicitud.FechaGeneracion = DateTime.Now;
            nuevaSolicitud.Estado = "Pendiente de Compras";

            repositorio.GuardarSolicitud(nuevaSolicitud);
        }
        public bool ValidarProductoPendienteCompras(string codigoBarra)
        {
            return repositorio.ProductoTieneSolicitudPendiente(codigoBarra);
        }
    }
    
}