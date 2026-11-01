using System;
using System.Collections.Generic;
using System.Data;
using System.Linq; // Fundamental para la minería de datos
using BE;

namespace DAL
{
    public class RepositorioVentas
    {
        public RepositorioVentas()
        {
        }

        public DataView ObtenerClientesActivos()
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable? dtCliente = ds.Tables["CLIENTE"];
            if (dtCliente == null) return new DataView(); // Previene advertencias CS8602

            DataView dv = new DataView(dtCliente);
            dv.RowFilter = "Activo = 1";
            return dv;
        }

        public void RegistrarVentaCompleta(Venta nuevaVenta, List<DetalleVenta> detalles, List<Cobro> pagosIngresados)
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();

            DataTable? dtVenta = ds.Tables["VENTA"];
            DataTable? dtDetalle = ds.Tables["DETALLE_VENTA"];
            DataTable? dtProducto = ds.Tables["PRODUCTO"];
            DataTable? dtCobro = ds.Tables["COBRO"];

            if (dtVenta == null || dtDetalle == null || dtProducto == null || dtCobro == null)
                throw new Exception("Faltan tablas del módulo de ventas en el DataSet.");

            // 1. Insertar Cabecera de la VENTA
            DataRow rowVenta = dtVenta.NewRow();
            rowVenta["IdCliente"] = nuevaVenta.IdCliente;
            rowVenta["IdUsuario"] = nuevaVenta.IdUsuario;
            rowVenta["FechaVenta"] = nuevaVenta.FechaVenta;
            rowVenta["MontoTotal"] = nuevaVenta.MontoTotal;
            rowVenta["Estado"] = nuevaVenta.Estado;
            dtVenta.Rows.Add(rowVenta);

            int idVentaGenerado = Convert.ToInt32(rowVenta["IdVenta"]);

            // 2. Insertar DETALLES y descontar STOCK
            foreach (var detalle in detalles)
            {
                DataRow rowDetalle = dtDetalle.NewRow();
                rowDetalle["IdVenta"] = idVentaGenerado;
                rowDetalle["IdProducto"] = detalle.IdProducto;
                rowDetalle["Cantidad"] = detalle.Cantidad;
                rowDetalle["PrecioUnitario"] = detalle.PrecioUnitario;
                rowDetalle["Subtotal"] = detalle.Subtotal;
                dtDetalle.Rows.Add(rowDetalle);

                DataRow[] filasProducto = dtProducto.Select($"IdProducto = '{detalle.IdProducto}'");
                if (filasProducto.Length > 0)
                {
                    DataRow rowProd = filasProducto[0];
                    int stockActual = Convert.ToInt32(rowProd["StockActual"]);
                    rowProd["StockActual"] = stockActual - detalle.Cantidad;
                }
            }

            // 3. Insertar Lista de COBROS
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

            DAO.GetInstance.SubirCambiosBD();
        }

        public DataView ObtenerHistorialVentas()
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            if (ds.Tables.Contains("VISTA_HISTORIAL_VENTAS"))
            {
                return ds.Tables["VISTA_HISTORIAL_VENTAS"]!.DefaultView;
            }
            return new DataView(); // Previene error si la tabla es nula
        }

        // =========================================================
        // MÉTODOS AGREGADOS PARA EL MOTOR DE OFERTAS (ARREGLA CS1061)
        // =========================================================

        public int ContarVentasTotales()
        {
            DataTable? dtVenta = DAO.GetInstance.ObtenerDataSet().Tables["VENTA"];
            return dtVenta != null ? dtVenta.Rows.Count : 0;
        }

        public string? ObtenerProductoMasAsociado(string idProductoAncla)
        {
            DataTable? dtDetalle = DAO.GetInstance.ObtenerDataSet().Tables["DETALLE_VENTA"];
            if (dtDetalle == null || dtDetalle.Rows.Count == 0) return null;

            // 1. Encontramos los IDs de las Ventas donde el cliente compró el producto Ancla
            var idsVentasDelAncla = dtDetalle.AsEnumerable()
                .Where(row => row.Field<string>("IdProducto") == idProductoAncla)
                .Select(row => row.Field<int>("IdVenta"))
                .Distinct()
                .ToList();

            if (!idsVentasDelAncla.Any()) return null;

            // 2. Buscamos qué OTRO producto aparece con más frecuencia en ESOS mismos tickets
            var idProductoGancho = dtDetalle.AsEnumerable()
                .Where(row => idsVentasDelAncla.Contains(row.Field<int>("IdVenta")) && row.Field<string>("IdProducto") != idProductoAncla)
                .GroupBy(row => row.Field<string>("IdProducto"))
                .OrderByDescending(grupo => grupo.Count()) // Ordena por el más comprado junto al ancla
                .Select(grupo => grupo.Key)
                .FirstOrDefault();

            return idProductoGancho;
        }
    }
}