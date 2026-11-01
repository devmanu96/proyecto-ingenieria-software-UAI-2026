using System.IO.Compression;
using System.Text;

namespace DocGenerator;

/// <summary>
/// Genera diagramas PlantUML y los renderiza a PNG usando el servicio Kroki.io.
/// Soporta: Diagramas de Clases, DER, Modelo de Datos y Secuencia.
/// </summary>
public static class PlantUmlService
{
    private const string KROKI_URL = "https://kroki.io/plantuml/png";
    private const string PLANTUML_SERVER = "https://www.plantuml.com/plantuml/png/";

    // ═══════════════════════════════════════════════════════════════════
    // Diagrama de Clases
    // ═══════════════════════════════════════════════════════════════════

    public static string GenerateClassDiagram(List<ClassData> classes, string title)
    {
        var sb = new StringBuilder();
        sb.AppendLine("@startuml");
        sb.AppendLine($"title {title}");
        sb.AppendLine();
        sb.AppendLine("skinparam classAttributeIconSize 0");
        sb.AppendLine("skinparam classFontSize 12");
        sb.AppendLine("skinparam classHeaderBackgroundColor #E3F2FD");
        sb.AppendLine("skinparam classBorderColor #1565C0");
        sb.AppendLine("skinparam classArrowColor #1565C0");
        sb.AppendLine("skinparam stereotypeCBackgroundColor #BBDEFB");
        sb.AppendLine("skinparam packageBackgroundColor #FAFAFA");
        sb.AppendLine("skinparam packageBorderColor #90CAF9");
        sb.AppendLine("skinparam linetype ortho");
        sb.AppendLine("hide empty members");
        sb.AppendLine();

        // Agrupar por namespace si hay múltiples
        var groups = classes.GroupBy(c => c.Namespace).OrderBy(g => g.Key);

        foreach (var group in groups)
        {
            bool usePackage = groups.Count() > 1 && !string.IsNullOrEmpty(group.Key);
            if (usePackage)
            {
                string packageName = group.Key.Split('.').Last();
                sb.AppendLine($"package \"{packageName}\" {{");
            }

            foreach (var cls in group)
            {
                WriteClassDefinition(sb, cls);
            }

            if (usePackage)
                sb.AppendLine("}");

            sb.AppendLine();
        }

        // Relaciones (evitar duplicados)
        var writtenRelations = new HashSet<string>();
        foreach (var cls in classes)
        {
            foreach (var rel in cls.Relationships)
            {
                // Solo dibujar si el target existe en la lista
                if (!classes.Any(c => c.Name == rel.TargetClass))
                    continue;

                string relKey = $"{cls.Name}-{rel.TargetClass}-{rel.Type}";
                if (writtenRelations.Contains(relKey)) continue;
                writtenRelations.Add(relKey);

                string arrow = rel.Type switch
                {
                    RelationshipType.Inheritance => $"{cls.Name} --|> {rel.TargetClass}",
                    RelationshipType.Implementation => $"{cls.Name} ..|> {rel.TargetClass}",
                    RelationshipType.Composition => $"{cls.Name} *-- \"{rel.Cardinality}\" {rel.TargetClass}",
                    RelationshipType.Aggregation => $"{cls.Name} o-- {rel.TargetClass}",
                    RelationshipType.Association => $"{cls.Name} --> {rel.TargetClass} : {rel.Label}",
                    RelationshipType.ForeignKey => $"{cls.Name} --> {rel.TargetClass} : {rel.Label}",
                    _ => ""
                };

                if (!string.IsNullOrEmpty(arrow))
                    sb.AppendLine(arrow);
            }
        }

        sb.AppendLine();
        sb.AppendLine("@enduml");
        return sb.ToString();
    }

    private static void WriteClassDefinition(StringBuilder sb, ClassData cls)
    {
        string stereotype = "";
        string keyword;

        if (cls.IsInterface)
        {
            keyword = "interface";
        }
        else if (cls.IsAbstract)
        {
            keyword = "abstract class";
        }
        else if (cls.IsEnum)
        {
            keyword = "enum";
        }
        else
        {
            keyword = "class";
            if (cls.Name.EndsWith("DTO"))
                stereotype = " <<DTO>>";
        }

        sb.AppendLine($"  {keyword} {cls.Name}{stereotype} {{");

        // Propiedades públicas
        foreach (var prop in cls.Properties.Where(p => !p.IsPrivate))
        {
            string access = "+";
            string readOnly = (!prop.HasSetter) ? " {readonly}" : "";
            sb.AppendLine($"    {access}{prop.TypeName} {prop.Name}{readOnly}");
        }

        // Separador si hay propiedades y métodos
        if (cls.Properties.Any(p => !p.IsPrivate) && cls.Methods.Any())
            sb.AppendLine("    --");

        // Métodos
        foreach (var method in cls.Methods)
        {
            string access = "+";
            string modifier = "";
            if (method.IsAbstract) modifier = "{abstract} ";
            else if (method.IsStatic) modifier = "{static} ";

            string parameters = string.Join(", ", method.Parameters.Select(p => $"{p.TypeName} {p.Name}"));
            sb.AppendLine($"    {access}{modifier}{method.ReturnType} {method.Name}({parameters})");
        }

        sb.AppendLine("  }");
    }

    // ═══════════════════════════════════════════════════════════════════
    // Diagrama Entidad-Relación (DER)
    // ═══════════════════════════════════════════════════════════════════

    public static string GenerateErDiagram(List<ClassData> classes, string title)
    {
        var sb = new StringBuilder();
        sb.AppendLine("@startuml");
        sb.AppendLine($"title {title}");
        sb.AppendLine();
        sb.AppendLine("skinparam linetype ortho");
        sb.AppendLine("skinparam EntityBackgroundColor #E8F5E9");
        sb.AppendLine("skinparam EntityBorderColor #2E7D32");
        sb.AppendLine("skinparam EntityFontSize 11");
        sb.AppendLine("hide circle");
        sb.AppendLine();

        // Solo entidades persistentes (excluir interfaces, DTOs, abstracts puros, utils)
        var entities = classes.Where(c =>
            !c.IsInterface &&
            !c.IsEnum &&
            !c.Name.EndsWith("DTO") &&
            !c.Name.Contains("Validation") &&
            !c.Name.Contains("Memento") &&
            c.Name != "Entidad" &&
            !c.Name.StartsWith("I") || (c.Name.StartsWith("I") && c.Name.Length > 1 && !char.IsUpper(c.Name[1])))
            .ToList();

        // Filtrar mejor: solo clases que tengan al menos una propiedad tipo Id o similares
        entities = entities.Where(c => c.Properties.Any(p =>
            p.Name.Contains("Id") || p.Name == "Codigo" || p.Name == "CodigoBarra" ||
            p.Name == "CUIT" || p.Name == "Username")).ToList();

        foreach (var entity in entities)
        {
            sb.AppendLine($"entity \"{entity.Name}\" as {entity.Name} {{");

            // PK: primera propiedad que sea Id o similar
            var pkProp = entity.Properties.FirstOrDefault(p =>
                p.Name == "Id" || p.Name == $"Id{entity.Name}" ||
                p.Name == "CodigoBarra" || p.Name == "Codigo" ||
                p.Name == "IdSolicitud" || p.Name == "IdDetalle" ||
                p.Name.StartsWith("Id") && !entity.Properties.Any(pp => pp != p && pp.Name.StartsWith("Id") && pp.Name.Length < p.Name.Length));

            if (pkProp != null)
                sb.AppendLine($"  * {pkProp.Name} : {pkProp.TypeName} <<PK>>");

            sb.AppendLine("  --");

            // Resto de propiedades
            foreach (var prop in entity.Properties.Where(p => !p.IsPrivate && p != pkProp))
            {
                bool isFk = prop.Name.StartsWith("Id") && prop.Name.Length > 2 &&
                             entities.Any(e => e.Name == prop.Name.Substring(2));
                string fkTag = isFk ? " <<FK>>" : "";
                sb.AppendLine($"  {prop.Name} : {prop.TypeName}{fkTag}");
            }

            sb.AppendLine("}");
            sb.AppendLine();
        }

        // Relaciones
        var writtenRels = new HashSet<string>();
        foreach (var entity in entities)
        {
            foreach (var rel in entity.Relationships)
            {
                if (!entities.Any(e => e.Name == rel.TargetClass)) continue;

                string relKey = $"{entity.Name}-{rel.TargetClass}";
                string reverseKey = $"{rel.TargetClass}-{entity.Name}";
                if (writtenRels.Contains(relKey) || writtenRels.Contains(reverseKey)) continue;
                writtenRels.Add(relKey);

                string erRel = rel.Type switch
                {
                    RelationshipType.Composition => $"{entity.Name} ||--o{{ {rel.TargetClass}",
                    RelationshipType.ForeignKey => $"{rel.TargetClass} ||--o{{ {entity.Name}",
                    RelationshipType.Association => $"{entity.Name} }}o--|| {rel.TargetClass}",
                    _ => ""
                };

                if (!string.IsNullOrEmpty(erRel))
                    sb.AppendLine(erRel);
            }
        }

        sb.AppendLine();
        sb.AppendLine("@enduml");
        return sb.ToString();
    }

    // ═══════════════════════════════════════════════════════════════════
    // Modelo de Datos (tablas con tipos de datos detallados)
    // ═══════════════════════════════════════════════════════════════════

    public static string GenerateDataModel(List<ClassData> classes, string title)
    {
        var sb = new StringBuilder();
        sb.AppendLine("@startuml");
        sb.AppendLine($"title {title}");
        sb.AppendLine();
        sb.AppendLine("skinparam ClassBackgroundColor #FFF3E0");
        sb.AppendLine("skinparam ClassBorderColor #E65100");
        sb.AppendLine("skinparam ClassFontSize 11");
        sb.AppendLine("skinparam classFontColor #333333");
        sb.AppendLine("skinparam stereotypeCBackgroundColor #FFE0B2");
        sb.AppendLine("skinparam linetype ortho");
        sb.AppendLine("hide methods");
        sb.AppendLine();

        // Todas las clases concretas con propiedades
        var dataClasses = classes.Where(c =>
            !c.IsInterface && !c.IsEnum && c.Properties.Any(p => !p.IsPrivate)).ToList();

        foreach (var cls in dataClasses)
        {
            string stereo = cls.IsAbstract ? " <<abstract>>" : "";
            if (cls.Name.EndsWith("DTO")) stereo = " <<DTO>>";

            sb.AppendLine($"class \"{cls.Name}\"{stereo} {{");

            foreach (var prop in cls.Properties.Where(p => !p.IsPrivate))
            {
                string nullable = prop.TypeName.EndsWith("?") ? " [nullable]" : "";
                string pk = (prop.Name == "Id" || prop.Name == $"Id{cls.Name}" || prop.Name == "CodigoBarra") ? " <<PK>>" : "";
                string fk = (prop.Name.StartsWith("Id") && prop.Name.Length > 2 && pk == "") ? " <<FK>>" : "";
                sb.AppendLine($"  {prop.TypeName} {prop.Name}{pk}{fk}{nullable}");
            }

            sb.AppendLine("}");
            sb.AppendLine();
        }

        sb.AppendLine("@enduml");
        return sb.ToString();
    }

    // ═══════════════════════════════════════════════════════════════════
    // Diagrama de Secuencia (desde configuración JSON)
    // ═══════════════════════════════════════════════════════════════════

    public static string GenerateSequenceDiagram(SequenceDiagram seq)
    {
        var sb = new StringBuilder();
        sb.AppendLine("@startuml");
        sb.AppendLine($"title {seq.Name}");
        sb.AppendLine();
        sb.AppendLine("skinparam sequenceArrowThickness 2");
        sb.AppendLine("skinparam sequenceParticipantBorderColor #1565C0");
        sb.AppendLine("skinparam sequenceParticipantBackgroundColor #E3F2FD");
        sb.AppendLine("skinparam sequenceLifeLineBorderColor #64B5F6");
        sb.AppendLine("skinparam sequenceMessageAlign center");
        sb.AppendLine("autonumber");
        sb.AppendLine();

        // Participantes
        foreach (var p in seq.Participants)
        {
            string alias = p.Alias ?? p.Name;
            string displayName = p.Name.Contains(" ") ? $"\"{p.Name}\"" : p.Name;
            sb.AppendLine($"{p.Type} {displayName} as {alias}");
        }
        sb.AppendLine();

        // Mensajes
        foreach (var msg in seq.Messages)
        {
            string from = GetParticipantAlias(seq, msg.From);
            string to = GetParticipantAlias(seq, msg.To);

            switch (msg.Type.ToLower())
            {
                case "call":
                    sb.AppendLine($"{from} -> {to} : {msg.Label}");
                    sb.AppendLine($"activate {to}");
                    break;
                case "reply":
                case "return":
                    sb.AppendLine($"{from} --> {to} : {msg.Label}");
                    sb.AppendLine($"deactivate {from}");
                    break;
                case "self":
                    sb.AppendLine($"{from} -> {from} : {msg.Label}");
                    break;
                case "create":
                    sb.AppendLine($"create {to}");
                    sb.AppendLine($"{from} -> {to} : <<create>>");
                    break;
                case "destroy":
                    sb.AppendLine($"{from} -> {to} : <<destroy>>");
                    sb.AppendLine($"destroy {to}");
                    break;
                case "note":
                    string pos = msg.Position ?? "right";
                    if (pos == "over")
                        sb.AppendLine($"note over {from} : {msg.Label}");
                    else
                        sb.AppendLine($"note {pos} of {from} : {msg.Label}");
                    break;
                case "alt":
                    sb.AppendLine($"alt {msg.Label}");
                    break;
                case "else":
                    sb.AppendLine($"else {msg.Label}");
                    break;
                case "end":
                    sb.AppendLine("end");
                    break;
                case "loop":
                    sb.AppendLine($"loop {msg.Label}");
                    break;
                case "group":
                    sb.AppendLine($"group {msg.Label}");
                    break;
                default:
                    sb.AppendLine($"{from} -> {to} : {msg.Label}");
                    break;
            }
        }

        sb.AppendLine();
        sb.AppendLine("@enduml");
        return sb.ToString();
    }

    private static string GetParticipantAlias(SequenceDiagram seq, string name)
    {
        var participant = seq.Participants.FirstOrDefault(p =>
            p.Name == name || p.Alias == name);
        return participant?.Alias ?? participant?.Name ?? name;
    }

    // ═══════════════════════════════════════════════════════════════════
    // Renderizado a PNG
    // ═══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Renderiza PlantUML a PNG usando Kroki.io (primario) o PlantUML Server (fallback).
    /// </summary>
    public static async Task<bool> RenderToPngAsync(string plantUmlText, string outputPath)
    {
        // Intento 1: Kroki.io (soporta POST, ideal para diagramas grandes)
        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var content = new StringContent(plantUmlText, Encoding.UTF8, "text/plain");
            var response = await client.PostAsync(KROKI_URL, content);

            if (response.IsSuccessStatusCode)
            {
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(outputPath, imageBytes);
                return true;
            }
        }
        catch
        {
            // Fallback al servidor PlantUML
        }

        // Intento 2: PlantUML Server (usa codificación en URL)
        try
        {
            string encoded = PlantUmlEncode(plantUmlText);
            string url = PLANTUML_SERVER + encoded;

            // Si la URL es muy larga, no se puede usar este método
            if (url.Length > 8000)
            {
                Console.Write(" [URL muy larga para PlantUML Server]");
                return false;
            }

            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var imageBytes = await client.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(outputPath, imageBytes);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // Codificación PlantUML para URLs
    // ═══════════════════════════════════════════════════════════════════

    private static string PlantUmlEncode(string text)
    {
        byte[] data = Encoding.UTF8.GetBytes(text);
        byte[] compressed = DeflateCompress(data);
        return Encode64(compressed);
    }

    private static byte[] DeflateCompress(byte[] data)
    {
        using var output = new MemoryStream();
        using (var deflate = new DeflateStream(output, CompressionLevel.Optimal, true))
        {
            deflate.Write(data, 0, data.Length);
        }
        return output.ToArray();
    }

    private static string Encode64(byte[] data)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < data.Length; i += 3)
        {
            byte b1 = data[i];
            byte b2 = (i + 1 < data.Length) ? data[i + 1] : (byte)0;
            byte b3 = (i + 2 < data.Length) ? data[i + 2] : (byte)0;

            int c1 = b1 >> 2;
            int c2 = ((b1 & 0x3) << 4) | (b2 >> 4);
            int c3 = ((b2 & 0xF) << 2) | (b3 >> 6);
            int c4 = b3 & 0x3F;

            sb.Append(Encode6bit(c1 & 0x3F));
            sb.Append(Encode6bit(c2 & 0x3F));
            sb.Append(Encode6bit(c3 & 0x3F));
            sb.Append(Encode6bit(c4 & 0x3F));
        }
        return sb.ToString();
    }

    private static char Encode6bit(int b)
    {
        if (b < 10) return (char)(48 + b);
        b -= 10;
        if (b < 26) return (char)(65 + b);
        b -= 26;
        if (b < 26) return (char)(97 + b);
        b -= 26;
        if (b == 0) return '-';
        if (b == 1) return '_';
        return '?';
    }
}
