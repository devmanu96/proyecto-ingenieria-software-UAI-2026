using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL
{
    public class RepositorioCatalogoProveedor
    {
        public DataView ObtenerVistaCatalogo()
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable? dtCatalogo = ds.Tables["CATALOGO_PROVEEDOR"];

            if (dtCatalogo == null) throw new Exception("La tabla CATALOGO_PROVEEDOR no está en el DataSet.");

            return new DataView(dtCatalogo);
        }

        public Dictionary<int, int> MapearTabsConIdsProveedores()
        {
            DataSet ds = DAO.GetInstance.ObtenerDataSet();
            DataTable? dtProv = ds.Tables["PROVEEDOR"];

            if (dtProv == null) throw new Exception("La tabla PROVEEDOR no está en el DataSet.");

            var mapa = new Dictionary<int, int>();

            // Mapeamos el índice del TabControl visual con el IdProveedor real buscando por CUIT
            var coca = dtProv.AsEnumerable().FirstOrDefault(r => r.Field<string>("CUIT") == "30-50673003-8");
            if (coca != null) mapa.Add(0, Convert.ToInt32(coca["IdProveedor"]));

            var pepsi = dtProv.AsEnumerable().FirstOrDefault(r => r.Field<string>("CUIT") == "30-53758070-1");
            if (pepsi != null) mapa.Add(1, Convert.ToInt32(pepsi["IdProveedor"]));

            var manaos = dtProv.AsEnumerable().FirstOrDefault(r => r.Field<string>("CUIT") == "30-70894042-7");
            if (manaos != null) mapa.Add(2, Convert.ToInt32(manaos["IdProveedor"]));

            var baggio = dtProv.AsEnumerable().FirstOrDefault(r => r.Field<string>("CUIT") == "30-50013003-4");
            if (baggio != null) mapa.Add(3, Convert.ToInt32(baggio["IdProveedor"]));

            return mapa;
        }
    }
}