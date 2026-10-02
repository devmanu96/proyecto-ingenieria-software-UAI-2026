namespace BE
{
    public class Perfil : Permiso
    {
        public List<Permiso> PermisosAsociados { get; private set; }

        public Perfil(uint id, string nombre, int nivelJerarquico) : base(id, nombre, nivelJerarquico)
        {
            PermisosAsociados = new List<Permiso>();
        }

        public override bool ValidarPermiso(string nombrePermiso)
        {
            return PermisosAsociados.Any(p => p.ValidarPermiso(nombrePermiso));
        }

        public void AgregarPermiso(Permiso permiso)
        {
            if (!PermisosAsociados.Contains(permiso))
            {
                PermisosAsociados.Add(permiso);
            }
            else
            {
                throw new Exception($"El permiso con ID {permiso.ID} ya está asociado al perfil.");
            }
        }

        public void EliminarPermiso(uint IdPermiso)
        {
            Permiso? foundPermiso = PermisosAsociados.FirstOrDefault(p => p.ID == IdPermiso);
            if (foundPermiso != null)
            {
                PermisosAsociados.Remove(foundPermiso);
            }
            else
            {
                throw new Exception($"No se encontró el permiso con ID {IdPermiso} en el perfil.");
            }
        }
    }
}
