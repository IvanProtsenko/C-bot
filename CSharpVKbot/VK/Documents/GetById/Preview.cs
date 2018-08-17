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
    /// Класс типа Preview
    /// </summary>
    public class Preview
    {
        /// <summary>
        /// Если это изображение
        /// </summary>
        [DataMember]
        public Photo photo;

        /// <summary>
        /// Если это видео
        /// </summary>
        [DataMember]
        public Video[] video;
    }
}
