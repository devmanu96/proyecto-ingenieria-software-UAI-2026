using DAL;
using System;
using System.Data;

namespace BLL
{
    public class GestorHistorialVentas
    {
        private RepositorioHistorialVentas repositorio;

        public GestorHistorialVentas()
        {
            repositorio = new RepositorioHistorialVentas();
        }

        public DataView ObtenerHistorial()
        {
            // Reglas de negocio futuras irían aquí (filtros por fecha, permisos, etc.)
            return repositorio.ObtenerHistorialCompleto();
        }
    }
}