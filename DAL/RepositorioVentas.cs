using BE;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class RepositorioVentas
    {
        // 1. Obtener la lista de clientes para mostrarlos en el Punto de Venta
        public DataView ObtenerClientesActivos()
        {
            DataTable? dt = DAO.GetInstance.ObtenerDataSet().Tables["CLIENTE"];
            if (dt == null) throw new Exception("La tabla CLIENTE no está mapeada en el DAO.");

            DataView dv = new DataView(dt);
            dv.RowFilter = "Activo = 1";
            dv.Sort = "RazonSocial ASC"; // Para que aparezcan ordenados alfabéticamente
            return dv;
        }

        // 2. Registrar toda la operación comercial en un solo movimiento
        public void RegistrarVentaCompleta(Venta venta, List<DetalleVenta> detalles, Cobro cobro)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();

            DataTable? dtVenta = ds.Tables["VENTA"];
            DataTable? dtDetalle = ds.Tables["DETALLE_VENTA"];
            DataTable? dtCobro = ds.Tables["COBRO"];
            DataTable? dtProducto = ds.Tables["Producto"];

            if (dtVenta == null || dtDetalle == null || dtCobro == null || dtProducto == null)
                throw new Exception("Faltan tablas de ventas o inventario en el DataSet.");

            // A. Insertar Cabecera de Venta
            DataRow filaVenta = dtVenta.NewRow();
            filaVenta["IdCliente"] = venta.IdCliente;
            filaVenta["IdUsuario"] = venta.IdUsuario;
            filaVenta["FechaVenta"] = venta.FechaVenta;
            filaVenta["MontoTotal"] = venta.MontoTotal;
            filaVenta["Estado"] = venta.Estado;
            dtVenta.Rows.Add(filaVenta);

            // Capturamos el ID autogenerado en memoria por el DataSet
            int idVentaGenerado = Convert.ToInt32(filaVenta["IdVenta"]);

            // B. Insertar Detalles y Descontar Stock
            foreach (var det in detalles)
            {
                // Guardar detalle
                DataRow filaDetalle = dtDetalle.NewRow();
                filaDetalle["IdVenta"] = idVentaGenerado;
                filaDetalle["IdProducto"] = det.IdProducto;
                filaDetalle["Cantidad"] = det.Cantidad;
                filaDetalle["PrecioUnitario"] = det.PrecioUnitario;
                filaDetalle["Subtotal"] = det.Subtotal;
                dtDetalle.Rows.Add(filaDetalle);

                // Descontar Stock del Inventario Principal
                DataRow[] filasProd = dtProducto.Select($"IdProducto = '{det.IdProducto}'");
                if (filasProd.Length > 0)
                {
                    int stockActual = Convert.ToInt32(filasProd[0]["StockActual"]);

                    // Validación defensiva por si venden más de lo que hay
                    if (stockActual < det.Cantidad)
                        throw new Exception($"Stock insuficiente para el código {det.IdProducto}. Stock actual: {stockActual}");

                    filasProd[0]["StockActual"] = stockActual - det.Cantidad;
                }
                else
                {
                    throw new Exception($"El producto {det.IdProducto} no existe en el inventario.");
                }
            }

            // C. Insertar el recibo de Cobro
            if (cobro != null)
            {
                DataRow filaCobro = dtCobro.NewRow();
                filaCobro["IdVenta"] = idVentaGenerado;
                filaCobro["FechaCobro"] = cobro.FechaCobro;
                filaCobro["MontoCobrado"] = cobro.MontoCobrado;
                filaCobro["MetodoPago"] = cobro.MetodoPago;
                filaCobro["NumeroComprobante"] = string.IsNullOrEmpty(cobro.NumeroComprobante) ? (object)DBNull.Value : cobro.NumeroComprobante;
                dtCobro.Rows.Add(filaCobro);
            }

            // D. Persistir todo en la base de datos de una sola vez
            dao.SubirCambiosBD();
        }
    }
}