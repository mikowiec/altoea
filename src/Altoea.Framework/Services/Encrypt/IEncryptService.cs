
namespace Altoea.Framework.Services.Encrypt
{
    public interface IEncryptService
    {
        byte[] Encrypt(byte[] source);
        byte[] Decrypt(byte[] source);
    }
}