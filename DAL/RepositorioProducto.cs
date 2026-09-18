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
                p.Nombre = dr["NombreBebida"].ToString() ?? string.Empty;
                p.StockActual = Convert.ToInt32(dr["StockActual"]);
                p.PuntoPedido = Convert.ToInt32(dr["PuntoPedido"]);

                listInventario.Add(p);
            }

            return listInventario;
        }
    }
}
