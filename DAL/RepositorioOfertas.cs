using BE;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class RepositorioOfertas
    {
        public void GuardarOfertasSugeridas(List<OfertaSugerida> ofertas)
        {
            DAO dao = DAO.GetInstance;
            DataTable? dtOfertas = dao.ObtenerDataSet().Tables["OFERTA_SUGERIDA"];

            if (dtOfertas == null) throw new Exception("La tabla OFERTA_SUGERIDA no está mapeada en el DataSet.");

            foreach (var oferta in ofertas)
            {
                DataRow row = dtOfertas.NewRow();
                row["IdLote"] = oferta.IdLote;

                // Manejo de nulos para el Fallback (cuando no hay gancho asociado)
                if (string.IsNullOrEmpty(oferta.IdProductoAsociado))
                    row["IdProductoAsociado"] = DBNull.Value;
                else
                    row["IdProductoAsociado"] = oferta.IdProductoAsociado;

                row["TituloOferta"] = oferta.TituloOferta;
                row["DescuentoSugerido"] = oferta.DescuentoSugerido;
                row["Estado"] = oferta.Estado;
                row["FechaGeneracion"] = oferta.FechaGeneracion;

                dtOfertas.Rows.Add(row);

                // Capturar el ID generado localmente por si lo necesitamos
                oferta.IdOferta = Convert.ToInt32(row["IdOferta"]);
            }

            dao.SubirCambiosBD();
        }
        public List<OfertaSugerida> ObtenerOfertasAprobadas()
        {
            DataTable? dtOfertas = DAO.GetInstance.ObtenerDataSet().Tables["OFERTA_SUGERIDA"];
            List<OfertaSugerida> activas = new List<OfertaSugerida>();

            if (dtOfertas != null)
            {
                // Filtramos solo las aprobadas por el administrador
                foreach (DataRow row in dtOfertas.Select("Estado = 'Aprobada'"))
                {
                    activas.Add(new OfertaSugerida
                    {
                        IdOferta = Convert.ToInt32(row["IdOferta"]),
                        IdLote = Convert.ToInt32(row["IdLote"]),
                        IdProductoAsociado = row["IdProductoAsociado"] != DBNull.Value ? row["IdProductoAsociado"].ToString() : null,
                        TituloOferta = row["TituloOferta"].ToString(),
                        DescuentoSugerido = Convert.ToDecimal(row["DescuentoSugerido"]),
                        Estado = row["Estado"].ToString()
                    });
                }
            }
            return activas;
        }

        public DataTable ObtenerHistorialOfertasParaUI()
        {
            DataTable? dtOfertas = DAO.GetInstance.ObtenerDataSet().Tables["OFERTA_SUGERIDA"];
            DataTable? dtLotes = DAO.GetInstance.ObtenerDataSet().Tables["LOTE_BEBIDA"];
            DataTable? dtProductos = DAO.GetInstance.ObtenerDataSet().Tables["PRODUCTO"];

            DataTable dtHistorial = new DataTable();
            dtHistorial.Columns.Add("IdOferta", typeof(int));
            dtHistorial.Columns.Add("Producto", typeof(string));
            dtHistorial.Columns.Add("Descuento", typeof(decimal));
            dtHistorial.Columns.Add("Estado", typeof(string));
            dtHistorial.Columns.Add("Fecha", typeof(DateTime));

            if (dtOfertas != null && dtLotes != null && dtProductos != null)
            {
                var query = from oferta in dtOfertas.AsEnumerable()
                            // Aseguramos que la fila no esté marcada como eliminada en memoria
                            where oferta.RowState != DataRowState.Deleted
                            join lote in dtLotes.AsEnumerable() on oferta.Field<int>("IdLote") equals lote.Field<int>("IdLote")
                            join prod in dtProductos.AsEnumerable() on lote.Field<string>("IdProducto") equals prod.Field<string>("IdProducto")
                            orderby oferta.Field<int>("IdOferta") descending
                            select new
                            {
                                IdOferta = oferta.Field<int>("IdOferta"),
                                Producto = prod.Field<string>("NombreBebida"),
                                Descuento = oferta.Field<decimal>("DescuentoSugerido"),
                                Estado = oferta.Field<string>("Estado"),
                                Fecha = oferta.Field<DateTime>("FechaGeneracion")
                            };

                foreach (var item in query)
                {
                    dtHistorial.Rows.Add(item.IdOferta, item.Producto, item.Descuento, item.Estado, item.Fecha);
                }
            }
            return dtHistorial;
        }

        public void EliminarOfertaFisica(int idOferta)
        {
            DataTable? dtOfertas = DAO.GetInstance.ObtenerDataSet().Tables["OFERTA_SUGERIDA"];
            if (dtOfertas == null) return;

            // Buscamos la fila por su ID
            DataRow[] rows = dtOfertas.Select($"IdOferta = {idOferta}");
            if (rows.Length > 0)
            {
                // En ADO.NET, .Delete() marca la fila para borrado, no la quita de memoria al instante
                rows[0].Delete();

                // Impactamos el DELETE en la base de datos SQL
                DAO.GetInstance.SubirCambiosBD();
            }
        }
        public List<OfertaSugerida> ObtenerOfertasPendientes()
        {
            DataTable? dtOfertas = DAO.GetInstance.ObtenerDataSet().Tables["OFERTA_SUGERIDA"];
            List<OfertaSugerida> pendientes = new List<OfertaSugerida>();

            if (dtOfertas != null)
            {
                // Filtramos específicamente las filas cuyo estado sea "Pendiente"
                foreach (DataRow row in dtOfertas.Select("Estado = 'Pendiente'"))
                {
                    // Omitimos las filas que estén marcadas para eliminación en memoria
                    if (row.RowState == DataRowState.Deleted) continue;

                    pendientes.Add(new OfertaSugerida
                    {
                        IdOferta = Convert.ToInt32(row["IdOferta"]),
                        IdLote = Convert.ToInt32(row["IdLote"]),
                        IdProductoAsociado = row["IdProductoAsociado"] != DBNull.Value ? row["IdProductoAsociado"].ToString() : null,
                        TituloOferta = row["TituloOferta"].ToString(),
                        DescuentoSugerido = Convert.ToDecimal(row["DescuentoSugerido"]),
                        Estado = row["Estado"].ToString(),
                        FechaGeneracion = row["FechaGeneracion"] != DBNull.Value ? Convert.ToDateTime(row["FechaGeneracion"]) : DateTime.Now
                    });
                }
            }
            return pendientes;
        }
        // Método para que la BLL pueda leer los lotes sin tocar el DAO
        public DataTable? ObtenerLotes()
        {
            return DAO.GetInstance.ObtenerDataSet().Tables["LOTE_BEBIDA"];
        }

        // Método para que la BLL pueda leer los productos sin tocar el DAO
        public DataTable? ObtenerProductos()
        {
            return DAO.GetInstance.ObtenerDataSet().Tables["PRODUCTO"];
        }
        public void ActualizarEstadoOferta(OfertaSugerida oferta)
        {
            DAO dao = DAO.GetInstance;
            DataTable? dtOfertas = dao.ObtenerDataSet().Tables["OFERTA_SUGERIDA"];

            if (dtOfertas != null)
            {
                DataRow[] filas = dtOfertas.Select($"IdOferta = {oferta.IdOferta}");
                if (filas.Length > 0)
                {
                    filas[0]["Estado"] = oferta.Estado;
                    if (oferta.Estado == "Aprobada")
                    {
                        filas[0]["DescuentoSugerido"] = oferta.DescuentoSugerido; // Por si el admin lo ajustó
                    }
                    dao.SubirCambiosBD();
                }
            }
        }
    }
}