using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Documents.GetById
{
    /// <summary>
    /// Данные, полученные в результате выполнения запроса docs.GetById
    /// </summary>
    [DataContract]
    public class Response
    {
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        [DataMember(Name = "id")]
        public int Id;

        /// <summary>
        /// Идентификатор отправившего документ
        /// </summary>
        [DataMember(Name = "owner_id")]
        public int OwnerId;

        /// <summary>
        /// Название документа
        /// </summary>
        [DataMember(Name = "title")]
        public string Title;

        /// <summary>
        /// Размер документа
        /// </summary>
        [DataMember(Name = "size")]
        public int Size;

        /// <summary>
        /// Расширение документа
        /// </summary>
        [DataMember(Name = "ext")]
        public string Extension;

        /// <summary>
        /// Ссылка на документ
        /// </summary>
        [DataMember(Name = "url")]
        public string URL;

        /// <summary>
        /// Дата посылки документа
        /// </summary>
        [DataMember(Name = "date")]
        public int Date;

        /// <summary>
        /// Тип документа
        /// </summary>
        [DataMember(Name = "type")]
        public int Type;

        /// <summary>
        /// Информация о визуальных качествах документа
        /// </summary>
        [DataMember(Name = "preview")]
        public Preview[] preview;
    }
}
