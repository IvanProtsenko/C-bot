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
    public class Result
    {
        /// <summary>
        /// Данные, возвращаемые в ответ на запрос
        /// </summary>
        [DataMember(Name ="response")]
        public Response Response;

        /// <summary>
        /// Информация об ошибке
        /// </summary>
        [DataMember(Name = "error")]
        public Error Error;
    }
}
