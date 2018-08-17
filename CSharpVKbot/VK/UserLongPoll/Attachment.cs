using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.UserLongPoll
{
    /// <summary>
    /// Класс типа Attachment
    /// </summary>
    [DataContract()]
    public class Attachment
    {
        /// <summary>
        /// Тип вложенного документа
        /// </summary>
        [DataMember(Name = "attach1_type")]
        public string AttachType; 
        
        /// <summary>
        /// Идентификатор вложенного документа
        /// </summary>
        [DataMember(Name = "attach1")]
        public string Attach;

        [DataMember(Name = "title")]
        public string Title;
    }
}
