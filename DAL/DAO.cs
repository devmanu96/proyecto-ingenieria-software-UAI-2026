using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;

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

        // Adaptadores Base y Compras
        private SqlDataAdapter daUsers, daBitacora, daPermiso, daPermisoRelacion, daIdioma, daTraduccion, daPerfilUsuario, daHistorialUsuario, daDVV, daProducto, daSolicitudAbastecimiento, daDetalleSolicitud, daProveedor, daCatalogoProveedor, daOrdenCompra, daDetalleOC;
        private SqlDataAdapter daPresupuesto, daPagoEmitido, daFacturaProveedor, daRecepcion, daLoteBebida;

        // Nuevos Adaptadores Módulo Ventas
        private SqlDataAdapter daCliente, daVenta, daDetalleVenta, daCobro;

        // Constructores de comandos Base y Compras
        private SqlCommandBuilder cbUsers, cbBitacora, cbPermiso, cbPermisoRelacion, cbIdioma, cbTraduccion, cbPerfilUsuario, cbHistorialUsuario, cbDVV, cbProducto, cbSolicitudAbastecimiento, cbDetalleSolicitud, cbProveedor, cbCatalogoProveedor, cbOrdenCompra, cbDetalleOC;
        private SqlCommandBuilder cbPresupuesto, cbPagoEmitido, cbFacturaProveedor, cbRecepcion, cbLoteBebida;

        // Nuevos CommandBuilders Módulo Ventas
        private SqlCommandBuilder cbCliente, cbVenta, cbDetalleVenta, cbCobro;

        private DAO()
        {
            string connectionString = ObtenerStringConexionEnv();

            // 1. Inicializar Adaptadores
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

            daProveedor = new SqlDataAdapter("Select * From PROVEEDOR", connectionString);
            daCatalogoProveedor = new SqlDataAdapter("Select * From CATALOGO_PROVEEDOR", connectionString);
            daOrdenCompra = new SqlDataAdapter("Select * From ORDEN_COMPRA", connectionString);
            daDetalleOC = new SqlDataAdapter("Select * From DETALLE_OC", connectionString);
            daPresupuesto = new SqlDataAdapter("Select * From PRESUPUESTO", connectionString);
            daPagoEmitido = new SqlDataAdapter("Select * From PAGO_EMITIDO", connectionString);
            daFacturaProveedor = new SqlDataAdapter("Select * From FACTURA_PROVEEDOR", connectionString);
            daRecepcion = new SqlDataAdapter("Select * From RECEPCION", connectionString);
            daLoteBebida = new SqlDataAdapter("Select * From LOTE_BEBIDA", connectionString);

            // Inicializar Adaptadores Ventas
            daCliente = new SqlDataAdapter("Select * From CLIENTE", connectionString);
            daVenta = new SqlDataAdapter("Select * From VENTA", connectionString);
            daDetalleVenta = new SqlDataAdapter("Select * From DETALLE_VENTA", connectionString);
            daCobro = new SqlDataAdapter("Select * From COBRO", connectionString);

            // 2. Configurar MissingSchemaAction
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
            daProveedor.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daCatalogoProveedor.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daOrdenCompra.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daDetalleOC.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daPresupuesto.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daPagoEmitido.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daFacturaProveedor.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daRecepcion.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daLoteBebida.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            // MissingSchemaAction Ventas
            daCliente.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daVenta.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daDetalleVenta.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            daCobro.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            mainDataSet = new DataSet("SistemaGestion");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open) throw new Exception("Conexión a base de datos fallida");

                // 3. Cargar Esquemas y Datos
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
                CargarTablaConEsquema(daProveedor, "PROVEEDOR", conn);
                CargarTablaConEsquema(daCatalogoProveedor, "CATALOGO_PROVEEDOR", conn);
                CargarTablaConEsquema(daOrdenCompra, "ORDEN_COMPRA", conn);
                CargarTablaConEsquema(daDetalleOC, "DETALLE_OC", conn);
                CargarTablaConEsquema(daPresupuesto, "PRESUPUESTO", conn);
                CargarTablaConEsquema(daPagoEmitido, "PAGO_EMITIDO", conn);
                CargarTablaConEsquema(daFacturaProveedor, "FACTURA_PROVEEDOR", conn);
                CargarTablaConEsquema(daRecepcion, "RECEPCION", conn);
                CargarTablaConEsquema(daLoteBebida, "LOTE_BEBIDA", conn);

                // Cargar Esquemas Ventas
                CargarTablaConEsquema(daCliente, "CLIENTE", conn);
                CargarTablaConEsquema(daVenta, "VENTA", conn);
                CargarTablaConEsquema(daDetalleVenta, "DETALLE_VENTA", conn);
                CargarTablaConEsquema(daCobro, "COBRO", conn);
            }

            // 4. Inicializar CommandBuilders
            cbUsers = new SqlCommandBuilder(daUsers);
            cbBitacora = new SqlCommandBuilder(daBitacora);
            cbPermiso = new SqlCommandBuilder(daPermiso);
            cbPermisoRelacion = new SqlCommandBuilder(daPermisoRelacion);
            cbIdioma = new SqlCommandBuilder(daIdioma);
            cbTraduccion = new SqlCommandBuilder(daTraduccion);
            cbPerfilUsuario = new SqlCommandBuilder(daPerfilUsuario);
            cbHistorialUsuario = new SqlCommandBuilder(daHistorialUsuario);
            cbDVV = new SqlCommandBuilder(daDVV);
            cbProducto = new SqlCommandBuilder(daProducto);
            cbSolicitudAbastecimiento = new SqlCommandBuilder(daSolicitudAbastecimiento);
            cbDetalleSolicitud = new SqlCommandBuilder(daDetalleSolicitud);
            cbProveedor = new SqlCommandBuilder(daProveedor);
            cbCatalogoProveedor = new SqlCommandBuilder(daCatalogoProveedor);
            cbOrdenCompra = new SqlCommandBuilder(daOrdenCompra);
            cbDetalleOC = new SqlCommandBuilder(daDetalleOC);
            cbPresupuesto = new SqlCommandBuilder(daPresupuesto);
            cbPagoEmitido = new SqlCommandBuilder(daPagoEmitido);
            cbFacturaProveedor = new SqlCommandBuilder(daFacturaProveedor);
            cbRecepcion = new SqlCommandBuilder(daRecepcion);
            cbLoteBebida = new SqlCommandBuilder(daLoteBebida);

            // CommandBuilders Ventas
            cbCliente = new SqlCommandBuilder(daCliente);
            cbVenta = new SqlCommandBuilder(daVenta);
            cbDetalleVenta = new SqlCommandBuilder(daDetalleVenta);
            cbCobro = new SqlCommandBuilder(daCobro);

            ConfigurarAutoincrementoGeneral();
            ArmarRelaciones();
        }

        private void CargarTablaConEsquema(SqlDataAdapter adapter, string tableName, SqlConnection connection)
        {
            if (adapter.SelectCommand == null)
                adapter.SelectCommand = new SqlCommand();

            adapter.SelectCommand.CommandText = $"Select * From {tableName}";
            adapter.SelectCommand.Connection = connection;

            adapter.FillSchema(mainDataSet, SchemaType.Source, tableName);
            adapter.Fill(mainDataSet, tableName);

            if (mainDataSet.Tables[tableName]!.PrimaryKey.Length == 0)
            {
                if (tableName == "PermisoRelacion") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["ID_Padre"]!, mainDataSet.Tables[tableName]!.Columns["ID_Hijo"]! };
                else if (tableName == "PerfilUsuario") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["ID_Usuario"]!, mainDataSet.Tables[tableName]!.Columns["ID_Perfil"]! };
                else if (tableName == "DVV") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["NombreTabla"]! };
                else if (tableName == "Producto") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdProducto"]! };
                else if (tableName == "SOLICITUD_ABASTECIMIENTO") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdSolicitud"]! };
                else if (tableName == "DETALLE_SOLICITUD") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdDetalle"]! };
                else if (tableName == "PROVEEDOR") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdProveedor"]! };
                else if (tableName == "CATALOGO_PROVEEDOR") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdProveedor"]!, mainDataSet.Tables[tableName]!.Columns["IdProducto"]! };
                else if (tableName == "ORDEN_COMPRA") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdOrden"]! };
                else if (tableName == "DETALLE_OC") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdDetalleOC"]! };
                else if (tableName == "PRESUPUESTO") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdPresupuesto"]! };
                else if (tableName == "PAGO_EMITIDO") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdPago"]! };
                else if (tableName == "FACTURA_PROVEEDOR") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdFactura"]! };
                else if (tableName == "RECEPCION") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdRecepcion"]! };
                else if (tableName == "LOTE_BEBIDA") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdLote"]! };
                else if (tableName == "CLIENTE") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdCliente"]! };
                else if (tableName == "VENTA") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdVenta"]! };
                else if (tableName == "DETALLE_VENTA") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdDetalleVenta"]! };
                else if (tableName == "COBRO") mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["IdCobro"]! };
                else mainDataSet.Tables[tableName]!.PrimaryKey = new DataColumn[] { mainDataSet.Tables[tableName]!.Columns["ID"]! };
            }
        }

        private void ArmarRelaciones()
        {
            // Base Relaciones
            DataTable? dtPermiso = mainDataSet.Tables["Permiso"];
            DataTable? dtPermisoRelacion = mainDataSet.Tables["PermisoRelacion"];
            DataTable? dtPerfilUsuario = mainDataSet.Tables["PerfilUsuario"];
            DataTable? dtUsuario = mainDataSet.Tables["Usuario"];

            if (dtPermiso != null && dtPermisoRelacion != null)
            {
                mainDataSet.Relations.Add(new DataRelation("FK_PermisoRelacion_Padre", dtPermiso.Columns["ID"]!, dtPermisoRelacion.Columns["ID_Padre"]!));
                mainDataSet.Relations.Add(new DataRelation("FK_PermisoRelacion_Hijo", dtPermiso.Columns["ID"]!, dtPermisoRelacion.Columns["ID_Hijo"]!));
            }

            if (dtUsuario != null && dtPerfilUsuario != null && dtPermiso != null)
            {
                mainDataSet.Relations.Add(new DataRelation("FK_PerfilUsuarioUsuario", dtUsuario.Columns["ID"]!, dtPerfilUsuario.Columns["ID_Usuario"]!));
                mainDataSet.Relations.Add(new DataRelation("FK_PerfilUsuarioPerfil", dtPermiso.Columns["ID"]!, dtPerfilUsuario.Columns["ID_Perfil"]!));
            }

            DataTable? dtIdioma = mainDataSet.Tables["Idioma"];
            DataTable? dtTraduccion = mainDataSet.Tables["Traduccion"];
            if (dtIdioma != null && dtTraduccion != null)
                mainDataSet.Relations.Add(new DataRelation("FK_Traduccion_Idioma", dtIdioma.Columns["Codigo"]!, dtTraduccion.Columns["CodigoIdioma"]!));

            // Relaciones Compras
            DataTable? dtSolicitud = mainDataSet.Tables["SOLICITUD_ABASTECIMIENTO"];
            DataTable? dtDetalleSol = mainDataSet.Tables["DETALLE_SOLICITUD"];
            if (dtSolicitud != null && dtDetalleSol != null)
                mainDataSet.Relations.Add(new DataRelation("FK_DetalleSolicitud_Cabecera", dtSolicitud.Columns["IdSolicitud"]!, dtDetalleSol.Columns["IdSolicitud"]!));

            DataTable? dtOrden = mainDataSet.Tables["ORDEN_COMPRA"];
            DataTable? dtDetalleOrden = mainDataSet.Tables["DETALLE_OC"];
            if (dtOrden != null && dtDetalleOrden != null)
                mainDataSet.Relations.Add(new DataRelation("FK_DetalleOC_Cabecera", dtOrden.Columns["IdOrden"]!, dtDetalleOrden.Columns["IdOrden"]!));

            // Relaciones Ventas
            DataTable? dtCliente = mainDataSet.Tables["CLIENTE"];
            DataTable? dtVenta = mainDataSet.Tables["VENTA"];
            DataTable? dtDetalleVenta = mainDataSet.Tables["DETALLE_VENTA"];
            DataTable? dtCobro = mainDataSet.Tables["COBRO"];

            if (dtCliente != null && dtVenta != null)
                mainDataSet.Relations.Add(new DataRelation("FK_Venta_Cliente", dtCliente.Columns["IdCliente"]!, dtVenta.Columns["IdCliente"]!));

            if (dtUsuario != null && dtVenta != null)
                mainDataSet.Relations.Add(new DataRelation("FK_Venta_Usuario", dtUsuario.Columns["ID"]!, dtVenta.Columns["IdUsuario"]!));

            if (dtVenta != null && dtDetalleVenta != null)
                mainDataSet.Relations.Add(new DataRelation("FK_DetalleVenta_Venta", dtVenta.Columns["IdVenta"]!, dtDetalleVenta.Columns["IdVenta"]!));

            if (dtVenta != null && dtCobro != null)
                mainDataSet.Relations.Add(new DataRelation("FK_Cobro_Venta", dtVenta.Columns["IdVenta"]!, dtCobro.Columns["IdVenta"]!));
        }

        private void ConfigurarAutoincrementoGeneral()
        {
            ConfigurarAutoincrementoTabla("Bitacora", "ID");
            ConfigurarAutoincrementoTabla("Permiso", "ID");
            ConfigurarAutoincrementoTabla("Traduccion", "IdTraduccion");
            ConfigurarAutoincrementoTabla("HistorialUsuario", "ID");
            ConfigurarAutoincrementoTabla("SOLICITUD_ABASTECIMIENTO", "IdSolicitud");
            ConfigurarAutoincrementoTabla("PROVEEDOR", "IdProveedor");
            ConfigurarAutoincrementoTabla("ORDEN_COMPRA", "IdOrden");
            ConfigurarAutoincrementoTabla("DETALLE_OC", "IdDetalleOC");
            ConfigurarAutoincrementoTabla("PRESUPUESTO", "IdPresupuesto");
            ConfigurarAutoincrementoTabla("PAGO_EMITIDO", "IdPago");
            ConfigurarAutoincrementoTabla("FACTURA_PROVEEDOR", "IdFactura");
            ConfigurarAutoincrementoTabla("RECEPCION", "IdRecepcion");
            ConfigurarAutoincrementoTabla("LOTE_BEBIDA", "IdLote");

            // Autoincrementos Ventas
            ConfigurarAutoincrementoTabla("CLIENTE", "IdCliente");
            ConfigurarAutoincrementoTabla("VENTA", "IdVenta");
            ConfigurarAutoincrementoTabla("DETALLE_VENTA", "IdDetalleVenta");
            ConfigurarAutoincrementoTabla("COBRO", "IdCobro");
        }

        private void ConfigurarAutoincrementoTabla(string tableName, string columnName)
        {
            if (mainDataSet.Tables.Contains(tableName) && mainDataSet.Tables[tableName]!.Columns.Contains(columnName))
            {
                DataTable dt = mainDataSet.Tables[tableName]!;
                DataColumn col = dt.Columns[columnName]!;
                int maxId = dt.Rows.Count > 0 ? dt.AsEnumerable().Max(r => r[columnName] == DBNull.Value ? 0 : Convert.ToInt32(r[columnName])) : 0;
                col.AutoIncrement = true;
                col.AutoIncrementSeed = maxId + 1;
                col.AutoIncrementStep = 1;
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
                directorioRaiz = directorioRaiz.Parent;

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
                            connectionString = top.GetString();
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

                void PrepararAdaptador(SqlDataAdapter adapter, SqlCommandBuilder builder)
                {
                    adapter.SelectCommand.Connection = conn;
                    adapter.InsertCommand = builder.GetInsertCommand();
                    adapter.UpdateCommand = builder.GetUpdateCommand();
                    adapter.DeleteCommand = builder.GetDeleteCommand();
                    adapter.InsertCommand.Connection = conn;
                    adapter.UpdateCommand.Connection = conn;
                    adapter.DeleteCommand.Connection = conn;
                }

                PrepararAdaptador(daUsers, cbUsers);
                PrepararAdaptador(daBitacora, cbBitacora);
                PrepararAdaptador(daPermiso, cbPermiso);
                PrepararAdaptador(daPermisoRelacion, cbPermisoRelacion);
                PrepararAdaptador(daIdioma, cbIdioma);
                PrepararAdaptador(daTraduccion, cbTraduccion);
                PrepararAdaptador(daPerfilUsuario, cbPerfilUsuario);
                PrepararAdaptador(daHistorialUsuario, cbHistorialUsuario);
                PrepararAdaptador(daDVV, cbDVV);
                PrepararAdaptador(daProducto, cbProducto);
                PrepararAdaptador(daSolicitudAbastecimiento, cbSolicitudAbastecimiento);
                PrepararAdaptador(daDetalleSolicitud, cbDetalleSolicitud);
                PrepararAdaptador(daProveedor, cbProveedor);
                PrepararAdaptador(daCatalogoProveedor, cbCatalogoProveedor);
                PrepararAdaptador(daOrdenCompra, cbOrdenCompra);
                PrepararAdaptador(daDetalleOC, cbDetalleOC);
                PrepararAdaptador(daPresupuesto, cbPresupuesto);
                PrepararAdaptador(daPagoEmitido, cbPagoEmitido);
                PrepararAdaptador(daFacturaProveedor, cbFacturaProveedor);
                PrepararAdaptador(daRecepcion, cbRecepcion);
                PrepararAdaptador(daLoteBebida, cbLoteBebida);

                // Preparar Adaptadores Ventas
                PrepararAdaptador(daCliente, cbCliente);
                PrepararAdaptador(daVenta, cbVenta);
                PrepararAdaptador(daDetalleVenta, cbDetalleVenta);
                PrepararAdaptador(daCobro, cbCobro);

                // Updates en bloque respetando jerarquías
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

                // Maestros y Compras
                daProveedor.Update(mainDataSet, "PROVEEDOR");
                daCatalogoProveedor.Update(mainDataSet, "CATALOGO_PROVEEDOR");
                daSolicitudAbastecimiento.Update(mainDataSet, "SOLICITUD_ABASTECIMIENTO");
                daDetalleSolicitud.Update(mainDataSet, "DETALLE_SOLICITUD");
                daPresupuesto.Update(mainDataSet, "PRESUPUESTO");
                daOrdenCompra.Update(mainDataSet, "ORDEN_COMPRA");
                daDetalleOC.Update(mainDataSet, "DETALLE_OC");
                daPagoEmitido.Update(mainDataSet, "PAGO_EMITIDO");
                daFacturaProveedor.Update(mainDataSet, "FACTURA_PROVEEDOR");
                daRecepcion.Update(mainDataSet, "RECEPCION");
                daLoteBebida.Update(mainDataSet, "LOTE_BEBIDA");

                // Módulo Ventas
                daCliente.Update(mainDataSet, "CLIENTE");
                daVenta.Update(mainDataSet, "VENTA");
                daDetalleVenta.Update(mainDataSet, "DETALLE_VENTA");
                daCobro.Update(mainDataSet, "COBRO");

                mainDataSet.AcceptChanges();
            }
        }
    }
}