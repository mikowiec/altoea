

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Altoea.Framework.Models
{
    public class RecordModel
    {
        /// <summary>
        /// record title
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// record description
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// record status
        /// </summary>
        public virtual int? Status { get; set; }
        /// <summary>
        /// record creator ID
        /// </summary>
        public virtual string CreateBy { get; set; }
        /// <summary>
        /// record creator name
        /// </summary>
        public virtual string CreateByName { get; set; }
        /// <summary>
        /// record creation date
        /// </summary>
        public virtual DateTime? CreatDate { get; set; }
        /// <summary>
        /// record editor ID
        /// </summary>
        public virtual string LastUpdateBy { get; set; }
        /// <summary>
        /// record editor name
        /// </summary>
        public virtual string LastUpdateByName { get; set; }
        /// <summary>
        /// record edition date
        /// </summary>
        public virtual DateTime? LastUpdateDate { get; set; }
        /// <summary>
        /// record action type
        /// </summary>
        [NotMapped]
        public virtual Constant.ActionType? ActionType { get; set; }    
    }


}






