using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVKbot
{
    internal class BotSession
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        internal int UserID;

        /// <summary>
        /// Состояние пользователя
        /// </summary>
       internal BotState State;

        /// <summary>
        /// Метка времени последнего взаимодействия с пользователем
        /// </summary>
        internal DateTime TimeStamp;

        /// <summary>
        /// Название события
        /// </summary>
        public string Topic;

        /// <summary>
        /// Начало занятия
        /// </summary>
        public DateTime BeginLesson;

        /// <summary>
        /// Длительность занятия
        /// </summary>
        public int Durability;

        /// <summary>
        /// Конструктор со значениями
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="state">Состояние пользователя</param>
        internal BotSession(int id, BotState state = BotState.None)
        {
            UserID = id;
            State = state;
            TimeStamp = DateTime.Now;
        }
    }
}
