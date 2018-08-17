using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Json;
using ApiAiSDK;
using ApiAiSDK.Model;

namespace CSharpVKbot
{
    /// <summary>
    /// Бот ВКонтакте для сообщества https://vk.com/csharpcroc 
    /// cf3050b60f213fd4d4022a47ec7c8c44e0c07c194a2e3281c8e3d6e71d3d761ec336190f2f50916f3ad84
    /// </summary>
    internal class VKontakteBot
    {
        /// <summary>
        /// API vk.com
        /// </summary>
        private VK.API api = new VK.API();

        /// <summary>
        /// Соединение с сервером VK.COM
        /// </summary>
        private VK.Messages.GetLongPollServer.Response session;

        /// <summary>
        /// База данных
        /// </summary>
        private Storage.DB db = Storage.DB.Open();

        /// <summary>
        /// Распознавание текста на естественном языке
        /// </summary>
        private ApiAi ai;

        /// <summary>
        /// Словарь состояний бота
        /// </summary>
        private Dictionary<int, BotSession> state = new Dictionary<int, BotSession>();

        /// <summary>
        /// Таймер для опроса данных
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        internal VKontakteBot()
        {
            InitSessionAsync();
            // Инициализация DialogFlow
            var config = new AIConfiguration(Program.Cfg.AIToken, SupportedLanguage.Russian);
            ai = new ApiAi(config);
            timer = new System.Timers.Timer(1000 * Program.Cfg.Timer);
            timer.Elapsed += NoAction_Timer;
            timer.Elapsed += LessonWarning;
            timer.Enabled = true;
        }

        /// <summary>
        /// Создание имитации ответа AI
        /// </summary>
        /// <param name="s"> Текст сообщения </param>
        /// <param name="r"> Текст, выводящийся на экран </param>
        /// <param name="a"> Действие AI </param>
        /// <returns></returns>
        AIResponse CreateResponse(string s, string r, string a)
        {
            AIResponse response;
            response = new AIResponse()
            {
                Result = new Result()
                {
                    Source = s,
                    // действие 
                    Action = a,
                    Fulfillment = new Fulfillment()
                    {
                        Speech = r
                    }
                }
            };
                   
            return response;
        }

        /// <summary>
        /// Событие срабатывания таймера по предупреждению о занятии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LessonWarning(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Текущий момент времени
            DateTime now = DateTime.Now;
            // Время начала лекции, при котором необходимо выполнять оповещение
            DateTime ts = now + TimeSpan.FromHours(Program.Cfg.HoursBeforeLesson);
            // Поиск лекции для оповещения
            Storage.Lesson l = db.Lessons.Where(a => a.BeginLesson.Second == ts.Second && a.BeginLesson.Minute == ts.Minute && a.BeginLesson.Hour == ts.Hour && a.BeginLesson.Day == ts.Day && a.BeginLesson.Month == ts.Month).FirstOrDefault();
            // Проверка на наличие такой лекции
            if (l == null) return;
            // Находим самый маленький ID в бд
            Storage.Student s = db.Students.OrderBy(a => a.VKID).FirstOrDefault();
            // Есди лекция найдена, выполнить оповещение всех зарегистрированных студентов
            for (int counter = 0; counter < db.Students.Where(a => a.VKID != 0).Count(); counter++)
            {
                api.Messages_SendMessage(Program.Cfg.CommunityID, s.VKID, s.FirstName + ", занятие " + l.Topic + " состоится " + l.BeginLesson + " и будет длиться " + (l.EndLesson-l.BeginLesson).TotalHours + " часа. Не забудьте");
                // Переходим к следующему студенту
                s = db.Students.OrderBy(a => a.VKID).Where(a => a.VKID > s.VKID).FirstOrDefault();
            }
        }

        /// <summary>
        /// Событие срабатывания таймера по отсутствию действий пользователя
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e"></param>
        private void NoAction_Timer(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Находим настоящее время
            DateTime now = DateTime.Now;
            foreach(KeyValuePair<int, BotSession> pair in state)
            {
                if (((now - pair.Value.TimeStamp).TotalSeconds > (Program.Cfg.Timer * 60)) && (pair.Value.State != BotState.None))
                {
                    api.Messages_SendMessage(Program.Cfg.CommunityID, pair.Value.UserID, "Кажется, вы про меня забыли(");
                    pair.Value.State = BotState.None;
                    pair.Value.TimeStamp = now;
                }
            }
        }

        /// <summary>
        /// Инициализация сеанса связи
        /// </summary>
        internal void InitSessionAsync()
        {
            // Получение данных сеанса обмена данными с сообществом
            VK.Messages.GetLongPollServer.Result result = api.Messages_GetLongPollServer(Program.Cfg.CommunityID);
            session = result.Response;
        }

        /// <summary>
        /// Текущее состояние пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        private BotState GetState(int userId)
        {
            if (state.ContainsKey(userId))
            {
                return state[userId].State;
            }
            else
            {
                state[userId] = new BotSession(userId, BotState.None);
                return state[userId].State;
            }
        }

        /// <summary>
        /// Установить состояние пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="newState">Новое состояние</param>
        private void SetState(int userId, BotState newState = BotState.None)
        {
            // Проверка на существование состояния
            if (!state.ContainsKey(userId))
            {
                state[userId] = new BotSession(userId, newState);
            }
            else
            {
                // Установка нового состояния и обновление метки времени
                state[userId].State = newState;
                state[userId].TimeStamp = DateTime.Now;
            }
        }

        #region "Обработчики переходов"

        /// <summary>
        /// Отправить стандартное сообщение
        /// </summary>
        /// <param name="item">Сообщение пользователя</param>
        /// <param name="response">Ответ AI</param>
        public BotState? SendMessage(VK.UserLongPoll.Update item, AIResponse response)
        {
            // Вывод сообщения в соответствии с распознанным действием
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, response.Result.Fulfillment.Speech);
            return null;
        }

        /// <summary>
        /// Отправить сообщение, если пользователь прислал документ
        /// </summary>
        /// <param name="item"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public BotState? DocumentSent(VK.UserLongPoll.Update item, AIResponse response)
        {
            // Вывод сообщения в соответствии с распознанным действием
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, response.Result.Fulfillment.Speech);
            //api.Documents_GetById(Program.Cfg.CommunityID, item.DocId);
            return null;
        }

        /// <summary>
        /// Отметка пользователя
        /// </summary>
        /// <param name="item">Сообщение пользователя</param>
        /// <param name="response">Ответ AI</param>
        /// <returns></returns>
        public BotState? CheckinUser(VK.UserLongPoll.Update item, AIResponse response)
        {
            // время отправки запроса
            DateTime now = DateTime.Now;
            // проверка на прошедших регистрацию
            Storage.Student s = db.Students.Where(a => a.VKID == item.PeerID).FirstOrDefault();
            if (s == null)
            {
                api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, "Вас нет в списке");
                return BotState.None;
            }
            // проверка на время занятий
            Storage.Lesson l = db.Lessons.Where(n => now > n.BeginLesson && now < n.EndLesson).FirstOrDefault();
            if (l == null)
            {
                api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, "Сейчас не идут занятия");
                return BotState.None;
            }
            // проверка на повторное отмечание
            Storage.Attendance att = db.Attendances.Where(x => x.Lesson.ID == l.ID && x.Attendant.ID == s.ID).FirstOrDefault();
            if (att != null)
            {
                api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, "Вы уже отметились на занятии!");
                return BotState.None;
            }
            // заполнение отметки студентов
            Storage.Attendance at = new Storage.Attendance()
            {
                ID = Guid.NewGuid(),
                timestamp = now,
                Attendant = s,
                Lesson = l
            };
            // сохранение данных в базу
            db.Attendances.Add(at);
            db.SaveChanges();
            // Вывод сообщения в соответствии с распознанным действием
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, response.Result.Fulfillment.Speech);
            return null;
        }

        /// <summary>
        /// Отправить сообщение о непонятном содержимом
        /// </summary>
        /// <param name="item">Сообщение пользователя</param>
        /// <param name="response">Ответ AI</param>
        public BotState? SendUnknown(VK.UserLongPoll.Update item, AIResponse response)
        {
            // Вывод сообщения 
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, "Не совсем понятно, зачем мне это сейчас");
            return null;
        }

        /// <summary>
        /// Вывод пользователю инструкции по боту
        /// </summary>
        /// <param name="item"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public BotState? SendHelp(VK.UserLongPoll.Update item, AIResponse response)
        {
            // Вывод сообщения 
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, "Вот команды: \n" +
                "1) Регистрация - введите свой email, чтобы я Вас запомнил\n" +
                "2) Отметиться - я запишу, что вы присутствовали на занятии, которое идет в данный момент\n" +
                "3) Создать лекцию - провести лекцию (только для администраторов!)");
            return null;
        }

        /// <summary>
        /// Добавление нового урока
        /// </summary>
        /// <param name="item"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public BotState? AddLesson(VK.UserLongPoll.Update item, AIResponse response)
        {
            VK.Groups.GetMembers.Response m = api.Groups_GetMembers(Program.Cfg.CommunityID, Program.Cfg.GroupId);
            // Вывод сообщения 
            if (!m.items[0].id.ToString().Contains("61835649"))
            {
                api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, "Сожалею, но у вас нет прав на это действие");
                return BotState.None;
            }
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, response.Result.Fulfillment.Speech);
           
            return null;
        }

        /// <summary>
        /// Название нового урока
        /// </summary>
        /// <param name="item"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public BotState? addTopic(VK.UserLongPoll.Update item, AIResponse response)
        {
            // запись в память бота названия урока
            BotSession b = state[item.PeerID];
            b.Topic = response.Result.Source;
            // вывод сообщения
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, response.Result.Fulfillment.Speech);
            return null;
        }

        /// <summary>
        /// Отправление боту время начала создаваемого урока
        /// </summary>
        /// <param name="item"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public BotState? AddBeginTime(VK.UserLongPoll.Update item, AIResponse response)
        {
            // запись в память бота начала урока
            string str = response.Result.Parameters["addBeginTime"].ToString();
            BotSession b = state[item.PeerID];
            b.BeginLesson = DateTime.Parse(str);
            // вывод сообщения
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, response.Result.Fulfillment.Speech);
            return null;
        }

        /// <summary>
        /// Отправлению боту длительность урока
        /// </summary>
        /// <param name="item"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public BotState? addDuration(VK.UserLongPoll.Update item, AIResponse response)
        {
            // запись в память бота длительности урока
            string str = response.Result.Parameters["number"].ToString();
            BotSession b = state[item.PeerID];
            b.Durability = Convert.ToInt32(str);
            Storage.Lesson l = new Storage.Lesson()
            {
                ID = Guid.NewGuid(),
                Topic = b.Topic,
                BeginLesson = b.BeginLesson,
                EndLesson = b.BeginLesson.AddHours(b.Durability)
            };
            // сохранение данных в базу
            db.Lessons.Add(l);
            db.SaveChanges();
            // вывод сообщения
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, response.Result.Fulfillment.Speech);
            return BotState.None;
        }

        /// <summary>
        /// Проверка наличия регистрации пользователя
        /// </summary>
        /// <param name="item">Сообщение пользователя</param>
        /// <param name="response">Ответ AI</param>
        public BotState? CheckUser(VK.UserLongPoll.Update item, AIResponse response)
        {
            Storage.Student s = db.Students.Where(a => a.VKID == item.PeerID).FirstOrDefault();
            if (s != null)
            {
                api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, string.Format("{0} {1}, Вы уже зарегистрированы, E-mail: {2}", s.FirstName, s.LastName, s.Email));
                // Отмена регистрации пользователя
                return BotState.None;
            }
            // Вывод сообщения в соответствии с распознанным действием
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, response.Result.Fulfillment.Speech);
            // Переход к регистрации пользователя
            return null;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="item">Сообщение пользователя</param>
        /// <param name="response">Ответ AI</param>
        public BotState? RegisterUser(VK.UserLongPoll.Update item, AIResponse response)
        {
            // Определение E-mail
            string email = response.Result.Parameters["email"].ToString();
            // Сообщаем пользователю
            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, string.Format("Начинаю регистрацию пользователя с адресом {0}", email));
            // Поиск студента в базе данных
            if (db.Students.Where(a => a.Email == email).FirstOrDefault() != null)
            {
                api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, string.Format("Пользователь с таким E-mail уже зарегистрирован"));
                return null;
            }
            // Запрос информации о пользователе
            VK.Users.Get.Response user = api.Users_Get(Program.Cfg.CommunityID, item.PeerID);
            // Создание нового объекта
            var s = new Storage.Student()
            {
                ID = Guid.NewGuid(),
                VKID = item.PeerID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = email
            };
            db.Students.Add(s);
            // Сохраняем изменение в базе данных
            db.SaveChanges();

            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, string.Format("Пользователь успешно зарегистрирован"));
            return null;
        }

        #endregion

        /// <summary>
        /// Обработка нового текстового сообщения
        /// </summary>
        /// <param name="item">Сообщение от пользователя</param>
        private void ProcessTextMessage(VK.UserLongPoll.Update item)
        {
            AIResponse response;
            Action action;

            // Определение текущего состояния пользователя
            BotState state = GetState(item.PeerID);

            // специальная обработка добавления названия лекции
            if (state == BotState.AddLesson)
            {
                response = CreateResponse(item.Text, "Введи время проведения лекции", Action.addTopic.ToString());
            }
            else
            {
                // Обработка естественной речи искусственным интеллектом
                if (string.IsNullOrWhiteSpace(item.Text))
                {
                    // на входе пустая строка
                    response = CreateResponse(item.Text, "Получен документ", "Document");
                }
                else
                {
                    response = ai.TextRequest(item.Text);
                }
              
            }
            // Определение действия пользователя
            action = (Action)Enum.Parse(typeof(Action), response.Result.Action, true); 
            // Поиск возможного перехода
            MachineState ms = Program.Cfg.State.Where(a => a.InitialState == state && a.Action == action).FirstOrDefault();

            if (ms == null)
            {
                string s = String.Format("Не найден переход {0} -> {1}", state, action);
                api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, s);
            }
            else
            {
                // Следующее состояние
                BotState? nextState = null;
                // Проверка необходимости вызова метода
                if (!string.IsNullOrEmpty(ms.Method))
                {
                    string[] methods = ms.Method.Split(',');
                    foreach (string method in methods)
                    {
                        System.Reflection.MethodInfo info = GetType().GetMethod(method);
                        if (info == null)
                        {
                            api.Messages_SendMessage(Program.Cfg.CommunityID, item.PeerID, "Метод не найден: " + method);
                        }
                        else
                        {
                            // Вызов метода по имени
                            nextState = (BotState?)info.Invoke(this, new object[] { item, response });
                        }
                    }
                }
                // Переход в новое состояние
                SetState(item.PeerID, nextState ?? ms.NextState);
            }
        }

        /// <summary>
        /// Выполнить опрос сервера и обработать поступающие сообщения
        /// </summary>
        internal void PollServer()
        {
            // Запрос событий
            VK.UserLongPoll.Response m = api.UserLongPoll(session);
            // Проверка на наличие ошибки
            if (m.Failed.HasValue)
            {
                switch (m.Failed.Value)
                {
                    case 1: // история событий устарела или была частично утеряна, приложение может получать события далее, используя новое значение ts из ответа. 
                        session.TS = m.TS;
                        // При следующеи опросе будет использовано вновь полученное значение ts
                        return;

                    case 2: // истекло время действия ключа, нужно заново получить key 
                    case 3: // информация о пользователе утрачена, нужно запросить новые key и ts
                            // Повторная инициализация сессии
                        InitSessionAsync();
                        // При следующем опросе будут использованы вновь запрошенные выше данные сессии
                        return;

                    case 4: // передан недопустимый номер версии в параметре version. 
                        throw new VK.VKException("Передан недопустимый номер версии в параметре version");

                    default:
                        throw new VK.VKException("Неизвестный код failed: " + m.Failed.Value);
                }
            }
            // Продвижение опроса
            session.TS = m.TS;
            // Обработка входящих
            foreach (var item in m.GetUpdates())
            {
                switch (item.Code)
                {
                    case VK.UserLongPoll.UpdateCode.NewMessage:
                        // надо проверить на флаг +2 OUTBOX (должен быть сброшен)
                        // OUTBOX - это наши собственные сообщения
                        if (item.IsOutbox) continue;
                        // Трассировка
                        if (Program.Cfg.ConsoleTrace)
                        {
                            Console.WriteLine("Пользователь написал {1} - {0}", item.Text, item.Flags);
                        }
                        // Обработка входящего сообщения
                        ProcessTextMessage(item);
                        break;

                    case VK.UserLongPoll.UpdateCode.UserIsTyping:
                        // Трассировка
                        if (Program.Cfg.ConsoleTrace)
                        {
                            Console.WriteLine("Пользователь {0} пишет", item.UserID);
                        }
                        break;
                }
            }
        }
    }
}
