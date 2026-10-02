using System;
using System.Data;

namespace DAL
{
    public class RepositorioHistorialVentas
    {
        public DataView ObtenerHistorialCompleto()
        {
            // Consumimos la tabla fresca directamente
            DataTable dt = DAO.GetInstance.RefrescarVistaHistorial();

            if (dt != null)
            {
                return dt.DefaultView;
            }

            throw new Exception("Error al cargar la vista de historial de ventas desde la base de datos.");
        }
    }
}