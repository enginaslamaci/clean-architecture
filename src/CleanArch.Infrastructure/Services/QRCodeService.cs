using CleanArch.Application.Abstractions.Infrastructure.Services;
using QRCoder;

namespace CleanArch.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {
        public byte[] GenerateQRCode(string text)
        {
            QRCodeGenerator generator = new();
            QRCodeData data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(data);

            byte[] byteGraphic = qrCode.GetGraphic(10, new byte[] { 52, 53, 36 }, new byte[] { 240, 240, 240 });
            return byteGraphic;

            //"data:image/png;base64," + Convert.ToBase64String(byteGraphic); //if you want to image from byte
        }
    }
}
