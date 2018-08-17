using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSharpVKbot
{
    /// <summary>
    /// Состояние конечного автомата
    /// </summary>
    public class MachineState
    {
        /// <summary>
        /// Исходное состояние 
        /// </summary>
        [XmlAttribute()]
        public BotState InitialState;

        /// <summary>
        /// Следующее состояние
        /// </summary>
        [XmlAttribute()]
        public BotState NextState;

        /// <summary>
        /// Действие пользователя
        /// </summary>
        [XmlAttribute()]
        public Action Action;

        /// <summary>
        /// Вызываемый метод
        /// </summary>
        [XmlAttribute()]
        public string Method;
    }
}
