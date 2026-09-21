
create database DistriuidoraMegaDrink;
GO
use DistriuidoraMegaDrink;
GO

CREATE TABLE Usuario (
    ID UNIQUEIDENTIFIER PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    NumTelefono NVARCHAR(20) NOT NULL,
    EstaBloqueado BIT NOT NULL,
    Idioma VARCHAR(5) NOT NULL DEFAULT 'ES',
    IntentosFallidos INT NOT NULL DEFAULT 0,
	DVH NVARCHAR(256) NULL
);

CREATE TABLE DVV (
	NombreTabla NVARCHAR(50) PRIMARY KEY,
	ValorHash NVARCHAR(100) NOT NULL
);

CREATE TABLE Bitacora (
	ID INT PRIMARY KEY IDENTITY(0,1),
	Username NVARCHAR(50) NOT NULL,
	Fecha DATETIME NOT NULL,
	Accion NVARCHAR(50) NOT NULL
);

CREATE TABLE Permiso (
    ID INT PRIMARY KEY IDENTITY(0,1),
    Nombre VARCHAR(30) NOT NULL UNIQUE,
    EsPerfil BIT NOT NULL
);

CREATE TABLE PermisoRelacion (
    ID_Padre INT NOT NULL,
    ID_Hijo INT NOT NULL,
    PRIMARY KEY (ID_Padre, ID_Hijo),
    CONSTRAINT FK_PermisoRelacion_Padre FOREIGN KEY (ID_Padre) REFERENCES Permiso(ID),
    CONSTRAINT FK_PermisoRelacion_Hijo FOREIGN KEY (ID_Hijo) REFERENCES Permiso(ID)
);

CREATE TABLE PerfilUsuario (
	ID_Usuario UNIQUEIDENTIFIER,
	ID_Perfil INT,
	PRIMARY KEY (ID_Usuario, ID_Perfil),
	CONSTRAINT FK_PerfilUsuarioUsuario FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID),
	CONSTRAINT FK_PerfilUsuarioPerfil FOREIGN KEY (ID_Perfil) REFERENCES Permiso(ID)
);

CREATE TABLE Idioma (
    Codigo VARCHAR(5) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    CONSTRAINT PK_Idioma PRIMARY KEY (Codigo)
);

CREATE TABLE Traduccion (
    IdTraduccion INT IDENTITY(1,1) NOT NULL,
    CodigoIdioma VARCHAR(5) NOT NULL,
    KeyEtiqueta VARCHAR(100) NOT NULL,
    Texto NVARCHAR(MAX) NOT NULL,
    CONSTRAINT PK_Traduccion PRIMARY KEY (IdTraduccion),
    CONSTRAINT FK_Traduccion_Idioma FOREIGN KEY (CodigoIdioma) REFERENCES Idioma(Codigo)
);

CREATE UNIQUE INDEX UIX_Idioma_Etiqueta ON Traduccion(CodigoIdioma, KeyEtiqueta);

CREATE TABLE HistorialUsuario (
    ID INT PRIMARY KEY IDENTITY(0,1),
    ID_Usuario UNIQUEIDENTIFIER NOT NULL,
    Email VARCHAR(100),
    NumTelefono VARCHAR(20),
    Fecha DATETIME NOT NULL,
    CONSTRAINT FK_HistorialUsuarioUsuario FOREIGN KEY (ID_Usuario) REFERENCES Usuario(ID)
);

-- =========================================================================
-- FASE 2: CREACIÓN DE TABLAS DE NEGOCIO (B2B E-PROCUREMENT)
-- =========================================================================

CREATE TABLE PROVEEDOR (
    IdProveedor INT PRIMARY KEY IDENTITY(1,1),
    CUIT VARCHAR(20) NOT NULL UNIQUE,
    RazonSocial VARCHAR(100) NOT NULL,
    CondicionComercial VARCHAR(50),
    Activo BIT NOT NULL DEFAULT 1
);

CREATE TABLE PRODUCTO (
    IdProducto VARCHAR(50) PRIMARY KEY,
    CodigoSKU VARCHAR(50) NOT NULL UNIQUE,
    NombreBebida VARCHAR(100) NOT NULL,
    PrecioUnitarioLocal DECIMAL(18,2) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    StockActual INT NOT NULL DEFAULT 0,
    PuntoPedido INT NOT NULL DEFAULT 0
);

CREATE TABLE CATALOGO_PROVEEDOR (
    IdProveedor INT NOT NULL,
    IdProducto VARCHAR(50) NOT NULL,
    NombreArticuloProveedor VARCHAR(100) NOT NULL, 
    PrecioPallet DECIMAL(12,2) NOT NULL,  
    UnidadesPorPallet INT NOT NULL,       
    PRIMARY KEY (IdProveedor, IdProducto),
    CONSTRAINT FK_Catalogo_Proveedor FOREIGN KEY (IdProveedor) REFERENCES PROVEEDOR(IdProveedor),
    CONSTRAINT FK_Catalogo_Producto FOREIGN KEY (IdProducto) REFERENCES PRODUCTO(IdProducto)
);

CREATE TABLE SOLICITUD_ABASTECIMIENTO (
    IdSolicitud INT PRIMARY KEY IDENTITY(1,1),
    FechaGeneracion DATETIME NOT NULL,
    Estado VARCHAR(50) NOT NULL
);

CREATE TABLE PRESUPUESTO (
    IdPresupuesto INT PRIMARY KEY IDENTITY(1,1),
    IdSolicitud INT NOT NULL,
    IdProveedor INT NOT NULL,
    FechaCalculo DATETIME NOT NULL,
    MontoTotal DECIMAL(18,2) NOT NULL,
    Estado VARCHAR(50) NOT NULL,
    CONSTRAINT FK_Presupuesto_Solicitud FOREIGN KEY (IdSolicitud) REFERENCES SOLICITUD_ABASTECIMIENTO(IdSolicitud),
    CONSTRAINT FK_Presupuesto_Proveedor FOREIGN KEY (IdProveedor) REFERENCES PROVEEDOR(IdProveedor)
);

CREATE TABLE ORDEN_COMPRA (
    IdOrden INT PRIMARY KEY IDENTITY(1,1),
    IdPresupuesto INT NOT NULL,
    FechaEmision DATETIME NOT NULL,
    Estado VARCHAR(50) NOT NULL,
    CONSTRAINT FK_OrdenCompra_Presupuesto FOREIGN KEY (IdPresupuesto) REFERENCES PRESUPUESTO(IdPresupuesto)
);

CREATE TABLE DETALLE_OC (
    IdDetalleOC INT PRIMARY KEY IDENTITY(1,1),
    IdOrden INT NOT NULL,
    IdProducto VARCHAR(50) NOT NULL,
    CantidadSolicitada INT NOT NULL,
    PrecioAcordado DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_DetalleOC_Orden FOREIGN KEY (IdOrden) REFERENCES ORDEN_COMPRA(IdOrden),
    CONSTRAINT FK_DetalleOC_Producto FOREIGN KEY (IdProducto) REFERENCES PRODUCTO(IdProducto)
);

CREATE TABLE PAGO_EMITIDO (
    IdPago INT PRIMARY KEY IDENTITY(1,1),
    IdOrden INT NOT NULL,
    FechaPago DATETIME NOT NULL,
    MontoTransferido DECIMAL(18,2) NOT NULL,
    NumeroComprobante VARCHAR(50) NOT NULL,
    CONSTRAINT FK_Pago_Orden FOREIGN KEY (IdOrden) REFERENCES ORDEN_COMPRA(IdOrden)
);

CREATE TABLE FACTURA_PROVEEDOR (
    IdFactura INT PRIMARY KEY IDENTITY(1,1),
    IdOrden INT NOT NULL,
    NumeroFactura VARCHAR(50) NOT NULL,
    FechaEmision DATETIME NOT NULL,
    CONSTRAINT FK_Factura_Orden FOREIGN KEY (IdOrden) REFERENCES ORDEN_COMPRA(IdOrden)
);

CREATE TABLE RECEPCION (
    IdRecepcion INT PRIMARY KEY IDENTITY(1,1),
    IdOrden INT NOT NULL,
    NumeroRemito VARCHAR(50) NOT NULL,
    FechaIngreso DATETIME NOT NULL,
    CONSTRAINT FK_Recepcion_Orden FOREIGN KEY (IdOrden) REFERENCES ORDEN_COMPRA(IdOrden)
);

CREATE TABLE LOTE_BEBIDA (
    IdLote INT PRIMARY KEY IDENTITY(1,1),
    IdRecepcion INT NOT NULL,
    IdProducto VARCHAR(50) NOT NULL,
    NumeroLote VARCHAR(50) NOT NULL,
    FechaVencimiento DATETIME NOT NULL,
    CantidadRecibida INT NOT NULL,
    StockActual INT NOT NULL,
    CONSTRAINT FK_Lote_Recepcion FOREIGN KEY (IdRecepcion) REFERENCES RECEPCION(IdRecepcion),
    CONSTRAINT FK_Lote_Producto FOREIGN KEY (IdProducto) REFERENCES PRODUCTO(IdProducto)
);

CREATE TABLE DETALLE_SOLICITUD(
    IdDetalle UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    IdSolicitud INT NOT NULL,
    IdProducto VARCHAR(50) NOT NULL, 
    CantidadSolicitada INT NOT NULL,
    CONSTRAINT FK_DetalleSolicitud_Cabecera FOREIGN KEY(IdSolicitud) REFERENCES SOLICITUD_ABASTECIMIENTO(IdSolicitud),
    CONSTRAINT FK_DetalleSolicitud_Producto FOREIGN KEY(IdProducto) REFERENCES PRODUCTO(IdProducto) 
);

-- =========================================================================
-- FASE 3: INSERCIONES DE CONFIGURACIÓN Y SEGURIDAD
-- =========================================================================

INSERT INTO Usuario VALUES (
	'd1eda407-3582-4e0c-85cc-ae51eb67b826',
	'admin',
	'8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918',
	'admin@gmail.com',
	'+54 1120202020',
	0,
	DEFAULT,
	DEFAULT,
	'8D35CFBE2038902867920E5E76AFC58508CBDB31EB92820A1F1F444FF994A615'
);

INSERT INTO DVV VALUES (
	'Usuario',
	'86E0E82C91C298A1D8FB1A735EDFBADBF12E91562C6BD3482DEC23F261314D85'
);

-- Permisos Base
INSERT INTO Permiso (Nombre, EsPerfil) VALUES
('PERM-GESTIONAR-USR', 0),
('PERM-GESTIONAR-IDM', 0),
('PERM-DESBLOQUEAR-USR', 0),
('PERM-GESTIONAR-PERFIL', 0),
('PERM-CONSULTA-BIT', 0),
('PERM-GESTIONAR-HISTORIAL', 0),
('PERM-AGREGAR-IDM', 0),
('PERF-ADMIN', 1);

-- Permisos Nuevos (Proceso B2B)
INSERT INTO Permiso (Nombre, EsPerfil) VALUES
('PERM-ABM-PROD', 0),
('PERM-GEN-REPORTE', 0),
('PERM-SELECCIONAR-PROD', 0),
('PERM-EMITIR-OC', 0),
('PERM-REG-FACTURA', 0),
('PERM-EVALUAR-COTIZ', 0),
('PERM-EFECTUAR-PAGO', 0),
('PERM-ABM-PROV', 0),
('PERM-AUDITAR-COMPRA', 0),
('PERM-GEN-SOLICITUD', 0);

-- Roles Nuevos (Proceso B2B)
INSERT INTO Permiso (Nombre, EsPerfil) VALUES
('PERF-ALMACEN', 1),
('PERF-COMPRAS', 1),
('PERF-CONTABLE', 1);

-- Asignación Perfil ADMIN (Base + Nuevos Módulos)
INSERT INTO PermisoRelacion (ID_Padre, ID_Hijo) VALUES
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-GESTIONAR-USR' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-GESTIONAR-IDM' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-GESTIONAR-PERFIL' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-GESTIONAR-HISTORIAL' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-CONSULTA-BIT' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-AGREGAR-IDM' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-ABM-PROV' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-AUDITAR-COMPRA' AND EsPerfil = 0));

-- Asignación Perfil ALMACÉN
INSERT INTO PermisoRelacion (ID_Padre, ID_Hijo) VALUES
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ALMACEN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-ABM-PROD' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ALMACEN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-GEN-REPORTE' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-ALMACEN' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-GEN-SOLICITUD' AND EsPerfil = 0));

-- Asignación Perfil COMPRAS
INSERT INTO PermisoRelacion (ID_Padre, ID_Hijo) VALUES
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-COMPRAS' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-SELECCIONAR-PROD' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-COMPRAS' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-EMITIR-OC' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-COMPRAS' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-REG-FACTURA' AND EsPerfil = 0));

-- Asignación Perfil CONTABLE
INSERT INTO PermisoRelacion (ID_Padre, ID_Hijo) VALUES
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-CONTABLE' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-EVALUAR-COTIZ' AND EsPerfil = 0)),
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-CONTABLE' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-EFECTUAR-PAGO' AND EsPerfil = 0));

-- Asignar Admin al Usuario Base
INSERT INTO PerfilUsuario VALUES ('d1eda407-3582-4e0c-85cc-ae51eb67b826', (SELECT ID FROM Permiso WHERE Nombre = 'PERF-ADMIN' AND EsPerfil = 1));


-- =========================================================================
-- FASE 4: INSERCIÓN DE IDIOMAS Y TRADUCCIONES BASE
-- =========================================================================

INSERT INTO Idioma (Codigo, Nombre) VALUES ('ES', 'Español');
INSERT INTO Idioma (Codigo, Nombre) VALUES ('EN', 'English');
INSERT INTO Idioma (Codigo, Nombre) VALUES ('PT', 'Português');

-- Traducciones al Español
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'MainUI', 'Sistema de gestion');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemCerrarSesion', 'Cerrar sesión');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemIniciarSesion', 'Iniciar sesión');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemGestionDeUsuarios', 'Gestión de usuarios');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemABMUsuarios', 'ABM Usuarios');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemDesbloqueoUsuarios', 'Desploqueo de usuarios');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemGestionDePerfiles', 'Gestión de perfiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemABMPerfiles', 'Alta y asignación de perfiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemInicio', 'Inicio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemBitacora', 'Bitacora');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemConsultarBitacora', 'Consultar bitacora');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemPerfiles', 'Perfiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemGestionarPerfiles', 'Gestionar perfiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'label1', 'Idioma');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIGroupBoxAltaUsuario', 'Registrar usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIRegistroLabelUsername', 'Username');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIRegistroLabelEmail', 'Email');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIRegistroLabelNumTelefono', 'Número de telefono');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIRegistroLabelContrasena', 'Contraseña');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIRegistroLabelConfirmContrasena', 'Repetir Contraseña');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIButtonConfirmarRegistrarUsuario', 'Confirmar registro');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIGroupBoxListadoUsuarios', 'Listado de usuarios');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIGroupBoxModificacionUsuarios', 'Modificar usuario seleccionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIButtonConfirmarEliminarUsuario', 'Eliminar usuario seleccionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIModificacionLabelEmail', 'Email');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIModificacionLabelNumTelefono', 'Número de telefono');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionUsuariosUIModificacionButtonConfirmarModificar', 'Confirmar modificación');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'loginUILabelUsername', 'Usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'loginUILabelContrasena', 'Contraseña');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'loginUIButtonIniciarSesion', 'Iniciar sesión');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUIGroupBoxTreeView', 'Arbol de perfiles y permisos');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUILabelNombrePerfil', 'Nombre del nuevo perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUIButtonCrearPerfil', 'Crear perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUIGroupBoxListBoxPerfiles', 'Perfiles disponibles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilUIButtonAsignarPerfil', 'Asignar perfil a perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUIGroupBoxUsuarios', 'Usuarios disponibles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilUIButtonAsignarPerfilUsuario', 'Asignar perfil a usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilUIButtonDesasignarPerfilUsuario', 'Desasignar perfil a usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilesUIGroupBoxListBoxPermisos', 'Permisos disponibles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'perfilUIButtonAsignarPermiso', 'Asignar permiso a perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'bitacoraUILabelGrid', 'Registros de la bitacora');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'bitacoraUILabelComboBoxAccion', 'Filtrado por acción');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'bitacoraUILabelComboBoxUsername', 'Filtrado por username');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'bitacoraUIButtonLimpiarFiltros', 'Limpiar filtros');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_InicioSesionExito', 'Inicio de sesión exitoso.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_TituloExito', 'Éxito');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_CierreSesionExito', 'Sesión cerrada correctamente.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_TituloCierreSesion', 'Cerrar sesión');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_MaxIntentos', 'Ha superado los 3 intentos fallidos. Su cuenta ha sido bloqueada por seguridad.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_QuedanIntentos', 'Contraseña incorrecta. Le quedan {0} intentos antes de bloquearse.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_NoUserLogout', 'Usuario activo no encontrado en logout.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'log_InicioSesion', 'Inicio de Sesion');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'log_CierreSesion', 'Cierre de Sesion');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_UsuarioIncorrecto', 'Usuario incorrecto');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_UsuarioBloqueado', 'El usuario se encuentra bloqueado.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_EmailVacio', 'El correo electrónico no puede estar vacío');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_EmailFormato', 'El formato del correo electrónico no es válido');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_UsernameVacio', 'El nombre de usuario no puede estar vacio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_UsernameFormato', 'El nombre de usuario debe tener entre 3 y 16 caracteres y solo puede contener letras, números, guiones bajos y guiones');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_PhoneVacio', 'El número de teléfono no puede estar vacío');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_PhoneFormato', 'El formato del número de teléfono no es válido');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_OnlyLettersVacio', 'El campo de texto no puede estar vacío');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_OnlyLettersFormato', 'El campo solo puede contener letras y espacios (se permiten acentos y eñes)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_AlphaNumStrictVacio', 'El código o ID no puede estar vacío');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_AlphaNumStrictFormato', 'El campo solo puede contener letras (sin acentos) y números, sin espacios');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_AlphaNumSpacesVacio', 'El texto no puede estar vacío');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_AlphaNumSpacesFormato', 'El campo solo puede contener letras, números y espacios (sin caracteres especiales)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_PassVacia', 'La contraseña no puede estar vacía.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_PassNoCoincide', 'Las contraseñas no coinciden.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_NoUserModificar', 'No se ha seleccionado ningún usuario para modificar.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_NoUserEliminar', 'No se ha seleccionado ningún usuario para eliminar.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_TituloError', 'Error');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'btnDesbloquear', 'Desbloquear usuario seleccionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_NoUserDesbloquear', 'No se ha seleccionado ningún usuario para desbloquear.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_DesbloqueoExito', 'Usuario desbloqueado correctamente.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_LOGIN', 'Inició sesión en el sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_LOGOUT', 'Cerró sesión');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_USER_ADD', 'Registró a un nuevo usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_USER_MOD', 'Modificó los datos de un usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_USER_DEL', 'Eliminó a un usuario del sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_PERFIL_ADD', 'Asignó un perfil a un usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'LOG_PERMISOS_MOD', 'Modificó permisos del sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'GridBitacora_Usuario', 'Usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'GridBitacora_Fecha', 'Fecha y Hora');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'GridBitacora_Accion', 'Acción Realizada');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionHistorialUILabelGridUsuarios', 'Usuarios disponibles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionHistorialUILabelGridEstadoUsuarios', 'Historial del usuario seleccionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'mainUIStripMenuItemHistorialUsuario', 'Historial usuario');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'gestionHistorialUIButtonRecuperarEstado', 'Recuperar estado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'agregarIdiomaToolStripMenuItem', 'Agregar Idioma');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'GestionIdiomasUI', 'Configuración de Nuevos Idiomas');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'labelCodigo', 'Código (Ej: FR):');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'labelNombre', 'Nombre Idioma:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'btnGuardarIdioma', 'Guardar Idioma');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'GridIdioma_ColKey', 'Componente / Etiqueta');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'GridIdioma_ColRef', 'Referencia (Español)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'GridIdioma_ColNuevo', 'Nueva Traducción');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_IdiomaGuardadoExito', 'El idioma y sus respectivas traducciones se han guardado exitosamente.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_CodigoNombreObligatorios', 'El código y el nombre del idioma son obligatorios.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_IdiomaYaExiste', 'El código de idioma ya se encuentra registrado en el sistema.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_TraduccionObligatoria', 'Debe proveer al menos una traducción para el nuevo idioma.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'err_CargarEtiquetas', 'Error al cargar etiquetas de referencia: ');

-- Traducciones al Inglés
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'MainUI', 'Management System');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemCerrarSesion', 'Logout');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemIniciarSesion', 'Login');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemGestionDeUsuarios', 'User Management');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemABMUsuarios', 'CRUD Users');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemDesbloqueoUsuarios', 'Unlock Users');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemGestionDePerfiles', 'Profile Management');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemABMPerfiles', 'Profile Creation & Assignment');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemInicio', 'Home');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemBitacora', 'Logbook');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemConsultarBitacora', 'View Logbook');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemPerfiles', 'Profiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemGestionarPerfiles', 'Manage Profiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'label1', 'Language');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIGroupBoxAltaUsuario', 'Register User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIRegistroLabelUsername', 'Username');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIRegistroLabelEmail', 'Email');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIRegistroLabelNumTelefono', 'Phone Number');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIRegistroLabelContrasena', 'Password');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIRegistroLabelConfirmContrasena', 'Repeat Password');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIButtonConfirmarRegistrarUsuario', 'Confirm Registration');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIGroupBoxListadoUsuarios', 'User List');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIGroupBoxModificacionUsuarios', 'Modify Selected User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIButtonConfirmarEliminarUsuario', 'Delete Selected User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIModificacionLabelEmail', 'Email');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIModificacionLabelNumTelefono', 'Phone Number');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionUsuariosUIModificacionButtonConfirmarModificar', 'Confirm Modification');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'loginUILabelUsername', 'Username');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'loginUILabelContrasena', 'Password');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'loginUIButtonIniciarSesion', 'Login');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUIGroupBoxTreeView', 'Profiles and Permissions Tree');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUILabelNombrePerfil', 'New Profile Name');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUIButtonCrearPerfil', 'Create Profile');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUIGroupBoxListBoxPerfiles', 'Available Profiles');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilUIButtonAsignarPerfil', 'Assign Profile to Profile');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUIGroupBoxUsuarios', 'Available Users');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilUIButtonAsignarPerfilUsuario', 'Assign Profile to User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilUIButtonDesasignarPerfilUsuario', 'Unassign Profile from User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilesUIGroupBoxListBoxPermisos', 'Available Permissions');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'perfilUIButtonAsignarPermiso', 'Assign Permission to Profile');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'bitacoraUILabelGrid', 'Binnacle entries');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'bitacoraUILabelComboBoxAccion', 'Filter by action');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'bitacoraUILabelComboBoxUsername', 'Filter by username');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'bitacoraUIButtonLimpiarFiltros', 'Clean filters');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_InicioSesionExito', 'Successful login.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_TituloExito', 'Success');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_CierreSesionExito', 'Session closed successfully.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_TituloCierreSesion', 'Logout');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_MaxIntentos', 'Maximum failed attempts exceeded. Your account has been locked for security.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_QuedanIntentos', 'Incorrect password. You have {0} attempts left before being locked.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_NoUserLogout', 'Active user not found on logout.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'log_InicioSesion', 'Login');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'log_CierreSesion', 'Logout');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_UsuarioIncorrecto', 'Incorrect user');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_UsuarioBloqueado', 'The user is locked.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_EmailVacio', 'Email cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_EmailFormato', 'Invalid email format');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_UsernameVacio', 'Username cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_UsernameFormato', 'Username must be between 3 and 16 characters and can only contain letters, numbers, underscores, and hyphens');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_PhoneVacio', 'Phone number cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_PhoneFormato', 'Invalid phone number format');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_OnlyLettersVacio', 'The text field cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_OnlyLettersFormato', 'The field can only contain letters and spaces (accents and ñ are allowed)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_AlphaNumStrictVacio', 'Code or ID cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_AlphaNumStrictFormato', 'The field can only contain letters (no accents) and numbers, without spaces');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_AlphaNumSpacesVacio', 'Text cannot be empty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_AlphaNumSpacesFormato', 'The field can only contain letters, numbers, and spaces (no special characters)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_PassVacia', 'Password cannot be empty.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_PassNoCoincide', 'Passwords do not match.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_NoUserModificar', 'No user selected to modify.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_NoUserEliminar', 'No user selected to delete.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_TituloError', 'Error');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'btnDesbloquear', 'Unlock selected user');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_NoUserDesbloquear', 'No user selected to unlock.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_DesbloqueoExito', 'User unlocked successfully.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_LOGIN', 'Logged into the system');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_LOGOUT', 'Logged out');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_USER_ADD', 'Registered a new user');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_USER_MOD', 'Modified user details');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_USER_DEL', 'Deleted a user from the system');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_PERFIL_ADD', 'Assigned a profile to a user');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'LOG_PERMISOS_MOD', 'Modified system permissions');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'GridBitacora_Usuario', 'User');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'GridBitacora_Fecha', 'Date and Time');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'GridBitacora_Accion', 'Action Performed');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionHistorialUILabelGridUsuarios', 'Available users');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionHistorialUILabelGridEstadoUsuarios', 'Selected user history');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'mainUIStripMenuItemHistorialUsuario', 'User history');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'gestionHistorialUIButtonRecuperarEstado', 'Restore state');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'agregarIdiomaToolStripMenuItem', 'Add Language');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'GestionIdiomasUI', 'New Languages Configuration');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'labelCodigo', 'Code (e.g., FR):');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'labelNombre', 'Language Name:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'btnGuardarIdioma', 'Save Language');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'GridIdioma_ColKey', 'Component / Label');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'GridIdioma_ColRef', 'Reference (Spanish)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'GridIdioma_ColNuevo', 'New Translation');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_IdiomaGuardadoExito', 'The language and its translations have been saved successfully.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_CodigoNombreObligatorios', 'Language code and name are required.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_IdiomaYaExiste', 'The language code is already registered in the system.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_TraduccionObligatoria', 'You must provide at least one translation for the new language.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'err_CargarEtiquetas', 'Error loading reference labels: ');

-- Traducciones al Portugués
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'MainUI', 'Sistema de Gestão');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemCerrarSesion', 'Sair');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemIniciarSesion', 'Entrar');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemGestionDeUsuarios', 'Gestão de Usuários');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemABMUsuarios', 'CRUD de Usuários');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemDesbloqueoUsuarios', 'Desbloquear Usuários');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemGestionDePerfiles', 'Gestão de Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemABMPerfiles', 'Criação e Atribuição de Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemInicio', 'Início');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemBitacora', 'Livro de Bordo');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemConsultarBitacora', 'Visualizar Livro de Bordo');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemPerfiles', 'Perfis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemGestionarPerfiles', 'Gerenciar Perfis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'label1', 'Idioma');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIGroupBoxAltaUsuario', 'Registrar Usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIRegistroLabelUsername', 'Nome de usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIRegistroLabelEmail', 'E-mail');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIRegistroLabelNumTelefono', 'Número de Telefone');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIRegistroLabelContrasena', 'Senha');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIRegistroLabelConfirmContrasena', 'Repetir Senha');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIButtonConfirmarRegistrarUsuario', 'Confirmar Registro');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIGroupBoxListadoUsuarios', 'Lista de Usuários');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIGroupBoxModificacionUsuarios', 'Modificar Usuário Selecionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIButtonConfirmarEliminarUsuario', 'Excluir Usuário Selecionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIModificacionLabelEmail', 'E-mail');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIModificacionLabelNumTelefono', 'Número de Telefone');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionUsuariosUIModificacionButtonConfirmarModificar', 'Confirmar Modificação');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'loginUILabelUsername', 'Nome de usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'loginUILabelContrasena', 'Senha');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'loginUIButtonIniciarSesion', 'Entrar');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUIGroupBoxTreeView', 'Árvore de Perfis e Permissões');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUILabelNombrePerfil', 'Nome do Novo Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUIButtonCrearPerfil', 'Criar Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUIGroupBoxListBoxPerfiles', 'Perfis Disponíveis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilUIButtonAsignarPerfil', 'Atribuir Perfil a Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUIGroupBoxUsuarios', 'Usuários Disponíveis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilUIButtonAsignarPerfilUsuario', 'Atribuir Perfil a Usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilUIButtonDesasignarPerfilUsuario', 'Remover Perfil do Usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilesUIGroupBoxListBoxPermisos', 'Permissões Disponíveis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'perfilUIButtonAsignarPermiso', 'Atribuir Permissão ao Perfil');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'bitacoraUILabelGrid', 'Registros de Borda');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'bitacoraUILabelComboBoxAccion', 'Filtrar por ação');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'bitacoraUILabelComboBoxUsername', 'Filtrar por nome de usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'bitacoraUIButtonLimpiarFiltros', 'Limpar filtros');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_InicioSesionExito', 'Login bem-sucedido.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_TituloExito', 'Sucesso');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_CierreSesionExito', 'Sessão encerrada com sucesso.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_TituloCierreSesion', 'Sair');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_MaxIntentos', 'Limite de tentativas excedido. Sua conta foi bloqueada por segurança.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_QuedanIntentos', 'Senha incorreta. Você tem {0} tentativas restantes antes de ser bloqueado.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_NoUserLogout', 'Usuário ativo não encontrado no logout.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'log_InicioSesion', 'Início de Sessão');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'log_CierreSesion', 'Encerramento de Sessão');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_UsuarioIncorrecto', 'Usuário incorreto');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_UsuarioBloqueado', 'O usuário está bloqueado.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_EmailVacio', 'O e-mail não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_EmailFormato', 'Formato de e-mail inválido');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_UsernameVacio', 'O nome de usuário não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_UsernameFormato', 'O nome de usuário deve ter entre 3 e 16 caracteres e só pode conter letras, números, sublinhados e hifens');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_PhoneVacio', 'O número de telefone não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_PhoneFormato', 'Formato de número de telefone inválido');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_OnlyLettersVacio', 'O campo de texto não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_OnlyLettersFormato', 'O campo só pode conter letras e espaços (acentos e cedilhas são permitidos)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_AlphaNumStrictVacio', 'O código ou ID não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_AlphaNumStrictFormato', 'O campo só pode conter letras (sem acentos) e números, sem espaços');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_AlphaNumSpacesVacio', 'O texto não pode estar vazio');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_AlphaNumSpacesFormato', 'O campo só pode conter letras, números e espaços (sem caracteres especiais)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_PassVacia', 'A senha não pode estar vazia.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_PassNoCoincide', 'As senhas não coincidem.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_NoUserModificar', 'Nenhum usuário selecionado para modificar.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_NoUserEliminar', 'Nenhum usuário selecionado para excluir.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_TituloError', 'Erro');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'btnDesbloquear', 'Desbloquear usuário selecionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_NoUserDesbloquear', 'Nenhum usuário selecionado para desbloquear.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_DesbloqueoExito', 'Usuário desbloqueado com sucesso.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_LOGIN', 'Entrou no sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_LOGOUT', 'Saiu do sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_USER_ADD', 'Registrou um novo usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_USER_MOD', 'Modificou os dados de um usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_USER_DEL', 'Excluiu um usuário do sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_PERFIL_ADD', 'Atribuiu um perfil a um usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'LOG_PERMISOS_MOD', 'Modificou as permissões do sistema');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'GridBitacora_Usuario', 'Usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'GridBitacora_Fecha', 'Data e Hora');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'GridBitacora_Accion', 'Ação Realizada');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionHistorialUILabelGridUsuarios', 'Usuários disponíveis');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionHistorialUILabelGridEstadoUsuarios', 'Histórico do usuário selecionado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'mainUIStripMenuItemHistorialUsuario', 'Histórico do usuário');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'gestionHistorialUIButtonRecuperarEstado', 'Restaurar estado');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'agregarIdiomaToolStripMenuItem', 'Adicionar Idioma');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'GestionIdiomasUI', 'Configuração de Novos Idiomas');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'labelCodigo', 'Código (Ex: FR):');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'labelNombre', 'Nome do Idioma:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'btnGuardarIdioma', 'Salvar Idioma');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'GridIdioma_ColKey', 'Componente / Rótulo');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'GridIdioma_ColRef', 'Referência (Espanhol)');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'GridIdioma_ColNuevo', 'Nova Tradução');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_IdiomaGuardadoExito', 'O idioma e suas traduções foram salvos com sucesso.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_CodigoNombreObligatorios', 'O código e o nome do idioma são obrigatórios.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_IdiomaYaExiste', 'O código do idioma já está registrado no sistema.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_TraduccionObligatoria', 'Você deve fornecer pelo menos uma tradução para o novo idioma.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'err_CargarEtiquetas', 'Erro ao carregar rótulos de referência: ');


-- =========================================================================
-- 1. CREACIÓN DEL ROL VENDEDOR (Si no existía previamente)
-- =========================================================================
IF NOT EXISTS (SELECT 1 FROM Permiso WHERE Nombre = 'PERF-VENDEDOR')
BEGIN
    INSERT INTO Permiso (Nombre, EsPerfil) VALUES ('PERF-VENDEDOR', 1);
END

-- =========================================================================
-- 2. CREACIÓN DE USUARIOS
-- =========================================================================

-- Insertar vendedor1
INSERT INTO Usuario (ID, Username, PasswordHash, Email, NumTelefono, EstaBloqueado, Idioma, IntentosFallidos, DVH)
VALUES (
    NEWID(), 
    'vendedor1', 
    '5994471ABB01112AFCC18159F6CC74B4F511B99806DA59B3CAF5A9C173CACFC5', -- Hash de '12345'
    'vendedor1@empresa.com', 
    '+541100000001', 
    0, 
    'ES', 
    0, 
    '0F988652BC99ED283EC500C36A2974D1B915134AB50B8C50743B11CEB8A09C11'
);

-- Insertar vendedor2
INSERT INTO Usuario (ID, Username, PasswordHash, Email, NumTelefono, EstaBloqueado, Idioma, IntentosFallidos, DVH)
VALUES (
    NEWID(), 
    'vendedor2', 
    '5994471ABB01112AFCC18159F6CC74B4F511B99806DA59B3CAF5A9C173CACFC5', -- Hash de '12345'
    'vendedor2@empresa.com', 
    '+541100000002', 
    0, 
    'ES', 
    0, 
    'E55FFC885CCF686DB51ABADB3F1F320B6A2910630E7E0A64D2E56327FE2F91BF'
);

-- Insertar almacen1
INSERT INTO Usuario (ID, Username, PasswordHash, Email, NumTelefono, EstaBloqueado, Idioma, IntentosFallidos, DVH)
VALUES (
    NEWID(), 
    'almacen1', 
    '5994471ABB01112AFCC18159F6CC74B4F511B99806DA59B3CAF5A9C173CACFC5', -- Hash de '12345'
    'almacen1@empresa.com', 
    '+541100000003', 
    0, 
    'ES', 
    0, 
    '48A0D8F27F77BF3261F0D3E1C890FE6FB1DFD7AA971297F40E582650D4BB1710'
);

-- Insertar analista1
INSERT INTO Usuario (ID, Username, PasswordHash, Email, NumTelefono, EstaBloqueado, Idioma, IntentosFallidos, DVH)
VALUES (
    NEWID(), 
    'analista1', 
    '5994471ABB01112AFCC18159F6CC74B4F511B99806DA59B3CAF5A9C173CACFC5', -- Hash de '12345'
    'analista1@empresa.com', 
    '+541100000004', 
    0, 
    'ES', 
    0, 
    '1ABC49B84ADE1CAED8195E50A5502641E3B690477D056B65F2F6F484D759EEF9'
);

-- =========================================================================
-- CREACIÓN DE USUARIO: SECTOR COMPRAS
-- =========================================================================

-- Insertar compras1
INSERT INTO Usuario (ID, Username, PasswordHash, Email, NumTelefono, EstaBloqueado, Idioma, IntentosFallidos, DVH)
VALUES (
    NEWID(), 
    'compras1', 
    '5994471ABB01112AFCC18159F6CC74B4F511B99806DA59B3CAF5A9C173CACFC5', -- Hash de '12345'
    'compras1@empresa.com', 
    '+541100000005', 
    0, 
    'ES', 
    0, 
    'TEMPORAL'
);

-- Asignar rol a compras1 (Solo Compras)
INSERT INTO PerfilUsuario (ID_Usuario, ID_Perfil) 
VALUES (
    (SELECT ID FROM Usuario WHERE Username = 'compras1'), 
    (SELECT ID FROM Permiso WHERE Nombre = 'PERF-COMPRAS' AND EsPerfil = 1)
);
-- =========================================================================
-- 3. ASIGNACIÓN DE ROLES A LOS USUARIOS
-- =========================================================================

-- Asignar rol a vendedor1
INSERT INTO PerfilUsuario (ID_Usuario, ID_Perfil) 
VALUES (
    (SELECT ID FROM Usuario WHERE Username = 'vendedor1'), 
    (SELECT ID FROM Permiso WHERE Nombre = 'PERF-VENDEDOR' AND EsPerfil = 1)
);

-- Asignar rol a vendedor2
INSERT INTO PerfilUsuario (ID_Usuario, ID_Perfil) 
VALUES (
    (SELECT ID FROM Usuario WHERE Username = 'vendedor2'), 
    (SELECT ID FROM Permiso WHERE Nombre = 'PERF-VENDEDOR' AND EsPerfil = 1)
);

-- Asignar roles a almacen1 (Almacén + Compras)
INSERT INTO PerfilUsuario (ID_Usuario, ID_Perfil) 
VALUES (
    (SELECT ID FROM Usuario WHERE Username = 'almacen1'), 
    (SELECT ID FROM Permiso WHERE Nombre = 'PERF-ALMACEN' AND EsPerfil = 1)
);

-- Asignar rol a analista1 (Contable)
INSERT INTO PerfilUsuario (ID_Usuario, ID_Perfil) 
VALUES (
    (SELECT ID FROM Usuario WHERE Username = 'analista1'), 
    (SELECT ID FROM Permiso WHERE Nombre = 'PERF-CONTABLE' AND EsPerfil = 1)
);


-- 2. Creamos la tabla con la estructura correcta para manejar Pallets
CREATE TABLE [dbo].[CATALOGO_PROVEEDOR] (
    [IdProveedor] INT FOREIGN KEY REFERENCES [PROVEEDOR]([IdProveedor]),
    [IdProducto] VARCHAR(50), -- Se ajusta a VARCHAR(50) para coincidir con tu tabla PRODUCTO
    [NombreArticuloProveedor] VARCHAR(100), 
    [PrecioPallet] DECIMAL(12,2) NOT NULL,  
    [UnidadesPorPallet] INT NOT NULL,       
    PRIMARY KEY ([IdProveedor], [IdProducto])
);
GO

---------------------------------------------------------------------------------------------------------------------
INSERT INTO [dbo].[PROVEEDOR] ([CUIT], [RazonSocial], [CondicionComercial], [Activo])
VALUES 
    ('30-50673003-8', 'Coca-Cola FEMSA', 'Fábrica Directa', 1),
    ('30-53758070-1', 'PepsiCo Argentina', 'Fábrica Directa', 1),
    ('30-70894042-7', 'Refres Now S.A. (Manaos)', 'Fábrica Directa', 1),
    ('30-50013003-4', 'RPB S.A. (Baggio)', 'Fábrica Directa', 1);


-- 1. Agregamos los productos faltantes a tu tabla principal para que el Almacén los reconozca
INSERT INTO PRODUCTO (IdProducto, CodigoSKU, NombreBebida, PrecioUnitarioLocal, Activo, StockActual, PuntoPedido) VALUES 
('7790895001999', 'SKU-011', 'Coca-Cola Lata 473ml', 1000, 1, 50, 100),
('7798099881038', 'SKU-012', 'Manaos Pomelo 2.25L', 900, 1, 80, 50),
('7790503000001', 'SKU-013', 'Baggio Multifruta 1L', 1200, 1, 40, 60),
('7790503000002', 'SKU-014', 'Baggio Naranja 1L', 1200, 1, 40, 60),
('7790895000997', 'SKU-015', 'Coca-Cola Original 2.25L', 1500, 1, 30, 80),
('7791813421112', 'SKU-016', '7Up Regular 1.5L', 1100, 1, 40, 70),
('7791813421051', 'SKU-017', 'Paso de los Toros 1.5L', 1100, 1, 45, 70),
('7798099881014', 'SKU-018', 'Manaos Cola 2.25L', 900, 1, 100, 50),
('7798099881021', 'SKU-019', 'Manaos Lima Limón 2.25L', 900, 1, 90, 50);

-- 2. Buscamos los IDs reales
DECLARE @IdCoca INT = (SELECT IdProveedor FROM PROVEEDOR WHERE CUIT = '30-50673003-8');
DECLARE @IdPepsi INT = (SELECT IdProveedor FROM PROVEEDOR WHERE CUIT = '30-53758070-1');
DECLARE @IdManaos INT = (SELECT IdProveedor FROM PROVEEDOR WHERE CUIT = '30-70894042-7');
DECLARE @IdBaggio INT = (SELECT IdProveedor FROM PROVEEDOR WHERE CUIT = '30-50013003-4');

-- 3. Insertamos todo de una sola pasada
INSERT INTO CATALOGO_PROVEEDOR (IdProveedor, IdProducto, NombreArticuloProveedor, PrecioPallet, UnidadesPorPallet) VALUES 
(@IdCoca, '7790895000997', 'Pallet Coca-Cola Original 2.25L (40 packs x 6)', 360000.00, 240),
(@IdCoca, '7790895001999', 'Pallet Coca-Cola Lata 473ml (100 packs x 6)', 450000.00, 600),
(@IdPepsi, '7791813421112', 'Pallet 7Up Regular 1.5L (60 packs x 6)', 300000.00, 360),
(@IdPepsi, '7791813421051', 'Pallet Paso de los Toros Pomelo 1.5L (60 packs x 6)', 280000.00, 360),
(@IdManaos, '7798099881014', 'Pallet Manaos Cola 2.25L (50 packs x 6)', 200000.00, 300),
(@IdManaos, '7798099881021', 'Pallet Manaos Lima Limón 2.25L (50 packs x 6)', 200000.00, 300),
(@IdManaos, '7798099881038', 'Pallet Manaos Pomelo 2.25L (50 packs x 6)', 200000.00, 300),
(@IdBaggio, '7790503000001', 'Pallet Baggio Multifruta 1L (80 cajas x 8)', 400000.00, 640),
(@IdBaggio, '7790503000002', 'Pallet Baggio Naranja 1L (80 cajas x 8)', 400000.00, 640);

USE DistriuidoraMegaDrink;
GO

-- ==========================================
-- TRADUCCIONES AL ESPAÑOL (ES)
-- ==========================================
-- Menú MainUI
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'recuperarIntegridadToolStripMenuItem', 'Recuperar integridad');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'menuStripItemAlmacen', 'Almacén');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'almacenToolStripMenuItemGenerarSolicitud', 'Generar Solicitud de Abastecimiento');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'menuStripItemCompras', 'Compras');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'comprasToolStripMenuItemEmitirOC', 'Orden de Compra');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'menuStripItemContabilidad', 'Contabilidad');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'contabilidadToolStripMenuItemEvaluarCotiz', 'Evaluar Cotización');

-- Controles visuales (Mapeo directo por Name)
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'Agregar', 'Agregar');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'btn_Solicitud_Abastecimiento', 'Confirmar Solicitud');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'lblSolicitudesPendientes', 'Solicitudes Pendientes de Revisión:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'lblDetalleProductos', 'Productos Requeridos en la Solicitud');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'btnVolver', 'Volver');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'btnAtenderSolicitud', 'Atender Solicitud y Emitir Orden');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'lblFaltantesSolicitados', 'FALTANTES SOLICITADOS');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'label2', 'CATÁLOGO PROVEEDORES DIRECTOS');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'label3', 'ORDEN DE COMPRA');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'label4', 'Cantidad Pallets:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'lblTotalOC', 'Total Neto:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'btnCancelarOC', 'Volver');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'btnEmitirOrden', 'Emitir Orden Compra');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'btnAgregarCarrito', 'Agregar al Carrito');

-- Textos dinámicos (Grillas y MessageBox)
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'grid_CodigoBarras', 'Código de Barras');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'grid_Producto', 'Producto');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'grid_StockActual', 'Stock Actual');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'grid_PuntoPedido', 'Punto de Pedido');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'grid_ProdFaltante', 'Producto Faltante');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'grid_CantSugerida', 'Cant. Sugerida');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'grid_FechaSol', 'Fecha de Solicitud');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'grid_SolicitadoPor', 'Solicitado por');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'grid_NumSolicitud', 'N° Solicitud');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'grid_SKU', 'SKU / Código');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_Atencion', 'Atención');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_SeleccioneProdAbastecimiento', 'Por favor, seleccione un producto del inventario que necesite reabastecimiento.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_ProdYaEnFaltantes', 'Este producto ya está en la lista actual de faltantes.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_ProdYaEnCompras', 'Este producto ya se encuentra en una solicitud anterior pendiente de revisión por Compras.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_NoHayProductosSolicitar', 'No hay productos en la lista para solicitar.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_SolicitudEnviadaExito', 'Solicitud enviada a Compras exitosamente.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_CantidadMayorCero', 'La cantidad debe ser mayor a 0.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_SeleccioneProdCatalogo', 'Seleccione un producto del catálogo.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_CarritoVacioPallets', 'El carrito está vacío. Agregue pallets antes de emitir la orden.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_Validacion', 'Validación');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_OrdenEmitidaExito', 'Orden de Compra generada y enviada a Contabilidad exitosamente.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('ES', 'msg_ExitoB2B', 'Éxito B2B');

-- ==========================================
-- TRADUCCIONES AL INGLÉS (EN)
-- ==========================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'recuperarIntegridadToolStripMenuItem', 'Recover integrity');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'menuStripItemAlmacen', 'Warehouse');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'almacenToolStripMenuItemGenerarSolicitud', 'Generate Supply Request');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'menuStripItemCompras', 'Purchasing');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'comprasToolStripMenuItemEmitirOC', 'Purchase Order');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'menuStripItemContabilidad', 'Accounting');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'contabilidadToolStripMenuItemEvaluarCotiz', 'Evaluate Quote');

INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'Agregar', 'Add');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'btn_Solicitud_Abastecimiento', 'Confirm Request');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'lblSolicitudesPendientes', 'Pending Requests for Review:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'lblDetalleProductos', 'Products Required in Request');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'btnVolver', 'Back');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'btnAtenderSolicitud', 'Handle Request & Issue Order');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'lblFaltantesSolicitados', 'REQUESTED SHORTAGES');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'label2', 'DIRECT SUPPLIERS CATALOG');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'label3', 'PURCHASE ORDER');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'label4', 'Pallet Quantity:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'lblTotalOC', 'Net Total:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'btnCancelarOC', 'Back');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'btnEmitirOrden', 'Issue Purchase Order');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'btnAgregarCarrito', 'Add to Cart');

INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'grid_CodigoBarras', 'Barcode');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'grid_Producto', 'Product');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'grid_StockActual', 'Current Stock');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'grid_PuntoPedido', 'Reorder Point');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'grid_ProdFaltante', 'Missing Product');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'grid_CantSugerida', 'Suggested Qty');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'grid_FechaSol', 'Request Date');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'grid_SolicitadoPor', 'Requested by');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'grid_NumSolicitud', 'Request No.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'grid_SKU', 'SKU / Code');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_Atencion', 'Warning');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_SeleccioneProdAbastecimiento', 'Please select an inventory product that needs restocking.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_ProdYaEnFaltantes', 'This product is already in the current shortage list.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_ProdYaEnCompras', 'This product is already in a pending request under Purchasing review.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_NoHayProductosSolicitar', 'No products in the list to request.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_SolicitudEnviadaExito', 'Request successfully sent to Purchasing.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_CantidadMayorCero', 'Quantity must be greater than 0.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_SeleccioneProdCatalogo', 'Select a product from the catalog.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_CarritoVacioPallets', 'Cart is empty. Add pallets before issuing the order.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_Validacion', 'Validation');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_OrdenEmitidaExito', 'Purchase Order successfully generated and sent to Accounting.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('EN', 'msg_ExitoB2B', 'B2B Success');

-- ==========================================
-- TRADUCCIONES AL PORTUGUÉS (PT)
-- ==========================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'recuperarIntegridadToolStripMenuItem', 'Recuperar integridade');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'menuStripItemAlmacen', 'Armazém');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'almacenToolStripMenuItemGenerarSolicitud', 'Gerar Solicitação de Abastecimento');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'menuStripItemCompras', 'Compras');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'comprasToolStripMenuItemEmitirOC', 'Ordem de Compra');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'menuStripItemContabilidad', 'Contabilidade');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'contabilidadToolStripMenuItemEvaluarCotiz', 'Avaliar Cotação');

INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'Agregar', 'Adicionar');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'btn_Solicitud_Abastecimiento', 'Confirmar Solicitação');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'lblSolicitudesPendientes', 'Solicitações Pendentes de Revisão:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'lblDetalleProductos', 'Produtos Requeridos na Solicitação');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'btnVolver', 'Voltar');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'btnAtenderSolicitud', 'Atender Solicitação e Emitir Ordem');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'lblFaltantesSolicitados', 'FALTAS SOLICITADAS');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'label2', 'CATÁLOGO DE FORNECEDORES DIRETOS');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'label3', 'ORDEM DE COMPRA');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'label4', 'Quantidade de Paletes:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'lblTotalOC', 'Total Líquido:');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'btnCancelarOC', 'Voltar');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'btnEmitirOrden', 'Emitir Ordem de Compra');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'btnAgregarCarrito', 'Adicionar ao Carrinho');

INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'grid_CodigoBarras', 'Código de Barras');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'grid_Producto', 'Produto');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'grid_StockActual', 'Estoque Atual');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'grid_PuntoPedido', 'Ponto de Pedido');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'grid_ProdFaltante', 'Produto Faltante');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'grid_CantSugerida', 'Qtd. Sugerida');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'grid_FechaSol', 'Data da Solicitação');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'grid_SolicitadoPor', 'Solicitado por');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'grid_NumSolicitud', 'N° Solicitação');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'grid_SKU', 'SKU / Código');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_Atencion', 'Atenção');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_SeleccioneProdAbastecimiento', 'Por favor, selecione um produto do estoque que precise de reabastecimento.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_ProdYaEnFaltantes', 'Este produto já está na lista atual de faltas.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_ProdYaEnCompras', 'Este produto já se encontra numa solicitação anterior pendente de revisão por Compras.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_NoHayProductosSolicitar', 'Não há produtos na lista para solicitar.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_SolicitudEnviadaExito', 'Solicitação enviada a Compras com sucesso.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_CantidadMayorCero', 'A quantidade deve ser maior que 0.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_SeleccioneProdCatalogo', 'Selecione um produto do catálogo.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_CarritoVacioPallets', 'O carrinho está vazio. Adicione paletes antes de emitir a ordem.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_Validacion', 'Validação');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_OrdenEmitidaExito', 'Ordem de Compra gerada e enviada à Contabilidade com sucesso.');
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES ('PT', 'msg_ExitoB2B', 'Sucesso B2B');