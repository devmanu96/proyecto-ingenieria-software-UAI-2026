using System.Reflection;

namespace DocGenerator;

/// <summary>
/// Analiza ensamblados compilados (.dll) usando reflexión para extraer
/// información de clases, propiedades, métodos y relaciones.
/// </summary>
public static class AssemblyAnalyzer
{
    /// <summary>
    /// Analiza un ensamblado y retorna la información de todos sus tipos públicos.
    /// </summary>
    public static List<ClassData> Analyze(string dllPath)
    {
        var result = new List<ClassData>();

        try
        {
            // Cargar en contexto de solo-lectura para no bloquear los archivos
            var assembly = Assembly.LoadFrom(dllPath);
            var types = assembly.GetExportedTypes();

            foreach (var type in types)
            {
                if (type.IsSpecialName || type.Name.StartsWith("<"))
                    continue;

                var classData = new ClassData
                {
                    Name = type.Name,
                    FullName = type.FullName ?? type.Name,
                    Namespace = type.Namespace ?? "",
                    IsAbstract = type.IsAbstract && !type.IsInterface,
                    IsInterface = type.IsInterface,
                    IsEnum = type.IsEnum,
                    IsSealed = type.IsSealed,
                    BaseClass = GetBaseClassName(type),
                    Interfaces = type.GetInterfaces()
                        .Where(i => i.Assembly == assembly || i.Namespace?.StartsWith("BE") == true)
                        .Select(i => i.Name)
                        .ToList()
                };

                // Propiedades
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    classData.Properties.Add(new PropertyData
                    {
                        Name = prop.Name,
                        TypeName = GetFriendlyTypeName(prop.PropertyType),
                        RawType = prop.PropertyType,
                        HasGetter = prop.CanRead,
                        HasSetter = prop.CanWrite
                    });
                }

                // Propiedades privadas (para detectar relaciones)
                foreach (var prop in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    classData.Properties.Add(new PropertyData
                    {
                        Name = prop.Name,
                        TypeName = GetFriendlyTypeName(prop.PropertyType),
                        RawType = prop.PropertyType,
                        HasGetter = prop.CanRead,
                        HasSetter = prop.CanWrite,
                        IsPrivate = true
                    });
                }

                // Métodos públicos (excluyendo los generados automáticamente)
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (method.IsSpecialName) continue; // get_, set_, etc.

                    classData.Methods.Add(new MethodData
                    {
                        Name = method.Name,
                        ReturnType = GetFriendlyTypeName(method.ReturnType),
                        Parameters = method.GetParameters()
                            .Select(p => new ParameterData
                            {
                                Name = p.Name ?? "arg",
                                TypeName = GetFriendlyTypeName(p.ParameterType)
                            })
                            .ToList(),
                        IsAbstract = method.IsAbstract,
                        IsVirtual = method.IsVirtual && !method.IsAbstract,
                        IsOverride = method.GetBaseDefinition() != method,
                        IsStatic = method.IsStatic
                    });
                }

                // Constructores
                foreach (var ctor in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
                {
                    var parameters = ctor.GetParameters();
                    if (parameters.Length == 0) continue; // Omitir constructor vacío

                    classData.Constructors.Add(new ConstructorData
                    {
                        Parameters = parameters.Select(p => new ParameterData
                        {
                            Name = p.Name ?? "arg",
                            TypeName = GetFriendlyTypeName(p.ParameterType)
                        }).ToList()
                    });
                }

                // Detectar relaciones
                DetectRelationships(classData, type, assembly);

                result.Add(classData);
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            // Algunos tipos pueden no cargarse, procesar los que sí
            foreach (var type in ex.Types.Where(t => t != null))
            {
                result.Add(new ClassData { Name = type!.Name, FullName = type.FullName ?? type.Name });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ⚠️  Error analizando {dllPath}: {ex.Message}");
        }

        return result;
    }

    private static void DetectRelationships(ClassData classData, Type type, Assembly sourceAssembly)
    {
        // Herencia
        if (type.BaseType != null && type.BaseType != typeof(object))
        {
            classData.Relationships.Add(new RelationshipData
            {
                TargetClass = type.BaseType.Name,
                Type = RelationshipType.Inheritance,
                Label = "hereda"
            });
        }

        // Interfaces del mismo ensamblado
        foreach (var iface in type.GetInterfaces())
        {
            if (iface.Assembly == sourceAssembly)
            {
                classData.Relationships.Add(new RelationshipData
                {
                    TargetClass = iface.Name,
                    Type = RelationshipType.Implementation,
                    Label = "implementa"
                });
            }
        }

        // Composición/Agregación por propiedades
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var propType = prop.PropertyType;

            // List<T> → composición 1:N
            if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var innerType = propType.GetGenericArguments()[0];
                if (innerType.Assembly == sourceAssembly)
                {
                    classData.Relationships.Add(new RelationshipData
                    {
                        TargetClass = innerType.Name,
                        Type = RelationshipType.Composition,
                        Label = $"1..*  ({prop.Name})",
                        Cardinality = "1..*"
                    });
                }
            }
            // Referencia directa a otra entidad del dominio
            else if (propType.Assembly == sourceAssembly && !propType.IsEnum && !propType.IsInterface)
            {
                classData.Relationships.Add(new RelationshipData
                {
                    TargetClass = propType.Name,
                    Type = RelationshipType.Association,
                    Label = prop.Name
                });
            }
            // Clave foránea por convención (IdXxx → asociación con Xxx)
            else if (prop.Name.StartsWith("Id") && prop.Name.Length > 2 &&
                     (propType == typeof(int) || propType == typeof(Guid)))
            {
                string possibleEntity = prop.Name.Substring(2);
                // Solo agregar si hay una entidad con ese nombre en el mismo ensamblado
                var targetType = sourceAssembly.GetTypes().FirstOrDefault(t => t.Name == possibleEntity);
                if (targetType != null)
                {
                    classData.Relationships.Add(new RelationshipData
                    {
                        TargetClass = possibleEntity,
                        Type = RelationshipType.ForeignKey,
                        Label = $"FK ({prop.Name})",
                        Cardinality = "N:1"
                    });
                }
            }
        }
    }

    private static string GetBaseClassName(Type type)
    {
        if (type.BaseType == null || type.BaseType == typeof(object))
            return "";
        return type.BaseType.Name;
    }

    private static string GetFriendlyTypeName(Type type)
    {
        if (type == typeof(void)) return "void";
        if (type == typeof(int)) return "int";
        if (type == typeof(long)) return "long";
        if (type == typeof(string)) return "string";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(decimal)) return "decimal";
        if (type == typeof(double)) return "double";
        if (type == typeof(float)) return "float";
        if (type == typeof(DateTime)) return "DateTime";
        if (type == typeof(Guid)) return "Guid";

        if (Nullable.GetUnderlyingType(type) is Type underlying)
            return $"{GetFriendlyTypeName(underlying)}?";

        if (type.IsGenericType)
        {
            var genericName = type.Name.Split('`')[0];
            var args = string.Join(", ", type.GetGenericArguments().Select(GetFriendlyTypeName));
            return $"{genericName}<{args}>";
        }

        return type.Name;
    }
}

// ═══════════════════════════════════════════════════════════════════════
// Modelos de datos para el análisis
// ═══════════════════════════════════════════════════════════════════════

public class ClassData
{
    public string Name { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Namespace { get; set; } = "";
    public bool IsAbstract { get; set; }
    public bool IsInterface { get; set; }
    public bool IsEnum { get; set; }
    public bool IsSealed { get; set; }
    public string BaseClass { get; set; } = "";
    public List<string> Interfaces { get; set; } = new();
    public List<PropertyData> Properties { get; set; } = new();
    public List<MethodData> Methods { get; set; } = new();
    public List<ConstructorData> Constructors { get; set; } = new();
    public List<RelationshipData> Relationships { get; set; } = new();
}

public class PropertyData
{
    public string Name { get; set; } = "";
    public string TypeName { get; set; } = "";
    public Type? RawType { get; set; }
    public bool HasGetter { get; set; }
    public bool HasSetter { get; set; }
    public bool IsPrivate { get; set; }
}

public class MethodData
{
    public string Name { get; set; } = "";
    public string ReturnType { get; set; } = "";
    public List<ParameterData> Parameters { get; set; } = new();
    public bool IsAbstract { get; set; }
    public bool IsVirtual { get; set; }
    public bool IsOverride { get; set; }
    public bool IsStatic { get; set; }
}

public class ConstructorData
{
    public List<ParameterData> Parameters { get; set; } = new();
}

public class ParameterData
{
    public string Name { get; set; } = "";
    public string TypeName { get; set; } = "";
}

public class RelationshipData
{
    public string TargetClass { get; set; } = "";
    public RelationshipType Type { get; set; }
    public string Label { get; set; } = "";
    public string Cardinality { get; set; } = "";
}

public enum RelationshipType
{
    Inheritance,
    Implementation,
    Composition,
    Aggregation,
    Association,
    ForeignKey
}

// ═══════════════════════════════════════════════════════════════════════
// Modelos para la configuración de diagramas de secuencia (JSON)
// ═══════════════════════════════════════════════════════════════════════

public class SequenceConfig
{
    public List<SequenceDiagram> Sequences { get; set; } = new();
}

public class SequenceDiagram
{
    public string Name { get; set; } = "";
    public List<Participant> Participants { get; set; } = new();
    public List<SeqMessage> Messages { get; set; } = new();
}

public class Participant
{
    public string Name { get; set; } = "";
    public string? Alias { get; set; }
    /// <summary>actor, participant, boundary, control, entity, database, collections, queue</summary>
    public string Type { get; set; } = "participant";
}

public class SeqMessage
{
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public string Label { get; set; } = "";
    /// <summary>call, reply, create, destroy, self, note, alt, else, end, loop, group</summary>
    public string Type { get; set; } = "call";
    /// <summary>Solo para type=note: posición (left, right, over)</summary>
    public string? Position { get; set; }
}
