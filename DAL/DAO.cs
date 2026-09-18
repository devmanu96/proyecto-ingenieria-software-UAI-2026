using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DAL
{
    public class DAO
    {
        private static DAO? Instance;
        private static readonly object _lock = new object();

        private DataSet mainDataSet;

        private SqlDataAdapter daUsers, daBitacora, daPermiso, daPermisoRelacion, daIdioma, daTraduccion, daPerfilUsuario, daHistorialUsuario, daDVV, daProducto, daSolicitudAbastecimiento, daDetalleSolicitud;
        private SqlCommandBuilder cbUsers, cbBitacora, cbPermiso, cbPermisoRelacion, cbIdioma, cbTraduccion, cbPerfilUsuario, cbHistorialUsuario, cbDVV, cbProducto, cbSolicitudAbastecimiento, cbDetalleSolicitud;

        private DAO()
        {
            string connectionString = ObtenerStringConexionEnv();

            daUsers = new SqlDataAdapter("Select * From Usuario", connectionString);
            daBitacora = new SqlDataAdapter("Select * From Bitacora", connectionString);
            daPermiso = new SqlDataAdapter("Select * From Permiso", connectionString);
            daPermisoRelacion = new SqlDataAdapter("Select * From PermisoRelacion", connectionString);
            daIdioma = new SqlDataAdapter("Select * From Idioma", connectionString);
            daTraduccion = new SqlDataAdapter("Select * From Traduccion", connectionString);
            daPerfilUsuario = new SqlDataAdapter("Select * From PerfilUsuario", connectionString);
            daHistorialUsuario = new SqlDataAdapter("Select * From HistorialUsuario", connectionString);
            daDVV = new SqlDataAdapter("Select * From DVV", connectionString);
            daProducto = new SqlDataAdapter("Select * From PRODUCTO", connectionString);
            daSolicitudAbastecimiento = new SqlDataAdapter("Select * From SOLICITUD_ABASTECIMIENTO", connectionString);
            daDetalleSolicitud = new SqlDataAdapter("Select * From DETALLE_SOLICITUD", connectionString);

            daUsers.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daBitacora.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daPermiso.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daPermisoRelacion.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daIdioma.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daTraduccion.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daPerfilUsuario.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daHistorialUsuario.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daDVV.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daProducto.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daSolicitudAbastecimiento.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daDetalleSolicitud.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            cbUsers = new SqlCommandBuilder(daUsers);
            cbBitacora = new SqlCommandBuilder(daBitacora);
            cbPermiso = new SqlCommandBuilder(daPermiso);
            cbPermisoRelacion = new SqlCommandBuilder(daPermisoRelacion);
            cbIdioma = new SqlCommandBuilder(daIdioma);
            cbTraduccion = new SqlCommandBuilder(daTraduccion);
            cbPerfilUsuario = new SqlCommandBuilder(daPerfilUsuario);

            mainDataSet = new DataSet("Users");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open) throw new Exception("Conexión a base de datos fallida");

                CargarTablaConEsquema(daUsers, "Usuario", conn);
                CargarTablaConEsquema(daBitacora, "Bitacora", conn);
                CargarTablaConEsquema(daPermiso, "Permiso", conn);
                CargarTablaConEsquema(daPermisoRelacion, "PermisoRelacion", conn);
                CargarTablaConEsquema(daIdioma, "Idioma", conn);
                CargarTablaConEsquema(daTraduccion, "Traduccion", conn);
                CargarTablaConEsquema(daPerfilUsuario, "PerfilUsuario", conn);
                CargarTablaConEsquema(daHistorialUsuario, "HistorialUsuario", conn);
                CargarTablaConEsquema(daDVV, "DVV", conn);
                CargarTablaConEsquema(daProducto, "Producto", conn);
                CargarTablaConEsquema(daSolicitudAbastecimiento, "SOLICITUD_ABASTECIMIENTO", conn);
                CargarTablaConEsquema(daDetalleSolicitud, "DETALLE_SOLICITUD", conn);
            }

            cbUsers = new SqlCommandBuilder(daUsers);
            cbBitacora = new SqlCommandBuilder(daBitacora);
            cbPermiso = new SqlCommandBuilder(daPermiso);
            cbPermisoRelacion = new SqlCommandBuilder(daPermisoRelacion);
            cbPerfilUsuario = new SqlCommandBuilder(daPerfilUsuario);
            cbHistorialUsuario = new SqlCommandBuilder(daHistorialUsuario);
            cbDVV = new SqlCommandBuilder(daDVV);
            cbProducto = new SqlCommandBuilder(daProducto);
            cbSolicitudAbastecimiento = new SqlCommandBuilder(daSolicitudAbastecimiento);
            cbDetalleSolicitud = new SqlCommandBuilder(daDetalleSolicitud);

            ConfigurarAutoincrementoGeneral();
            ArmarRelaciones();
        }

        private void CargarTablaConEsquema(SqlDataAdapter adapter, string tableName, SqlConnection connection)
        {
            if (adapter.SelectCommand == null)
            {
                adapter.SelectCommand = new SqlCommand();
            }

            adapter.SelectCommand.CommandText = $"Select * From {tableName}";
            adapter.SelectCommand.Connection = connection;

            adapter.FillSchema(mainDataSet, SchemaType.Source, tableName);
            adapter.Fill(mainDataSet, tableName);

            if (mainDataSet.Tables[tableName]!.PrimaryKey.Length == 0)
            {
                if (tableName == "PermisoRelacion")
                {
                    mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] {
                        mainDataSet.Tables[tableName]!.Columns["ID_Padre"]!,
                        mainDataSet.Tables[tableName]!.Columns["ID_Hijo"]!
                    };
                }
                else if (tableName == "PerfilUsuario")
                {
                    mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] {
                        mainDataSet.Tables[tableName]!.Columns["ID_Usuario"]!,
                        mainDataSet.Tables[tableName]!.Columns["ID_Perfil"]!
                    };
                }
                else if (tableName == "DVV")
                {
                    mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["NombreTabla"]! };
                }
                else if (tableName == "Producto")
                {
                    mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdProducto"]! };
                }
                else if (tableName == "SOLICITUD_ABASTECIMIENTO")
                {
                    mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdSolicitud"]! };
                }
                else if (tableName == "DETALLE_SOLICITUD")
                {
                    mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdDetalle"]! };
                }
                else
                {
                    mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["ID"]! };
                }
            }
        }

        private void ArmarRelaciones()
        {
            DataTable? dtPermiso = mainDataSet.Tables["Permiso"];
            DataTable? dtPermisoRelacion = mainDataSet.Tables["PermisoRelacion"];
            DataTable? dtPerfilUsuario = mainDataSet.Tables["PerfilUsuario"];
            DataTable? dtUsuario = mainDataSet.Tables["Usuario"];

            if (dtPermiso == null || dtPermisoRelacion == null || dtPerfilUsuario == null || dtUsuario == null) throw new Exception("Error en el armado de relaciones DAO");

            DataRelation drPermisoPadre = new DataRelation(
                "FK_PermisoRelacion_Padre",
                dtPermiso.Columns["ID"]!,
                dtPermisoRelacion.Columns["ID_Padre"]!
            );

            DataRelation drPermisoHijo = new DataRelation(
                "FK_PermisoRelacion_Hijo",
                dtPermiso.Columns["ID"]!,
                dtPermisoRelacion.Columns["ID_Hijo"]!
            );

            DataRelation drPerfilUsuarioUsuario = new DataRelation(
                "FK_PerfilUsuarioUsuario",
                dtUsuario.Columns["ID"]!,
                dtPerfilUsuario.Columns["ID_Usuario"]!
            );

            DataRelation drPerfilUsuarioPerfil = new DataRelation(
                "FK_PerfilUsuarioPerfil",
                dtPermiso.Columns["ID"]!,
                dtPerfilUsuario.Columns["ID_Perfil"]!
            );

            mainDataSet.Relations.Add(drPermisoPadre);
            mainDataSet.Relations.Add(drPermisoHijo);

            DataTable? dtIdioma = mainDataSet.Tables["Idioma"];
            DataTable? dtTraduccion = mainDataSet.Tables["Traduccion"];

            if (dtIdioma == null || dtTraduccion == null) throw new Exception("Error en el armado de relaciones DAO para Idiomas");

            DataRelation drIdiomaTraduccion = new DataRelation(
                "FK_Traduccion_Idioma",
                dtIdioma.Columns["Codigo"]!,
                dtTraduccion.Columns["CodigoIdioma"]!
            );

            mainDataSet.Relations.Add(drIdiomaTraduccion);
            mainDataSet.Relations.Add(drPerfilUsuarioUsuario);
            mainDataSet.Relations.Add(drPerfilUsuarioPerfil);

            // Relación Cabecera-Detalle de Solicitudes
            DataTable? dtSolicitud = mainDataSet.Tables["SOLICITUD_ABASTECIMIENTO"];
            DataTable? dtDetalleSol = mainDataSet.Tables["DETALLE_SOLICITUD"];

            if (dtSolicitud != null && dtDetalleSol != null)
            {
                DataRelation drSolicitudDetalle = new DataRelation(
                    "FK_DetalleSolicitud_Cabecera",
                    dtSolicitud.Columns["IdSolicitud"]!,
                    dtDetalleSol.Columns["IdSolicitud"]!
                );
                mainDataSet.Relations.Add(drSolicitudDetalle);
            }
        }

        private void ConfigurarAutoincrementoGeneral()
        {
            if (mainDataSet.Tables.Contains("Bitacora") && mainDataSet.Tables["Bitacora"]!.Columns.Contains("ID"))
            {
                DataTable dtBitacora = mainDataSet.Tables["Bitacora"]!;
                DataColumn columnaIdRegistroBitacora = dtBitacora.Columns["ID"]!;
                int maxId = dtBitacora.Rows.Count > 0 ? dtBitacora.AsEnumerable().Max(r => r["ID"] == DBNull.Value ? 0 : Convert.ToInt32(r["ID"])) : 0;
                columnaIdRegistroBitacora.AutoIncrement = true;
                columnaIdRegistroBitacora.AutoIncrementSeed = maxId + 1;
                columnaIdRegistroBitacora.AutoIncrementStep = 1;
            }

            if (mainDataSet.Tables.Contains("Permiso") && mainDataSet.Tables["Permiso"]!.Columns.Contains("ID"))
            {
                DataTable dtPermiso = mainDataSet.Tables["Permiso"]!;
                DataColumn columnaIdRegistroPermiso = dtPermiso.Columns["ID"]!;
                int maxIdPermiso = dtPermiso.Rows.Count > 0 ? dtPermiso.AsEnumerable().Max(r => r["ID"] == DBNull.Value ? 0 : Convert.ToInt32(r["ID"])) : 0;
                columnaIdRegistroPermiso.AutoIncrement = true;
                columnaIdRegistroPermiso.AutoIncrementSeed = maxIdPermiso + 1;
                columnaIdRegistroPermiso.AutoIncrementStep = 1;
            }

            if (mainDataSet.Tables.Contains("Traduccion") && mainDataSet.Tables["Traduccion"]!.Columns.Contains("IdTraduccion"))
            {
                DataTable dtTraduccion = mainDataSet.Tables["Traduccion"]!;
                DataColumn colIdTrad = dtTraduccion.Columns["IdTraduccion"]!;
                int maxIdTrad = dtTraduccion.Rows.Count > 0 ? dtTraduccion.AsEnumerable().Max(r => r["IdTraduccion"] == DBNull.Value ? 0 : Convert.ToInt32(r["IdTraduccion"])) : 0;
                colIdTrad.AutoIncrement = true;
                colIdTrad.AutoIncrementSeed = maxIdTrad + 1;
                colIdTrad.AutoIncrementStep = 1;
            }

            if (mainDataSet.Tables.Contains("HistorialUsuario") && mainDataSet.Tables["HistorialUsuario"]!.Columns.Contains("ID"))
            {
                DataTable dtHistorial = mainDataSet.Tables["HistorialUsuario"]!;
                DataColumn colIdHistorial = dtHistorial.Columns["ID"]!;
                int maxIdHistorial = dtHistorial.Rows.Count > 0 ? dtHistorial.AsEnumerable().Max(r => r["ID"] == DBNull.Value ? 0 : Convert.ToInt32(r["ID"])) : 0;
                colIdHistorial.AutoIncrement = true;
                colIdHistorial.AutoIncrementSeed = maxIdHistorial + 1;
                colIdHistorial.AutoIncrementStep = 1;
            }

            if (mainDataSet.Tables.Contains("SOLICITUD_ABASTECIMIENTO") && mainDataSet.Tables["SOLICITUD_ABASTECIMIENTO"]!.Columns.Contains("IdSolicitud"))
            {
                DataTable dtSol = mainDataSet.Tables["SOLICITUD_ABASTECIMIENTO"]!;
                DataColumn colIdSol = dtSol.Columns["IdSolicitud"]!;
                int maxIdSol = dtSol.Rows.Count > 0 ? dtSol.AsEnumerable().Max(r => r["IdSolicitud"] == DBNull.Value ? 0 : Convert.ToInt32(r["IdSolicitud"])) : 0;
                colIdSol.AutoIncrement = true;
                colIdSol.AutoIncrementSeed = maxIdSol + 1;
                colIdSol.AutoIncrementStep = 1;
            }
        }

        public static DAO GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (_lock)
                    {
                        if (Instance == null)
                        {
                            Instance = new DAO();
                        }
                    }
                }
                return Instance;
            }
        }

        private string ObtenerStringConexionEnv()
        {
            string? connectionString = null;
            string directorioActualDAO = AppContext.BaseDirectory;
            DirectoryInfo? directorioRaiz = new DirectoryInfo(directorioActualDAO);

            while (directorioRaiz != null && directorioRaiz.GetFiles("*.sln").Length == 0)
            {
                directorioRaiz = directorioRaiz.Parent;
            }

            if (directorioRaiz == null) throw new Exception("Raiz del proyecto no encontrada para cargar archivo de configuración");

            string rutaArchivoEnv = Path.Combine(directorioRaiz.FullName, ".env");
            if (File.Exists(rutaArchivoEnv))
            {
                try { DotNetEnv.Env.Load(rutaArchivoEnv); } catch { }
            }

            connectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING")
                ?? Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                ?? Environment.GetEnvironmentVariable("ConnectionStrings:Default")
                ?? Environment.GetEnvironmentVariable("DefaultConnection");

            if (connectionString == null)
            {
                string rutaAppSettings = Path.Combine(directorioRaiz.FullName, "appsettings.json");
                if (File.Exists(rutaAppSettings))
                {
                    try
                    {
                        string json = File.ReadAllText(rutaAppSettings);
                        using JsonDocument doc = JsonDocument.Parse(json);
                        JsonElement root = doc.RootElement;

                        if (root.TryGetProperty("ConnectionStrings", out JsonElement cs))
                        {
                            if (cs.TryGetProperty("Default", out JsonElement def) && def.ValueKind == JsonValueKind.String)
                                connectionString = def.GetString();
                            else if (cs.TryGetProperty("SQL_SERVER_CONNECTION_STRING", out JsonElement ss) && ss.ValueKind == JsonValueKind.String)
                                connectionString = ss.GetString();
                        }
                        else if (root.TryGetProperty("SQL_SERVER_CONNECTION_STRING", out JsonElement top) && top.ValueKind == JsonValueKind.String)
                        {
                            connectionString = top.GetString();
                        }
                    }
                    catch { }
                }
            }

            if (connectionString == null) throw new Exception("Configuración para conexión no encontrada.");

            return connectionString;
        }

        public DataSet ObtenerDataSet()
        {
            return mainDataSet;
        }

        public void SubirCambiosBD()
        {
            string connectionString = ObtenerStringConexionEnv();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                daUsers.SelectCommand.Connection = conn;
                daBitacora.SelectCommand.Connection = conn;
                daPermiso.SelectCommand.Connection = conn;
                daPermisoRelacion.SelectCommand.Connection = conn;
                daIdioma.SelectCommand.Connection = conn;
                daTraduccion.SelectCommand.Connection = conn;
                daPerfilUsuario.SelectCommand.Connection = conn;
                daHistorialUsuario.SelectCommand.Connection = conn;
                daDVV.SelectCommand.Connection = conn;
                daProducto.SelectCommand.Connection = conn;
                daSolicitudAbastecimiento.SelectCommand.Connection = conn;
                daDetalleSolicitud.SelectCommand.Connection = conn;

                daUsers.InsertCommand = cbUsers.GetInsertCommand();
                daUsers.UpdateCommand = cbUsers.GetUpdateCommand();
                daUsers.DeleteCommand = cbUsers.GetDeleteCommand();
                daUsers.InsertCommand.Connection = conn;
                daUsers.UpdateCommand.Connection = conn;
                daUsers.DeleteCommand.Connection = conn;

                daBitacora.InsertCommand = cbBitacora.GetInsertCommand();
                daBitacora.UpdateCommand = cbBitacora.GetUpdateCommand();
                daBitacora.DeleteCommand = cbBitacora.GetDeleteCommand();
                daBitacora.InsertCommand.Connection = conn;
                daBitacora.UpdateCommand.Connection = conn;
                daBitacora.DeleteCommand.Connection = conn;

                daPermiso.InsertCommand = cbPermiso.GetInsertCommand();
                daPermiso.UpdateCommand = cbPermiso.GetUpdateCommand();
                daPermiso.DeleteCommand = cbPermiso.GetDeleteCommand();
                daPermiso.InsertCommand.Connection = conn;
                daPermiso.UpdateCommand.Connection = conn;
                daPermiso.DeleteCommand.Connection = conn;

                daPermisoRelacion.InsertCommand = cbPermisoRelacion.GetInsertCommand();
                daPermisoRelacion.UpdateCommand = cbPermisoRelacion.GetUpdateCommand();
                daPermisoRelacion.DeleteCommand = cbPermisoRelacion.GetDeleteCommand();
                daPermisoRelacion.InsertCommand.Connection = conn;
                daPermisoRelacion.UpdateCommand.Connection = conn;
                daPermisoRelacion.DeleteCommand.Connection = conn;

                daIdioma.InsertCommand = cbIdioma.GetInsertCommand();
                daIdioma.UpdateCommand = cbIdioma.GetUpdateCommand();
                daIdioma.DeleteCommand = cbIdioma.GetDeleteCommand();
                daIdioma.InsertCommand.Connection = conn;
                daIdioma.UpdateCommand.Connection = conn;
                daIdioma.DeleteCommand.Connection = conn;

                daTraduccion.InsertCommand = cbTraduccion.GetInsertCommand();
                daTraduccion.UpdateCommand = cbTraduccion.GetUpdateCommand();
                daTraduccion.DeleteCommand = cbTraduccion.GetDeleteCommand();
                daTraduccion.InsertCommand.Connection = conn;
                daTraduccion.UpdateCommand.Connection = conn;
                daTraduccion.DeleteCommand.Connection = conn;

                daPerfilUsuario.InsertCommand = cbPerfilUsuario.GetInsertCommand();
                daPerfilUsuario.UpdateCommand = cbPerfilUsuario.GetUpdateCommand();
                daPerfilUsuario.DeleteCommand = cbPerfilUsuario.GetDeleteCommand();
                daPerfilUsuario.InsertCommand.Connection = conn;
                daPerfilUsuario.UpdateCommand.Connection = conn;
                daPerfilUsuario.DeleteCommand.Connection = conn;

                daHistorialUsuario.InsertCommand = cbHistorialUsuario.GetInsertCommand();
                daHistorialUsuario.UpdateCommand = cbHistorialUsuario.GetUpdateCommand();
                daHistorialUsuario.DeleteCommand = cbHistorialUsuario.GetDeleteCommand();
                daHistorialUsuario.InsertCommand.Connection = conn;
                daHistorialUsuario.UpdateCommand.Connection = conn;
                daHistorialUsuario.DeleteCommand.Connection = conn;

                daDVV.InsertCommand = cbDVV.GetInsertCommand();
                daDVV.UpdateCommand = cbDVV.GetUpdateCommand();
                daDVV.DeleteCommand = cbDVV.GetDeleteCommand();
                daDVV.InsertCommand.Connection = conn;
                daDVV.UpdateCommand.Connection = conn;
                daDVV.DeleteCommand.Connection = conn;

                daProducto.InsertCommand = cbProducto.GetInsertCommand();
                daProducto.UpdateCommand = cbProducto.GetUpdateCommand();
                daProducto.DeleteCommand = cbProducto.GetDeleteCommand();
                daProducto.InsertCommand.Connection = conn;
                daProducto.UpdateCommand.Connection = conn;
                daProducto.DeleteCommand.Connection = conn;

                daSolicitudAbastecimiento.InsertCommand = cbSolicitudAbastecimiento.GetInsertCommand();
                daSolicitudAbastecimiento.UpdateCommand = cbSolicitudAbastecimiento.GetUpdateCommand();
                daSolicitudAbastecimiento.DeleteCommand = cbSolicitudAbastecimiento.GetDeleteCommand();
                daSolicitudAbastecimiento.InsertCommand.Connection = conn;
                daSolicitudAbastecimiento.UpdateCommand.Connection = conn;
                daSolicitudAbastecimiento.DeleteCommand.Connection = conn;

                daDetalleSolicitud.InsertCommand = cbDetalleSolicitud.GetInsertCommand();
                daDetalleSolicitud.UpdateCommand = cbDetalleSolicitud.GetUpdateCommand();
                daDetalleSolicitud.DeleteCommand = cbDetalleSolicitud.GetDeleteCommand();
                daDetalleSolicitud.InsertCommand.Connection = conn;
                daDetalleSolicitud.UpdateCommand.Connection = conn;
                daDetalleSolicitud.DeleteCommand.Connection = conn;

                daUsers.Update(mainDataSet, "Usuario");
                daBitacora.Update(mainDataSet, "Bitacora");
                daPermiso.Update(mainDataSet, "Permiso");
                daPermisoRelacion.Update(mainDataSet, "PermisoRelacion");
                daIdioma.Update(mainDataSet, "Idioma");
                daTraduccion.Update(mainDataSet, "Traduccion");
                daPerfilUsuario.Update(mainDataSet, "PerfilUsuario");
                daHistorialUsuario.Update(mainDataSet, "HistorialUsuario");
                daDVV.Update(mainDataSet, "DVV");
                daProducto.Update(mainDataSet, "Producto");
                daSolicitudAbastecimiento.Update(mainDataSet, "SOLICITUD_ABASTECIMIENTO");
                daDetalleSolicitud.Update(mainDataSet, "DETALLE_SOLICITUD");

                mainDataSet.AcceptChanges();
            }
        }
    }
}