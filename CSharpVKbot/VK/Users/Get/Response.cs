using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Users.Get
{
    /// <summary>
    /// Данные, полученные в результате выполнения запроса users.get
    /// </summary>
    [DataContract]
    public class Response
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [DataMember(Name = "id")]
        public int ID;

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember(Name = "first_name")]
        public string FirstName;

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [DataMember(Name = "last_name")]
        public string LastName;
    }
}
