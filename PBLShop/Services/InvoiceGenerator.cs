using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PBLShop.ViewModels;

namespace PBLShop.Services
{
    public class InvoiceGenerator
    {
        public void GenerateInvoicePdf(DonHangVM donHang)
        {
            string pdfPath = "~/Invoices/Invoice_" + donHang.MaDh + ".pdf";

            // Tạo tài liệu PDF
            PdfWriter writer = new PdfWriter(pdfPath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Thêm nội dung vào tài liệu
            document.Add(new Paragraph("Hóa đơn").SetFontSize(20).SetBold());
            document.Add(new Paragraph("Mã đơn hàng: " + donHang.MaDh));
            document.Add(new Paragraph("Tên khách hàng: " + donHang.TenKh));
            document.Add(new Paragraph("Tên nhân viên: " + donHang.TenNv));
            document.Add(new Paragraph("Ngày đặt: " + donHang.NgayDat));
            document.Add(new Paragraph("Ngày hoàn thành: " + donHang.NgayHoanThanh));
            document.Add(new Paragraph("Total Amount: " + donHang.TongTien.ToString("C0")));

            // Thêm bảng chi tiết đơn hàng
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 50, 25, 25 })).UseAllAvailableWidth();
            table.AddCell("Sản phẩm");
            table.AddCell("Số lượng");
            table.AddCell("Đơn giá");

            foreach (var item in donHang.chiTietDhVMs)
            {
                table.AddCell(item.TenSp);
                table.AddCell(item.SoLuong.ToString());
                table.AddCell(item.DonGia.ToString("N0") + " đ");
            }

            document.Add(table);

            // Đóng tài liệu
            document.Close();
        }
    }
}
