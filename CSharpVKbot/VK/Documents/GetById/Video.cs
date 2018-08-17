using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.Documents.GetById
{
    [DataContract]
    /// <summary>
    /// Класс типа Video
    /// </summary>
    public class Video
    {
        /// <summary>
        /// Источник видео
        /// </summary>
        [DataMember]
        public string src;

        /// <summary>
        /// Ширина окна
        /// </summary>
        [DataMember]
        public int width;

        /// <summary>
        /// Высота окна
        /// </summary>
        [DataMember]
        public int height;

        /// <summary>
        /// Размер видео
        /// </summary>
        [DataMember]
        public int filesize;
    }
}
