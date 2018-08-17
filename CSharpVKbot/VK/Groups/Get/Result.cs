using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Groups.Get
{
    /// <summary>
    /// результат выполнения запроса groups.get
    /// </summary>
    [DataContract]
    public class Result
    {
        /// <summary>
        /// Массив данных о групах, возвращаемые в ответ на запрос
        /// </summary>
        [DataMember(Name = "response")]
        public Response[] Response;

        /// <summary>
        /// Информация об ошибке
        /// </summary>
        [DataMember(Name = "error")]
        public Error Error;
    }
}
