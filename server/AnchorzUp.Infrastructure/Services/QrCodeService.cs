using AnchorzUp.Application.Interfaces;
using QRCoder;

namespace AnchorzUp.Infrastructure.Services;

public class QrCodeService : IQrCodeService
{
    public async Task<byte[]> GenerateQrCodeAsync(string url, int size = 200)
    {
        return await Task.Run(() =>
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(size);
        });
    }

    public async Task<string> GetQrCodeAsBase64Async(string url, int size = 200)
    {
        var qrCodeBytes = await GenerateQrCodeAsync(url, size);
        return Convert.ToBase64String(qrCodeBytes);
    }
}
