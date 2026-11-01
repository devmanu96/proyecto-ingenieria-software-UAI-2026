using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BE;
using DAL; // Asumiremos que los repositorios tendrán estos métodos en el próximo paso

namespace BLL
{
    public class GestorOfertas
    {
        private RepositorioOfertas repoOfertas;
        private RepositorioRecepcion repoLotes;
        private RepositorioVentas repoVentas;
        private RepositorioProducto repoProducto;

        // Umbrales heurísticos del algoritmo
        private const int DIAS_ALERTA_VENCIMIENTO = 90;
        private const int MIN_VENTAS_MADUREZ = 50;

        public GestorOfertas()
        {
            repoOfertas = new RepositorioOfertas();
            repoLotes = new RepositorioRecepcion();
            repoVentas = new RepositorioVentas();
            repoProducto = new RepositorioProducto();
        }

        //public List<OfertaSugerida> AnalizarYGenerarOfertas()
        //{
        //    List<OfertaSugerida> nuevasOfertas = new List<OfertaSugerida>();

        //    // 1. Obtener todos los lotes con stock > 0
        //    List<LoteBebida> lotesActivos = repoLotes.ObtenerLotesConStock();

        //    // 2. Identificar "Productos Ancla": Lotes en riesgo de vencer
        //    var lotesEnRiesgo = lotesActivos
        //        .Where(l => l.DiasParaVencimiento <= DIAS_ALERTA_VENCIMIENTO)
        //        .ToList();

        //    if (!lotesEnRiesgo.Any())
        //        return nuevasOfertas; // Almacén sano, no se generan ofertas

        //    // 3. Evaluar Madurez de Datos (Solución al Arranque en Frío)
        //    int totalVentasHistoricas = repoVentas.ContarVentasTotales();
        //    bool datosMaduros = totalVentasHistoricas >= MIN_VENTAS_MADUREZ;

        //    foreach (var loteAncla in lotesEnRiesgo)
        //    {
        //        OfertaSugerida propuesta = new OfertaSugerida
        //        {
        //            IdLote = loteAncla.IdLote,
        //            Estado = "Pendiente",
        //            FechaGeneracion = DateTime.Now
        //        };

        //        if (datosMaduros)
        //        {
        //            // ==========================================================
        //            // MODO A: MINERÍA DE DATOS (Market Basket Analysis)
        //            // ==========================================================
        //            string? idProductoGancho = repoVentas.ObtenerProductoMasAsociado(loteAncla.IdProducto);

        //            if (!string.IsNullOrEmpty(idProductoGancho))
        //            {
        //                propuesta.IdProductoAsociado = idProductoGancho;
        //                propuesta.DescuentoSugerido = CalcularDescuento(loteAncla);
        //                propuesta.TituloOferta = $"Combo Inteligente: Llevando {ObtenerNombre(idProductoGancho)}, {propuesta.DescuentoSugerido}% OFF en {ObtenerNombre(loteAncla.IdProducto)} (Vence en {loteAncla.DiasParaVencimiento} días)";
        //            }
        //        }

        //        // Si no hay datos maduros o el Modo A no encontró asociaciones fuertes, entra el Fallback
        //        if (string.IsNullOrEmpty(propuesta.IdProductoAsociado))
        //        {
        //            // ==========================================================
        //            // MODO B: ARRANQUE EN FRÍO (Fallback Logístico)
        //            // ==========================================================
        //            string? idProductoSobreStock = repoProducto.ObtenerProductoConMayorStock();

        //            if (!string.IsNullOrEmpty(idProductoSobreStock) && idProductoSobreStock != loteAncla.IdProducto)
        //            {
        //                // Fallback 1: Cruzar con el producto que más abunda en el galpón
        //                propuesta.IdProductoAsociado = idProductoSobreStock;
        //                propuesta.DescuentoSugerido = CalcularDescuento(loteAncla);
        //                propuesta.TituloOferta = $"Oportunidad Logística: Llevando {ObtenerNombre(idProductoSobreStock)}, {propuesta.DescuentoSugerido}% OFF en {ObtenerNombre(loteAncla.IdProducto)}";
        //            }
        //            else
        //            {
        //                // Fallback 2: Liquidación directa por volumen
        //                propuesta.IdProductoAsociado = null;
        //                propuesta.DescuentoSugerido = CalcularDescuento(loteAncla) + 10; // Extra descuento
        //                propuesta.TituloOferta = $"LIQUIDACIÓN FEFO: {propuesta.DescuentoSugerido}% OFF en {ObtenerNombre(loteAncla.IdProducto)}";
        //            }
        //        }

        //        nuevasOfertas.Add(propuesta);
        //    }

        //    // Guardamos masivamente las sugerencias en la Base de Datos usando el DAO Desconectado
        //    repoOfertas.GuardarOfertasSugeridas(nuevasOfertas);

        //    return nuevasOfertas;
        //}
        // 1. Método para mostrar el cartel preventivo apenas el vendedor selecciona un producto
        public void EjecutarMotorAnaliticoCompleto()
        {
            DataTable? dtLotes = repoOfertas.ObtenerLotes();
            DataTable? dtProductos = repoOfertas.ObtenerProductos();

            // 1. Cargamos el "historial" para no repetir sugerencias que ya están en curso
            List<OfertaSugerida> pendientes = repoOfertas.ObtenerOfertasPendientes();
            List<OfertaSugerida> aprobadas = repoOfertas.ObtenerOfertasAprobadas();
            var ofertasActivasDB = pendientes.Concat(aprobadas).ToList();

            if (dtLotes == null || dtProductos == null) return;

            List<OfertaSugerida> nuevasOfertas = new List<OfertaSugerida>();

            foreach (DataRow filaLote in dtLotes.Rows)
            {
                if (filaLote.RowState == DataRowState.Deleted) continue;

                int stockActual = Convert.ToInt32(filaLote["StockActual"]);
                if (stockActual <= 0) continue;

                DateTime fechaVto = Convert.ToDateTime(filaLote["FechaVencimiento"]);
                int diasRestantes = (fechaVto - DateTime.Now).Days;

                int idLote = Convert.ToInt32(filaLote["IdLote"]);
                string idProducto = filaLote["IdProducto"].ToString() ?? "";

                string nombreBebida = dtProductos.AsEnumerable()
                    .FirstOrDefault(p => p.Field<string>("IdProducto") == idProducto)?
                    .Field<string>("NombreBebida") ?? "Producto";

                // Variables temporales para evaluar antes de guardar
                string tituloPropuesto = "";
                decimal descuentoPropuesto = 0;
                string? ganchoPropuesto = null;

                // REGLA 1: URGENTE FEFO
                if (diasRestantes > 0 && diasRestantes <= 30)
                {
                    tituloPropuesto = $"¡URGENTE FEFO! {nombreBebida} vence en {diasRestantes} días. 60% OFF directo.";
                    descuentoPropuesto = 60.00m;
                }
                // REGLA 2: EXCESO STOCK
                else if (diasRestantes > 30 && diasRestantes <= 90 && stockActual > 100)
                {
                    tituloPropuesto = $"ALERTA STOCK: Excedente de {nombreBebida} ({stockActual} unids). 35% OFF recomendado.";
                    descuentoPropuesto = 35.00m;
                }
                // REGLA 3: VENTA CRUZADA
                else if (diasRestantes > 30 && diasRestantes <= 60 && stockActual <= 100)
                {
                    var loteGancho = dtLotes.AsEnumerable()
                        .FirstOrDefault(l => l.RowState != DataRowState.Deleted &&
                                             l.Field<string>("IdProducto") != idProducto &&
                                             l.Field<int>("StockActual") > 20);

                    if (loteGancho != null)
                    {
                        string idGancho = loteGancho.Field<string>("IdProducto") ?? "";
                        string nombreGancho = dtProductos.AsEnumerable()
                            .FirstOrDefault(p => p.Field<string>("IdProducto") == idGancho)?
                            .Field<string>("NombreBebida") ?? "Otro Producto";

                        tituloPropuesto = $"Oportunidad Logística: Llevando {nombreGancho}, 50% OFF en {nombreBebida}.";
                        descuentoPropuesto = 50.00m;
                        ganchoPropuesto = idGancho;
                    }
                }

                // ==============================================================================
                // FILTRO INTELIGENTE ANTI-DUPLICADOS
                // ==============================================================================
                if (!string.IsNullOrEmpty(tituloPropuesto))
                {
                    // A) Evita textos idénticos en la misma ejecución (Ej: múltiples combos iguales)
                    bool esDuplicadoEnMemoria = nuevasOfertas.Any(o => o.TituloOferta == tituloPropuesto);

                    // B) Evita proponer algo que el usuario ya tiene pendiente o ya aprobó en BD
                    bool esDuplicadoEnBD = ofertasActivasDB.Any(o => o.TituloOferta == tituloPropuesto);

                    // C) Evita que un mismo lote reciba múltiples ofertas si cambian los días
                    bool loteYaTieneOferta = ofertasActivasDB.Any(o => o.IdLote == idLote) ||
                                             nuevasOfertas.Any(o => o.IdLote == idLote);

                    if (!esDuplicadoEnMemoria && !esDuplicadoEnBD && !loteYaTieneOferta)
                    {
                        nuevasOfertas.Add(new OfertaSugerida
                        {
                            IdLote = idLote,
                            IdProductoAsociado = ganchoPropuesto,
                            TituloOferta = tituloPropuesto,
                            DescuentoSugerido = descuentoPropuesto,
                            Estado = "Pendiente",
                            FechaGeneracion = DateTime.Now
                        });
                    }
                }
            }

            if (nuevasOfertas.Any())
            {
                repoOfertas.GuardarOfertasSugeridas(nuevasOfertas);
            }
        }

        public string? ObtenerTextoPromoRelacionada(string idProducto)
        {
            DataTable? dtOfertas = DAO.GetInstance.ObtenerDataSet().Tables["OFERTA_SUGERIDA"];
            DataTable? dtLotes = DAO.GetInstance.ObtenerDataSet().Tables["LOTE_BEBIDA"];
            if (dtOfertas == null || dtLotes == null) return null;

            var promo = (from oferta in dtOfertas.AsEnumerable()
                         where oferta.Field<string>("Estado") == "Aprobada"
                         join lote in dtLotes.AsEnumerable() on oferta.Field<int>("IdLote") equals lote.Field<int>("IdLote")
                         // Filtramos si el producto seleccionado es el que tiene rebaja (Baggio) O es el gancho (Pepsi)
                         where lote.Field<string>("IdProducto") == idProducto ||
                              (!oferta.IsNull("IdProductoAsociado") && oferta.Field<string>("IdProductoAsociado") == idProducto)
                         orderby oferta.Field<int>("IdOferta") descending
                         select oferta.Field<string>("TituloOferta")).FirstOrDefault();

            return promo;
        }

        // 2. Método que calcula el descuento evaluando qué más hay en el carrito
        public decimal CalcularDescuentoParaProducto(string idProducto, List<string> productosEnCarrito)
        {
            DataTable? dtOfertas = DAO.GetInstance.ObtenerDataSet().Tables["OFERTA_SUGERIDA"];
            DataTable? dtLotes = DAO.GetInstance.ObtenerDataSet().Tables["LOTE_BEBIDA"];
            if (dtOfertas == null || dtLotes == null) return 0m;

            // A) Prioridad 1: Buscar Combos (Ej: Si analizo Baggio, reviso si Pepsi está en la lista del carrito)
            var combo = (from oferta in dtOfertas.AsEnumerable()
                         where oferta.Field<string>("Estado") == "Aprobada" && !oferta.IsNull("IdProductoAsociado")
                         join lote in dtLotes.AsEnumerable() on oferta.Field<int>("IdLote") equals lote.Field<int>("IdLote")
                         where lote.Field<string>("IdProducto") == idProducto
                         && productosEnCarrito.Contains(oferta.Field<string>("IdProductoAsociado")) // Condición clave
                         orderby oferta.Field<int>("IdOferta") descending
                         select oferta.Field<decimal>("DescuentoSugerido")).FirstOrDefault();

            if (combo > 0) return combo;

            // B) Prioridad 2: Liquidación directa por fecha de vencimiento individual
            var directa = (from oferta in dtOfertas.AsEnumerable()
                           where oferta.Field<string>("Estado") == "Aprobada" && oferta.IsNull("IdProductoAsociado")
                           join lote in dtLotes.AsEnumerable() on oferta.Field<int>("IdLote") equals lote.Field<int>("IdLote")
                           where lote.Field<string>("IdProducto") == idProducto
                           orderby oferta.Field<int>("IdOferta") descending
                           select oferta.Field<decimal>("DescuentoSugerido")).FirstOrDefault();

            return directa;
        }
        public OfertaSugerida? VerificarOportunidadUpselling(string idProductoEscaneado)
        {
            var ofertasActivas = repoOfertas.ObtenerOfertasAprobadas();

            // Buscamos si el producto que el cliente está comprando es el "Gancho" de una promo
            // O si es una promoción de liquidación directa (donde IdProductoAsociado es null, aplicable a todo)
            return ofertasActivas.FirstOrDefault(o => o.IdProductoAsociado == idProductoEscaneado);
        }
        public void AprobarOferta(OfertaSugerida oferta)
        {
            if (oferta == null) throw new Exception("Oferta inválida.");
            oferta.Estado = "Aprobada";
            repoOfertas.ActualizarEstadoOferta(oferta);
        }

        public void RechazarOferta(OfertaSugerida oferta)
        {
            if (oferta == null) throw new Exception("Oferta inválida.");
            oferta.Estado = "Rechazada";
            repoOfertas.ActualizarEstadoOferta(oferta);
        }

        // --- Métodos Privados Auxiliares ---

        public List<OfertaSugerida> ObtenerOfertasPendientes()
        {
            return repoOfertas.ObtenerOfertasPendientes();
        }
        public DataTable ConsultarHistorialCompleto()
        {
            return repoOfertas.ObtenerHistorialOfertasParaUI();
        }

        public void EliminarOfertaDefinitivamente(int idOferta)
        {
            repoOfertas.EliminarOfertaFisica(idOferta);
        }

        private string ObtenerNombre(string idProducto)
        {
            var prod = repoProducto.ObtenerPorId(idProducto);
            return prod != null ? prod.Nombre : "Bebida Seleccionada";
        }
    }
}