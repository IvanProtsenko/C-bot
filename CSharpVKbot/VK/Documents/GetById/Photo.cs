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
    /// Класс типа Photo
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// Размеры изображений
        /// </summary>
        [DataMember]
        public Sizes[] sizes;
    }
}
