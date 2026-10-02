namespace BE
{
    public abstract class Permiso
    {
        public uint ID { get; private set; }
        public string Nombre { get; private set; }
        public int NivelJerarquico { get; private set; }

        // Agregas el parámetro al constructor
        protected Permiso(uint id, string nombre, int nivelJerarquico)
        {
            ID = id;
            Nombre = nombre;
            NivelJerarquico = nivelJerarquico;
        }

        public abstract bool ValidarPermiso(string nombrePermiso);
    }
}
