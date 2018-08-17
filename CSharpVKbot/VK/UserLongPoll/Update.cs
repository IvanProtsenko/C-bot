using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVKbot.VK.UserLongPoll
{
    public class Update
    {
        /// <summary>
        /// Код обновления
        /// </summary>
        public UpdateCode Code;

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserID;

        /// <summary>
        /// Флаги сообщения
        /// </summary>
        public int Flags;

        /// <summary>
        /// Идентификатор назначения. 
        /// Для пользователя: id пользователя. 
        /// Для групповой беседы: 2000000000 + id беседы. 
        /// Для сообщества: -id сообщества либо id сообщества + 1000000000 (для version = 0). 
        /// </summary>
        public int PeerID;

        /// <summary>
        /// Номер сообщения
        /// </summary>
        public int LocalID;

        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public int MessageID;

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text;

        /// <summary>
        /// идентификатор пришедшего документа
        /// </summary>
        public Attachment DocId = new Attachment();

        /// <summary>
        /// Время отправки в Unixtime
        /// </summary>
        public int TimeStamp;

        /// <summary>
        /// Значение счетчика в левом меню
        /// </summary>
        public int Count;

        /// <summary>
        /// Маска флагов сообщения
        /// </summary>
        public int Mask;

        /// <summary>
        /// OUTBOX - исходящее сообщение
        /// </summary>
        public bool IsOutbox
        {
            get { return (Flags & 2) != 0; }
        }
    }
}
