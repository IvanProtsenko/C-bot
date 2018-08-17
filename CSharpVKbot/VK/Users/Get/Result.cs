using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Users.Get
{
    /// <summary>
    /// Результат выполнения запроса users.get
    /// </summary>
    [DataContract]
    public class Result
    {
        /// <summary>
        /// Массив данных о пользователях, возвращаемые в ответ на запрос
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
