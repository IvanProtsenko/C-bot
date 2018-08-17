using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Groups.GetMembers
{
    /// <summary>
    /// результат выполнения запроса groups.getMembers
    /// </summary>
    [DataContract]
    public class Result
    {
        /// <summary>
        /// Массив данных о участниках групп, возвращаемые в ответ на запрос
        /// </summary>
        [DataMember(Name = "response")]
        public Response Response;

        /// <summary>
        /// Информация об ошибке
        /// </summary>
        [DataMember(Name = "error")]
        public Error Error;
    }
}
