using System;

namespace BE
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string CUIT { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
    }
}