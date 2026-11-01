using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BE;

namespace DAL
{
    public class RepositorioProveedorJSON
    {
        private readonly string rutaArchivo;

        public RepositorioProveedorJSON()
        {
            // 1. Apuntamos a la carpeta segura de Windows para datos de aplicaciones (AppData\Roaming)
            string rutaAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // 2. Definimos una subcarpeta exclusiva para tu sistema
            string rutaCarpetaSistema = Path.Combine(rutaAppData, "MegaDrink");

            // 3. Si la carpeta no existe en la PC del cliente, la creamos al vuelo
            if (!Directory.Exists(rutaCarpetaSistema))
            {
                Directory.CreateDirectory(rutaCarpetaSistema);
            }

            // 4. Asignamos la ruta final segura para el archivo
            rutaArchivo = Path.Combine(rutaCarpetaSistema, "proveedores.json");
        }

        // ==========================================
        // DESERIALIZACIÓN (Leer de Texto a Objetos)
        // ==========================================
        public List<Proveedor> LeerProveedores()
        {
            if (!File.Exists(rutaArchivo))
            {
                // Si el archivo no existe (primera vez), devolvemos una lista vacía
                return new List<Proveedor>();
            }

            try
            {
                string jsonString = File.ReadAllText(rutaArchivo);

                // Transforma el string JSON nuevamente en una List<Proveedor>
                return JsonSerializer.Deserialize<List<Proveedor>>(jsonString) ?? new List<Proveedor>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al deserializar el archivo de proveedores: " + ex.Message);
            }
        }

        // ==========================================
        // SERIALIZACIÓN (Escribir de Objetos a Texto)
        // ==========================================
        public void GuardarProveedores(List<Proveedor> proveedores)
        {
            try
            {
                // WriteIndented = true hace que el JSON sea legible y editable por humanos
                var opciones = new JsonSerializerOptions { WriteIndented = true };

                // Transforma la List<Proveedor> en un string estructurado
                string jsonString = JsonSerializer.Serialize(proveedores, opciones);

                File.WriteAllText(rutaArchivo, jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al serializar el archivo de proveedores: " + ex.Message);
            }
        }
    }
}