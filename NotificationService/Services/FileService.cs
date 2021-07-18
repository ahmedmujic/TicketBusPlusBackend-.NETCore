using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Logging;
using NotificationService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Services
{
    public class FileService : IFileService
    {
        private readonly IConverter _converter;
        private readonly ILogger<FileService> _logger;

        public FileService(IConverter converter, ILogger<FileService> logger)
        {
            _converter = converter;
            _logger = logger;
        }

        public byte[] CreateInvoicePDF(string body)
        {
            try
            {
                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "PDF Report"
                };
                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = body,
                    WebSettings = { DefaultEncoding = "utf-8"},
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = DateTime.Now.ToString() }
                };
                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };
                return  _converter.Convert(pdf);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateInvoicePDF));
                throw;
            }
        }
    }
}
