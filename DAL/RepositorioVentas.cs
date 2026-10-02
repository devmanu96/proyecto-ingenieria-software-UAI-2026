using System;
using System.Collections.Generic;
using System.Data;
using BE;

namespace DAL
{
    public class RepositorioVentas
    {
        public RepositorioVentas()
        {
        }

        // Se mantiene tu método original intacto
        public DataView ObtenerClientesActivos()
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            // Filtramos directamente sobre la tabla CLIENTE del DataSet en memoria
            DataView dv = new DataView(ds.Tables["CLIENTE"]);
            dv.RowFilter = "Activo = 1";
            return dv;
        }

        // Método actualizado para recibir la lista de Detalles y la lista de Cobros
        public void RegistrarVentaCompleta(Venta nuevaVenta, List<DetalleVenta> detalles, List<Cobro> pagosIngresados)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();

            // 1. Insertar Cabecera de la VENTA
            DataTable dtVenta = ds.Tables["VENTA"];
            DataRow rowVenta = dtVenta.NewRow();

            rowVenta["IdCliente"] = nuevaVenta.IdCliente;
            rowVenta["IdUsuario"] = nuevaVenta.IdUsuario;
            rowVenta["FechaVenta"] = nuevaVenta.FechaVenta;
            rowVenta["MontoTotal"] = nuevaVenta.MontoTotal;
            rowVenta["Estado"] = nuevaVenta.Estado;

            dtVenta.Rows.Add(rowVenta);

            // Obtenemos el IdVenta generado localmente por el DataTable para vincular los hijos
            int idVentaGenerado = Convert.ToInt32(rowVenta["IdVenta"]);

            // 2. Insertar DETALLES y descontar STOCK
            DataTable dtDetalle = ds.Tables["DETALLE_VENTA"];
            DataTable dtProducto = ds.Tables["PRODUCTO"];

            foreach (var detalle in detalles)
            {
                // A. Insertar el detalle
                DataRow rowDetalle = dtDetalle.NewRow();
                rowDetalle["IdVenta"] = idVentaGenerado;
                rowDetalle["IdProducto"] = detalle.IdProducto;
                rowDetalle["Cantidad"] = detalle.Cantidad;
                rowDetalle["PrecioUnitario"] = detalle.PrecioUnitario;
                rowDetalle["Subtotal"] = detalle.Subtotal;

                dtDetalle.Rows.Add(rowDetalle);

                // B. Descontar el stock en la tabla PRODUCTO en memoria
                DataRow[] filasProducto = dtProducto.Select($"IdProducto = '{detalle.IdProducto}'");
                if (filasProducto.Length > 0)
                {
                    DataRow rowProd = filasProducto[0];
                    int stockActual = Convert.ToInt32(rowProd["StockActual"]);
                    rowProd["StockActual"] = stockActual - detalle.Cantidad;
                }
            }

            // 3. Insertar Lista de COBROS (Múltiples métodos de pago)
            DataTable dtCobro = ds.Tables["COBRO"];

            foreach (var cobro in pagosIngresados)
            {
                DataRow rowCobro = dtCobro.NewRow();
                rowCobro["IdVenta"] = idVentaGenerado;
                rowCobro["FechaCobro"] = cobro.FechaCobro;
                rowCobro["MontoCobrado"] = cobro.MontoCobrado;
                rowCobro["MetodoPago"] = cobro.MetodoPago;
                rowCobro["NumeroTarjeta"] = string.IsNullOrEmpty(cobro.NumeroTarjeta) ? DBNull.Value : cobro.NumeroTarjeta;
                rowCobro["Cuotas"] = cobro.Cuotas == 0 ? 1 : cobro.Cuotas;
                rowCobro["NumeroComprobante"] = string.IsNullOrEmpty(cobro.NumeroComprobante) ? DBNull.Value : cobro.NumeroComprobante;

                dtCobro.Rows.Add(rowCobro);
            }

            // 4. Impactar todos los cambios (Venta, Detalles, Cobros y actualización de Productos) en SQL Server
            DAO.GetInstance.SubirCambiosBD();
        }

        // Nuevo método para consumir la vista VISTA_HISTORIAL_VENTAS
        public DataView ObtenerHistorialVentas()
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();

            // Verificamos que la vista exista en el DataSet (asegúrate de que tu DAO haga el 'Fill' de esta vista)
            if (ds.Tables.Contains("VISTA_HISTORIAL_VENTAS"))
            {
                return ds.Tables["VISTA_HISTORIAL_VENTAS"].DefaultView;
            }

            return null;
        }
    }
}