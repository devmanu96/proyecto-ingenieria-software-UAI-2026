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

        // Se cambia 'string metodoPago' por 'List<Cobro> pagosIngresados'
        public string ProcesarNuevaVenta(int idCliente, Guid idUsuario, List<DetalleVenta> detalles, List<Cobro> pagosIngresados)
        {
            if (detalles == null || !detalles.Any())
                throw new Exception("No se puede registrar una venta sin productos.");

            decimal montoTotalCalculado = 0;

            foreach (var detalle in detalles)
            {
                if (detalle.Cantidad <= 0)
                    throw new Exception($"La cantidad para el producto {detalle.IdProducto} debe ser mayor a cero.");
                if (detalle.PrecioUnitario <= 0)
                    throw new Exception($"El precio del producto {detalle.IdProducto} es inválido.");

                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                montoTotalCalculado += detalle.Subtotal;
            }

            // Validar que la lista de pagos cubra el monto de la venta
            if (pagosIngresados == null || !pagosIngresados.Any())
                throw new Exception("Debe ingresar al menos un método de pago.");

            decimal totalPagado = pagosIngresados.Sum(p => p.MontoCobrado);
            if (totalPagado < montoTotalCalculado)
                throw new Exception($"El pago ingresado (${totalPagado}) es menor al total de la venta (${montoTotalCalculado}).");

            Venta nuevaVenta = new Venta
            {
                IdCliente = idCliente,
                IdUsuario = idUsuario,
                FechaVenta = DateTime.Now,
                MontoTotal = montoTotalCalculado,
                Estado = "Completada"
            };

            string ticketGenerado = "TKT-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            foreach (var cobro in pagosIngresados)
            {
                cobro.FechaCobro = DateTime.Now;
                cobro.NumeroComprobante = ticketGenerado;
            }

            repositorio.RegistrarVentaCompleta(nuevaVenta, detalles, pagosIngresados);

            // NUEVO: Retornamos el ticket para mostrarlo en la UI
            return ticketGenerado;
        }
    }
}