using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVKbot.VK.UserLongPoll
{
    /// <summary>
    /// Код события
    /// </summary>
    public enum UpdateCode
    {
        Unknown = 0,
        /// <summary>
        /// Сброс флагов сообщения (FLAGS&=~$mask) 
        /// </summary>
        FlagReset = 3,
        /// <summary>
        /// Добавление нового сообщения
        /// </summary>
        NewMessage = 4,
        /// <summary>
        /// Прочтение всех входящих сообщений в $peer_id, пришедших до сообщения с $local_id. 
        /// </summary>
        IncomingRead = 6,
        /// <summary>
        /// Прочтение всех исходящих сообщений в $peer_id, пришедших до сообщения с $local_id
        /// </summary>
        OutcomingRead = 7,
        /// <summary>
        /// Сброс флагов диалога $peer_id. 
        /// Соответствует операции (PEER_FLAGS &= ~$flags). 
        /// Только для диалогов сообществ.  
        /// </summary>
        ResetPeerFlags = 10,
        /// <summary>
        /// Установка флагов диалога $peer_id. 
        /// Соответствует операции (PEER_FLAGS|= $flags). 
        /// Только для диалогов сообществ.  
        /// </summary>
        SetPeerFlags = 12,
        /// <summary>
        /// Пользователь набирает текст в диалоге. 
        /// Событие приходит раз в ~5 секунд при наборе текста. 
        /// </summary>
        UserIsTyping = 61,
        /// <summary>
        /// Счетчик в левом меню стал равен $count
        /// </summary>
        Counter = 80
    }
}
