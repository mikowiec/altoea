
namespace Altoea.Framework.Helpers
{
    public class RegExp
    {
        /// <summary>
        /// email parser
        /// </summary>
        public const string Email = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        /// <summary>
        /// url parser
        /// </summary>
        public const string Url = @"[a-zA-z]+://[^\s]*";
        /// <summary>
        /// chinese parser
        /// </summary>
        public const string Chinese = @"[\u4e00-\u9fa5]";
        /// <summary>
        /// HTML parser
        /// </summary>
        public const string Html = @"<(\S*?)[^>]*>.*?</\1>|<.*? />";
        /// <summary>
        /// user name parser
        /// </summary>
        public const string UserName = @"^[a-zA-Z][a-zA-Z0-9_]{4,15}$";
        /// <summary>
        /// fixed phone parser
        /// </summary>
        public const string ChinesePhone = @"\d{3}-\d{8}|\d{4}-\d{7}";
        /// <summary>
        /// zip code parser
        /// </summary>
        public const string ZipCode = @"[0-9]\d{5}(?!\d)";
        /// <summary>
        /// ID card parser
        /// </summary>
        public const string CardID = @"\d{15}|\d{18}";
        /// <summary>
        /// IP address parser
        /// </summary>
        public const string IpAddress = @"\d+\.\d+\.\d+\.\d+";
        /// <summary>
        /// positive integer parser
        /// </summary>
        public const string PositiveIntegers = @"^[1-9]\d*$";
        /// <summary>
        /// negative integer parser
        /// </summary>
        public const string NegativeIntegers = @"^-[1-9]\d*$";
        /// <summary>
        /// integer parser
        /// </summary>
        public const string Integer = @"^-?[0-9]+$";
        /// <summary>
        /// positive integer + 0 parser
        /// </summary>
        public const string PositiveIntegersAndZero = @"^[0-9]+$";
        /// <summary>
        /// negative integer + 0 parser
        /// </summary>
        public const string NegativeIntegersAndZero = @"^-[0-9]+$";
        /// <summary>
        /// positive float parser
        /// </summary>
        public const string Float = @"^(\-|\+)?\d+(\.\d+)?$";
        /// <summary>
        /// english letters parser
        /// </summary>
        public const string Letters = @"^[A-Za-z]+$";
        /// <summary>
        /// english uppercase letters parser
        /// </summary>
        public const string UppercaseLetters = @"^[A-Z]+$";
        /// <summary>
        /// english lowercase letters parser
        /// </summary>
        public const string LowercaseLetters = @"^[a-z]+$";
        /// <summary>
        /// english letters + numbers parser
        /// </summary>
        public const string LettersAndNumber = @"^[A-Za-z0-9]+$";
        /// <summary>
        /// english letters + numbers + `_` parser
        /// </summary>
        public const string LettersAndNumberAndLine = @"^\w+$";
    }
}
