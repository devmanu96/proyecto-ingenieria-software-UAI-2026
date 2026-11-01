using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace DocGenerator;

/// <summary>
/// Genera documentos Word (.docx) con diagramas embebidos usando Open XML SDK.
/// Soporta dos modos:
///   1. Generación desde cero con todas las secciones
///   2. Actualización de plantilla existente reemplazando marcadores {{NOMBRE}}
/// </summary>
public static class WordDocBuilder
{
    // Ancho máximo de imagen: ~15cm en EMUs (A4 con márgenes de 2.5cm)
    private const long MAX_IMAGE_WIDTH_EMU = 5400000L;
    // 1 pixel = 9525 EMUs (a 96 DPI)
    private const long EMU_PER_PIXEL = 9525L;

    private static uint _imageCounter = 1;

    // ═══════════════════════════════════════════════════════════════════
    // Modo 1: Crear documento desde cero
    // ═══════════════════════════════════════════════════════════════════

    public static void CreateDocument(
        string outputPath,
        Dictionary<string, string> diagramImages,
        Dictionary<string, List<ClassData>> analysis)
    {
        using var doc = WordprocessingDocument.Create(outputPath, WordprocessingDocumentType.Document);
        var mainPart = doc.AddMainDocumentPart();
        mainPart.Document = new Document(new Body());
        var body = mainPart.Document.Body!;

        // Configurar estilos
        AddStyles(mainPart);

        // Configurar tamaño de página A4
        var sectionProps = new SectionProperties(
            new PageSize { Width = 11906, Height = 16838, Orient = PageOrientationValues.Portrait },
            new PageMargin
            {
                Top = 1440,    // 1 pulgada
                Right = 1440,
                Bottom = 1440,
                Left = 1440,
                Header = 720,
                Footer = 720
            });

        // ── Portada ────────────────────────────────────────────────────
        AddEmptyLines(body, 6);
        AddParagraph(body, "Documentación Técnica", 36, true, JustificationValues.Center, "1565C0");
        AddEmptyLines(body, 1);
        AddParagraph(body, "Proyecto de Ingeniería de Software", 20, false, JustificationValues.Center, "424242");
        AddParagraph(body, "UAI — 2026", 16, false, JustificationValues.Center, "757575");
        AddEmptyLines(body, 2);
        AddParagraph(body, $"Generado automáticamente el {DateTime.Now:dd/MM/yyyy HH:mm}",
            11, false, JustificationValues.Center, "9E9E9E");
        AddPageBreak(body);

        // ── Índice (campo TOC) ─────────────────────────────────────────
        AddHeading(body, "Índice", 1);
        AddTableOfContents(body);
        AddPageBreak(body);

        // ── Secciones de diagramas ─────────────────────────────────────
        int sectionNum = 1;

        // Agrupar diagramas por tipo
        var classDiagrams = diagramImages.Where(d => d.Key.Contains("Clase")).ToList();
        var erDiagrams = diagramImages.Where(d => d.Key.Contains("Entidad-Relación") || d.Key.Contains("DER")).ToList();
        var dataDiagrams = diagramImages.Where(d => d.Key.Contains("Modelo de Datos")).ToList();
        var seqDiagrams = diagramImages.Where(d => d.Key.Contains("Secuencia")).ToList();

        // 1. Diagramas de Clases
        if (classDiagrams.Any())
        {
            AddHeading(body, $"{sectionNum}. Diagramas de Clases", 1);
            AddParagraph(body,
                "Los diagramas de clases muestran la estructura estática del sistema, " +
                "incluyendo las clases, sus atributos, métodos y las relaciones entre ellas.",
                11, false);
            AddEmptyLines(body, 1);

            int subNum = 1;
            foreach (var (title, imagePath) in classDiagrams)
            {
                AddHeading(body, $"{sectionNum}.{subNum}. {title}", 2);
                AddImage(mainPart, body, imagePath);
                AddEmptyLines(body, 1);
                subNum++;
            }
            AddPageBreak(body);
            sectionNum++;
        }

        // 2. DER
        if (erDiagrams.Any())
        {
            AddHeading(body, $"{sectionNum}. Diagrama Entidad-Relación (DER)", 1);
            AddParagraph(body,
                "El Diagrama Entidad-Relación representa las entidades del sistema y sus " +
                "relaciones, incluyendo cardinalidades y claves primarias/foráneas.",
                11, false);
            AddEmptyLines(body, 1);

            foreach (var (title, imagePath) in erDiagrams)
            {
                AddImage(mainPart, body, imagePath);
                AddEmptyLines(body, 1);
            }
            AddPageBreak(body);
            sectionNum++;
        }

        // 3. Modelo de Datos
        if (dataDiagrams.Any())
        {
            AddHeading(body, $"{sectionNum}. Modelo de Datos", 1);
            AddParagraph(body,
                "El modelo de datos muestra la estructura de cada entidad con sus " +
                "tipos de datos, claves primarias (PK) y claves foráneas (FK).",
                11, false);
            AddEmptyLines(body, 1);

            foreach (var (title, imagePath) in dataDiagrams)
            {
                AddImage(mainPart, body, imagePath);
                AddEmptyLines(body, 1);
            }
            AddPageBreak(body);
            sectionNum++;
        }

        // 4. Diagramas de Secuencia
        if (seqDiagrams.Any())
        {
            AddHeading(body, $"{sectionNum}. Diagramas de Secuencia", 1);
            AddParagraph(body,
                "Los diagramas de secuencia muestran la interacción entre los componentes " +
                "del sistema para los casos de uso principales.",
                11, false);
            AddEmptyLines(body, 1);

            int subNum = 1;
            foreach (var (title, imagePath) in seqDiagrams)
            {
                AddHeading(body, $"{sectionNum}.{subNum}. {title}", 2);
                AddImage(mainPart, body, imagePath);
                AddEmptyLines(body, 1);
                subNum++;
            }
            AddPageBreak(body);
            sectionNum++;
        }

        // 5. Diccionario de Datos (tabla con descripción de cada entidad)
        if (analysis.ContainsKey("BE"))
        {
            AddHeading(body, $"{sectionNum}. Diccionario de Datos", 1);
            AddParagraph(body,
                "El diccionario de datos describe cada entidad del dominio con sus " +
                "atributos, tipos de datos y restricciones.",
                11, false);
            AddEmptyLines(body, 1);

            foreach (var cls in analysis["BE"].Where(c => !c.IsInterface && !c.IsEnum))
            {
                AddHeading(body, cls.Name, 3);

                // Crear tabla de propiedades
                var table = CreatePropertyTable(cls);
                body.Append(table);
                AddEmptyLines(body, 1);
            }
            sectionNum++;
        }

        // Agregar propiedades de sección al final
        body.Append(sectionProps);
    }

    // ═══════════════════════════════════════════════════════════════════
    // Modo 2: Actualizar plantilla existente
    // ═══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Abre una plantilla .docx existente, busca marcadores como {{DIAGRAMA_CLASES}},
    /// {{DER}}, {{MODELO_DATOS}}, {{SECUENCIA_xxx}} y los reemplaza con las imágenes generadas.
    /// </summary>
    public static void UpdateTemplate(string templatePath, string outputPath, Dictionary<string, string> images)
    {
        // Copiar plantilla al destino
        File.Copy(templatePath, outputPath, overwrite: true);

        using var doc = WordprocessingDocument.Open(outputPath, isEditable: true);
        var mainPart = doc.MainDocumentPart!;
        var body = mainPart.Document!.Body!;

        // Mapeo de marcadores a imágenes
        var markerMap = new Dictionary<string, List<string>>();

        foreach (var (section, imagePath) in images)
        {
            string marker;
            if (section.Contains("Clase"))
                marker = "{{DIAGRAMA_CLASES}}";
            else if (section.Contains("DER") || section.Contains("Entidad"))
                marker = "{{DER}}";
            else if (section.Contains("Modelo"))
                marker = "{{MODELO_DATOS}}";
            else if (section.Contains("Secuencia"))
                marker = $"{{{{SECUENCIA_{SanitizeMarker(section)}}}}}";
            else
                continue;

            if (!markerMap.ContainsKey(marker))
                markerMap[marker] = new List<string>();
            markerMap[marker].Add(imagePath);
        }

        // Buscar y reemplazar marcadores en párrafos
        var paragraphs = body.Descendants<Paragraph>().ToList();
        foreach (var para in paragraphs)
        {
            string text = para.InnerText;
            foreach (var (marker, imagePaths) in markerMap)
            {
                if (text.Contains(marker))
                {
                    // Limpiar el párrafo del marcador
                    foreach (var run in para.Elements<Run>().ToList())
                        run.Remove();

                    // Insertar las imágenes después de este párrafo
                    var insertAfter = para;
                    foreach (var imgPath in imagePaths)
                    {
                        if (!File.Exists(imgPath)) continue;

                        var imgParagraph = new Paragraph();
                        var imgRun = new Run();
                        var drawing = CreateImageElement(mainPart, imgPath);
                        if (drawing != null)
                        {
                            imgRun.Append(drawing);
                            imgParagraph.Append(imgRun);
                            insertAfter.InsertAfterSelf(imgParagraph);
                            insertAfter = imgParagraph;
                        }
                    }

                    // Eliminar el párrafo del marcador
                    para.Remove();
                    break;
                }
            }
        }

        mainPart.Document.Save();
    }

    // ═══════════════════════════════════════════════════════════════════
    // Métodos auxiliares para construcción del documento
    // ═══════════════════════════════════════════════════════════════════

    private static void AddStyles(MainDocumentPart mainPart)
    {
        var stylesPart = mainPart.AddNewPart<StyleDefinitionsPart>();
        var styles = new Styles();

        // Estilo Heading 1
        styles.Append(CreateHeadingStyle("Heading1", "heading 1", "28", "1565C0", true));
        // Estilo Heading 2
        styles.Append(CreateHeadingStyle("Heading2", "heading 2", "24", "1976D2", true));
        // Estilo Heading 3
        styles.Append(CreateHeadingStyle("Heading3", "heading 3", "22", "2196F3", false));
        // Estilo TOC Heading
        styles.Append(CreateHeadingStyle("TOCHeading", "toc heading", "28", "1565C0", true));

        stylesPart.Styles = styles;
    }

    private static Style CreateHeadingStyle(string styleId, string name, string fontSize, string color, bool bold)
    {
        return new Style(
            new StyleName { Val = name },
            new BasedOn { Val = "Normal" },
            new NextParagraphStyle { Val = "Normal" },
            new StyleParagraphProperties(
                new SpacingBetweenLines { Before = "240", After = "120" },
                new OutlineLevel { Val = styleId switch
                {
                    "Heading1" => 0,
                    "Heading2" => 1,
                    "Heading3" => 2,
                    _ => 0
                }}),
            new StyleRunProperties(
                new RunFonts { Ascii = "Calibri", HighAnsi = "Calibri" },
                new FontSize { Val = fontSize },
                new Color { Val = color },
                new Bold { Val = OnOffValue.FromBoolean(bold) }))
        {
            Type = StyleValues.Paragraph,
            StyleId = styleId
        };
    }

    private static void AddHeading(Body body, string text, int level)
    {
        string styleId = level switch
        {
            1 => "Heading1",
            2 => "Heading2",
            3 => "Heading3",
            _ => "Heading1"
        };

        body.Append(new Paragraph(
            new ParagraphProperties(new ParagraphStyleId { Val = styleId }),
            new Run(new Text(text))));
    }

    private static void AddParagraph(Body body, string text, int fontSize = 11,
        bool bold = false, JustificationValues? justification = null, string? color = null)
    {
        var runProps = new RunProperties();
        runProps.Append(new RunFonts { Ascii = "Calibri", HighAnsi = "Calibri" });
        runProps.Append(new FontSize { Val = (fontSize * 2).ToString() });
        if (bold) runProps.Append(new Bold());
        if (color != null) runProps.Append(new Color { Val = color });

        var paraProps = new ParagraphProperties();
        if (justification.HasValue)
            paraProps.Append(new Justification { Val = justification.Value });
        paraProps.Append(new SpacingBetweenLines { After = "120" });

        body.Append(new Paragraph(paraProps, new Run(runProps, new Text(text))));
    }

    private static void AddEmptyLines(Body body, int count)
    {
        for (int i = 0; i < count; i++)
            body.Append(new Paragraph());
    }

    private static void AddPageBreak(Body body)
    {
        body.Append(new Paragraph(new Run(new Break { Type = BreakValues.Page })));
    }

    private static void AddTableOfContents(Body body)
    {
        // Campo TOC que Word actualizará al abrir el documento
        var p = new Paragraph(
            new ParagraphProperties(new ParagraphStyleId { Val = "TOCHeading" }));

        // SimpleField con instrucción TOC
        var field = new SimpleField { Instruction = @"TOC \o ""1-3"" \h \z \u" };
        field.Append(new Run(
            new RunProperties(new FontSize { Val = "22" }),
            new Text("(Hacé clic derecho → Actualizar campo para ver el índice)") { Space = SpaceProcessingModeValues.Preserve }));

        p.Append(field);
        body.Append(p);
    }

    private static void AddImage(MainDocumentPart mainPart, Body body, string imagePath)
    {
        if (!File.Exists(imagePath)) return;

        var drawing = CreateImageElement(mainPart, imagePath);
        if (drawing == null) return;

        var para = new Paragraph(
            new ParagraphProperties(
                new Justification { Val = JustificationValues.Center }),
            new Run(drawing));

        body.Append(para);
    }

    private static Drawing? CreateImageElement(MainDocumentPart mainPart, string imagePath)
    {
        try
        {
            var imagePart = mainPart.AddImagePart(ImagePartType.Png);
            using (var stream = File.OpenRead(imagePath))
            {
                imagePart.FeedData(stream);
            }

            string relId = mainPart.GetIdOfPart(imagePart);
            var (widthPx, heightPx) = GetPngDimensions(imagePath);

            // Calcular tamaño en EMUs, ajustando al ancho máximo
            long widthEmu = widthPx * EMU_PER_PIXEL;
            long heightEmu = heightPx * EMU_PER_PIXEL;

            if (widthEmu > MAX_IMAGE_WIDTH_EMU)
            {
                double ratio = (double)MAX_IMAGE_WIDTH_EMU / widthEmu;
                widthEmu = MAX_IMAGE_WIDTH_EMU;
                heightEmu = (long)(heightEmu * ratio);
            }

            uint id = _imageCounter++;

            return new Drawing(
                new DW.Inline(
                    new DW.Extent { Cx = widthEmu, Cy = heightEmu },
                    new DW.EffectExtent { LeftEdge = 0, TopEdge = 0, RightEdge = 0, BottomEdge = 0 },
                    new DW.DocProperties { Id = id, Name = $"Diagram{id}" },
                    new DW.NonVisualGraphicFrameDrawingProperties(
                        new A.GraphicFrameLocks { NoChangeAspect = true }),
                    new A.Graphic(
                        new A.GraphicData(
                            new PIC.Picture(
                                new PIC.NonVisualPictureProperties(
                                    new PIC.NonVisualDrawingProperties { Id = id, Name = $"diagram_{id}.png" },
                                    new PIC.NonVisualPictureDrawingProperties()),
                                new PIC.BlipFill(
                                    new A.Blip(
                                        new A.BlipExtensionList(
                                            new A.BlipExtension { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" }))
                                    {
                                        Embed = relId,
                                        CompressionState = A.BlipCompressionValues.Print
                                    },
                                    new A.Stretch(new A.FillRectangle())),
                                new PIC.ShapeProperties(
                                    new A.Transform2D(
                                        new A.Offset { X = 0, Y = 0 },
                                        new A.Extents { Cx = widthEmu, Cy = heightEmu }),
                                    new A.PresetGeometry(new A.AdjustValueList())
                                    {
                                        Preset = A.ShapeTypeValues.Rectangle
                                    })))
                        { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" }))
                {
                    DistanceFromTop = 0U,
                    DistanceFromBottom = 0U,
                    DistanceFromLeft = 0U,
                    DistanceFromRight = 0U
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    ⚠️  Error insertando imagen {imagePath}: {ex.Message}");
            return null;
        }
    }

    private static Table CreatePropertyTable(ClassData cls)
    {
        var table = new Table();

        // Propiedades de la tabla
        var tblProps = new TableProperties(
            new TableBorders(
                new TopBorder { Val = BorderValues.Single, Color = "E0E0E0", Size = 4 },
                new BottomBorder { Val = BorderValues.Single, Color = "E0E0E0", Size = 4 },
                new LeftBorder { Val = BorderValues.Single, Color = "E0E0E0", Size = 4 },
                new RightBorder { Val = BorderValues.Single, Color = "E0E0E0", Size = 4 },
                new InsideHorizontalBorder { Val = BorderValues.Single, Color = "E0E0E0", Size = 4 },
                new InsideVerticalBorder { Val = BorderValues.Single, Color = "E0E0E0", Size = 4 }),
            new TableWidth { Type = TableWidthUnitValues.Pct, Width = "5000" }); // 100%

        table.Append(tblProps);

        // Encabezado
        var headerRow = new TableRow();
        headerRow.Append(
            CreateHeaderCell("Atributo", "2000"),
            CreateHeaderCell("Tipo", "1500"),
            CreateHeaderCell("Acceso", "1000"),
            CreateHeaderCell("Observaciones", "2500"));
        table.Append(headerRow);

        // Filas de datos
        foreach (var prop in cls.Properties.Where(p => !p.IsPrivate))
        {
            string access = prop.HasGetter && prop.HasSetter ? "get/set" :
                           prop.HasGetter ? "get" : "set";

            string notes = "";
            if (prop.Name.StartsWith("Id") && prop.Name == $"Id{cls.Name}")
                notes = "Clave primaria";
            else if (prop.Name.StartsWith("Id") && prop.Name.Length > 2)
                notes = $"FK → {prop.Name.Substring(2)}";
            else if (!prop.HasSetter)
                notes = "Calculado (solo lectura)";

            var dataRow = new TableRow();
            dataRow.Append(
                CreateDataCell(prop.Name),
                CreateDataCell(prop.TypeName),
                CreateDataCell(access),
                CreateDataCell(notes));
            table.Append(dataRow);
        }

        return table;
    }

    private static TableCell CreateHeaderCell(string text, string width)
    {
        return new TableCell(
            new TableCellProperties(
                new TableCellWidth { Width = width },
                new Shading { Val = ShadingPatternValues.Clear, Fill = "1565C0" }),
            new Paragraph(
                new ParagraphProperties(new Justification { Val = JustificationValues.Center }),
                new Run(
                    new RunProperties(
                        new RunFonts { Ascii = "Calibri", HighAnsi = "Calibri" },
                        new FontSize { Val = "20" },
                        new Bold(),
                        new Color { Val = "FFFFFF" }),
                    new Text(text))));
    }

    private static TableCell CreateDataCell(string text)
    {
        return new TableCell(
            new TableCellProperties(
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center }),
            new Paragraph(
                new Run(
                    new RunProperties(
                        new RunFonts { Ascii = "Calibri", HighAnsi = "Calibri" },
                        new FontSize { Val = "20" }),
                    new Text(text) { Space = SpaceProcessingModeValues.Preserve })));
    }

    private static (int width, int height) GetPngDimensions(string path)
    {
        try
        {
            using var stream = File.OpenRead(path);
            var buffer = new byte[24];
            if (stream.Read(buffer, 0, 24) < 24)
                return (600, 400); // fallback

            // PNG header: bytes 16-19 = width, 20-23 = height (big-endian)
            int width = (buffer[16] << 24) | (buffer[17] << 16) | (buffer[18] << 8) | buffer[19];
            int height = (buffer[20] << 24) | (buffer[21] << 16) | (buffer[22] << 8) | buffer[23];

            return (Math.Max(width, 1), Math.Max(height, 1));
        }
        catch
        {
            return (600, 400); // fallback razonable
        }
    }

    private static string SanitizeMarker(string text)
    {
        return new string(text
            .Replace("Diagrama de Secuencia — ", "")
            .Where(c => char.IsLetterOrDigit(c) || c == '_')
            .ToArray())
            .ToUpper();
    }
}
