using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class GestorProducto
    {
        private RepositorioProducto repositorioProducto;

        public GestorProducto()
        {
            repositorioProducto = new RepositorioProducto();
        }

        public List<Producto> ObtenerInventario()
        {
            try
            {
                // Aquí en el futuro podés agregar reglas de validación si es necesario.
                // Por ahora, simplemente delegamos la obtención de datos al repositorio.
                return repositorioProducto.ObtenerInventario();
            }
            catch (Exception ex)
            {
                throw new Exception("Error desde la capa de negocio al intentar obtener el inventario de productos: " + ex.Message);
            }
        }
    }
}
