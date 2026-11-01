using System.Reflection;
using System.Text.Json;
using DocGenerator;

// ╔══════════════════════════════════════════════════════════════════════╗
// ║  DocGenerator — Generador automático de documentación técnica       ║
// ║  Genera: Diagramas de Clases, DER, Modelo de Datos, Secuencia      ║
// ║  Salida: Documento Word (.docx) con todas las imágenes embebidas   ║
// ╚══════════════════════════════════════════════════════════════════════╝

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("╔══════════════════════════════════════════════════════╗");
Console.WriteLine("║   DocGenerator — Documentación Técnica Automática   ║");
Console.WriteLine("╚══════════════════════════════════════════════════════╝");
Console.WriteLine();

// ── Configuración ──────────────────────────────────────────────────────
string solutionDir = FindSolutionDirectory();
string outputDir = Path.Combine(solutionDir, "DocGenerator", "Output");
string diagramDir = Path.Combine(outputDir, "Diagramas");
string outputDocx = Path.Combine(outputDir, "Documentacion_Tecnica.docx");
string sequencesJson = Path.Combine(AppContext.BaseDirectory, "Config", "sequences.json");

// Si se pasa un argumento, se usa como nombre de archivo de salida
if (args.Length > 0 && args[0] == "--output" && args.Length > 1)
    outputDocx = args[1];

// Si se pasa --template, se usa como plantilla para reemplazar marcadores
string? templatePath = null;
for (int i = 0; i < args.Length - 1; i++)
{
    if (args[i] == "--template")
    {
        templatePath = args[i + 1];
        break;
    }
}

Directory.CreateDirectory(outputDir);
Directory.CreateDirectory(diagramDir);

// ── Paso 1: Análisis de ensamblados ────────────────────────────────────
Console.WriteLine("📦 Paso 1/4: Analizando ensamblados del proyecto...");

string[] assemblyNames = ["BE", "BLL", "DAL", "servicios"];
var allAnalysis = new Dictionary<string, List<ClassData>>();

foreach (var name in assemblyNames)
{
    var dllPath = FindAssemblyPath(solutionDir, name);
    if (dllPath == null)
    {
        Console.WriteLine($"  ⚠️  No se encontró el ensamblado {name}.dll — compilá el proyecto primero.");
        continue;
    }

    Console.WriteLine($"  ✅ Cargando {name}.dll ...");
    var classes = AssemblyAnalyzer.Analyze(dllPath);
    allAnalysis[name] = classes;
    Console.WriteLine($"     → {classes.Count} tipos encontrados");
}

if (allAnalysis.Count == 0)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n❌ No se encontraron ensamblados. Compilá la solución primero (Ctrl+Shift+B).");
    Console.ResetColor();
    return 1;
}

// ── Paso 2: Generar diagramas PlantUML ─────────────────────────────────
Console.WriteLine("\n🎨 Paso 2/4: Generando diagramas PlantUML...");

var diagramFiles = new Dictionary<string, string>(); // sección → ruta PNG

// 2a. Diagrama de clases por capa
foreach (var (layer, classes) in allAnalysis)
{
    string puml = PlantUmlService.GenerateClassDiagram(classes, $"Diagrama de Clases — Capa {layer.ToUpper()}");
    string pumlPath = Path.Combine(diagramDir, $"clases_{layer}.puml");
    File.WriteAllText(pumlPath, puml);
    diagramFiles[$"Diagrama de Clases — {layer.ToUpper()}"] = pumlPath;
    Console.WriteLine($"  ✅ Diagrama de clases: {layer}");
}

// 2b. Diagrama de clases completo (solo BE y BLL para no sobrecargar)
if (allAnalysis.ContainsKey("BE"))
{
    var beClasses = allAnalysis["BE"];
    var bllClasses = allAnalysis.GetValueOrDefault("BLL") ?? new List<ClassData>();
    string pumlFull = PlantUmlService.GenerateClassDiagram(
        beClasses.Concat(bllClasses).ToList(),
        "Diagrama de Clases General — BE + BLL");
    string pumlFullPath = Path.Combine(diagramDir, "clases_general.puml");
    File.WriteAllText(pumlFullPath, pumlFull);
    diagramFiles["Diagrama de Clases General"] = pumlFullPath;
    Console.WriteLine("  ✅ Diagrama de clases general (BE + BLL)");
}

// 2c. DER (Diagrama Entidad-Relación)
if (allAnalysis.ContainsKey("BE"))
{
    string pumlEr = PlantUmlService.GenerateErDiagram(allAnalysis["BE"], "Diagrama Entidad-Relación (DER)");
    string erPath = Path.Combine(diagramDir, "der.puml");
    File.WriteAllText(erPath, pumlEr);
    diagramFiles["Diagrama Entidad-Relación (DER)"] = erPath;
    Console.WriteLine("  ✅ DER (Entidad-Relación)");
}

// 2d. Modelo de Datos
if (allAnalysis.ContainsKey("BE"))
{
    string pumlDm = PlantUmlService.GenerateDataModel(allAnalysis["BE"], "Modelo de Datos");
    string dmPath = Path.Combine(diagramDir, "modelo_datos.puml");
    File.WriteAllText(dmPath, pumlDm);
    diagramFiles["Modelo de Datos"] = dmPath;
    Console.WriteLine("  ✅ Modelo de datos");
}

// 2e. Diagramas de secuencia desde JSON
if (File.Exists(sequencesJson))
{
    try
    {
        var json = File.ReadAllText(sequencesJson);
        var sequences = JsonSerializer.Deserialize<SequenceConfig>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (sequences?.Sequences != null)
        {
            foreach (var seq in sequences.Sequences)
            {
                string pumlSeq = PlantUmlService.GenerateSequenceDiagram(seq);
                string seqPath = Path.Combine(diagramDir, $"secuencia_{Sanitize(seq.Name)}.puml");
                File.WriteAllText(seqPath, pumlSeq);
                diagramFiles[$"Diagrama de Secuencia — {seq.Name}"] = seqPath;
                Console.WriteLine($"  ✅ Secuencia: {seq.Name}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  ⚠️  Error leyendo sequences.json: {ex.Message}");
    }
}
else
{
    Console.WriteLine("  ℹ️  No se encontró Config/sequences.json — se omiten diagramas de secuencia.");
}

// ── Paso 3: Renderizar diagramas a PNG ─────────────────────────────────
Console.WriteLine("\n🖼️  Paso 3/4: Renderizando diagramas a PNG (via Kroki.io)...");

var renderedImages = new Dictionary<string, string>(); // sección → ruta PNG

foreach (var (section, pumlPath) in diagramFiles)
{
    string pngPath = Path.ChangeExtension(pumlPath, ".png");
    string pumlText = File.ReadAllText(pumlPath);

    Console.Write($"  ⏳ Renderizando: {section}...");

    bool ok = await PlantUmlService.RenderToPngAsync(pumlText, pngPath);
    if (ok)
    {
        renderedImages[section] = pngPath;
        Console.WriteLine(" ✅");
    }
    else
    {
        Console.WriteLine(" ❌ (se guardó el .puml para renderizado manual)");
    }
}

// ── Paso 4: Generar documento Word ─────────────────────────────────────
Console.WriteLine("\n📄 Paso 4/4: Generando documento Word...");

if (templatePath != null && File.Exists(templatePath))
{
    Console.WriteLine($"  📋 Usando plantilla: {templatePath}");
    WordDocBuilder.UpdateTemplate(templatePath, outputDocx, renderedImages);
}
else
{
    WordDocBuilder.CreateDocument(outputDocx, renderedImages, allAnalysis);
}

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"✅ ¡Documento generado exitosamente!");
Console.WriteLine($"   📂 {outputDocx}");
Console.WriteLine($"   📂 Diagramas en: {diagramDir}");
Console.ResetColor();
Console.WriteLine();
Console.WriteLine("Tip: Los archivos .puml también se guardaron. Podés editarlos y regenerar.");
Console.WriteLine("Tip: Usá --template \"ruta.docx\" para actualizar un documento existente.");
Console.WriteLine("     Marcadores soportados: {{DIAGRAMA_CLASES}}, {{DER}}, {{MODELO_DATOS}}, {{SECUENCIA}}");

return 0;

// ── Funciones auxiliares ───────────────────────────────────────────────

static string FindSolutionDirectory()
{
    // Busca hacia arriba desde el directorio actual hasta encontrar un .sln
    string dir = AppContext.BaseDirectory;
    for (int i = 0; i < 10; i++)
    {
        if (Directory.GetFiles(dir, "*.sln").Length > 0)
            return dir;
        var parent = Directory.GetParent(dir);
        if (parent == null) break;
        dir = parent.FullName;
    }

    // Fallback: directorio de trabajo actual
    dir = Directory.GetCurrentDirectory();
    for (int i = 0; i < 10; i++)
    {
        if (Directory.GetFiles(dir, "*.sln").Length > 0)
            return dir;
        var parent = Directory.GetParent(dir);
        if (parent == null) break;
        dir = parent.FullName;
    }

    return Directory.GetCurrentDirectory();
}

static string? FindAssemblyPath(string solutionDir, string assemblyName)
{
    // Busca en bin/Debug y bin/Release de cada proyecto
    string[] searchPaths = [
        Path.Combine(solutionDir, assemblyName, "bin", "Debug"),
        Path.Combine(solutionDir, assemblyName, "bin", "Release"),
    ];

    foreach (var basePath in searchPaths)
    {
        if (!Directory.Exists(basePath)) continue;
        var files = Directory.GetFiles(basePath, $"{assemblyName}.dll", SearchOption.AllDirectories);
        if (files.Length > 0)
            return files.OrderByDescending(File.GetLastWriteTime).First();
    }
    return null;
}

static string Sanitize(string name) =>
    new string(name.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());
