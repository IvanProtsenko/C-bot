using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Groups.GetMembers
{
    [DataContract]
    /// <summary>
    /// Класс типа item
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [DataMember]
        public int id;

        /// <summary>
        /// Роль пользователя в группе
        /// </summary>
        [DataMember]
        public string role;
    }
}
