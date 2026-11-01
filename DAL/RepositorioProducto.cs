using BE;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class RepositorioProducto
    {
        public RepositorioProducto() { }

        public List<Producto> ObtenerInventario()
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();

            DataTable? tablaProducto = ds.Tables["Producto"];

            if (tablaProducto == null)
                throw new Exception("Tabla de producto no encontrada en el DataSet");

            List<Producto> listInventario = new List<Producto>();

            foreach (DataRow dr in tablaProducto.Rows)
            {
                Producto p = new Producto();

                p.CodigoBarra = dr["IdProducto"].ToString() ?? string.Empty;
                p.CodigoSKU = dr["CodigoSKU"].ToString() ?? string.Empty; // Nuevo
                p.Nombre = dr["NombreBebida"].ToString() ?? string.Empty;

                // Nuevos campos para Ventas
                p.Precio = Convert.ToDecimal(dr["PrecioUnitarioLocal"]);
                p.Activo = Convert.ToBoolean(dr["Activo"]);

                p.StockActual = Convert.ToInt32(dr["StockActual"]);
                p.PuntoPedido = Convert.ToInt32(dr["PuntoPedido"]);

                listInventario.Add(p);
            }

            return listInventario;
        }
        public string? ObtenerProductoConMayorStock()
        {
            DataTable? dt = DAO.GetInstance.ObtenerDataSet().Tables["PRODUCTO"];
            if (dt == null || dt.Rows.Count == 0) return null;

            // Buscamos el producto inmovilizado (mayor stock actual)
            var idProducto = dt.AsEnumerable()
                .OrderByDescending(row => row.Field<int>("StockActual"))
                .Select(row => row.Field<string>("IdProducto"))
                .FirstOrDefault();

            return idProducto;
        }
        public DataTable ObtenerLotesPorProducto(string idProducto)
        {
            DataTable? dtLotes = DAO.GetInstance.ObtenerDataSet().Tables["LOTE_BEBIDA"];
            if (dtLotes == null) throw new Exception("La tabla LOTE_BEBIDA no está en el DataSet.");

            DataTable dtResultado = dtLotes.Clone();

            // Filtramos por el producto seleccionado, stock > 0, y ordenamos por vencimiento
            DataRow[] filas = dtLotes.Select($"IdProducto = '{idProducto}' AND StockActual > 0", "FechaVencimiento ASC");

            foreach (DataRow fila in filas)
            {
                dtResultado.ImportRow(fila);
            }
            return dtResultado;
        }
        public DataTable ObtenerTodosLosLotesActivos()
        {
            DataTable? dtLotes = DAO.GetInstance.ObtenerDataSet().Tables["LOTE_BEBIDA"];
            DataTable? dtProductos = DAO.GetInstance.ObtenerDataSet().Tables["PRODUCTO"];

            if (dtLotes == null || dtProductos == null)
                throw new Exception("Las tablas de Lotes o Productos no están en el DataSet.");

            // Armamos una tabla limpia para la vista
            DataTable dtResultado = new DataTable();
            dtResultado.Columns.Add("CodigoBarra", typeof(string));
            dtResultado.Columns.Add("Producto", typeof(string));
            dtResultado.Columns.Add("NumeroLote", typeof(string));
            dtResultado.Columns.Add("StockActual", typeof(int));
            dtResultado.Columns.Add("FechaVencimiento", typeof(DateTime));

            // LINQ para hacer un JOIN en memoria (solo lotes con stock)
            var query = from lote in dtLotes.AsEnumerable()
                        where lote.Field<int>("StockActual") > 0
                        join prod in dtProductos.AsEnumerable()
                        on lote.Field<string>("IdProducto") equals prod.Field<string>("IdProducto")
                        orderby lote.Field<DateTime>("FechaVencimiento") ascending
                        select new
                        {
                            // CORRECCIÓN: Leemos los nombres exactos de tu tabla SQL PRODUCTO
                            SKU = prod.Field<string>("CodigoSKU"),     // O "IdProducto", según uses
                            Nombre = prod.Field<string>("NombreBebida"), // Antes decía "Nombre"
                            Lote = lote.Field<string>("NumeroLote"),
                            Stock = lote.Field<int>("StockActual"),
                            Vto = lote.Field<DateTime>("FechaVencimiento")
                        };

            foreach (var item in query)
            {
                dtResultado.Rows.Add(item.SKU, item.Nombre, item.Lote, item.Stock, item.Vto);
            }

            return dtResultado;
        }

        public Producto? ObtenerPorId(string idProducto)
        {
            DataTable? dt = DAO.GetInstance.ObtenerDataSet().Tables["PRODUCTO"];
            DataRow[]? rows = dt?.Select($"IdProducto = '{idProducto}'");

            if (rows != null && rows.Length > 0)
            {
                return new Producto
                {
                    CodigoBarra = rows[0]["IdProducto"].ToString(),
                    Nombre = rows[0]["NombreBebida"].ToString(),
                    StockActual = Convert.ToInt32(rows[0]["StockActual"])
                };
            }
            return null;
        }
        public void GuardarNuevoProducto(Producto prod)
        {
            DAO dao = DAO.GetInstance;
            DataTable? dt = dao.ObtenerDataSet().Tables["PRODUCTO"];
            if (dt == null) throw new Exception("Tabla PRODUCTO no encontrada en el DataSet.");

            DataRow dr = dt.NewRow();
            dr["IdProducto"] = prod.CodigoBarra;
            dr["CodigoSKU"] = prod.CodigoBarra; // En tu modelo usamos el mismo valor para ID y SKU
            dr["NombreBebida"] = prod.Nombre;
            dr["PrecioUnitarioLocal"] = prod.Precio;
            dr["StockActual"] = 0; // Todo producto nuevo nace con 0 stock hasta que Compras lo recibe
            dr["PuntoPedido"] = prod.PuntoPedido;
            dr["Activo"] = 1; // 1 = True (Activo)

            dt.Rows.Add(dr);
            dao.SubirCambiosBD();
        }

        public void ActualizarProducto(Producto prod)
        {
            DAO dao = DAO.GetInstance;
            DataTable? dt = dao.ObtenerDataSet().Tables["PRODUCTO"];
            if (dt == null) throw new Exception("Tabla PRODUCTO no encontrada.");

            DataRow[] rows = dt.Select($"IdProducto = '{prod.CodigoBarra}'");
            if (rows.Length > 0)
            {
                rows[0]["NombreBebida"] = prod.Nombre;
                rows[0]["PrecioUnitarioLocal"] = prod.Precio;
                rows[0]["PuntoPedido"] = prod.PuntoPedido;

                dao.SubirCambiosBD();
            }
        }

        public void BajaLogicaProducto(string idProducto)
        {
            DAO dao = DAO.GetInstance;
            DataTable? dt = dao.ObtenerDataSet().Tables["PRODUCTO"];
            if (dt == null) throw new Exception("Tabla PRODUCTO no encontrada.");

            DataRow[] rows = dt.Select($"IdProducto = '{idProducto}'");
            if (rows.Length > 0)
            {
                rows[0]["Activo"] = 0; // Baja lógica
                dao.SubirCambiosBD();
            }
        }
    }
}
