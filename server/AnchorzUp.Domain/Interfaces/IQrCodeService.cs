namespace AnchorzUp.Domain.Interfaces;

public interface IQrCodeService
{
    Task<byte[]> GenerateQrCodeAsync(string url, int size = 200);
    Task<string> GetQrCodeAsBase64Async(string url, int size = 200);
}
