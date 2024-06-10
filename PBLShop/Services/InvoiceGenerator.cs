using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PBLShop.ViewModels;
using System.Diagnostics;
using HtmlToPdfMaster;

namespace PBLShop.Services
{
    public class InvoiceGenerator
    {
        public void GenerateInvoicePdf(DonHangVM donHang)
        {
            var templateContent = File.ReadAllText("~/Invoices/Invoice_Template.html");

        }
    }
}
