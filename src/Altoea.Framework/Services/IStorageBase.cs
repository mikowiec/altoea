

namespace Altoea.Framework.Services
{
    public interface IStorageBase
    {
        string CreateFolder(string folder);
        string DeleteFolder(string folder);
        string SaveFile(string file);
        string DeleteFile(string file);
    }

}



