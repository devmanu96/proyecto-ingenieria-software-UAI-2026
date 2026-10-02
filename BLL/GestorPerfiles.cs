using BE;
using DAL;

namespace BLL
{
    public class GestorPerfiles : ISujeto
    {
        private RepositorioPerfiles repositorioPerfiles;
        private List<IObserver> ObserversAttached = new List<IObserver>();

        public GestorPerfiles()
        {
            repositorioPerfiles = new RepositorioPerfiles();
            Attach(GestorBitacora.GetInstance);
        }

        public void CrearPerfil(string nombrePerfil)
        {
            List<Permiso> permisosGlobales = repositorioPerfiles.ObtenerPermisos();
            bool existePermiso = permisosGlobales.Any(p => p.Nombre.Equals(nombrePerfil));
            if (existePermiso)
            {
                throw new Exception($"Ya existe un permiso o perfil con el nombre '{nombrePerfil}'.");
            }
            repositorioPerfiles.CrearPerfil(nombrePerfil);
            Notificar(ObtenerUsuarioActivo().Username, $"Creación de perfil");
        }

        public void AgregarPermisoAPerfil(uint idPerfil, uint idPermiso)
        {
            List<Permiso> permisosGlobales = repositorioPerfiles.ObtenerPermisos();

            Perfil? perfil = permisosGlobales.OfType<Perfil>().FirstOrDefault(p => p.ID == idPerfil);
            if (perfil == null)
            {
                throw new Exception($"No se encontró el perfil con ID {idPerfil}.");
            }

            Permiso? permiso = permisosGlobales.FirstOrDefault(p => p.ID == idPermiso);
            if (permiso == null)
            {
                throw new Exception($"No se encontró el permiso con ID {idPermiso}.");
            }

            bool permisoYaExiste = perfil.PermisosAsociados.Any(p => p.ID == idPermiso);
            if (permisoYaExiste)
            {
                throw new Exception($"El permiso con ID {idPermiso} ya está asociado al perfil '{perfil.Nombre}'.");
            }

            repositorioPerfiles.AsociarPermisoConPermiso(idPerfil, idPermiso);
            Notificar(ObtenerUsuarioActivo().Username, $"Asignación de permiso a perfil");
        }

        public void AgregarPerfilAPerfil(uint idPerfilPadre, uint idPerfilHijo)
        {
            List<Permiso> permisosGlobales = repositorioPerfiles.ObtenerPermisos();

            Perfil? perfilPadre = permisosGlobales.OfType<Perfil>().FirstOrDefault(p => p.ID == idPerfilPadre);
            if (perfilPadre == null)
            {
                throw new Exception($"No se encontró el perfil padre con ID {idPerfilPadre}.");
            }

            Perfil? perfilHijo = permisosGlobales.OfType<Perfil>().FirstOrDefault(p => p.ID == idPerfilHijo);
            if (perfilHijo == null)
            {
                throw new Exception($"No se encontró el perfil hijo con ID {idPerfilHijo}.");
            }

            // Validar jerarquía de negocio (Un rol menor no puede heredar permisos de un rol mayor)
            if (perfilHijo.NivelJerarquico < perfilPadre.NivelJerarquico)
            {
                string plantillaError = GestorIdioma.GetInstance.TraducirMensaje(
                    "err_JerarquiaInvalida",
                    "Un perfil de menor rango ({0}) no puede contener a un perfil de mayor rango ({1})."
                );
                string mensajeFinal = string.Format(plantillaError, perfilPadre.Nombre, perfilHijo.Nombre);
                throw new Exception(mensajeFinal);
            }

            // Validar que no sea auto-referencia
            if (idPerfilPadre == idPerfilHijo)
            {
                throw new Exception($"Un perfil no puede contenerse a sí mismo.");
            }

            // Validar que no exista ya
            bool perfilYaExiste = perfilPadre.PermisosAsociados.Any(p => p.ID == idPerfilHijo);
            if (perfilYaExiste)
            {
                throw new Exception($"El perfil con ID {idPerfilHijo} ya está asociado al perfil '{perfilPadre.Nombre}'.");
            }

            // Validar referencias circulares (anidación)
            if (ContieneReferenciaCircular(perfilHijo, idPerfilPadre, permisosGlobales))
            {
                throw new Exception($"Agregar el perfil '{perfilHijo.Nombre}' al perfil '{perfilPadre.Nombre}' crearía una referencia circular.");
            }

            repositorioPerfiles.AsociarPermisoConPermiso(idPerfilPadre, idPerfilHijo);
            Notificar(ObtenerUsuarioActivo().Username, $"Asociación de perfil a perfil");
        }

        public void AgregarPerfilAUsuario(string username, uint idPerfil)
        {
            Usuario usuarioSeleccionado = RepositorioUsuarios.GetInstance.ObtenerUsuario(username);

            if (usuarioSeleccionado.Permisos.Any(p => p.ID == idPerfil))
                throw new Exception("El usuario seleccionado ya posee ese perfil");

            // Buscamos el permiso que se está intentando asignar
            List<Permiso> permisosGlobales = repositorioPerfiles.ObtenerPermisos();
            Permiso? permisoAAsignar = permisosGlobales.FirstOrDefault(p => p.ID == idPerfil);

            if (permisoAAsignar == null)
                throw new Exception("No se encontró el permiso a asignar.");

            // VALIDACIÓN DE NEGOCIO (Segregación de Funciones): 
            // Si lo que se intenta asignar es un rol base estructurado (Nivel < 99)
            if (permisoAAsignar is Perfil && permisoAAsignar.NivelJerarquico < 99)
            {
                // Verificamos si el usuario ya tiene algún otro rol base asignado directamente
                bool tieneRolBase = usuarioSeleccionado.Permisos.Any(p => p is Perfil && p.NivelJerarquico < 99);

                if (tieneRolBase)
                {
                    // Lanzamos la excepción con soporte multi-idioma
                    string msgError = GestorIdioma.GetInstance.TraducirMensaje(
                        "err_MultiplesPerfiles",
                        "Incompatibilidad de roles: El usuario ya posee un perfil jerárquico principal asignado. Debe desasignarlo antes de otorgarle uno nuevo."
                    );
                    throw new Exception(msgError);
                }
            }

            repositorioPerfiles.AsociarPerfilAUsuario(usuarioSeleccionado, idPerfil);
            Notificar(ObtenerUsuarioActivo().Username, $"Asignación de perfil a usuario");
        }

        private bool ContieneReferenciaCircular(Perfil perfilHijo, uint idPerfilPadre, List<Permiso> permisosGlobales)
        {
            HashSet<uint> visitados = new HashSet<uint>();
            return VerificarCircularRecursivo(perfilHijo, idPerfilPadre, visitados, permisosGlobales);
        }

        private bool VerificarCircularRecursivo(Perfil perfilActual, uint idObjetivo, HashSet<uint> visitados, List<Permiso> permisosGlobales)
        {
            // Si ya visitamos este perfil, hay un ciclo
            if (visitados.Contains(perfilActual.ID))
            {
                return true;
            }

            visitados.Add(perfilActual.ID);

            // Verificar si alguno de los permisos asociados contiene al perfil padre
            foreach (Permiso permisoAsociado in perfilActual.PermisosAsociados)
            {
                if (permisoAsociado.ID == idObjetivo)
                {
                    return true;
                }

                // Si es un perfil, recursivamente verificar sus permisos
                if (permisoAsociado is Perfil perfilAnidado)
                {
                    if (VerificarCircularRecursivo(perfilAnidado, idObjetivo, visitados, permisosGlobales))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void DesasignarPerfilDeUsuario(string username, uint idPerfil)
        {
            Usuario usuarioSeleccionado = RepositorioUsuarios.GetInstance.ObtenerUsuario(username);
            if (!usuarioSeleccionado.Permisos.Any(p => p.ID == idPerfil)) throw new Exception("El usuario seleccionado no posee ese perfil");
            repositorioPerfiles.DesasociarPerfilDeUsuario(usuarioSeleccionado.Id, idPerfil);
            Notificar(ObtenerUsuarioActivo().Username, $"Desasignación de perfil de usuario");
        }

        public List<Permiso> ListarPermisos()
        {
            return repositorioPerfiles.ObtenerPermisos();
        }

        public List<Usuario> ListarUsuarios()
        {
            return RepositorioUsuarios.GetInstance.ObtenerListadoTotalUsuarios();
        }

        public void Update(Usuario usuarioInvolucrado, string action)
        {

        }

        public void Attach(IObserver observer)
        {
            ObserversAttached.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            IObserver? foundObserver = ObserversAttached.Find((item) => item == observer);
            if (foundObserver == null) throw new Exception("Observer no agregado");
            ObserversAttached.Remove(observer);
        }

        public void Notificar(string username, string accion)
        {
            foreach (IObserver item in ObserversAttached)
            {
                item.Update(username, accion);
            }
        }

        private Usuario ObtenerUsuarioActivo()
        {
            Usuario? usuarioActivo = SessionManager.getInstance.ObtenerUsuarioActivo();
            if (usuarioActivo == null) throw new Exception("Debe haber un usuario activo para registrar la acción.");
            return usuarioActivo;
        }
    }
}
