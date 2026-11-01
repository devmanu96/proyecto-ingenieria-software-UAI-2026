using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class GestorProveedor
    {
        private RepositorioProveedorJSON repoJson;

        public GestorProveedor()
        {
            repoJson = new RepositorioProveedorJSON();
        }

        public List<Proveedor> ObtenerProveedoresActivos()
        {
            return repoJson.LeerProveedores().Where(p => p.Activo).ToList();
        }

        public void RegistrarProveedor(Proveedor prov)
        {
            if (string.IsNullOrWhiteSpace(prov.CUIT) || string.IsNullOrWhiteSpace(prov.RazonSocial))
                throw new Exception("El CUIT y la Razón Social son obligatorios.");

            var lista = repoJson.LeerProveedores();

            // Verificamos que no exista el CUIT (Regla de negocio)
            if (lista.Any(p => p.CUIT == prov.CUIT && p.Activo))
                throw new Exception("Ya existe un proveedor activo con este CUIT en el archivo JSON.");

            prov.Id = Guid.NewGuid();
            lista.Add(prov);

            repoJson.GuardarProveedores(lista);
        }

        public void ModificarProveedor(Proveedor provModificado)
        {
            var lista = repoJson.LeerProveedores();
            var index = lista.FindIndex(p => p.Id == provModificado.Id);

            if (index != -1)
            {
                lista[index] = provModificado;
                repoJson.GuardarProveedores(lista);
            }
        }

        public void DarDeBaja(Guid idProveedor)
        {
            var lista = repoJson.LeerProveedores();
            var prov = lista.FirstOrDefault(p => p.Id == idProveedor);

            if (prov != null)
            {
                prov.Activo = false;
                repoJson.GuardarProveedores(lista);
            }
        }

        // =========================================================
        // MÉTODO DE MIGRACIÓN: SQL -> JSON (Lo que pediste)
        // =========================================================
        public void MigrarProveedoresDesdeSQL()
        {
            DataTable? dtProveedoresSQL = DAO.GetInstance.ObtenerDataSet().Tables["PROVEEDOR"];

            if (dtProveedoresSQL == null || dtProveedoresSQL.Rows.Count == 0)
                throw new Exception("No hay proveedores en la base de datos SQL para migrar.");

            List<Proveedor> proveedoresNuevos = new List<Proveedor>();

            foreach (DataRow row in dtProveedoresSQL.Rows)
            {
                proveedoresNuevos.Add(new Proveedor
                {
                    Id = Guid.NewGuid(), // Se genera un ID único para gestionar el JSON
                    CUIT = row["CUIT"].ToString() ?? "00-00000000-0",
                    RazonSocial = row["RazonSocial"].ToString() ?? "Sin Nombre",
                    // Manejamos el caso de que la CondicionComercial sea NULL en la base de datos
                    CondicionComercial = row["CondicionComercial"] != DBNull.Value ? row["CondicionComercial"].ToString() ?? "" : "N/A",
                    Activo = Convert.ToBoolean(row["Activo"])
                });
            }

            // Sobrescribimos el archivo JSON con los datos extraídos
            repoJson.GuardarProveedores(proveedoresNuevos);
        }
    }
}