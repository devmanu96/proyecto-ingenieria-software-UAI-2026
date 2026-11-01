using System;

namespace BE
{
    public class Proveedor
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CUIT { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string CondicionComercial { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }
}