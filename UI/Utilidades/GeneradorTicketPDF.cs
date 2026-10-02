using BE;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace UI.Utilidades
{
    public static class GeneradorTicketPDF
    {
        public static string GenerarTicket(string nroComprobante, string vendedor, List<VistaCarritoDTO> detalles, List<Cobro> pagos, decimal totalVenta)
        {
            // 1. Crear carpeta de Tickets en el directorio del programa
            string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tickets");
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

            // 2. Definir ruta y nombre del archivo
            string rutaDestino = Path.Combine(carpeta, $"Ticket_{nroComprobante}.pdf");

            // 3. Crear el documento (Usamos formato A5, ideal para comprobantes medianos)
            Document doc = new Document(PageSize.A5, 30, 30, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(rutaDestino, FileMode.Create));

            doc.Open();

            // 4. Definir Tipografías
            iTextSharp.text.Font fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            iTextSharp.text.Font fontSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            iTextSharp.text.Font fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            iTextSharp.text.Font fontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

            // 5. Encabezado de la Empresa
            Paragraph header = new Paragraph("MegaDrink", fontTitulo) { Alignment = Element.ALIGN_CENTER };
            doc.Add(header);

            Paragraph subheader = new Paragraph("Comprobante de Venta", fontSubtitulo) { Alignment = Element.ALIGN_CENTER };
            doc.Add(subheader);
            doc.Add(new Paragraph("\n"));

            // 6. Datos Generales de la Operación
            doc.Add(new Paragraph($"Nro Comprobante: {nroComprobante}", fontBold));
            doc.Add(new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", fontNormal));
            doc.Add(new Paragraph($"Vendedor: {vendedor}", fontNormal));
            doc.Add(new Paragraph("\n"));

            // 7. Tabla de Productos
            PdfPTable tablaProductos = new PdfPTable(4);
            tablaProductos.WidthPercentage = 100;
            tablaProductos.SetWidths(new float[] { 4f, 1.5f, 2f, 2.5f }); // Proporción de las columnas

            // Encabezados de la tabla (usando Border = 0 para quitar los bordes laterales y BorderWidthBottom para la línea inferior)
            tablaProductos.AddCell(new PdfPCell(new Phrase("Producto", fontBold)) { Border = 0, BorderWidthBottom = 1f });
            tablaProductos.AddCell(new PdfPCell(new Phrase("Cant.", fontBold)) { Border = 0, BorderWidthBottom = 1f });
            tablaProductos.AddCell(new PdfPCell(new Phrase("P.Unit", fontBold)) { Border = 0, BorderWidthBottom = 1f });
            tablaProductos.AddCell(new PdfPCell(new Phrase("Subtotal", fontBold)) { Border = 0, BorderWidthBottom = 1f });

            // Llenar tabla con el carrito (sin bordes)
            foreach (var item in detalles)
            {
                tablaProductos.AddCell(new PdfPCell(new Phrase(item.NombreProducto, fontNormal)) { Border = 0 });
                tablaProductos.AddCell(new PdfPCell(new Phrase(item.Cantidad.ToString(), fontNormal)) { Border = 0 });
                tablaProductos.AddCell(new PdfPCell(new Phrase(item.PrecioUnitario, fontNormal)) { Border = 0 });
                tablaProductos.AddCell(new PdfPCell(new Phrase(item.Subtotal, fontNormal)) { Border = 0 });
            }
            doc.Add(tablaProductos);

            doc.Add(new Paragraph("-----------------------------------------------------------------------------------", fontNormal));

            // 8. Total
            Paragraph total = new Paragraph($"TOTAL: {totalVenta:C2}", fontSubtitulo) { Alignment = Element.ALIGN_RIGHT };
            doc.Add(total);
            doc.Add(new Paragraph("\n"));

            // 9. Detalle de los Métodos de Pago
            doc.Add(new Paragraph("Detalle de Pagos:", fontBold));
            foreach (var pago in pagos)
            {
                doc.Add(new Paragraph($"- {pago.MetodoPago}: {pago.MontoCobrado:C2}", fontNormal));
            }

            // 10. Pie de página
            doc.Add(new Paragraph("\n*** Gracias por su compra ***", fontNormal) { Alignment = Element.ALIGN_CENTER });

            doc.Close();
            writer.Close();

            return rutaDestino; // Retornamos dónde se guardó para poder abrirlo
        }
    }
}