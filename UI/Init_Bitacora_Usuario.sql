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
    EsPerfil BIT NOT NULL,
    NivelJerarquico INT NOT NULL DEFAULT 99 -- Se agrega para control de jerarquías
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
    IdProveedor INT NOT NULL,
    FechaEmision DATETIME NOT NULL,
    Estado VARCHAR(50) NOT NULL,
    CONSTRAINT FK_OrdenCompra_Proveedor FOREIGN KEY (IdProveedor) REFERENCES PROVEEDOR(IdProveedor)
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

----Tablas Venta-------------
-- =========================================================================
-- FASE 5: CREACIÓN DE TABLAS DE NEGOCIO (VENTAS Y DISTRIBUCIÓN)
-- =========================================================================

-- 1. Tabla de Clientes (Kioscos, Supermercados, Minoristas)
CREATE TABLE CLIENTE (
    IdCliente INT PRIMARY KEY IDENTITY(1,1),
    CUIT VARCHAR(20) NOT NULL UNIQUE,
    RazonSocial VARCHAR(100) NOT NULL,
    Direccion VARCHAR(150) NOT NULL,
    Telefono VARCHAR(20),
    Email VARCHAR(100),
    Activo BIT NOT NULL DEFAULT 1
);

-- 2. Cabecera de la Venta (Factura o Remito de Salida)
CREATE TABLE VENTA (
    IdVenta INT PRIMARY KEY IDENTITY(1,1),
    IdCliente INT NOT NULL,
    IdUsuario UNIQUEIDENTIFIER NOT NULL, -- El vendedor que registra la operación
    FechaVenta DATETIME NOT NULL,
    MontoTotal DECIMAL(18,2) NOT NULL,
    Estado VARCHAR(50) NOT NULL, -- Ej: 'Emitida', 'Pagada', 'Anulada'
    CONSTRAINT FK_Venta_Cliente FOREIGN KEY (IdCliente) REFERENCES CLIENTE(IdCliente),
    CONSTRAINT FK_Venta_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(ID)
);

-- 3. Detalle de los productos vendidos (Crucial para restar el Stock)
CREATE TABLE DETALLE_VENTA (
    IdDetalleVenta INT PRIMARY KEY IDENTITY(1,1),
    IdVenta INT NOT NULL,
    IdProducto VARCHAR(50) NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    Subtotal DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_DetalleVenta_Venta FOREIGN KEY (IdVenta) REFERENCES VENTA(IdVenta),
    CONSTRAINT FK_DetalleVenta_Producto FOREIGN KEY (IdProducto) REFERENCES PRODUCTO(IdProducto)
);

-- 4. Registro de Cobranza (Ingreso de dinero, simétrico a PAGO_EMITIDO)
CREATE TABLE COBRO (
    IdCobro INT PRIMARY KEY IDENTITY(1,1),
    IdVenta INT NOT NULL,
    FechaCobro DATETIME NOT NULL,
    MontoCobrado DECIMAL(18,2) NOT NULL,
    MetodoPago VARCHAR(50) NOT NULL, -- 'Efectivo', 'Debito', 'Credito', 'Transferencia'
    NumeroTarjeta VARCHAR(20) NULL,  -- Para guardar "XXXX-XXXX-XXXX-1234" (Mock)
    Cuotas INT NOT NULL DEFAULT 1,   -- 1, 6, 9, 12
    NumeroComprobante VARCHAR(50) NULL,
    CONSTRAINT FK_Cobro_Venta FOREIGN KEY (IdVenta) REFERENCES VENTA(IdVenta)
);
CREATE TABLE OFERTA_SUGERIDA (
    IdOferta INT PRIMARY KEY IDENTITY(1,1),
    IdLote INT NOT NULL,                      -- El lote de bebida que está próximo a vencer
    IdProductoAsociado VARCHAR(50) NULL,      -- Producto "gancho" sugerido (ej: el más vendido junto a este)
    TituloOferta VARCHAR(150) NOT NULL,       -- Ej: "Promo 15% OFF - Lote Próximo a Vencer"
    DescuentoSugerido DECIMAL(5,2) NOT NULL,  -- Porcentaje de descuento sugerido (Ej: 15.00)
    Estado VARCHAR(50) NOT NULL,              -- 'Pendiente', 'Aprobada', 'Rechazada', 'Expirada'
    FechaGeneracion DATETIME NOT NULL,
    CONSTRAINT FK_Oferta_Lote FOREIGN KEY (IdLote) REFERENCES LOTE_BEBIDA(IdLote),
    CONSTRAINT FK_Oferta_ProductoAsociado FOREIGN KEY (IdProductoAsociado) REFERENCES PRODUCTO(IdProducto)
);
GO

CREATE VIEW VISTA_HISTORIAL_VENTAS AS
SELECT 
    v.IdVenta,
    v.FechaVenta,
    c.RazonSocial AS Cliente,
    u.Username AS Vendedor,
    v.MontoTotal,
    v.Estado,
    ISNULL(STUFF((
        SELECT ', ' + co.MetodoPago + ' ($' + CAST(co.MontoCobrado AS VARCHAR(20)) + ')'
        FROM COBRO co
        WHERE co.IdVenta = v.IdVenta
        FOR XML PATH('')
    ), 1, 2, ''), 'Sin pagos registrados') AS PagosCombinados
FROM VENTA v
INNER JOIN CLIENTE c ON v.IdCliente = c.IdCliente
INNER JOIN Usuario u ON v.IdUsuario = u.ID;
GO
-- =========================================================================
-- INSERCIÓN DE DATOS DE PRUEBA (CLIENTES BASE)
-- =========================================================================

INSERT INTO CLIENTE (CUIT, RazonSocial, Direccion, Telefono, Email, Activo) VALUES 
(
    '00-00000000-0',         -- CUIT genérico (o DNI si fuera consumidor final estándar en Argentina)
    'Consumidor Final',      -- Nombre a mostrar en la factura/ticket
    'S/D',                   -- Sin Dirección
    'S/D',
	'S/D',
	1),
('30-11223344-5', 'Kiosco El Sol', 'Av. Rivadavia 1234, CABA', '+54 11 4444-5555', 'contacto@elsol.com', 1),
('30-99887766-1', 'Supermercado Los Chinos', 'Calle Falsa 123, CABA', '+54 11 6666-7777', 'compras@loschinos.com', 1),
('30-55443322-9', 'Almacén Don Manolo', 'Av. Corrientes 4321, CABA', '+54 11 8888-9999', 'manolo@almacen.com', 1);

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
INSERT INTO Permiso (Nombre, EsPerfil, NivelJerarquico) VALUES
('PERM-GESTIONAR-USR', 0, 99),
('PERM-GESTIONAR-IDM', 0, 99),
('PERM-DESBLOQUEAR-USR', 0, 99),
('PERM-GESTIONAR-PERFIL', 0, 99),
('PERM-CONSULTA-BIT', 0, 99),
('PERM-GESTIONAR-HISTORIAL', 0, 99),
('PERM-AGREGAR-IDM', 0, 99),
('PERF-ADMIN', 1, 1); -- Nivel 1: Rango máximo

-- Permisos Nuevos (Proceso B2B) y Perfil Vendedor
INSERT INTO Permiso (Nombre, EsPerfil, NivelJerarquico) VALUES
('PERM-ABM-PROD', 0, 99),
('PERM-GEN-REPORTE', 0, 99),
('PERM-SELECCIONAR-PROD', 0, 99),
('PERM-EMITIR-OC', 0, 99),
('PERM-REG-FACTURA', 0, 99),
('PERM-EVALUAR-COTIZ', 0, 99),
('PERM-EFECTUAR-PAGO', 0, 99),
('PERM-ABM-PROV', 0, 99),
('PERM-AUDITAR-COMPRA', 0, 99),
('PERM-GEN-SOLICITUD', 0, 99),
('PERF-VENDEDOR', 1, 3); -- Nivel 3: Rango operativo

-- Roles Nuevos (Proceso B2B)
INSERT INTO Permiso (Nombre, EsPerfil, NivelJerarquico) VALUES
('PERF-ALMACEN', 1, 2), -- Nivel 2: Rango departamental
('PERF-COMPRAS', 1, 2),
('PERF-CONTABLE', 1, 2);

-- Permiso de ventas
INSERT INTO Permiso (Nombre, EsPerfil, NivelJerarquico) VALUES 
('PERM-REALIZAR-VENTA', 0, 99);

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
-- 2. CREACIÓN DE USUARIOS MOCKUP
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

-- =========================================================================
-- 3. ASIGNACIÓN DE ROLES A LOS USUARIOS MOCKUP
-- =========================================================================
INSERT INTO PerfilUsuario (ID_Usuario, ID_Perfil) VALUES (
    (SELECT ID FROM Usuario WHERE Username = 'compras1'), 
    (SELECT ID FROM Permiso WHERE Nombre = 'PERF-COMPRAS' AND EsPerfil = 1));

INSERT INTO PerfilUsuario (ID_Usuario, ID_Perfil) VALUES (
    (SELECT ID FROM Usuario WHERE Username = 'vendedor1'), 
    (SELECT ID FROM Permiso WHERE Nombre = 'PERF-VENDEDOR' AND EsPerfil = 1));

INSERT INTO PerfilUsuario (ID_Usuario, ID_Perfil) VALUES (
    (SELECT ID FROM Usuario WHERE Username = 'vendedor2'), 
    (SELECT ID FROM Permiso WHERE Nombre = 'PERF-VENDEDOR' AND EsPerfil = 1));

INSERT INTO PerfilUsuario (ID_Usuario, ID_Perfil) VALUES (
    (SELECT ID FROM Usuario WHERE Username = 'almacen1'), 
    (SELECT ID FROM Permiso WHERE Nombre = 'PERF-ALMACEN' AND EsPerfil = 1));

INSERT INTO PerfilUsuario (ID_Usuario, ID_Perfil) VALUES (
    (SELECT ID FROM Usuario WHERE Username = 'analista1'), 
    (SELECT ID FROM Permiso WHERE Nombre = 'PERF-CONTABLE' AND EsPerfil = 1));


-- Asignar el permiso de venta al perfil PERF-VENDEDOR
INSERT INTO PermisoRelacion (ID_Padre, ID_Hijo) VALUES 
((SELECT ID FROM Permiso WHERE Nombre = 'PERF-VENDEDOR' AND EsPerfil = 1), (SELECT ID FROM Permiso WHERE Nombre = 'PERM-REALIZAR-VENTA' AND EsPerfil = 0));
---------------------------------------------------------------------------------------------------------------------
-- 4. POBLACIÓN DE PRODUCTOS Y PROVEEDORES
---------------------------------------------------------------------------------------------------------------------
INSERT INTO [dbo].[PROVEEDOR] ([CUIT], [RazonSocial], [CondicionComercial], [Activo])
VALUES 
    ('30-50673003-8', 'Coca-Cola FEMSA', 'Fábrica Directa', 1),
    ('30-53758070-1', 'PepsiCo Argentina', 'Fábrica Directa', 1),
    ('30-70894042-7', 'Refres Now S.A. (Manaos)', 'Fábrica Directa', 1),
    ('30-50013003-4', 'RPB S.A. (Baggio)', 'Fábrica Directa', 1);

INSERT INTO PRODUCTO (IdProducto, CodigoSKU, NombreBebida, PrecioUnitarioLocal, Activo, StockActual, PuntoPedido) VALUES 
('7790895001999', 'SKU-011', 'Coca-Cola Lata 473ml', 1000, 1, 50, 100),
('7798099881038', 'SKU-012', 'Manaos Pomelo 2.25L', 900, 1, 80, 50),
('7790503000001', 'SKU-013', 'Baggio Multifruta 1L', 1200, 1, 40, 60),
('7790503000002', 'SKU-014', 'Baggio Naranja 1L', 1200, 1, 40, 60),
('7790895000997', 'SKU-015', 'Coca-Cola Original 2.25L', 1500, 1, 30, 80),
('7791813421112', 'SKU-016', '7Up Regular 1.5L', 1100, 1, 40, 70),
('7791813421051', 'SKU-017', 'Paso de los Toros 1.5L', 1100, 1, 45, 70),
('7798099881014', 'SKU-018', 'Manaos Cola 2.25L', 900, 1, 100, 50),
('7798099881021', 'SKU-019', 'Manaos Lima Limón 2.25L', 900, 1, 90, 50),
('7790895003333', 'SKU-020', 'Sprite Regular 2.25L', 1400, 1, 40, 80),
('7790895004444', 'SKU-021', 'Fanta Naranja 2.25L', 1400, 1, 40, 80),
('7790895005555', 'SKU-022', 'Coca-Cola Zero 2.25L', 1500, 1, 50, 80),
('7791813421129', 'SKU-025', 'Pepsi Black 1.5L', 1100, 1, 60, 70),
('7798099881045', 'SKU-023', 'Manaos Naranja 2.25L', 900, 1, 70, 50),
('7798099881052', 'SKU-024', 'Manaos Pomelo Blanco 2.25L', 900, 1, 60, 50),
('7790503000003', 'SKU-026', 'Baggio Manzana 1L', 1200, 1, 45, 60);

DECLARE @IdCoca INT = (SELECT IdProveedor FROM PROVEEDOR WHERE CUIT = '30-50673003-8');
DECLARE @IdPepsi INT = (SELECT IdProveedor FROM PROVEEDOR WHERE CUIT = '30-53758070-1');
DECLARE @IdManaos INT = (SELECT IdProveedor FROM PROVEEDOR WHERE CUIT = '30-70894042-7');
DECLARE @IdBaggio INT = (SELECT IdProveedor FROM PROVEEDOR WHERE CUIT = '30-50013003-4');

INSERT INTO CATALOGO_PROVEEDOR (IdProveedor, IdProducto, NombreArticuloProveedor, PrecioPallet, UnidadesPorPallet) VALUES 
(@IdCoca, '7790895000997', 'Pallet Coca-Cola Original 2.25L (40 packs x 6)', 360000.00, 240),
(@IdCoca, '7790895001999', 'Pallet Coca-Cola Lata 473ml (100 packs x 6)', 450000.00, 600),
(@IdPepsi, '7791813421112', 'Pallet 7Up Regular 1.5L (60 packs x 6)', 300000.00, 360),
(@IdPepsi, '7791813421051', 'Pallet Paso de los Toros Pomelo 1.5L (60 packs x 6)', 280000.00, 360),
(@IdManaos, '7798099881014', 'Pallet Manaos Cola 2.25L (50 packs x 6)', 200000.00, 300),
(@IdManaos, '7798099881021', 'Pallet Manaos Lima Limón 2.25L (50 packs x 6)', 200000.00, 300),
(@IdManaos, '7798099881038', 'Pallet Manaos Pomelo 2.25L (50 packs x 6)', 200000.00, 300),
(@IdBaggio, '7790503000001', 'Pallet Baggio Multifruta 1L (80 cajas x 8)', 400000.00, 640),
(@IdBaggio, '7790503000002', 'Pallet Baggio Naranja 1L (80 cajas x 8)', 400000.00, 640),
(@IdCoca, '7790895003333', 'Pallet Sprite Regular 2.25L (40 packs x 6)', 340000.00, 240),
(@IdCoca, '7790895004444', 'Pallet Fanta Naranja 2.25L (40 packs x 6)', 340000.00, 240),
(@IdCoca, '7790895005555', 'Pallet Coca-Cola Zero 2.25L (40 packs x 6)', 360000.00, 240),
(@IdPepsi, '7791813421129', 'Pallet Pepsi Black 1.5L (60 packs x 6)', 300000.00, 360),
(@IdManaos, '7798099881045', 'Pallet Manaos Naranja 2.25L (50 packs x 6)', 200000.00, 300),
(@IdManaos, '7798099881052', 'Pallet Manaos Pomelo Blanco 2.25L (50 packs x 6)', 200000.00, 300),
(@IdBaggio, '7790503000003', 'Pallet Baggio Manzana 1L (80 cajas x 8)', 400000.00, 640);

GO

-- 1. Creamos una Orden y Recepción de "Ajuste de Inventario"
INSERT INTO ORDEN_COMPRA (IdProveedor, FechaEmision, Estado) VALUES (1, GETDATE(), 'Pagada');
DECLARE @IdOrdenAjuste INT = SCOPE_IDENTITY();

INSERT INTO RECEPCION (IdOrden, NumeroRemito, FechaIngreso) VALUES (@IdOrdenAjuste, 'REMITO-AJUSTE-INICIAL', GETDATE());
DECLARE @IdRecepcionAjuste INT = SCOPE_IDENTITY();

-- 2. Migramos el stock suelto a Lotes formales
INSERT INTO LOTE_BEBIDA (IdRecepcion, IdProducto, NumeroLote, FechaVencimiento, CantidadRecibida, StockActual)
SELECT 
    @IdRecepcionAjuste, 
    IdProducto, 
    'LT-INICIAL-' + CodigoSKU,           -- Genera lotes como "LT-INICIAL-SKU-011"
    DATEADD(month, 3, GETDATE()),        -- Les da 6 meses de vida útil por defecto
    StockActual,                         -- La cantidad original ingresada
    StockActual
FROM PRODUCTO 
WHERE StockActual > 0 
  AND IdProducto NOT IN (SELECT IdProducto FROM LOTE_BEBIDA); -- Solo a los que no tienen lote
GO
-- =========================================================================
-- FASE 4: INSERCIÓN DE IDIOMAS Y TRADUCCIONES
-- =========================================================================

INSERT INTO Idioma (Codigo, Nombre) VALUES ('ES', 'Español');
INSERT INTO Idioma (Codigo, Nombre) VALUES ('EN', 'English');
INSERT INTO Idioma (Codigo, Nombre) VALUES ('PT', 'Português');

-- =========================================================
-- TRADUCCIONES AL ESPAÑOL (ES)
-- =========================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto)
SELECT CodigoIdioma, KeyEtiqueta, Texto FROM (VALUES 
('ES', 'MainUI', 'Sistema de gestion'),
('ES', 'mainUIStripMenuItemCerrarSesion', 'Cerrar sesión'),
('ES', 'mainUIStripMenuItemIniciarSesion', 'Iniciar sesión'),
('ES', 'mainUIStripMenuItemGestionDeUsuarios', 'Gestión de usuarios'),
('ES', 'mainUIStripMenuItemABMUsuarios', 'ABM Usuarios'),
('ES', 'mainUIStripMenuItemDesbloqueoUsuarios', 'Desploqueo de usuarios'),
('ES', 'mainUIStripMenuItemGestionDePerfiles', 'Gestión de perfiles'),
('ES', 'mainUIStripMenuItemABMPerfiles', 'Alta y asignación de perfiles'),
('ES', 'mainUIStripMenuItemInicio', 'Inicio'),
('ES', 'mainUIStripMenuItemBitacora', 'Bitacora'),
('ES', 'mainUIStripMenuItemConsultarBitacora', 'Consultar bitacora'),
('ES', 'mainUIStripMenuItemPerfiles', 'Perfiles'),
('ES', 'mainUIStripMenuItemGestionarPerfiles', 'Gestionar perfiles'),
('ES', 'label1', 'Idioma'),
('ES', 'gestionUsuariosUIGroupBoxAltaUsuario', 'Registrar usuario'),
('ES', 'gestionUsuariosUIRegistroLabelUsername', 'Username'),
('ES', 'gestionUsuariosUIRegistroLabelEmail', 'Email'),
('ES', 'gestionUsuariosUIRegistroLabelNumTelefono', 'Número de telefono'),
('ES', 'gestionUsuariosUIRegistroLabelContrasena', 'Contraseña'),
('ES', 'gestionUsuariosUIRegistroLabelConfirmContrasena', 'Repetir Contraseña'),
('ES', 'gestionUsuariosUIButtonConfirmarRegistrarUsuario', 'Confirmar registro'),
('ES', 'gestionUsuariosUIGroupBoxListadoUsuarios', 'Listado de usuarios'),
('ES', 'gestionUsuariosUIGroupBoxModificacionUsuarios', 'Modificar usuario seleccionado'),
('ES', 'gestionUsuariosUIButtonConfirmarEliminarUsuario', 'Eliminar usuario seleccionado'),
('ES', 'gestionUsuariosUIModificacionLabelEmail', 'Email'),
('ES', 'gestionUsuariosUIModificacionLabelNumTelefono', 'Número de telefono'),
('ES', 'gestionUsuariosUIModificacionButtonConfirmarModificar', 'Confirmar modificación'),
('ES', 'loginUILabelUsername', 'Usuario'),
('ES', 'loginUILabelContrasena', 'Contraseña'),
('ES', 'loginUIButtonIniciarSesion', 'Iniciar sesión'),
('ES', 'perfilesUIGroupBoxTreeView', 'Arbol de perfiles y permisos'),
('ES', 'perfilesUILabelNombrePerfil', 'Nombre del nuevo perfil'),
('ES', 'perfilesUIButtonCrearPerfil', 'Crear perfil'),
('ES', 'perfilesUIGroupBoxListBoxPerfiles', 'Perfiles disponibles'),
('ES', 'perfilUIButtonAsignarPerfil', 'Asignar perfil a perfil'),
('ES', 'perfilesUIGroupBoxUsuarios', 'Usuarios disponibles'),
('ES', 'perfilUIButtonAsignarPerfilUsuario', 'Asignar perfil a usuario'),
('ES', 'perfilUIButtonDesasignarPerfilUsuario', 'Desasignar perfil a usuario'),
('ES', 'perfilesUIGroupBoxListBoxPermisos', 'Permisos disponibles'),
('ES', 'perfilUIButtonAsignarPermiso', 'Asignar permiso a perfil'),
('ES', 'bitacoraUILabelGrid', 'Registros de la bitacora'),
('ES', 'bitacoraUILabelComboBoxAccion', 'Filtrado por acción'),
('ES', 'bitacoraUILabelComboBoxUsername', 'Filtrado por username'),
('ES', 'bitacoraUIButtonLimpiarFiltros', 'Limpiar filtros'),
('ES', 'msg_InicioSesionExito', 'Inicio de sesión exitoso.'),
('ES', 'msg_TituloExito', 'Éxito'),
('ES', 'msg_CierreSesionExito', 'Sesión cerrada correctamente.'),
('ES', 'msg_TituloCierreSesion', 'Cerrar sesión'),
('ES', 'err_MaxIntentos', 'Ha superado los 3 intentos fallidos. Su cuenta ha sido bloqueada por seguridad.'),
('ES', 'err_QuedanIntentos', 'Contraseña incorrecta. Le quedan {0} intentos antes de bloquearse.'),
('ES', 'err_NoUserLogout', 'Usuario activo no encontrado en logout.'),
('ES', 'log_InicioSesion', 'Inicio de Sesion'),
('ES', 'log_CierreSesion', 'Cierre de Sesion'),
('ES', 'err_UsuarioIncorrecto', 'Usuario incorrecto'),
('ES', 'err_UsuarioBloqueado', 'El usuario se encuentra bloqueado.'),
('ES', 'err_EmailVacio', 'El correo electrónico no puede estar vacío'),
('ES', 'err_EmailFormato', 'El formato del correo electrónico no es válido'),
('ES', 'err_UsernameVacio', 'El nombre de usuario no puede estar vacio'),
('ES', 'err_UsernameFormato', 'El nombre de usuario debe tener entre 3 y 16 caracteres y solo puede contener letras, números, guiones bajos y guiones'),
('ES', 'err_PhoneVacio', 'El número de teléfono no puede estar vacío'),
('ES', 'err_PhoneFormato', 'El formato del número de teléfono no es válido'),
('ES', 'err_OnlyLettersVacio', 'El campo de texto no puede estar vacío'),
('ES', 'err_OnlyLettersFormato', 'El campo solo puede contener letras y espacios (se permiten acentos y eñes)'),
('ES', 'err_AlphaNumStrictVacio', 'El código o ID no puede estar vacío'),
('ES', 'err_AlphaNumStrictFormato', 'El campo solo puede contener letras (sin acentos) y números, sin espacios'),
('ES', 'err_AlphaNumSpacesVacio', 'El texto no puede estar vacío'),
('ES', 'err_AlphaNumSpacesFormato', 'El campo solo puede contener letras, números y espacios (sin caracteres especiales)'),
('ES', 'err_PassVacia', 'La contraseña no puede estar vacía.'),
('ES', 'err_PassNoCoincide', 'Las contraseñas no coinciden.'),
('ES', 'err_NoUserModificar', 'No se ha seleccionado ningún usuario para modificar.'),
('ES', 'err_NoUserEliminar', 'No se ha seleccionado ningún usuario para eliminar.'),
('ES', 'msg_TituloError', 'Error'),
('ES', 'btnDesbloquear', 'Desbloquear usuario seleccionado'),
('ES', 'err_NoUserDesbloquear', 'No se ha seleccionado ningún usuario para desbloquear.'),
('ES', 'msg_DesbloqueoExito', 'Usuario desbloqueado correctamente.'),
('ES', 'LOG_LOGIN', 'Inició sesión en el sistema'),
('ES', 'LOG_LOGOUT', 'Cerró sesión'),
('ES', 'LOG_USER_ADD', 'Registró a un nuevo usuario'),
('ES', 'LOG_USER_MOD', 'Modificó los datos de un usuario'),
('ES', 'LOG_USER_DEL', 'Eliminó a un usuario del sistema'),
('ES', 'LOG_PERFIL_ADD', 'Asignó un perfil a un usuario'),
('ES', 'LOG_PERMISOS_MOD', 'Modificó permisos del sistema'),
('ES', 'GridBitacora_Usuario', 'Usuario'),
('ES', 'GridBitacora_Fecha', 'Fecha y Hora'),
('ES', 'GridBitacora_Accion', 'Acción Realizada'),
('ES', 'gestionHistorialUILabelGridUsuarios', 'Usuarios disponibles'),
('ES', 'gestionHistorialUILabelGridEstadoUsuarios', 'Historial del usuario seleccionado'),
('ES', 'mainUIStripMenuItemHistorialUsuario', 'Historial usuario'),
('ES', 'gestionHistorialUIButtonRecuperarEstado', 'Recuperar estado'),
('ES', 'agregarIdiomaToolStripMenuItem', 'Agregar Idioma'),
('ES', 'GestionIdiomasUI', 'Configuración de Nuevos Idiomas'),
('ES', 'labelCodigo', 'Código (Ej: FR):'),
('ES', 'labelNombre', 'Nombre Idioma:'),
('ES', 'btnGuardarIdioma', 'Guardar Idioma'),
('ES', 'GridIdioma_ColKey', 'Componente / Etiqueta'),
('ES', 'GridIdioma_ColRef', 'Referencia (Español)'),
('ES', 'GridIdioma_ColNuevo', 'Nueva Traducción'),
('ES', 'msg_IdiomaGuardadoExito', 'El idioma y sus respectivas traducciones se han guardado exitosamente.'),
('ES', 'err_CodigoNombreObligatorios', 'El código y el nombre del idioma son obligatorios.'),
('ES', 'err_IdiomaYaExiste', 'El código de idioma ya se encuentra registrado en el sistema.'),
('ES', 'err_TraduccionObligatoria', 'Debe proveer al menos una traducción para el nuevo idioma.'),
('ES', 'err_CargarEtiquetas', 'Error al cargar etiquetas de referencia: '),
('ES', 'recuperarIntegridadToolStripMenuItem', 'Recuperar integridad'),
('ES', 'menuStripItemAlmacen', 'Almacén'),
('ES', 'almacenToolStripMenuItemGenerarSolicitud', 'Generar Solicitud de Abastecimiento'),
('ES', 'menuStripItemCompras', 'Compras'),
('ES', 'comprasToolStripMenuItemEmitirOC', 'Orden de Compra'),
('ES', 'menuStripItemContabilidad', 'Contabilidad'),
('ES', 'contabilidadToolStripMenuItemEvaluarCotiz', 'Evaluar Cotización'),
('ES', 'Agregar', 'Agregar'),
('ES', 'btn_Solicitud_Abastecimiento', 'Confirmar Solicitud'),
('ES', 'lblSolicitudesPendientes', 'Solicitudes Pendientes de Revisión:'),
('ES', 'lblDetalleProductos', 'Productos Requeridos en la Solicitud'),
('ES', 'btnVolver', 'Volver'),
('ES', 'btnAtenderSolicitud', 'Atender Solicitud y Emitir Orden'),
('ES', 'lblFaltantesSolicitados', 'FALTANTES SOLICITADOS'),
('ES', 'lblCatalogo', 'CATÁLOGO PROVEEDORES DIRECTOS'),
('ES', 'lblOrdenCompra', 'ORDEN DE COMPRA'),
('ES', 'lblCantidadPallets', 'Cantidad Pallets:'),
('ES', 'lblTotalOC', 'Total Neto:'),
('ES', 'btnCancelarOC', 'Volver'),
('ES', 'btnEmitirOrden', 'Emitir Orden Compra'),
('ES', 'btnAgregarCarrito', 'Agregar al Carrito'),
('ES', 'grid_CodigoBarras', 'Código de Barras'),
('ES', 'grid_Producto', 'Producto'),
('ES', 'grid_StockActual', 'Stock Actual'),
('ES', 'grid_PuntoPedido', 'Punto de Pedido'),
('ES', 'grid_ProdFaltante', 'Producto Faltante'),
('ES', 'grid_CantSugerida', 'Cant. Sugerida'),
('ES', 'grid_FechaSol', 'Fecha de Solicitud'),
('ES', 'grid_SolicitadoPor', 'Solicitado por'),
('ES', 'grid_NumSolicitud', 'N° Solicitud'),
('ES', 'grid_SKU', 'SKU / Código'),
('ES', 'msg_Atencion', 'Atención'),
('ES', 'msg_SeleccioneProdAbastecimiento', 'Por favor, seleccione un producto del inventario que necesite reabastecimiento.'),
('ES', 'msg_ProdYaEnFaltantes', 'Este producto ya está en la lista actual de faltantes.'),
('ES', 'msg_ProdYaEnCompras', 'Este producto ya se encuentra en una solicitud anterior pendiente de revisión por Compras.'),
('ES', 'msg_NoHayProductosSolicitar', 'No hay productos en la lista para solicitar.'),
('ES', 'msg_SolicitudEnviadaExito', 'Solicitud enviada a Compras exitosamente.'),
('ES', 'msg_CantidadMayorCero', 'La cantidad debe ser mayor a 0.'),
('ES', 'msg_SeleccioneProdCatalogo', 'Seleccione un producto del catálogo.'),
('ES', 'msg_CarritoVacioPallets', 'El carrito está vacío. Agregue pallets antes de emitir la orden.'),
('ES', 'msg_Validacion', 'Validación'),
('ES', 'msg_OrdenEmitidaExito', 'Orden de Compra generada y enviada a Contabilidad exitosamente.'),
('ES', 'msg_ExitoB2B', 'Éxito B2B'),
('ES', 'EvaluarCotizacionUI', 'Evaluación de Cotizaciones y Pagos'),
('ES', 'btnEfectuarPago', 'Efectuar Pago a Proveedor'),
('ES', 'msg_ExitoContable', 'Éxito Contable'),
('ES', 'msg_PagoExitoso', 'Transferencia realizada y Orden de Compra saldada.'),
('ES', 'msg_SeleccioneOrden', 'Seleccione una orden para pagar.'),
('ES', 'grid_NumOrden', 'N° Orden'),
('ES', 'grid_FechaEmision', 'Fecha de Emisión'),
('ES', 'grid_EstadoOC', 'Estado Actual'),
('ES', 'grid_MontoUnitario', 'Monto Unitario'),
('ES', 'grid_Subtotal', 'Subtotal'),
('ES', 'grid_TotalCorte', '>>> TOTAL DE LA ORDEN <<<'),
('ES', 'msg_MontoCero', 'El monto de la orden es 0. Verifique los detalles.'),
('ES', 'PuntoVentaUI', 'Punto de Venta'),
('ES', 'GenerarOrdenCompraUI', 'Generar Orden de Compra'),
('ES', 'GenerarSolicitudUI', 'Generar Solicitud de Abastecimiento'),
('ES', 'BandejaSolicitudesUI', 'Bandeja de Solicitudes'),
('ES', 'BitacoraUI', 'Bitácora del Sistema'),
('ES', 'LoginUI', 'Inicio de Sesión'),
('ES', 'lblCliente', 'Cliente:'),
('ES', 'lblCantidad', 'Cantidad:'),
('ES', 'lblMetodoPago', 'Método de Pago:'),
('ES', 'btnConfirmarVenta', 'Cobrar y Emitir Ticket'),
('ES', 'lblTotalVenta', 'TOTAL:'),
('ES', 'pago_Efectivo', 'Efectivo'),
('ES', 'pago_Transferencia', 'Transferencia'),
('ES', 'pago_Debito', 'Tarjeta de Débito'),
('ES', 'pago_Credito', 'Tarjeta de Crédito'),
('ES', 'msg_StockInsuficiente', 'No hay suficiente stock. Disponible: {0}'),
('ES', 'msg_CarritoVacio', 'El carrito está vacío.'),
('ES', 'msg_VentaExitosa', 'Venta registrada exitosamente.'),
('ES', 'err_IntegridadCorrupta', 'Alerta Crítica: Se ha detectado una violación en la integridad de la base de datos. El sistema ha entrado en Modo de Recuperación. Solo los administradores pueden iniciar sesión.'),
('ES', 'err_SoloAdminIntegridad', 'Acceso denegado. El sistema se encuentra bloqueado por fallas de integridad. Solo el administrador puede acceder.'),
('ES', 'msg_RecuperacionExito', 'Integridad de la base de datos recuperada correctamente.'),
('ES', 'grid_IdProveedor', 'ID Proveedor'),
('ES', 'grid_Articulo', 'Artículo'),
('ES', 'grid_PrecioPallet', 'Precio Pallet'),
('ES', 'grid_UnidadesPallet', 'Unidades por Pallet'),
('ES', 'grid_PrecioAcordado', 'Precio Acordado'),
('ES', 'grid_FechaGeneracion', 'Fecha de Generación'),
('ES', 'grid_Estado', 'Estado'),
('ES', 'grid_Cantidad', 'Cantidad'),
('ES', 'estado_PendienteCompras', 'Pendiente de Compras'),
('ES', 'btnQuitar', 'Quitar del Carrito'),
('ES', 'msg_SeleccioneItemCarrito', 'Seleccione un producto de la grilla para quitar.'),
('ES', 'err_JerarquiaInvalida', 'Un perfil de menor rango ({0}) no puede contener a un perfil de mayor rango ({1}).'),
('ES', 'lbl_IdiomaGenerador', 'Idioma:')
) AS DatosNuevos(CodigoIdioma, KeyEtiqueta, Texto)
WHERE NOT EXISTS (
    SELECT 1 FROM Traduccion T 
    WHERE T.CodigoIdioma = DatosNuevos.CodigoIdioma 
    AND T.KeyEtiqueta = DatosNuevos.KeyEtiqueta
);

-- =========================================================
-- TRADUCCIONES AL INGLÉS (EN)
-- =========================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto)
SELECT CodigoIdioma, KeyEtiqueta, Texto FROM (VALUES 
('EN', 'MainUI', 'Management System'),
('EN', 'mainUIStripMenuItemCerrarSesion', 'Logout'),
('EN', 'mainUIStripMenuItemIniciarSesion', 'Login'),
('EN', 'mainUIStripMenuItemGestionDeUsuarios', 'User Management'),
('EN', 'mainUIStripMenuItemABMUsuarios', 'CRUD Users'),
('EN', 'mainUIStripMenuItemDesbloqueoUsuarios', 'Unlock Users'),
('EN', 'mainUIStripMenuItemGestionDePerfiles', 'Profile Management'),
('EN', 'mainUIStripMenuItemABMPerfiles', 'Profile Creation & Assignment'),
('EN', 'mainUIStripMenuItemInicio', 'Home'),
('EN', 'mainUIStripMenuItemBitacora', 'Logbook'),
('EN', 'mainUIStripMenuItemConsultarBitacora', 'View Logbook'),
('EN', 'mainUIStripMenuItemPerfiles', 'Profiles'),
('EN', 'mainUIStripMenuItemGestionarPerfiles', 'Manage Profiles'),
('EN', 'label1', 'Language'),
('EN', 'gestionUsuariosUIGroupBoxAltaUsuario', 'Register User'),
('EN', 'gestionUsuariosUIRegistroLabelUsername', 'Username'),
('EN', 'gestionUsuariosUIRegistroLabelEmail', 'Email'),
('EN', 'gestionUsuariosUIRegistroLabelNumTelefono', 'Phone Number'),
('EN', 'gestionUsuariosUIRegistroLabelContrasena', 'Password'),
('EN', 'gestionUsuariosUIRegistroLabelConfirmContrasena', 'Repeat Password'),
('EN', 'gestionUsuariosUIButtonConfirmarRegistrarUsuario', 'Confirm Registration'),
('EN', 'gestionUsuariosUIGroupBoxListadoUsuarios', 'User List'),
('EN', 'gestionUsuariosUIGroupBoxModificacionUsuarios', 'Modify Selected User'),
('EN', 'gestionUsuariosUIButtonConfirmarEliminarUsuario', 'Delete Selected User'),
('EN', 'gestionUsuariosUIModificacionLabelEmail', 'Email'),
('EN', 'gestionUsuariosUIModificacionLabelNumTelefono', 'Phone Number'),
('EN', 'gestionUsuariosUIModificacionButtonConfirmarModificar', 'Confirm Modification'),
('EN', 'loginUILabelUsername', 'Username'),
('EN', 'loginUILabelContrasena', 'Password'),
('EN', 'loginUIButtonIniciarSesion', 'Login'),
('EN', 'perfilesUIGroupBoxTreeView', 'Profiles and Permissions Tree'),
('EN', 'perfilesUILabelNombrePerfil', 'New Profile Name'),
('EN', 'perfilesUIButtonCrearPerfil', 'Create Profile'),
('EN', 'perfilesUIGroupBoxListBoxPerfiles', 'Available Profiles'),
('EN', 'perfilUIButtonAsignarPerfil', 'Assign Profile to Profile'),
('EN', 'perfilesUIGroupBoxUsuarios', 'Available Users'),
('EN', 'perfilUIButtonAsignarPerfilUsuario', 'Assign Profile to User'),
('EN', 'perfilUIButtonDesasignarPerfilUsuario', 'Unassign Profile from User'),
('EN', 'perfilesUIGroupBoxListBoxPermisos', 'Available Permissions'),
('EN', 'perfilUIButtonAsignarPermiso', 'Assign Permission to Profile'),
('EN', 'bitacoraUILabelGrid', 'Binnacle entries'),
('EN', 'bitacoraUILabelComboBoxAccion', 'Filter by action'),
('EN', 'bitacoraUILabelComboBoxUsername', 'Filter by username'),
('EN', 'bitacoraUIButtonLimpiarFiltros', 'Clean filters'),
('EN', 'msg_InicioSesionExito', 'Successful login.'),
('EN', 'msg_TituloExito', 'Success'),
('EN', 'msg_CierreSesionExito', 'Session closed successfully.'),
('EN', 'msg_TituloCierreSesion', 'Logout'),
('EN', 'err_MaxIntentos', 'Maximum failed attempts exceeded. Your account has been locked for security.'),
('EN', 'err_QuedanIntentos', 'Incorrect password. You have {0} attempts left before being locked.'),
('EN', 'err_NoUserLogout', 'Active user not found on logout.'),
('EN', 'log_InicioSesion', 'Login'),
('EN', 'log_CierreSesion', 'Logout'),
('EN', 'err_UsuarioIncorrecto', 'Incorrect user'),
('EN', 'err_UsuarioBloqueado', 'The user is locked.'),
('EN', 'err_EmailVacio', 'Email cannot be empty'),
('EN', 'err_EmailFormato', 'Invalid email format'),
('EN', 'err_UsernameVacio', 'Username cannot be empty'),
('EN', 'err_UsernameFormato', 'Username must be between 3 and 16 characters and can only contain letters, numbers, underscores, and hyphens'),
('EN', 'err_PhoneVacio', 'Phone number cannot be empty'),
('EN', 'err_PhoneFormato', 'Invalid phone number format'),
('EN', 'err_OnlyLettersVacio', 'The text field cannot be empty'),
('EN', 'err_OnlyLettersFormato', 'The field can only contain letters and spaces (accents and ñ are allowed)'),
('EN', 'err_AlphaNumStrictVacio', 'Code or ID cannot be empty'),
('EN', 'err_AlphaNumStrictFormato', 'The field can only contain letters (no accents) and numbers, without spaces'),
('EN', 'err_AlphaNumSpacesVacio', 'Text cannot be empty'),
('EN', 'err_AlphaNumSpacesFormato', 'The field can only contain letters, numbers, and spaces (no special characters)'),
('EN', 'err_PassVacia', 'Password cannot be empty.'),
('EN', 'err_PassNoCoincide', 'Passwords do not match.'),
('EN', 'err_NoUserModificar', 'No user selected to modify.'),
('EN', 'err_NoUserEliminar', 'No user selected to delete.'),
('EN', 'msg_TituloError', 'Error'),
('EN', 'btnDesbloquear', 'Unlock selected user'),
('EN', 'err_NoUserDesbloquear', 'No user selected to unlock.'),
('EN', 'msg_DesbloqueoExito', 'User unlocked successfully.'),
('EN', 'LOG_LOGIN', 'Logged into the system'),
('EN', 'LOG_LOGOUT', 'Logged out'),
('EN', 'LOG_USER_ADD', 'Registered a new user'),
('EN', 'LOG_USER_MOD', 'Modified user details'),
('EN', 'LOG_USER_DEL', 'Deleted a user from the system'),
('EN', 'LOG_PERFIL_ADD', 'Assigned a profile to a user'),
('EN', 'LOG_PERMISOS_MOD', 'Modified system permissions'),
('EN', 'GridBitacora_Usuario', 'User'),
('EN', 'GridBitacora_Fecha', 'Date and Time'),
('EN', 'GridBitacora_Accion', 'Action Performed'),
('EN', 'gestionHistorialUILabelGridUsuarios', 'Available users'),
('EN', 'gestionHistorialUILabelGridEstadoUsuarios', 'Selected user history'),
('EN', 'mainUIStripMenuItemHistorialUsuario', 'User history'),
('EN', 'gestionHistorialUIButtonRecuperarEstado', 'Restore state'),
('EN', 'agregarIdiomaToolStripMenuItem', 'Add Language'),
('EN', 'GestionIdiomasUI', 'New Languages Configuration'),
('EN', 'labelCodigo', 'Code (e.g., FR):'),
('EN', 'labelNombre', 'Language Name:'),
('EN', 'btnGuardarIdioma', 'Save Language'),
('EN', 'GridIdioma_ColKey', 'Component / Label'),
('EN', 'GridIdioma_ColRef', 'Reference (Spanish)'),
('EN', 'GridIdioma_ColNuevo', 'New Translation'),
('EN', 'msg_IdiomaGuardadoExito', 'The language and its translations have been saved successfully.'),
('EN', 'err_CodigoNombreObligatorios', 'Language code and name are required.'),
('EN', 'err_IdiomaYaExiste', 'The language code is already registered in the system.'),
('EN', 'err_TraduccionObligatoria', 'You must provide at least one translation for the new language.'),
('EN', 'err_CargarEtiquetas', 'Error loading reference labels: '),
('EN', 'recuperarIntegridadToolStripMenuItem', 'Recover integrity'),
('EN', 'menuStripItemAlmacen', 'Warehouse'),
('EN', 'almacenToolStripMenuItemGenerarSolicitud', 'Generate Supply Request'),
('EN', 'menuStripItemCompras', 'Purchasing'),
('EN', 'comprasToolStripMenuItemEmitirOC', 'Purchase Order'),
('EN', 'menuStripItemContabilidad', 'Accounting'),
('EN', 'contabilidadToolStripMenuItemEvaluarCotiz', 'Evaluate Quote'),
('EN', 'Agregar', 'Add'),
('EN', 'btn_Solicitud_Abastecimiento', 'Confirm Request'),
('EN', 'lblSolicitudesPendientes', 'Pending Requests for Review:'),
('EN', 'lblDetalleProductos', 'Products Required in Request'),
('EN', 'btnVolver', 'Back'),
('EN', 'btnAtenderSolicitud', 'Handle Request & Issue Order'),
('EN', 'lblFaltantesSolicitados', 'REQUESTED SHORTAGES'),
('EN', 'lblCatalogo', 'DIRECT SUPPLIERS CATALOG'),
('EN', 'lblOrdenCompra', 'PURCHASE ORDER'),
('EN', 'lblCantidadPallets', 'Pallet Quantity:'),
('EN', 'lblTotalOC', 'Net Total:'),
('EN', 'btnCancelarOC', 'Back'),
('EN', 'btnEmitirOrden', 'Issue Purchase Order'),
('EN', 'btnAgregarCarrito', 'Add to Cart'),
('EN', 'grid_CodigoBarras', 'Barcode'),
('EN', 'grid_Producto', 'Product'),
('EN', 'grid_StockActual', 'Current Stock'),
('EN', 'grid_PuntoPedido', 'Reorder Point'),
('EN', 'grid_ProdFaltante', 'Missing Product'),
('EN', 'grid_CantSugerida', 'Suggested Qty'),
('EN', 'grid_FechaSol', 'Request Date'),
('EN', 'grid_SolicitadoPor', 'Requested by'),
('EN', 'grid_NumSolicitud', 'Request No.'),
('EN', 'grid_SKU', 'SKU / Code'),
('EN', 'msg_Atencion', 'Warning'),
('EN', 'msg_SeleccioneProdAbastecimiento', 'Please select an inventory product that needs restocking.'),
('EN', 'msg_ProdYaEnFaltantes', 'This product is already in the current shortage list.'),
('EN', 'msg_ProdYaEnCompras', 'This product is already in a pending request under Purchasing review.'),
('EN', 'msg_NoHayProductosSolicitar', 'No products in the list to request.'),
('EN', 'msg_SolicitudEnviadaExito', 'Request successfully sent to Purchasing.'),
('EN', 'msg_CantidadMayorCero', 'Quantity must be greater than 0.'),
('EN', 'msg_SeleccioneProdCatalogo', 'Select a product from the catalog.'),
('EN', 'msg_CarritoVacioPallets', 'Cart is empty. Add pallets before issuing the order.'),
('EN', 'msg_Validacion', 'Validation'),
('EN', 'msg_OrdenEmitidaExito', 'Purchase Order successfully generated and sent to Accounting.'),
('EN', 'msg_ExitoB2B', 'B2B Success'),
('EN', 'EvaluarCotizacionUI', 'Quote Evaluation and Payments'),
('EN', 'btnEfectuarPago', 'Make Payment to Supplier'),
('EN', 'msg_ExitoContable', 'Accounting Success'),
('EN', 'msg_PagoExitoso', 'Transfer completed and Purchase Order settled.'),
('EN', 'msg_SeleccioneOrden', 'Select an order to pay.'),
('EN', 'grid_NumOrden', 'Order No.'),
('EN', 'grid_FechaEmision', 'Issue Date'),
('EN', 'grid_EstadoOC', 'Current Status'),
('EN', 'grid_MontoUnitario', 'Unit Amount'),
('EN', 'grid_Subtotal', 'Subtotal'),
('EN', 'grid_TotalCorte', '>>> ORDER TOTAL <<<'),
('EN', 'msg_MontoCero', 'The order amount is 0. Please verify details.'),
('EN', 'PuntoVentaUI', 'Point of Sale'),
('EN', 'GenerarOrdenCompraUI', 'Generate Purchase Order'),
('EN', 'GenerarSolicitudUI', 'Generate Supply Request'),
('EN', 'BandejaSolicitudesUI', 'Requests Inbox'),
('EN', 'BitacoraUI', 'System Logbook'),
('EN', 'LoginUI', 'Login'),
('EN', 'lblCliente', 'Customer:'),
('EN', 'lblCantidad', 'Quantity:'),
('EN', 'lblMetodoPago', 'Payment Method:'),
('EN', 'btnConfirmarVenta', 'Charge and Issue Ticket'),
('EN', 'lblTotalVenta', 'TOTAL:'),
('EN', 'pago_Efectivo', 'Cash'),
('EN', 'pago_Transferencia', 'Bank Transfer'),
('EN', 'pago_Debito', 'Debit Card'),
('EN', 'pago_Credito', 'Credit Card'),
('EN', 'msg_StockInsuficiente', 'Not enough stock. Available: {0}'),
('EN', 'msg_CarritoVacio', 'The cart is empty.'),
('EN', 'msg_VentaExitosa', 'Sale registered successfully.'),
('EN', 'err_IntegridadCorrupta', 'Critical Alert: A database integrity violation has been detected. The system has entered Recovery Mode. Only administrators can log in.'),
('EN', 'err_SoloAdminIntegridad', 'Access denied. The system is locked due to integrity failures. Only the administrator can access.'),
('EN', 'msg_RecuperacionExito', 'Database integrity recovered successfully.'),
('EN', 'grid_IdProveedor', 'Supplier ID'),
('EN', 'grid_Articulo', 'Article'),
('EN', 'grid_PrecioPallet', 'Pallet Price'),
('EN', 'grid_UnidadesPallet', 'Units per Pallet'),
('EN', 'grid_PrecioAcordado', 'Agreed Price'),
('EN', 'grid_FechaGeneracion', 'Generation Date'),
('EN', 'grid_Estado', 'Status'),
('EN', 'grid_Cantidad', 'Quantity'),
('EN', 'estado_PendienteCompras', 'Pending Purchasing'),
('EN', 'btnQuitar', 'Remove from Cart'),
('EN', 'msg_SeleccioneItemCarrito', 'Select a product from the grid to remove.'),
('EN', 'err_JerarquiaInvalida', 'A lower-ranking profile ({0}) cannot contain a higher-ranking profile ({1}).'),
('EN', 'lbl_IdiomaGenerador', 'Language:')
) AS DatosNuevos(CodigoIdioma, KeyEtiqueta, Texto)
WHERE NOT EXISTS (
    SELECT 1 FROM Traduccion T 
    WHERE T.CodigoIdioma = DatosNuevos.CodigoIdioma 
    AND T.KeyEtiqueta = DatosNuevos.KeyEtiqueta
);

-- =========================================================
-- TRADUCCIONES AL PORTUGUÉS (PT)
-- =========================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto)
SELECT CodigoIdioma, KeyEtiqueta, Texto FROM (VALUES 
('PT', 'MainUI', 'Sistema de Gestão'),
('PT', 'mainUIStripMenuItemCerrarSesion', 'Sair'),
('PT', 'mainUIStripMenuItemIniciarSesion', 'Entrar'),
('PT', 'mainUIStripMenuItemGestionDeUsuarios', 'Gestão de Usuários'),
('PT', 'mainUIStripMenuItemABMUsuarios', 'CRUD de Usuários'),
('PT', 'mainUIStripMenuItemDesbloqueoUsuarios', 'Desbloquear Usuários'),
('PT', 'mainUIStripMenuItemGestionDePerfiles', 'Gestão de Perfil'),
('PT', 'mainUIStripMenuItemABMPerfiles', 'Criação e Atribuição de Perfil'),
('PT', 'mainUIStripMenuItemInicio', 'Início'),
('PT', 'mainUIStripMenuItemBitacora', 'Livro de Bordo'),
('PT', 'mainUIStripMenuItemConsultarBitacora', 'Visualizar Livro de Bordo'),
('PT', 'mainUIStripMenuItemPerfiles', 'Perfis'),
('PT', 'mainUIStripMenuItemGestionarPerfiles', 'Gerenciar Perfis'),
('PT', 'label1', 'Idioma'),
('PT', 'gestionUsuariosUIGroupBoxAltaUsuario', 'Registrar Usuário'),
('PT', 'gestionUsuariosUIRegistroLabelUsername', 'Nome de usuário'),
('PT', 'gestionUsuariosUIRegistroLabelEmail', 'E-mail'),
('PT', 'gestionUsuariosUIRegistroLabelNumTelefono', 'Número de Telefone'),
('PT', 'gestionUsuariosUIRegistroLabelContrasena', 'Senha'),
('PT', 'gestionUsuariosUIRegistroLabelConfirmContrasena', 'Repetir Senha'),
('PT', 'gestionUsuariosUIButtonConfirmarRegistrarUsuario', 'Confirmar Registro'),
('PT', 'gestionUsuariosUIGroupBoxListadoUsuarios', 'Lista de Usuários'),
('PT', 'gestionUsuariosUIGroupBoxModificacionUsuarios', 'Modificar Usuário Selecionado'),
('PT', 'gestionUsuariosUIButtonConfirmarEliminarUsuario', 'Excluir Usuário Selecionado'),
('PT', 'gestionUsuariosUIModificacionLabelEmail', 'E-mail'),
('PT', 'gestionUsuariosUIModificacionLabelNumTelefono', 'Número de Telefone'),
('PT', 'gestionUsuariosUIModificacionButtonConfirmarModificar', 'Confirmar Modificação'),
('PT', 'loginUILabelUsername', 'Nome de usuário'),
('PT', 'loginUILabelContrasena', 'Senha'),
('PT', 'loginUIButtonIniciarSesion', 'Entrar'),
('PT', 'perfilesUIGroupBoxTreeView', 'Árvore de Perfis e Permissões'),
('PT', 'perfilesUILabelNombrePerfil', 'Nome do Novo Perfil'),
('PT', 'perfilesUIButtonCrearPerfil', 'Criar Perfil'),
('PT', 'perfilesUIGroupBoxListBoxPerfiles', 'Perfis Disponíveis'),
('PT', 'perfilUIButtonAsignarPerfil', 'Atribuir Perfil a Perfil'),
('PT', 'perfilesUIGroupBoxUsuarios', 'Usuários Disponíveis'),
('PT', 'perfilUIButtonAsignarPerfilUsuario', 'Atribuir Perfil a Usuário'),
('PT', 'perfilUIButtonDesasignarPerfilUsuario', 'Remover Perfil do Usuário'),
('PT', 'perfilesUIGroupBoxListBoxPermisos', 'Permissões Disponíveis'),
('PT', 'perfilUIButtonAsignarPermiso', 'Atribuir Permissão ao Perfil'),
('PT', 'bitacoraUILabelGrid', 'Registros de Borda'),
('PT', 'bitacoraUILabelComboBoxAccion', 'Filtrar por ação'),
('PT', 'bitacoraUILabelComboBoxUsername', 'Filtrar por nome de usuário'),
('PT', 'bitacoraUIButtonLimpiarFiltros', 'Limpar filtros'),
('PT', 'msg_InicioSesionExito', 'Login bem-sucedido.'),
('PT', 'msg_TituloExito', 'Sucesso'),
('PT', 'msg_CierreSesionExito', 'Sessão encerrada com sucesso.'),
('PT', 'msg_TituloCierreSesion', 'Sair'),
('PT', 'err_MaxIntentos', 'Limite de tentativas excedido. Sua conta foi bloqueada por segurança.'),
('PT', 'err_QuedanIntentos', 'Senha incorreta. Você tem {0} tentativas restantes antes de ser bloqueado.'),
('PT', 'err_NoUserLogout', 'Usuário ativo não encontrado no logout.'),
('PT', 'log_InicioSesion', 'Início de Sessão'),
('PT', 'log_CierreSesion', 'Encerramento de Sessão'),
('PT', 'err_UsuarioIncorrecto', 'Usuário incorreto'),
('PT', 'err_UsuarioBloqueado', 'O usuário está bloqueado.'),
('PT', 'err_EmailVacio', 'O e-mail não pode estar vazio'),
('PT', 'err_EmailFormato', 'Formato de e-mail inválido'),
('PT', 'err_UsernameVacio', 'O nome de usuário não pode estar vazio'),
('PT', 'err_UsernameFormato', 'O nome de usuário deve ter entre 3 e 16 caracteres e só pode conter letras, números, sublinhados e hifens'),
('PT', 'err_PhoneVacio', 'O número de telefone não pode estar vazio'),
('PT', 'err_PhoneFormato', 'Formato de número de telefone inválido'),
('PT', 'err_OnlyLettersVacio', 'O campo de texto não pode estar vazio'),
('PT', 'err_OnlyLettersFormato', 'O campo só pode conter letras e espaços (acentos e cedilhas são permitidos)'),
('PT', 'err_AlphaNumStrictVacio', 'O código ou ID não pode estar vazio'),
('PT', 'err_AlphaNumStrictFormato', 'O campo só pode conter letras (sem acentos) e números, sem espaços'),
('PT', 'err_AlphaNumSpacesVacio', 'O texto não pode estar vazio'),
('PT', 'err_AlphaNumSpacesFormato', 'O campo só pode conter letras, números e espaços (sem caracteres especiais)'),
('PT', 'err_PassVacia', 'A senha não pode estar vazia.'),
('PT', 'err_PassNoCoincide', 'As senhas não coincidem.'),
('PT', 'err_NoUserModificar', 'Nenhum usuário selecionado para modificar.'),
('PT', 'err_NoUserEliminar', 'Nenhum usuário selecionado para excluir.'),
('PT', 'msg_TituloError', 'Erro'),
('PT', 'btnDesbloquear', 'Desbloquear usuário selecionado'),
('PT', 'err_NoUserDesbloquear', 'Nenhum usuário selecionado para desbloquear.'),
('PT', 'msg_DesbloqueoExito', 'Usuário desbloqueado com sucesso.'),
('PT', 'LOG_LOGIN', 'Entrou no sistema'),
('PT', 'LOG_LOGOUT', 'Saiu do sistema'),
('PT', 'LOG_USER_ADD', 'Registrou um novo usuário'),
('PT', 'LOG_USER_MOD', 'Modificou os dados de um usuário'),
('PT', 'LOG_USER_DEL', 'Excluiu um usuário do sistema'),
('PT', 'LOG_PERFIL_ADD', 'Atribuiu um perfil a um usuário'),
('PT', 'LOG_PERMISOS_MOD', 'Modificou as permissões do sistema'),
('PT', 'GridBitacora_Usuario', 'Usuário'),
('PT', 'GridBitacora_Fecha', 'Data e Hora'),
('PT', 'GridBitacora_Accion', 'Ação Realizada'),
('PT', 'gestionHistorialUILabelGridUsuarios', 'Usuários disponíveis'),
('PT', 'gestionHistorialUILabelGridEstadoUsuarios', 'Histórico do usuário selecionado'),
('PT', 'mainUIStripMenuItemHistorialUsuario', 'Histórico do usuário'),
('PT', 'gestionHistorialUIButtonRecuperarEstado', 'Restaurar estado'),
('PT', 'agregarIdiomaToolStripMenuItem', 'Adicionar Idioma'),
('PT', 'GestionIdiomasUI', 'Configuração de Novos Idiomas'),
('PT', 'labelCodigo', 'Código (Ex: FR):'),
('PT', 'labelNombre', 'Nome do Idioma:'),
('PT', 'btnGuardarIdioma', 'Salvar Idioma'),
('PT', 'GridIdioma_ColKey', 'Componente / Rótulo'),
('PT', 'GridIdioma_ColRef', 'Referência (Espanhol)'),
('PT', 'GridIdioma_ColNuevo', 'Nova Tradução'),
('PT', 'msg_IdiomaGuardadoExito', 'O idioma e suas traduções foram salvos com sucesso.'),
('PT', 'err_CodigoNombreObligatorios', 'O código e o nome do idioma são obrigatórios.'),
('PT', 'err_IdiomaYaExiste', 'O código do idioma já está registrado no sistema.'),
('PT', 'err_TraduccionObligatoria', 'Você deve fornecer pelo menos uma tradução para o novo idioma.'),
('PT', 'err_CargarEtiquetas', 'Erro ao carregar rótulos de referência: '),
('PT', 'recuperarIntegridadToolStripMenuItem', 'Recuperar integridade'),
('PT', 'menuStripItemAlmacen', 'Armazém'),
('PT', 'almacenToolStripMenuItemGenerarSolicitud', 'Gerar Solicitação de Abastecimento'),
('PT', 'menuStripItemCompras', 'Compras'),
('PT', 'comprasToolStripMenuItemEmitirOC', 'Ordem de Compra'),
('PT', 'menuStripItemContabilidad', 'Contabilidade'),
('PT', 'contabilidadToolStripMenuItemEvaluarCotiz', 'Avaliar Cotação'),
('PT', 'Agregar', 'Adicionar'),
('PT', 'btn_Solicitud_Abastecimiento', 'Confirmar Solicitação'),
('PT', 'lblSolicitudesPendientes', 'Solicitações Pendentes de Revisão:'),
('PT', 'lblDetalleProductos', 'Produtos Requeridos na Solicitação'),
('PT', 'btnVolver', 'Voltar'),
('PT', 'btnAtenderSolicitud', 'Atender Solicitação e Emitir Ordem'),
('PT', 'lblFaltantesSolicitados', 'FALTAS SOLICITADAS'),
('PT', 'lblCatalogo', 'CATÁLOGO DE FORNECEDORES DIRETOS'),
('PT', 'lblOrdenCompra', 'ORDEM DE COMPRA'),
('PT', 'lblCantidadPallets', 'Quantidade de Paletes:'),
('PT', 'lblTotalOC', 'Total Líquido:'),
('PT', 'btnCancelarOC', 'Voltar'),
('PT', 'btnEmitirOrden', 'Emitir Ordem de Compra'),
('PT', 'btnAgregarCarrito', 'Adicionar ao Carrinho'),
('PT', 'grid_CodigoBarras', 'Código de Barras'),
('PT', 'grid_Producto', 'Produto'),
('PT', 'grid_StockActual', 'Estoque Atual'),
('PT', 'grid_PuntoPedido', 'Ponto de Pedido'),
('PT', 'grid_ProdFaltante', 'Produto Faltante'),
('PT', 'grid_CantSugerida', 'Qtd. Sugerida'),
('PT', 'grid_FechaSol', 'Data da Solicitação'),
('PT', 'grid_SolicitadoPor', 'Solicitado por'),
('PT', 'grid_NumSolicitud', 'N° Solicitação'),
('PT', 'grid_SKU', 'SKU / Código'),
('PT', 'msg_Atencion', 'Atenção'),
('PT', 'msg_SeleccioneProdAbastecimiento', 'Por favor, selecione um produto do estoque que precise de reabastecimento.'),
('PT', 'msg_ProdYaEnFaltantes', 'Este produto já está na lista atual de faltas.'),
('PT', 'msg_ProdYaEnCompras', 'Este produto já se encontra numa solicitação anterior pendente de revisão por Compras.'),
('PT', 'msg_NoHayProductosSolicitar', 'Não há produtos na lista para solicitar.'),
('PT', 'msg_SolicitudEnviadaExito', 'Solicitação enviada a Compras com sucesso.'),
('PT', 'msg_CantidadMayorCero', 'A quantidade deve ser maior que 0.'),
('PT', 'msg_SeleccioneProdCatalogo', 'Selecione um produto do catálogo.'),
('PT', 'msg_CarritoVacioPallets', 'O carrinho está vazio. Adicione paletes antes de emitir a ordem.'),
('PT', 'msg_Validacion', 'Validação'),
('PT', 'msg_OrdenEmitidaExito', 'Ordem de Compra gerada e enviada à Contabilidade com sucesso.'),
('PT', 'msg_ExitoB2B', 'Sucesso B2B'),
('PT', 'EvaluarCotizacionUI', 'Avaliação de Cotações e Pagamentos'),
('PT', 'btnEfectuarPago', 'Efetuar Pagamento ao Fornecedor'),
('PT', 'msg_ExitoContable', 'Sucesso Contábil'),
('PT', 'msg_PagoExitoso', 'Transferência realizada e Ordem de Compra liquidada.'),
('PT', 'msg_SeleccioneOrden', 'Selecione uma ordem para pagar.'),
('PT', 'grid_NumOrden', 'N° Ordem'),
('PT', 'grid_FechaEmision', 'Data de Emissão'),
('PT', 'grid_EstadoOC', 'Status Atual'),
('PT', 'grid_MontoUnitario', 'Valor Unitário'),
('PT', 'grid_Subtotal', 'Subtotal'),
('PT', 'grid_TotalCorte', '>>> TOTAL DA ORDEM <<<'),
('PT', 'msg_MontoCero', 'O valor da ordem é 0. Verifique os detalhes.'),
('PT', 'PuntoVentaUI', 'Ponto de Venda'),
('PT', 'GenerarOrdenCompraUI', 'Gerar Ordem de Compra'),
('PT', 'GenerarSolicitudUI', 'Gerar Solicitação de Abastecimento'),
('PT', 'BandejaSolicitudesUI', 'Caixa de Solicitações'),
('PT', 'BitacoraUI', 'Livro de Bordo do Sistema'),
('PT', 'LoginUI', 'Entrar'),
('PT', 'lblCliente', 'Cliente:'),
('PT', 'lblCantidad', 'Quantidade:'),
('PT', 'lblMetodoPago', 'Método de Pagamento:'),
('PT', 'btnConfirmarVenta', 'Cobrar e Emitir Recibo'),
('PT', 'lblTotalVenta', 'TOTAL:'),
('PT', 'pago_Efectivo', 'Dinheiro'),
('PT', 'pago_Transferencia', 'Transferência'),
('PT', 'pago_Debito', 'Cartão de Débito'),
('PT', 'pago_Credito', 'Cartão de Crédito'),
('PT', 'msg_StockInsuficiente', 'Estoque insuficiente. Disponível: {0}'),
('PT', 'msg_CarritoVacio', 'O carrinho está vazio.'),
('PT', 'msg_VentaExitosa', 'Venda registrada com sucesso.'),
('PT', 'err_IntegridadCorrupta', 'Alerta Crítico: Foi detectada uma violação de integridade no banco de dados. O sistema entrou no Modo de Recuperação. Apenas administradores podem entrar.'),
('PT', 'err_SoloAdminIntegridad', 'Acesso negado. O sistema está bloqueado devido a falhas de integridade. Apenas o administrador tem acesso.'),
('PT', 'msg_RecuperacionExito', 'Integridade do banco de dados recuperada com sucesso.'),
('PT', 'grid_IdProveedor', 'ID Fornecedor'),
('PT', 'grid_Articulo', 'Artigo'),
('PT', 'grid_PrecioPallet', 'Preço Palete'),
('PT', 'grid_UnidadesPallet', 'Unidades por Palete'),
('PT', 'grid_PrecioAcordado', 'Preço Acordado'),
('PT', 'grid_FechaGeneracion', 'Data de Geração'),
('PT', 'grid_Estado', 'Status'),
('PT', 'grid_Cantidad', 'Quantidade'),
('PT', 'estado_PendienteCompras', 'Pendente de Compras'),
('PT', 'btnQuitar', 'Remover do Carrinho'),
('PT', 'msg_SeleccioneItemCarrito', 'Selecione um produto da grade para remover.'),
('PT', 'err_JerarquiaInvalida', 'Um perfil de classificação inferior ({0}) não pode conter um perfil de classificação superior ({1}).'),
('PT', 'lbl_IdiomaGenerador', 'Idioma:')
) AS DatosNuevos(CodigoIdioma, KeyEtiqueta, Texto)
WHERE NOT EXISTS (
    SELECT 1 FROM Traduccion T 
    WHERE T.CodigoIdioma = DatosNuevos.CodigoIdioma 
    AND T.KeyEtiqueta = DatosNuevos.KeyEtiqueta
);
GO
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES 
('ES', 'GestionProductosUI', 'Gestión de Productos (ABM)'),
('ES', 'gbxFormularioABM', 'Datos del Producto'),
('ES', 'gbxListadoProductos', 'Catálogo Activo'),
('ES', 'lblCodigoBarra', 'Código de Barras (SKU):'),
('ES', 'lblNombreProd', 'Nombre del Producto:'),
('ES', 'lblPrecioUnitario', 'Precio Unitario local:'),
('ES', 'lblPuntoPedidoAbm', 'Punto de Pedido (Alerta):'),
('ES', 'btnGuardar', 'Guardar'),
('ES', 'btnEliminar', 'Dar de Baja'),
('ES', 'btnLimpiar', 'Limpiar Campos'),
('ES', 'err_CamposVacios', 'Todos los campos obligatorios deben estar completos.'),
('ES', 'err_PrecioInvalido', 'El precio debe ser mayor a cero.'),
('ES', 'msg_ProductoModificado', 'Producto modificado exitosamente.'),
('ES', 'msg_ProductoRegistrado', 'Producto registrado exitosamente.'),
('ES', 'msg_ConfirmarBajaProd', '¿Está seguro que desea dar de baja este producto del catálogo?'),
('ES', 'msg_ProductoBajaExitosa', 'Producto dado de baja exitosamente.');

-- TRADUCCIONES PARA EL ABM DE PRODUCTOS (INGLÉS)
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES 
('EN', 'GestionProductosUI', 'Product Management (CRUD)'),
('EN', 'gbxFormularioABM', 'Product Details'),
('EN', 'gbxListadoProductos', 'Active Catalog'),
('EN', 'lblCodigoBarra', 'Barcode (SKU):'),
('EN', 'lblNombreProd', 'Product Name:'),
('EN', 'lblPrecioUnitario', 'Local Unit Price:'),
('EN', 'lblPuntoPedidoAbm', 'Reorder Point (Alert):'),
('EN', 'btnGuardar', 'Save'),
('EN', 'btnEliminar', 'Deactivate'),
('EN', 'btnLimpiar', 'Clear Fields'),
('EN', 'err_CamposVacios', 'All required fields must be filled.'),
('EN', 'err_PrecioInvalido', 'The price must be greater than zero.'),
('EN', 'msg_ProductoModificado', 'Product modified successfully.'),
('EN', 'msg_ProductoRegistrado', 'Product registered successfully.'),
('EN', 'msg_ConfirmarBajaProd', 'Are you sure you want to deactivate this product from the catalog?'),
('EN', 'msg_ProductoBajaExitosa', 'Product deactivated successfully.');

-- TRADUCCIONES PARA EL ABM DE PRODUCTOS (PORTUGUÉS)
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto) VALUES 
('PT', 'GestionProductosUI', 'Gestão de Produtos (CRUD)'),
('PT', 'gbxFormularioABM', 'Detalhes do Produto'),
('PT', 'gbxListadoProductos', 'Catálogo Ativo'),
('PT', 'lblCodigoBarra', 'Código de Barras (SKU):'),
('PT', 'lblNombreProd', 'Nome do Produto:'),
('PT', 'lblPrecioUnitario', 'Preço Unitário local:'),
('PT', 'lblPuntoPedidoAbm', 'Ponto de Pedido (Alerta):'),
('PT', 'btnGuardar', 'Salvar'),
('PT', 'btnEliminar', 'Desativar'),
('PT', 'btnLimpiar', 'Limpar Campos'),
('PT', 'err_CamposVacios', 'Todos os campos obrigatórios devem ser preenchidos.'),
('PT', 'err_PrecioInvalido', 'O preço deve ser maior que zero.'),
('PT', 'msg_ProductoModificado', 'Produto modificado com sucesso.'),
('PT', 'msg_ProductoRegistrado', 'Produto registrado com sucesso.'),
('PT', 'msg_ConfirmarBajaProd', 'Tem certeza de que deseja desativar este produto do catálogo?'),
('PT', 'msg_ProductoBajaExitosa', 'Produto desativado com sucesso.');


GO

-- Variables de control para el bucle
DECLARE @TotalVentas INT = 200;
DECLARE @i INT = 1;

-- Obtenemos los ID de los vendedores
DECLARE @IdVendedor1 UNIQUEIDENTIFIER = (SELECT ID FROM Usuario WHERE Username = 'vendedor1');
DECLARE @IdVendedor2 UNIQUEIDENTIFIER = (SELECT ID FROM Usuario WHERE Username = 'vendedor2');

-- Obtenemos el ID del Consumidor Final (Por orden de inserción en tu script, es el IdCliente = 1)
DECLARE @IdClienteConsumidorFinal INT = (SELECT TOP 1 IdCliente FROM CLIENTE WHERE RazonSocial = 'Consumidor Final');

-- Tabla temporal en memoria para facilitar la selección aleatoria de productos cargados
DECLARE @Productos TABLE (RowId INT IDENTITY(1,1), IdProducto VARCHAR(50), Precio DECIMAL(18,2));
INSERT INTO @Productos (IdProducto, Precio) 
SELECT IdProducto, PrecioUnitarioLocal FROM PRODUCTO WHERE Activo = 1;

DECLARE @MaxProd INT = (SELECT COUNT(*) FROM @Productos);

BEGIN TRANSACTION;

WHILE @i <= @TotalVentas
BEGIN
    -- 1. Alternar entre vendedor 1 y vendedor 2
    DECLARE @VendedorActual UNIQUEIDENTIFIER = CASE WHEN @i % 2 = 0 THEN @IdVendedor1 ELSE @IdVendedor2 END;
    
    -- Generar una fecha aleatoria en los últimos 30 días para darle realismo a la bitácora
    DECLARE @DiasAtras INT = (ABS(CHECKSUM(NEWID())) % 30);
    DECLARE @HorasAtras INT = (ABS(CHECKSUM(NEWID())) % 24);
    DECLARE @FechaVenta DATETIME = DATEADD(HOUR, -@HorasAtras, DATEADD(DAY, -@DiasAtras, GETDATE()));

    -- 2. Crear Venta Cabecera (Inicializamos MontoTotal en 0)
    INSERT INTO VENTA (IdCliente, IdUsuario, FechaVenta, MontoTotal, Estado)
    VALUES (@IdClienteConsumidorFinal, @VendedorActual, @FechaVenta, 0, 'Pagada');
    
    DECLARE @IdVentaActual INT = SCOPE_IDENTITY();
    
    -- 3. Generar Detalles de Venta (entre 1 y 3 productos distintos por venta)[cite: 1]
    DECLARE @CantProductos INT = (ABS(CHECKSUM(NEWID())) % 3) + 1;
    DECLARE @j INT = 1;
    DECLARE @TotalVentaActual DECIMAL(18,2) = 0;
    
    WHILE @j <= @CantProductos
    BEGIN
        -- Seleccionar un producto aleatorio
        DECLARE @RandomProdIndex INT = (ABS(CHECKSUM(NEWID())) % @MaxProd) + 1;
        DECLARE @IdProdSeleccionado VARCHAR(50);
        DECLARE @PrecioProdSeleccionado DECIMAL(18,2);
        
        SELECT @IdProdSeleccionado = IdProducto, @PrecioProdSeleccionado = Precio 
        FROM @Productos WHERE RowId = @RandomProdIndex;
        
        -- Comprar entre 1 y 5 unidades de este producto
        DECLARE @CantidadComprada INT = (ABS(CHECKSUM(NEWID())) % 5) + 1; 
        DECLARE @Subtotal DECIMAL(18,2) = @CantidadComprada * @PrecioProdSeleccionado;
        
        -- Insertar Detalle[cite: 1]
        INSERT INTO DETALLE_VENTA (IdVenta, IdProducto, Cantidad, PrecioUnitario, Subtotal)
        VALUES (@IdVentaActual, @IdProdSeleccionado, @CantidadComprada, @PrecioProdSeleccionado, @Subtotal);
        
        SET @TotalVentaActual = @TotalVentaActual + @Subtotal;
        SET @j = @j + 1;
    END
    
    -- 4. Actualizar el Monto Total final de la Venta[cite: 1]
    UPDATE VENTA 
    SET MontoTotal = @TotalVentaActual 
    WHERE IdVenta = @IdVentaActual;
    
    -- 5. Registrar Cobro en Efectivo[cite: 1]
    INSERT INTO COBRO (IdVenta, FechaCobro, MontoCobrado, MetodoPago, NumeroTarjeta, Cuotas, NumeroComprobante)
    VALUES (@IdVentaActual, @FechaVenta, @TotalVentaActual, 'Efectivo', NULL, 1, 'TKT-FINAL-' + CAST(@IdVentaActual AS VARCHAR));
    
    SET @i = @i + 1;
END

COMMIT TRANSACTION;
PRINT '200 Ventas generadas exitosamente.';
GO


BEGIN TRANSACTION;

-- 1. Crear una Orden de Compra para justificar el ingreso de estos lotes
-- Usamos el IdProveedor = 1 (Coca-Cola FEMSA, según tu script) como ejemplo general
INSERT INTO ORDEN_COMPRA (IdProveedor, FechaEmision, Estado) 
VALUES (1, GETDATE(), 'Pagada');

DECLARE @IdOrdenVencimiento INT = SCOPE_IDENTITY();

-- 2. Crear la Recepción asociada a esa Orden de Compra
INSERT INTO RECEPCION (IdOrden, NumeroRemito, FechaIngreso) 
VALUES (@IdOrdenVencimiento, 'REMITO-TEST-VENC', GETDATE());

DECLARE @IdRecepcionVencimiento INT = SCOPE_IDENTITY();

-- 3. Calcular la fecha de vencimiento (20 días a futuro)
DECLARE @FechaVenc DATETIME = DATEADD(DAY, 20, GETDATE());

-- 4. Insertar los lotes utilizando productos reales de tu tabla PRODUCTO
INSERT INTO LOTE_BEBIDA (IdRecepcion, IdProducto, NumeroLote, FechaVencimiento, CantidadRecibida, StockActual) 
VALUES 
-- Lote 1: Coca-Cola Lata 473ml
(@IdRecepcionVencimiento, '7790895001999', 'LT-VENC20-001', @FechaVenc, 200, 200),

-- Lote 2: Manaos Pomelo 2.25L
(@IdRecepcionVencimiento, '7798099881038', 'LT-VENC20-002', @FechaVenc, 150, 120),

-- Lote 3: Baggio Multifruta 1L
(@IdRecepcionVencimiento, '7790503000001', 'LT-VENC20-003', @FechaVenc, 80, 80),

-- Lote 4: 7Up Regular 1.5L
(@IdRecepcionVencimiento, '7791813421112', 'LT-VENC20-004', @FechaVenc, 100, 45),

-- Lote 5: Coca-Cola Zero 2.25L
(@IdRecepcionVencimiento, '7790895005555', 'LT-VENC20-005', @FechaVenc, 60, 60),

-- Lote 6: Manaos Naranja 2.25L
(@IdRecepcionVencimiento, '7798099881045', 'LT-VENC20-006', @FechaVenc, 300, 250),

-- Lote 7: Baggio Manzana 1L
(@IdRecepcionVencimiento, '7790503000003', 'LT-VENC20-007', @FechaVenc, 50, 50);

COMMIT TRANSACTION;
PRINT '7 lotes próximos a vencer generados exitosamente.';
GO

-- =========================================================
-- TRADUCCIONES AL ESPAÑOL (ES) - Nuevos Módulos
-- =========================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto)
SELECT CodigoIdioma, KeyEtiqueta, Texto FROM (VALUES 
('ES', 'VisorTicketsUI_Titulo', 'Visor de Tickets Emitidos'),
('ES', 'msg_ArchivoNoExiste', 'El archivo físico ya no existe o fue movido.'),
('ES', 'grid_NombreArchivo', 'Nombre del Archivo'),
('ES', 'grid_FechaEmision', 'Fecha de Emisión'),
('ES', 'FormMockTarjetaUI', 'Pago con Tarjeta'),
('ES', 'msg_DatosTarjetaVacios', 'Complete los datos obligatorios (Titular y Número).'),
('ES', 'lblMontoTarjeta', 'Monto a cobrar:'),
('ES', 'lblTitular', 'Nombre del Titular:'),
('ES', 'lblNumeroTarjeta', 'Número de Tarjeta:'),
('ES', 'lblVencimiento', 'Vencimiento:'),
('ES', 'lblCVV', 'Código CVV:'),
('ES', 'grpTipoTarjeta', 'Tipo de Tarjeta'),
('ES', 'lblCuotas', 'Cuotas:'),
('ES', 'btnAceptar', 'Aceptar'),
('ES', 'btnCancelar', 'Cancelar'),
('ES', 'HistorialVentasUI', 'Historial de Ventas'),
('ES', 'btnActualizar', 'Actualizar Historial'),
('ES', 'grid_FechaVenta', 'Fecha'),
('ES', 'grid_Cliente', 'Cliente'),
('ES', 'grid_Vendedor', 'Vendedor'),
('ES', 'grid_MontoTotal', 'Monto Total'),
('ES', 'grid_PagosCombinados', 'Detalle de Pagos'),
('ES', 'GestionOfertasInteligentesUI', 'Gestor de Promociones Inteligentes'),
('ES', 'lblTituloUI_Ofertas', 'Bandeja de Recomendaciones (FEFO)'),
('ES', 'btnEjecutarMotor', 'Ejecutar Motor Analítico'),
('ES', 'lblTituloAjuste', 'Ajustar Decisión de la Propuesta:'),
('ES', 'btnAprobarOferta', 'Aprobar Oferta'),
('ES', 'btnDescartarOferta', 'Descartar'),
('ES', 'msg_MotorCompletado', 'El motor analítico ha terminado de procesar los lotes y el historial de ventas.'),
('ES', 'msg_TituloCompletado', 'Análisis Completado'),
('ES', 'grid_IdOferta', 'ID'),
('ES', 'grid_PropuestaComercial', 'Propuesta Comercial'),
('ES', 'grid_DescuentoSugerido', 'Descuento'),
('ES', 'lbl_SeleccionePropuesta', 'Seleccione una propuesta de la grilla...'),
('ES', 'msg_OfertaAprobada', 'Oferta Aprobada con {0}% de descuento. Ya está disponible para los vendedores.'),
('ES', 'msg_TituloOfertaActiva', 'Oferta Activa'),
('ES', 'msg_OfertaDescartada', 'La sugerencia del motor ha sido descartada.'),
('ES', 'msg_TituloOfertaDescartada', 'Oferta Rechazada'),
('ES', 'frmAuditoria_Titulo', 'Gestión y Auditoría de Ofertas'),
('ES', 'btnEliminarOfertaAudit', 'Eliminar Oferta Seleccionada'),
('ES', 'msg_ConfirmarEliminarOferta', '¿Está seguro que desea eliminar definitivamente la oferta #{0} para el producto {1}?'),
('ES', 'msg_OfertaEliminadaExito', 'Oferta eliminada de la base de datos.'),
('ES', 'ProveedoresUI_Text', 'ABM Proveedores (JSON)'),
('ES', 'ProveedoresUI_Titulo', 'Gestión de Proveedores (Persistencia JSON)'),
('ES', 'lblCUIT', 'CUIT'),
('ES', 'lblRazonSocial', 'Razón Social'),
('ES', 'lblCondicion', 'Condición Comercial'),
('ES', 'btnGuardarProv', 'Guardar Proveedor'),
('ES', 'btnLimpiarSeleccion', 'Limpiar Selección'),
('ES', 'btnEliminarProv', 'Eliminar Proveedor'),
('ES', 'btnMigrarSQL', 'Importar Datos de Prueba (SQL -> JSON)'),
('ES', 'msg_ProvGuardadoExito', 'Proveedor guardado en el archivo JSON exitosamente.'),
('ES', 'msg_ProvActualizadoExito', 'Proveedor actualizado en el archivo JSON exitosamente.'),
('ES', 'msg_ConfirmarBajaProv', '¿Está seguro que desea dar de baja este proveedor?'),
('ES', 'msg_SelecProvEliminar', 'Seleccione un proveedor de la grilla para eliminar.'),
('ES', 'msg_AvisoMigracion', '¿Desea importar los proveedores de la Base de Datos SQL al archivo JSON?' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + '¡ATENCIÓN! Esto sobrescribirá los datos del archivo local actual.'),
('ES', 'msg_TituloMigracion', 'Migración SQL -> JSON'),
('ES', 'msg_ExitoMigracion', '¡Migración completada! Los proveedores de prueba ahora viven en el archivo JSON.'),
('ES', 'msg_ErrorMigracion', 'Error de Migración')
) AS DatosNuevos(CodigoIdioma, KeyEtiqueta, Texto)
WHERE NOT EXISTS (
    SELECT 1 FROM Traduccion T 
    WHERE T.CodigoIdioma = DatosNuevos.CodigoIdioma 
    AND T.KeyEtiqueta = DatosNuevos.KeyEtiqueta
);

-- =========================================================
-- TRADUCCIONES AL INGLÉS (EN) - Nuevos Módulos
-- =========================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto)
SELECT CodigoIdioma, KeyEtiqueta, Texto FROM (VALUES 
('EN', 'VisorTicketsUI_Titulo', 'Issued Tickets Viewer'),
('EN', 'msg_ArchivoNoExiste', 'The physical file no longer exists or was moved.'),
('EN', 'grid_NombreArchivo', 'File Name'),
('EN', 'grid_FechaEmision', 'Issue Date'),
('EN', 'FormMockTarjetaUI', 'Credit/Debit Card Payment'),
('EN', 'msg_DatosTarjetaVacios', 'Please fill in the required fields (Cardholder and Number).'),
('EN', 'lblMontoTarjeta', 'Amount to charge:'),
('EN', 'lblTitular', 'Cardholder Name:'),
('EN', 'lblNumeroTarjeta', 'Card Number:'),
('EN', 'lblVencimiento', 'Expiration Date:'),
('EN', 'lblCVV', 'CVV Code:'),
('EN', 'grpTipoTarjeta', 'Card Type'),
('EN', 'lblCuotas', 'Installments:'),
('EN', 'btnAceptar', 'Accept'),
('EN', 'btnCancelar', 'Cancel'),
('EN', 'HistorialVentasUI', 'Sales History'),
('EN', 'btnActualizar', 'Update History'),
('EN', 'grid_FechaVenta', 'Date'),
('EN', 'grid_Cliente', 'Customer'),
('EN', 'grid_Vendedor', 'Seller'),
('EN', 'grid_MontoTotal', 'Total Amount'),
('EN', 'grid_PagosCombinados', 'Payment Details'),
('EN', 'GestionOfertasInteligentesUI', 'Smart Promotions Manager'),
('EN', 'lblTituloUI_Ofertas', 'Recommendations Inbox (FEFO)'),
('EN', 'btnEjecutarMotor', 'Execute Analytical Engine'),
('EN', 'lblTituloAjuste', 'Adjust Proposal Decision:'),
('EN', 'btnAprobarOferta', 'Approve Offer'),
('EN', 'btnDescartarOferta', 'Discard'),
('EN', 'msg_MotorCompletado', 'The analytical engine has finished processing the batches and sales history.'),
('EN', 'msg_TituloCompletado', 'Analysis Completed'),
('EN', 'grid_IdOferta', 'ID'),
('EN', 'grid_PropuestaComercial', 'Commercial Proposal'),
('EN', 'grid_DescuentoSugerido', 'Discount'),
('EN', 'lbl_SeleccionePropuesta', 'Select a proposal from the grid...'),
('EN', 'msg_OfertaAprobada', 'Offer Approved with a {0}% discount. It is now available to sellers.'),
('EN', 'msg_TituloOfertaActiva', 'Active Offer'),
('EN', 'msg_OfertaDescartada', 'The engine''s suggestion has been discarded.'),
('EN', 'msg_TituloOfertaDescartada', 'Offer Rejected'),
('EN', 'frmAuditoria_Titulo', 'Offer Management & Auditing'),
('EN', 'btnEliminarOfertaAudit', 'Delete Selected Offer'),
('EN', 'msg_ConfirmarEliminarOferta', 'Are you sure you want to permanently delete offer #{0} for product {1}?'),
('EN', 'msg_OfertaEliminadaExito', 'Offer successfully deleted from the database.'),
('EN', 'ProveedoresUI_Text', 'Supplier CRUD (JSON)'),
('EN', 'ProveedoresUI_Titulo', 'Supplier Management (JSON Persistence)'),
('EN', 'lblCUIT', 'Tax ID (CUIT)'),
('EN', 'lblRazonSocial', 'Company Name'),
('EN', 'lblCondicion', 'Commercial Condition'),
('EN', 'btnGuardarProv', 'Save Supplier'),
('EN', 'btnLimpiarSeleccion', 'Clear Selection'),
('EN', 'btnEliminarProv', 'Delete Supplier'),
('EN', 'btnMigrarSQL', 'Import Test Data (SQL -> JSON)'),
('EN', 'msg_ProvGuardadoExito', 'Supplier successfully saved in the JSON file.'),
('EN', 'msg_ProvActualizadoExito', 'Supplier successfully updated in the JSON file.'),
('EN', 'msg_ConfirmarBajaProv', 'Are you sure you want to deactivate this supplier?'),
('EN', 'msg_SelecProvEliminar', 'Please select a supplier from the grid to delete.'),
('EN', 'msg_AvisoMigracion', 'Do you want to import suppliers from the SQL Database to the JSON file?' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + 'WARNING! This will overwrite the current local file data.'),
('EN', 'msg_TituloMigracion', 'SQL -> JSON Migration'),
('EN', 'msg_ExitoMigracion', 'Migration completed! Test suppliers now live in the JSON file.'),
('EN', 'msg_ErrorMigracion', 'Migration Error')
) AS DatosNuevos(CodigoIdioma, KeyEtiqueta, Texto)
WHERE NOT EXISTS (
    SELECT 1 FROM Traduccion T 
    WHERE T.CodigoIdioma = DatosNuevos.CodigoIdioma 
    AND T.KeyEtiqueta = DatosNuevos.KeyEtiqueta
);

-- =========================================================
-- TRADUCCIONES AL PORTUGUÉS (PT) - Nuevos Módulos
-- =========================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto)
SELECT CodigoIdioma, KeyEtiqueta, Texto FROM (VALUES 
('PT', 'VisorTicketsUI_Titulo', 'Visualizador de Tickets Emitidos'),
('PT', 'msg_ArchivoNoExiste', 'O arquivo físico não existe mais ou foi movido.'),
('PT', 'grid_NombreArchivo', 'Nome do Arquivo'),
('PT', 'grid_FechaEmision', 'Data de Emissão'),
('PT', 'FormMockTarjetaUI', 'Pagamento com Cartão'),
('PT', 'msg_DatosTarjetaVacios', 'Preencha os dados obrigatórios (Titular e Número).'),
('PT', 'lblMontoTarjeta', 'Valor a cobrar:'),
('PT', 'lblTitular', 'Nome do Titular:'),
('PT', 'lblNumeroTarjeta', 'Número do Cartão:'),
('PT', 'lblVencimiento', 'Data de Validade:'),
('PT', 'lblCVV', 'Código CVV:'),
('PT', 'grpTipoTarjeta', 'Tipo de Cartão'),
('PT', 'lblCuotas', 'Parcelas:'),
('PT', 'btnAceptar', 'Aceitar'),
('PT', 'btnCancelar', 'Cancelar'),
('PT', 'HistorialVentasUI', 'Histórico de Vendas'),
('PT', 'btnActualizar', 'Atualizar Histórico'),
('PT', 'grid_FechaVenta', 'Data'),
('PT', 'grid_Cliente', 'Cliente'),
('PT', 'grid_Vendedor', 'Vendedor'),
('PT', 'grid_MontoTotal', 'Valor Total'),
('PT', 'grid_PagosCombinados', 'Detalhes de Pagamento'),
('PT', 'GestionOfertasInteligentesUI', 'Gestor de Promoções Inteligentes'),
('PT', 'lblTituloUI_Ofertas', 'Caixa de Recomendações (FEFO)'),
('PT', 'btnEjecutarMotor', 'Executar Motor Analítico'),
('PT', 'lblTituloAjuste', 'Ajustar Decisão da Proposta:'),
('PT', 'btnAprobarOferta', 'Aprovar Oferta'),
('PT', 'btnDescartarOferta', 'Descartar'),
('PT', 'msg_MotorCompletado', 'O motor analítico terminou de processar os lotes e o histórico de vendas.'),
('PT', 'msg_TituloCompletado', 'Análise Concluída'),
('PT', 'grid_IdOferta', 'ID'),
('PT', 'grid_PropuestaComercial', 'Proposta Comercial'),
('PT', 'grid_DescuentoSugerido', 'Desconto'),
('PT', 'lbl_SeleccionePropuesta', 'Selecione uma proposta na grade...'),
('PT', 'msg_OfertaAprobada', 'Oferta Aprovada com {0}% de desconto. Já está disponível para os vendedores.'),
('PT', 'msg_TituloOfertaActiva', 'Oferta Ativa'),
('PT', 'msg_OfertaDescartada', 'A sugestão do motor foi descartada.'),
('PT', 'msg_TituloOfertaDescartada', 'Oferta Rejeitada'),
('PT', 'frmAuditoria_Titulo', 'Gestão e Auditoria de Ofertas'),
('PT', 'btnEliminarOfertaAudit', 'Excluir Oferta Selecionada'),
('PT', 'msg_ConfirmarEliminarOferta', 'Tem certeza de que deseja excluir permanentemente a oferta #{0} para o produto {1}?'),
('PT', 'msg_OfertaEliminadaExito', 'Oferta excluída do banco de dados com sucesso.'),
('PT', 'ProveedoresUI_Text', 'CRUD Fornecedores (JSON)'),
('PT', 'ProveedoresUI_Titulo', 'Gestão de Fornecedores (Persistência JSON)'),
('PT', 'lblCUIT', 'CNPJ / CUIT'),
('PT', 'lblRazonSocial', 'Razão Social'),
('PT', 'lblCondicion', 'Condição Comercial'),
('PT', 'btnGuardarProv', 'Salvar Fornecedor'),
('PT', 'btnLimpiarSeleccion', 'Limpar Seleção'),
('PT', 'btnEliminarProv', 'Excluir Fornecedor'),
('PT', 'btnMigrarSQL', 'Importar Dados de Teste (SQL -> JSON)'),
('PT', 'msg_ProvGuardadoExito', 'Fornecedor salvo no arquivo JSON com sucesso.'),
('PT', 'msg_ProvActualizadoExito', 'Fornecedor atualizado no arquivo JSON com sucesso.'),
('PT', 'msg_ConfirmarBajaProv', 'Tem certeza de que deseja desativar este fornecedor?'),
('PT', 'msg_SelecProvEliminar', 'Selecione um fornecedor na grade para excluir.'),
('PT', 'msg_AvisoMigracion', 'Deseja importar os fornecedores do Banco de Dados SQL para o arquivo JSON?' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + 'ATENÇÃO! Isso substituirá os dados atuais do arquivo local.'),
('PT', 'msg_TituloMigracion', 'Migração SQL -> JSON'),
('PT', 'msg_ExitoMigracion', 'Migração concluída! Os fornecedores de teste agora estão no arquivo JSON.'),
('PT', 'msg_ErrorMigracion', 'Erro de Migração')
) AS DatosNuevos(CodigoIdioma, KeyEtiqueta, Texto)
WHERE NOT EXISTS (
    SELECT 1 FROM Traduccion T 
    WHERE T.CodigoIdioma = DatosNuevos.CodigoIdioma 
    AND T.KeyEtiqueta = DatosNuevos.KeyEtiqueta
);

-- =========================================================
-- TRADUCCIONES AL ESPAÑOL (ES) - RecepcionMercaderiaUI
-- =========================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto)
SELECT CodigoIdioma, KeyEtiqueta, Texto FROM (VALUES 
('ES', 'RecepcionMercaderiaUI', 'Recepción de Mercadería'),
('ES', 'lblTituloRecepcion', 'Paso 1: Seleccione la OC que acaba de llegar'),
('ES', 'lblRemitoRecepcion', 'Paso 2: Ingrese el Nro. de Remito Papel'),
('ES', 'lblInstruccionesRecepcion', 'Paso 3: Complete Lote y Vencimiento en las celdas amarillas'),
('ES', 'btnConfirmarIngreso', 'Confirmar Ingreso'),
('ES', 'grid_IdOrden', 'N° Orden'),
('ES', 'grid_LoteIngresado', 'Lote Ingresado'),
('ES', 'grid_FechaVto', 'Fecha de Vencimiento'),
('ES', 'grid_CantEsperada', 'Cant. Esperada'),
('ES', 'msg_ValidacionRemito', 'Debe ingresar el número de remito físico entregado por el transportista.'),
('ES', 'msg_ValidacionLoteVto', 'Falta ingresar el Lote o Vencimiento para el producto {0}.'),
('ES', 'msg_ErrorCarga', 'Error de Carga'),
('ES', 'msg_FormatoFechaVto', 'El formato de fecha de vencimiento es inválido para {0}. Use DD/MM/YYYY.'),
('ES', 'msg_ErrorFormato', 'Error de Formato'),
('ES', 'msg_RecepcionExitosa', 'Mercadería ingresada al inventario y Lotes creados exitosamente.'),
('ES', 'msg_ExitoOperativo', 'Éxito Operativo'),
('ES', 'msg_ErrorSistema', 'Error del Sistema')
) AS DatosNuevos(CodigoIdioma, KeyEtiqueta, Texto)
WHERE NOT EXISTS (
    SELECT 1 FROM Traduccion T 
    WHERE T.CodigoIdioma = DatosNuevos.CodigoIdioma 
    AND T.KeyEtiqueta = DatosNuevos.KeyEtiqueta
);

-- =========================================================
-- TRADUCCIONES AL INGLÉS (EN) - RecepcionMercaderiaUI
-- =========================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto)
SELECT CodigoIdioma, KeyEtiqueta, Texto FROM (VALUES 
('EN', 'RecepcionMercaderiaUI', 'Merchandise Reception'),
('EN', 'lblTituloRecepcion', 'Step 1: Select the arriving PO'),
('EN', 'lblRemitoRecepcion', 'Step 2: Enter the physical Receipt Number'),
('EN', 'lblInstruccionesRecepcion', 'Step 3: Complete Batch and Expiration in the yellow cells'),
('EN', 'btnConfirmarIngreso', 'Confirm Reception'),
('EN', 'grid_IdOrden', 'Order No.'),
('EN', 'grid_LoteIngresado', 'Entered Batch'),
('EN', 'grid_FechaVto', 'Expiration Date'),
('EN', 'grid_CantEsperada', 'Expected Qty'),
('EN', 'msg_ValidacionRemito', 'You must enter the physical receipt number provided by the carrier.'),
('EN', 'msg_ValidacionLoteVto', 'Batch or Expiration date is missing for product {0}.'),
('EN', 'msg_ErrorCarga', 'Entry Error'),
('EN', 'msg_FormatoFechaVto', 'Invalid expiration date format for {0}. Use DD/MM/YYYY.'),
('EN', 'msg_ErrorFormato', 'Format Error'),
('EN', 'msg_RecepcionExitosa', 'Merchandise successfully added to inventory and Batches created.'),
('EN', 'msg_ExitoOperativo', 'Operational Success'),
('EN', 'msg_ErrorSistema', 'System Error')
) AS DatosNuevos(CodigoIdioma, KeyEtiqueta, Texto)
WHERE NOT EXISTS (
    SELECT 1 FROM Traduccion T 
    WHERE T.CodigoIdioma = DatosNuevos.CodigoIdioma 
    AND T.KeyEtiqueta = DatosNuevos.KeyEtiqueta
);

-- =========================================================
-- TRADUCCIONES AL PORTUGUÉS (PT) - RecepcionMercaderiaUI
-- =========================================================
INSERT INTO Traduccion (CodigoIdioma, KeyEtiqueta, Texto)
SELECT CodigoIdioma, KeyEtiqueta, Texto FROM (VALUES 
('PT', 'RecepcionMercaderiaUI', 'Recepção de Mercadorias'),
('PT', 'lblTituloRecepcion', 'Passo 1: Selecione a OC que acaba de chegar'),
('PT', 'lblRemitoRecepcion', 'Passo 2: Insira o número do Recibo Físico'),
('PT', 'lblInstruccionesRecepcion', 'Passo 3: Preencha Lote e Validade nas células amarelas'),
('PT', 'btnConfirmarIngreso', 'Confirmar Recepção'),
('PT', 'grid_IdOrden', 'N° Ordem'),
('PT', 'grid_LoteIngresado', 'Lote Inserido'),
('PT', 'grid_FechaVto', 'Data de Validade'),
('PT', 'grid_CantEsperada', 'Qtd. Esperada'),
('PT', 'msg_ValidacionRemito', 'Você deve inserir o número do recibo físico fornecido pela transportadora.'),
('PT', 'msg_ValidacionLoteVto', 'Falta inserir o Lote ou Validade para o produto {0}.'),
('PT', 'msg_ErrorCarga', 'Erro de Preenchimento'),
('PT', 'msg_FormatoFechaVto', 'Formato de data de validade inválido para {0}. Use DD/MM/YYYY.'),
('PT', 'msg_ErrorFormato', 'Erro de Formato'),
('PT', 'msg_RecepcionExitosa', 'Mercadoria inserida no inventário e Lotes criados com sucesso.'),
('PT', 'msg_ExitoOperativo', 'Sucesso Operacional'),
('PT', 'msg_ErrorSistema', 'Erro do Sistema')
) AS DatosNuevos(CodigoIdioma, KeyEtiqueta, Texto)
WHERE NOT EXISTS (
    SELECT 1 FROM Traduccion T 
    WHERE T.CodigoIdioma = DatosNuevos.CodigoIdioma 
    AND T.KeyEtiqueta = DatosNuevos.KeyEtiqueta
);