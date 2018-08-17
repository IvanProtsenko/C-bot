using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVKbot.VK
{
    /// <summary>
    /// Исключение при работе с VK.COM
    /// </summary>
    public class VKException : Exception
    {
        /// <summary>
        /// Конструктор по сообщению
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public VKException(string message) : base(message)
        {
        }

        /// <summary>
        /// Конструктор по ошибке
        /// </summary>
        /// <param name="error">Ошибка, возвращенная VK.COM</param>
        public VKException(Error error) : base (error.Message)
        {            
        }
    }
}
