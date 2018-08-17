using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Groups.Get
{
    /// <summary>
    /// Данные, полученные в результате выполнения запроса groups.get
    /// </summary>
    [DataContract]
    public class Response
    {
        /// <summary>
        /// Идентификатор группы
        /// </summary>
        [DataMember(Name = "id")]
        public int ID;

        /// <summary>
        /// Название группы
        /// </summary>
        [DataMember(Name = "name")]
        public string Name;

        /// <summary>
        /// Короткое название сообщества
        /// </summary>
        [DataMember(Name = "screen_name")]
        public string ScreenName;

        /// <summary>
        /// Закрытая ли группа
        /// </summary>
        [DataMember(Name = "is_closed")]
        public int IsClosed;

        /// <summary>
        /// Тип
        /// </summary>
        [DataMember(Name = "type")]
        public string Type;

        /// <summary>
        /// Админ ли человек
        /// </summary>
        [DataMember(Name = "is_admin")]
        public int IsAdmin;

        /// <summary>
        /// Уровень администратора, если таковым является
        /// </summary>
        [DataMember(Name = "admin_level")]
        public int AdminLevel;

        /// <summary>
        /// Участник группы ли человек
        /// </summary>
        [DataMember(Name = "is_member")]
        public int IsMember;
    }
}
