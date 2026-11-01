using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class GestorProducto
    {
        private RepositorioProducto repositorio;

        public GestorProducto()
        {
            repositorio = new RepositorioProducto();
        }

        public List<Producto> ObtenerInventario()
        {
            return repositorio.ObtenerInventario();
        }

        public void RegistrarNuevoProducto(Producto prod)
        {
            if (string.IsNullOrWhiteSpace(prod.CodigoBarra))
                throw new Exception("El código de barras (SKU) no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(prod.Nombre))
                throw new Exception("El nombre del producto no puede estar vacío.");

            if (prod.Precio <= 0)
                throw new Exception("El precio unitario debe ser mayor a cero.");

            if (prod.PuntoPedido < 0)
                throw new Exception("El punto de pedido no puede ser negativo.");

            // Validar que el código no exista previamente en la base
            Producto? existente = repositorio.ObtenerPorId(prod.CodigoBarra);
            if (existente != null)
            {
                if (existente.Activo)
                    throw new Exception($"Ya existe un producto activo con el código {prod.CodigoBarra}.");
                else
                    throw new Exception($"El código {prod.CodigoBarra} pertenece a un producto dado de baja. Contacte al administrador de BD para reciclarlo.");
            }

            repositorio.GuardarNuevoProducto(prod);
        }

        public void ModificarProducto(Producto prod)
        {
            if (string.IsNullOrWhiteSpace(prod.CodigoBarra))
                throw new Exception("El código del producto es inválido.");

            if (string.IsNullOrWhiteSpace(prod.Nombre))
                throw new Exception("El nombre del producto no puede estar vacío.");

            if (prod.Precio <= 0)
                throw new Exception("El precio debe ser mayor a cero.");

            repositorio.ActualizarProducto(prod);
        }

        public void DarDeBajaProducto(string codigoBarra)
        {
            if (string.IsNullOrWhiteSpace(codigoBarra))
                throw new Exception("Debe especificar el código del producto a eliminar.");

            Producto? existente = repositorio.ObtenerPorId(codigoBarra);

            if (existente != null && existente.StockActual > 0)
            {
                throw new Exception($"No se puede dar de baja el producto porque aún posee {existente.StockActual} unidades en stock. Debe liquidar la mercadería primero.");
            }

            repositorio.BajaLogicaProducto(codigoBarra);
        }
        public DataTable ConsultarLotesActivos(string idProducto)
        {
            return repositorio.ObtenerLotesPorProducto(idProducto);
        }
        public DataTable ConsultarTodosLotesActivos()
        {
            return repositorio.ObtenerTodosLosLotesActivos();
        }
    }
}
