using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class RepositorioCompras
    {
        public DataView ObtenerCatalogoB2B()
        {
            DataTable? dt = DAO.GetInstance.ObtenerDataSet().Tables["CATALOGO_PROVEEDOR"];
            if (dt == null) throw new Exception("La tabla CATALOGO_PROVEEDOR no está en el DataSet.");

            return new DataView(dt);
        }

        // CORRECCIÓN: Devuelve DataView, previene NullReferenceException y aplica filtro combinado
        public DataView ObtenerOrdenesPendientesDeRecepcion()
        {
            DataTable? dtOrdenes = DAO.GetInstance.ObtenerDataSet().Tables["ORDEN_COMPRA"];

            if (dtOrdenes == null)
                throw new Exception("La tabla ORDEN_COMPRA no se encuentra en el DataSet.");

            DataView dvOrdenes = new DataView(dtOrdenes);

            // LA CLAVE: Mostramos emitidas (en viaje) y pagadas (habilitadas para recibir)
            dvOrdenes.RowFilter = "Estado IN ('Emitida', 'Pagada')";

            return dvOrdenes;
        }

        public Dictionary<int, int> ObtenerMapaProveedoresTab()
        {
            // Mapeo estático según tu estructura actual (TabIndex -> IdProveedor)
            // 0: Coca-Cola (ID 1)
            // 1: PepsiCo (ID 2)
            // 2: Manaos (ID 3)
            // 3: Baggio (ID 4)
            Dictionary<int, int> mapa = new Dictionary<int, int>
            {
                { 0, 1 },
                { 1, 2 },
                { 2, 3 },
                { 3, 4 }
            };
            return mapa;
        }
    }
}