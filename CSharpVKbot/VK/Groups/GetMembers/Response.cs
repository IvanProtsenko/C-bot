using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Groups.GetMembers
{
    /// <summary>
    /// Данные, полученные в результате выполнения запроса groups.get
    /// </summary>
    [DataContract]
    public class Response
    {
        /// <summary>
        /// Количество администраторов группы
        /// </summary>
        [DataMember(Name = "count")]
        public int Count;

        /// <summary>
        /// Массив информации о пользователе
        /// </summary>
        [DataMember(Name = "items")]
        public Item[] items;
    }
}
