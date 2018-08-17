using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Documents.GetById
{
    /// <summary>
    /// результат выполнения запроса documents.GetById
    /// </summary>
    [DataContract]
    public class Result
    {
        /// <summary>
        /// Массив данных о документах, возвращаемые в ответ на запрос
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
