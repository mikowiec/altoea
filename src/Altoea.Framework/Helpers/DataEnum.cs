
namespace Altoea.Framework.Helpers
{
    public enum SourceType
    {
        None = 1,
        Dictionary = 2,
        ViewData = 3
    }
    public enum RecordStatus
    {
        Active = 1,
        InActive = 2
    }
    public enum ActionType
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Design = 4,
        Publish = 5,
        Unattached = 6,
        Continue = 7,
        UnAttach = 8
    }
    public enum UserType
    {
        Administrator = 1,
        Customer = 2
    }
    public class DicKeys
    {
        public const string RecordStatus = "RecordStatus";
    }
}
