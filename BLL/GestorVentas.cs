using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class GestorVentas
    {
        private RepositorioVentas repositorio;

        public GestorVentas()
        {
            repositorio = new RepositorioVentas();
        }

        public DataView ObtenerClientesActivos()
        {
            return repositorio.ObtenerClientesActivos();
        }

        public void ProcesarNuevaVenta(int idCliente, Guid idUsuario, List<DetalleVenta> detalles, string metodoPago)
        {
            // 1. Reglas de Negocio: Validaciones defensivas
            if (detalles == null || !detalles.Any())
                throw new Exception("No se puede registrar una venta sin productos.");

            decimal montoTotalCalculado = 0;

            // 2. Recalcular todo en el backend por seguridad
            foreach (var detalle in detalles)
            {
                if (detalle.Cantidad <= 0)
                    throw new Exception($"La cantidad para el producto {detalle.IdProducto} debe ser mayor a cero.");

                if (detalle.PrecioUnitario <= 0)
                    throw new Exception($"El precio del producto {detalle.IdProducto} es inválido.");

                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                montoTotalCalculado += detalle.Subtotal;
            }

            if (montoTotalCalculado <= 0)
                throw new Exception("El monto total de la venta debe ser mayor a cero.");

            // 3. Armar la Cabecera de la Venta
            Venta nuevaVenta = new Venta
            {
                IdCliente = idCliente,
                IdUsuario = idUsuario,
                FechaVenta = DateTime.Now,
                MontoTotal = montoTotalCalculado,
                Estado = "Completada"
            };

            // 4. Armar el Recibo de Cobro
            Cobro nuevoCobro = new Cobro
            {
                FechaCobro = DateTime.Now,
                MontoCobrado = montoTotalCalculado,
                MetodoPago = metodoPago,
                // Generamos un número de ticket alfanumérico simulando comprobante
                NumeroComprobante = "TKT-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            };

            // 5. Enviar a la DAL para la transacción única
            repositorio.RegistrarVentaCompleta(nuevaVenta, detalles, nuevoCobro);
        }
    }
}