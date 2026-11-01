using BE;
using System;
using System.Data;

namespace DAL
{
    public class RepositorioRecepcion
    {
        public void GuardarRecepcion(Recepcion nuevaRecepcion)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();

            DataTable? dtRecepcion = ds.Tables["RECEPCION"];
            DataTable? dtLote = ds.Tables["LOTE_BEBIDA"];
            DataTable? dtProducto = ds.Tables["PRODUCTO"];
            DataTable? dtOrdenCompra = ds.Tables["ORDEN_COMPRA"];

            if (dtRecepcion == null || dtLote == null || dtProducto == null || dtOrdenCompra == null)
                throw new Exception("Faltan tablas del módulo de recepción en el DataSet.");

            // 1. Crear Cabecera (RECEPCION)
            DataRow rowRecepcion = dtRecepcion.NewRow();
            rowRecepcion["IdOrden"] = nuevaRecepcion.IdOrden;
            rowRecepcion["NumeroRemito"] = nuevaRecepcion.NumeroRemito;
            rowRecepcion["FechaIngreso"] = nuevaRecepcion.FechaIngreso;
            dtRecepcion.Rows.Add(rowRecepcion);

            // Obtenemos el ID autogenerado en memoria para enganchar los hijos
            int idRecepcionGenerado = Convert.ToInt32(rowRecepcion["IdRecepcion"]);

            // 2. Crear Detalles (LOTE_BEBIDA) y Actualizar Stock (PRODUCTO)
            foreach (LoteBebida lote in nuevaRecepcion.LotesRecibidos)
            {
                // A) Nuevo lote físico
                DataRow rowLote = dtLote.NewRow();
                rowLote["IdRecepcion"] = idRecepcionGenerado;
                rowLote["IdProducto"] = lote.IdProducto;
                rowLote["NumeroLote"] = lote.NumeroLote;
                rowLote["FechaVencimiento"] = lote.FechaVencimiento;
                rowLote["CantidadRecibida"] = lote.CantidadRecibida;
                rowLote["StockActual"] = lote.StockActual;
                dtLote.Rows.Add(rowLote);

                // B) Sumar al stock global del PRODUCTO
                DataRow[] filasProducto = dtProducto.Select($"IdProducto = '{lote.IdProducto}'");
                if (filasProducto.Length > 0)
                {
                    DataRow rowProd = filasProducto[0];
                    int stockActual = Convert.ToInt32(rowProd["StockActual"]);
                    rowProd["StockActual"] = stockActual + lote.CantidadRecibida;
                }
            }

            // 3. Cambiar estado de la ORDEN_COMPRA a 'Recibida'
            DataRow[] filasOC = dtOrdenCompra.Select($"IdOrden = {nuevaRecepcion.IdOrden}");
            if (filasOC.Length > 0)
            {
                filasOC[0]["Estado"] = "Recibida";
            }

            // 4. Impactar todos los DataRows modificados a la Base de Datos física
            dao.SubirCambiosBD();
        }
        public List<LoteBebida> ObtenerLotesConStock()
        {
            DataTable? dtLote = DAO.GetInstance.ObtenerDataSet().Tables["LOTE_BEBIDA"];
            List<LoteBebida> lotes = new List<LoteBebida>();

            if (dtLote != null)
            {
                // Filtramos en memoria solo los que tienen stock
                foreach (DataRow row in dtLote.Select("StockActual > 0"))
                {
                    lotes.Add(new LoteBebida
                    {
                        IdLote = Convert.ToInt32(row["IdLote"]),
                        IdRecepcion = Convert.ToInt32(row["IdRecepcion"]),
                        IdProducto = row["IdProducto"].ToString(),
                        NumeroLote = row["NumeroLote"].ToString(),
                        FechaVencimiento = Convert.ToDateTime(row["FechaVencimiento"]),
                        CantidadRecibida = Convert.ToInt32(row["CantidadRecibida"]),
                        StockActual = Convert.ToInt32(row["StockActual"])
                    });
                }
            }
            return lotes;
        }
    }
}