using BE;
using System.Data;

namespace DAL
{
    public class RepositorioPerfiles
    {
        public RepositorioPerfiles()
        {
        }

        public void CrearPerfil(string nombrePerfil)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable? dtPermiso = ds.Tables["Permiso"];

            if (dtPermiso == null) throw new Exception("Error al crear perfil");
            DataRow nDataRow = dtPermiso.NewRow();
            nDataRow["Nombre"] = nombrePerfil;
            nDataRow["EsPerfil"] = true;

            dtPermiso.Rows.Add(nDataRow);
            dao.SubirCambiosBD();
        }

        public void AsociarPermisoConPermiso(uint idPerfil, uint idPermiso)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable? dtPermisoRelacion = ds.Tables["PermisoRelacion"];
            if (dtPermisoRelacion == null) throw new Exception("Error al agregar permiso a perfil");
            DataRow nDataRow = dtPermisoRelacion.NewRow();
            nDataRow["ID_Padre"] = idPerfil;
            nDataRow["ID_Hijo"] = idPermiso;
            dtPermisoRelacion.Rows.Add(nDataRow);
            dao.SubirCambiosBD();
        }

        public List<Permiso> ObtenerPermisos()
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable? dtPermiso = ds.Tables["Permiso"];
            DataTable? dtPermisoRelacion = ds.Tables["PermisoRelacion"];
            DataTable? dtPerfilUsuario = ds.Tables["PerfilUsuario"];

            if (dtPermiso == null || dtPermisoRelacion == null || dtPerfilUsuario == null)
            {
                throw new Exception("No se pudieron obtener las tablas necesarias para los permisos.");
            }

            Dictionary<uint, Perfil> perfiles = new Dictionary<uint, Perfil>();
            Dictionary<uint, PermisoSimple> permisosSimples = new Dictionary<uint, PermisoSimple>();

            foreach (DataRow row in dtPermiso.Rows)
            {
                uint id = Convert.ToUInt32(row["ID"]);
                string nombre = row["Nombre"].ToString() ?? string.Empty;
                bool esPerfil = Convert.ToBoolean(row["EsPerfil"]);
                
                int nivelJerarquico = Convert.ToInt32(row["NivelJerarquico"]);

                Permiso permiso = esPerfil ? new Perfil(id, nombre, nivelJerarquico) : new PermisoSimple(id, nombre, nivelJerarquico);
                if (esPerfil)
                {
                    perfiles.Add(id, (Perfil)permiso);
                }
                else
                {
                    permisosSimples.Add(id, (PermisoSimple)permiso);
                }
            }

            foreach (DataRow row in dtPermisoRelacion.Rows)
            {
                uint idPadre = Convert.ToUInt32(row["ID_Padre"]);
                uint idHijo = Convert.ToUInt32(row["ID_Hijo"]);
                if (perfiles.ContainsKey(idPadre))
                {
                    Perfil perfilPadre = perfiles[idPadre];
                    if (perfiles.ContainsKey(idHijo))
                    {
                        perfilPadre.AgregarPermiso(perfiles[idHijo]);
                    }
                    else if (permisosSimples.ContainsKey(idHijo))
                    {
                        perfilPadre.AgregarPermiso(permisosSimples[idHijo]);
                    }
                }
            }

            return perfiles.Values.Cast<Permiso>().Concat(permisosSimples.Values.Cast<Permiso>()).ToList();
        }

        public void AsociarPerfilAUsuario(Usuario usuario, uint idPerfil)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable? dtPerfilUsuario = ds.Tables["PerfilUsuario"];
            if (dtPerfilUsuario == null) throw new Exception("Error al asociar perfil a usuario");
            DataRow nDataRow = dtPerfilUsuario.NewRow();
            nDataRow["ID_Perfil"] = idPerfil;
            nDataRow["ID_Usuario"] = usuario.Id;
            dtPerfilUsuario.Rows.Add(nDataRow);
            dao.SubirCambiosBD();
        }

        public void DesasociarPerfilDeUsuario(Guid idUsuario, uint idPerfil)
        {
            DAO dao = DAO.GetInstance;
            DataSet ds = dao.ObtenerDataSet();
            DataTable? dtPerfilUsuario = ds.Tables["PerfilUsuario"];
            if (dtPerfilUsuario == null) throw new Exception("Error al desasociar perfil de usuario");

            DataRow[] filasEncontradas = dtPerfilUsuario.Select($"ID_Usuario = '{idUsuario}' AND ID_Perfil = {idPerfil}");
            if (filasEncontradas.Length == 0)
            {
                throw new Exception("No se encontró la asociación entre usuario y perfil.");
            }

            filasEncontradas[0].Delete();
            dao.SubirCambiosBD();
        }
    }
}
