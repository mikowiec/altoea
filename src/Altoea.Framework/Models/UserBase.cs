

using System;

namespace Altoea.Framework.Models
{
    public class UserBase : RecordBase
    {
        /// <summary>
        /// user last name
        /// </summary>
        public virtual string LastName { get; set; }
        /// <summary>
        /// user first name
        /// </summary>
        public virtual string FirstName { get; set; }
        /// <summary>
        /// user nick name
        /// </summary>
        public virtual string NickName { get; set; }
        /// <summary>
        /// user name in english trascription
        /// </summary>
        public virtual string EnglishName { get; set; }
        /// <summary>
        /// user age
        /// </summary>
        public virtual int? Age 
        { 
            get 
            { 
                return Birthday.HasValue ? 
                    (DateTime.Now.Year - Birthday.Value.Year) : 0;
            } 
        }
        /// <summary>
        /// user birthday
        /// </summary>
        public virtual DateTime? Birthday { get; set; }
        /// <summary>
        /// user gender
        /// </summary>
        public virtual int? Sex { get; set; }
        /// <summary>
        /// user place of birth
        /// </summary>
        public virtual string Birthplace { get; set; }
        /// <summary>
        /// user address
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// user zip code
        /// </summary>
        public virtual string ZipCode { get; set; }
        /// <summary>
        /// user school
        /// </summary>
        public virtual string School { get; set; }
        /// <summary>
        /// user telephone
        /// </summary>
        public virtual string Telephone { get; set; }
        /// <summary>
        /// user mobile phone
        /// </summary>
        public virtual string MobilePhone { get; set; }
        /// <summary>
        /// user profession
        /// </summary>
        public virtual string Profession { get; set; }
        /// <summary>
        /// user marital status
        /// m arried, single, divorced, and widowed
        /// </summary>
        public virtual int? MaritalStatus  { get; set; }
        /// <summary>
        /// user hobby
        /// </summary>
        public virtual string Hobby { get; set; }
        /// <summary>
        /// Tencent QQ, instant messaging service 
        /// </summary>
        public virtual string QQ { get; set; }
        /// <summary>
        /// user email
        /// </summary>
        public virtual string Email { get; set; }
    
    }

}


