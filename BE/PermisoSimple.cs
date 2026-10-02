namespace BE
{
    public class PermisoSimple : Permiso
    {
        public PermisoSimple(uint id, string nombre, int nivelJerarquico) : base(id, nombre, nivelJerarquico) { }

        public override bool ValidarPermiso(string nombrePermiso)
        {
            return Nombre.Equals(nombrePermiso, StringComparison.OrdinalIgnoreCase);
        }
    }
}
