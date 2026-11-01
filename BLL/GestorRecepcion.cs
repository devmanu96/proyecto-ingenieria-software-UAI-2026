using BE;
using DAL;
using System;
using System.Data;
using System.Linq;

namespace BLL
{
    public class GestorRecepcion
    {
        private RepositorioRecepcion repositorio;

        public GestorRecepcion()
        {
            repositorio = new RepositorioRecepcion();
        }

        public void ProcesarIngresoMercaderia(Recepcion recepcion)
        {
            if (recepcion.IdOrden <= 0)
                throw new Exception("La recepción debe estar asociada a una Orden de Compra válida.");

            // NUEVO: Validación estricta BLL contra el DataSet (Flujo financiero)
            DataTable? dtOrdenes = DAO.GetInstance.ObtenerDataSet().Tables["ORDEN_COMPRA"];
            if (dtOrdenes != null)
            {
                DataRow[] filas = dtOrdenes.Select($"IdOrden = {recepcion.IdOrden}");
                if (filas.Length > 0)
                {
                    string estado = filas[0]["Estado"].ToString() ?? "";
                    if (estado != "Pagada")
                    {
                        throw new Exception($"Operación rechazada: La orden {recepcion.IdOrden} se encuentra en estado '{estado}'. Debe estar 'Pagada' por Contabilidad para recibir la mercadería.");
                    }
                }
                else
                {
                    throw new Exception("La orden de compra no existe en la base de datos.");
                }
            }

            if (string.IsNullOrWhiteSpace(recepcion.NumeroRemito))
                throw new Exception("El número de remito es obligatorio.");

            if (recepcion.LotesRecibidos == null || !recepcion.LotesRecibidos.Any())
                throw new Exception("No se puede procesar un ingreso de mercadería sin productos (lotes).");


            foreach (var lote in recepcion.LotesRecibidos)
            {
                if (string.IsNullOrWhiteSpace(lote.NumeroLote))
                    throw new Exception($"Debe ingresar el número de lote impreso en la caja para el producto {lote.IdProducto}.");

                if (lote.CantidadRecibida <= 0)
                    throw new Exception($"La cantidad recibida para el lote {lote.NumeroLote} debe ser mayor a cero.");


                if (lote.FechaVencimiento.Date <= DateTime.Now.Date)
                {
                    throw new Exception($"Operación rechazada: El lote {lote.NumeroLote} del producto {lote.IdProducto} figura como vencido. Devuelva la mercadería al proveedor.");
                }
            }

            repositorio.GuardarRecepcion(recepcion);
        }
    }
}