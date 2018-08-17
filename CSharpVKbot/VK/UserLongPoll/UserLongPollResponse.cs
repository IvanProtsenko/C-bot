using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CSharpVKbot.VK.UserLongPoll
{
    /// <summary>
    /// Ответ на запрос User Long Poll
    /// https://vk.com/dev/using_longpoll
    /// </summary>
    [DataContract]
    public class Response
    {
        /// <summary>
        /// Номер последнего события
        /// </summary>
        [DataMember(Name = "ts")]
        public int TS;

        /// <summary>
        ///  Массив, элементы которого содержат представление новых событий.
        ///  Каждый элемент также является массивом. 
        /// </summary>
        [DataMember(Name = "updates")]
        public object[][] Update;

        /// <summary>
        /// Код ошибки
        /// 1 — история событий устарела или была частично утеряна, приложение может получать события далее, используя новое значение ts из ответа. 
        /// 2 — истекло время действия ключа, нужно заново получить key методом messages.getLongPollServer.
        /// 3 — информация о пользователе утрачена, нужно запросить новые key и ts методом messages.getLongPollServer.
        /// 4 — передан недопустимый номер версии в параметре version.
        /// </summary>
        [DataMember(Name = "failed")]
        public int? Failed;

        /// <summary>
        /// Обработка входящих событий и формирование структурированного представления
        /// </summary>
        /// <returns></returns>
        public List<Update> GetUpdates()
        {
            var list = new List<Update>();

            // Обработка всех входящих обновлений по очереди
            foreach (object[] item in Update)
            {
                var u = new Update();
                // Проверка на наличие кода обновления
                if (item[0] == null) continue;
                u.Code = (UpdateCode)item[0];
                switch (u.Code)
                {
                    // Сброс флагов сообщения
                    case UpdateCode.FlagReset:
                        u.Mask = (int)item[1];
                        break;

                    // Добавление нового сообщения
                    case UpdateCode.NewMessage:
                        u.MessageID = (int)item[1];
                        u.Flags = (int)item[2];
                        u.PeerID = (int)item[3];
                        u.TimeStamp = (int)item[4];
                        u.Text = (string)item[5];                      
                        //u.DocId = (Attachment)item[6];
                        break;

                    // Прочтение всех входящих сообщений в $peer_id, пришедших до сообщения с $local_id
                    case UpdateCode.IncomingRead:
                        u.PeerID = (int)item[1];
                        u.LocalID = (int)item[2];
                        break;

                    // Прочтение всех исходящих сообщений в $peer_id, пришедших до сообщения с $local_id
                    case UpdateCode.OutcomingRead:
                        u.PeerID = (int)item[1];
                        u.LocalID = (int)item[2];
                        break;

                    // Сброс флагов диалога $peer_id.
                    case UpdateCode.ResetPeerFlags:
                        u.PeerID = (int)item[1];
                        u.Mask = (int)item[2];
                        break;

                    // Установка флагов диалога $peer_id.
                    case UpdateCode.SetPeerFlags:
                        u.PeerID = (int)item[1];
                        u.Mask = (int)item[2];
                        break;

                    // Пользователь $user_id набирает текст в диалоге. 
                    // Событие приходит раз в ~5 секунд при наборе текста. 
                    // $flags = 1. 
                    case UpdateCode.UserIsTyping:
                        u.UserID = (int)item[1];
                        u.Flags = (int)item[2];
                        break;

                    case UpdateCode.Counter:
                        u.Count = (int)item[1];
                        break;

                    default:
                        string m = string.Format("Неизвестный код обновления: {0}", u.Code);
                        throw new NotImplementedException(m);
                }

                list.Add(u);
            }


            return list;
        }
    }
}
