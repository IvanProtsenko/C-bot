using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    [DataContract]
    public class Error
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        [DataMember(Name = "error_code")]
        public int Code;

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        [DataMember(Name ="error_msg")]
        public string Message;
    }
}
