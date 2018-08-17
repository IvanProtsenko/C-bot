using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Messages.GetLongPollServer
{
    /// <summary>
    /// Результат выполнения запроса messages.getLongPollServer
    /// </summary>
    [DataContract]
    public class Response
    {
        /// <summary>
        /// Ключ доступа
        /// </summary>
        [DataMember(Name ="key")]
        public string Key;

        /// <summary>
        /// Сервер для опроса
        /// </summary>
        [DataMember(Name = "server")]
        public string Server;

        /// <summary>
        /// Номер последнего события
        /// </summary>
        [DataMember(Name = "ts")]
        public int TS;
    }

}
