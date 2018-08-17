using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;

namespace CSharpVKbot.VK
{
    /// <summary>
    /// Библиотека для работы с vk.com
    /// </summary>
    public class API
    {
        /// <summary>
        /// Версия API vk.com
        /// </summary>
        private const string version = "v=5.73";

        /// <summary>
        ///  Время ожидания 
        ///  Так как некоторые прокси-серверы обрывают соединение после 30 секунд, рекомендуется указывать wait=25.
        ///  Максимальное значение — 90. 
        /// </summary>
        private const int wait = 25;

        /// <summary>
        /// Дополнительные опции ответа. Сумма кодов опций из списка:
        /// 2 — получать вложения
        /// 8 — возвращать расширенный набор событий
        /// 32 — возвращать pts (это требуется для работы метода messages.getLongPollHistory без ограничения в 256 последних событий)
        /// 64 — в событии с кодом 8 (друг стал онлайн) возвращать дополнительные данные в поле $extra  
        /// 128 — возвращать поле random_id (random_id может быть передан при отправке сообщения методом messages.send)
        /// </summary>
        private const int mode = 2;

        /// <summary>
        /// Версия API User Long Poll
        /// </summary>
        private const int lpVersion = 2;

        /// <summary>
        /// Клиент для выполнения запросов
        /// </summary>
        private WebClient client;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public API()
        {
            // Создание нового клиента веб-сервисов vk.com
            client = new WebClient()
            {
                Encoding = Encoding.UTF8
            };
        }

        public string GetJson(Uri uri)
        {
            WebResponse response = WebRequest.Create(uri).GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string json = reader.ReadToEnd();
            return json;
        }
        /// <summary>
        /// Возвращает данные сессии, необходимые для подключения к Long Poll серверу. 
        /// https://vk.com/dev/messages.getLongPollServer
        /// </summary>
        /// <param name="communityID">Идентификатор сообщества</param>
        /// <returns></returns>
        public  Messages.GetLongPollServer.Result Messages_GetLongPollServer(string communityID)
        {
            // Построение строки запроса API
            var uri = new UriBuilder("https://api.vk.com/method/messages.getLongPollServer");
            NameValueCollection parameters = System.Web.HttpUtility.ParseQueryString(version);
            parameters["access_token"] = communityID;
            uri.Query = parameters.ToString();
            // Десериализатор JSON
            var ser = new DataContractJsonSerializer(typeof(Messages.GetLongPollServer.Result));
            // Выполнеие запроса и получение ответа в виде JSON-строки
            string json = GetJson(uri.Uri);
#if DEBUG
            Console.WriteLine(json);
#endif
            var ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(json));
            // Десериализация
            var result = ser.ReadObject(ms) as Messages.GetLongPollServer.Result;
            // Проверка наличия ошибки при выполнении запроса 
            if (result.Error != null)
            {
                // Ошибка при обработке запроса
                throw new VKException(result.Error);
            }
            // Возврат результата
            return result;
        }

        /// <summary>
        /// Отправляет сообщение
        /// </summary>
        /// <param name="access_token">Ключ доступа сообщества</param>
        /// <param name="user_id">идентификатор пользователя, которому отправляется сообщение</param>
        /// <param name="message">текст личного сообщения. Обязательный параметр, если не задан параметр attachment</param>
        public int Messages_SendMessage(string access_token, int user_id, string message)
        {
            var uri = new UriBuilder("https://api.vk.com/method/messages.send");
            NameValueCollection parameters = System.Web.HttpUtility.ParseQueryString(version);
            parameters["access_token"] = access_token;
            parameters["user_id"] = user_id.ToString();
            // Пока не получилось справиться с кодировкой
            uri.Query = parameters.ToString() + "&message="+message;
            // Десериализатор JSON
            var ser = new DataContractJsonSerializer(typeof(Messages.Send.Result));
            string json = GetJson(uri.Uri);
#if DEBUG
            Console.WriteLine(json);
#endif
            var ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(json));
            // Десериализация
            var result = ser.ReadObject(ms) as Messages.Send.Result;
            // Проверка наличия ошибки при выполнении запроса 
            if (result.Error != null)
            {
                // Ошибка при обработке запроса
                throw new VKException(result.Error);
            }
            // Возврат результата
            return result.Response;
        }

        /// <summary>
        /// Опрос новых событий
        /// </summary>
        /// <param name="session">Сессия (следует вызвать Messages_GetLongPollServer)</param>
        /// <returns></returns>
        public UserLongPoll.Response UserLongPoll(Messages.GetLongPollServer.Response session)
        {
            var uri = new UriBuilder("https://" + session.Server);
            NameValueCollection parameters = System.Web.HttpUtility.ParseQueryString("act=a_check");
            parameters["key"] = session.Key;
            parameters["ts"] = session.TS.ToString();
            parameters["wait"] = wait.ToString();
            parameters["mode"] = mode.ToString();
            parameters["version"] = lpVersion.ToString();
            uri.Query = parameters.ToString();
            // Десериализатор JSON
          
            var ser = new DataContractJsonSerializer(typeof(UserLongPoll.Response));
            string json = GetJson(uri.Uri);           
#if DEBUG
            Console.WriteLine(json);
#endif
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            // Десериализация                    
            return ser.ReadObject(ms) as UserLongPoll.Response;
        }

        /// <summary>
        /// Запрос информации о пользователе
        /// </summary>
        /// <param name="access_token">Ключ доступа сообщества</param>
        /// <param name="user_id">Идентификатор пользователя</param>
        /// <returns>Информация о пользователе</returns>
        public Users.Get.Response Users_Get(string access_token, int user_id)
        {
            var uri = new UriBuilder("https://api.vk.com/method/users.get");
            NameValueCollection parameters = System.Web.HttpUtility.ParseQueryString(version);
            parameters["access_token"] = access_token;
            parameters["user_ids"] = user_id.ToString();
            uri.Query = parameters.ToString();
            // Десериализатор JSON
            var ser = new DataContractJsonSerializer(typeof(Users.Get.Result));
            string json = GetJson(uri.Uri);
#if DEBUG
            Console.WriteLine(json);
#endif
            var ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(json));
            // Десериализация
            var result = ser.ReadObject(ms) as Users.Get.Result;
            // Проверка наличия ошибки при выполнении запроса 
            if (result == null)
            {
                throw new VKException("Сервер вернул пустой ответ");
            }
            if (result.Error != null)
            {
                // Ошибка при обработке запроса
                throw new VKException(result.Error);
            }
            // Возврат результата
            return result.Response[0];
        }

        /// <summary>
        /// Ненужный метод, использовался вместо Get_Members
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="group_id"></param>
        /// <returns></returns>
        public Groups.Get.Response Groups_GetByID(string access_token, string group_id)
        {
            var uri = new UriBuilder("https://api.vk.com/method/groups.getById");
            NameValueCollection parameters = System.Web.HttpUtility.ParseQueryString(version);
            parameters["access_token"] = access_token;
            parameters["group_id"] = group_id;
            uri.Query = parameters.ToString();
            var ser = new DataContractJsonSerializer(typeof(Groups.Get.Result));
            string json = GetJson(uri.Uri);
#if DEBUG
            Console.WriteLine(json);
#endif
            var ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(json));
            var result = ser.ReadObject(ms) as Groups.Get.Result;
            // Проверка наличия ошибки при выполнении запроса 
            if (result == null)
            {
                throw new VKException("Сервер вернул пустой ответ");
            }
            if (result.Error != null)
            {
                // Ошибка при обработке запроса
                throw new VKException(result.Error);
            }
            // Возврат результата
            return result.Response[0];
        }

        /// <summary>
        /// Получение данных об администраторах группы
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="group_id"> Идентификатор группы </param>
        /// <returns></returns>
        public Groups.GetMembers.Response Groups_GetMembers(string access_token, string group_id)
        {
            var uri = new UriBuilder("https://api.vk.com/method/groups.getMembers");
            NameValueCollection parameters = System.Web.HttpUtility.ParseQueryString(version);
            parameters["access_token"] = access_token;
            parameters["group_id"] = group_id;
            parameters["filter"] = "managers";
            uri.Query = parameters.ToString();
            var ser = new DataContractJsonSerializer(typeof(Groups.GetMembers.Result));
            string json = GetJson(uri.Uri);
#if DEBUG
            Console.WriteLine(json);
#endif
            var ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(json));
            var result = ser.ReadObject(ms) as Groups.GetMembers.Result;
            // Проверка наличия ошибки при выполнении запроса 
            if (result == null)
            {
                throw new VKException("Сервер вернул пустой ответ");
            }
            if (result.Error != null)
            {
                // Ошибка при обработке запроса
                throw new VKException(result.Error);
            }
            // Возврат результата
            return result.Response;
        }

        /// <summary>
        /// Получение данных об отправленном документе
        /// </summary>
        /// <param name="doc_id"> Идентификатор документа </param>
        /// <returns></returns>
        public Documents.GetById.Response Documents_GetById(string access_token, string doc_id)
        {
            var uri = new UriBuilder("https://api.vk.com/method/docs.getById");
            NameValueCollection parameters = System.Web.HttpUtility.ParseQueryString(version);
            parameters["docs"] = doc_id;
            parameters["access_token"] = access_token;
            uri.Query = parameters.ToString();
            var ser = new DataContractJsonSerializer(typeof(Groups.GetMembers.Result));
            string json = GetJson(uri.Uri);
#if DEBUG
            Console.WriteLine(json);
#endif
            var ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(json));
            var result = ser.ReadObject(ms) as Documents.GetById.Result;
            // Проверка наличия ошибки при выполнении запроса 
            if (result == null)
            {
                throw new VKException("Сервер вернул пустой ответ");
            }
            if (result.Error != null)
            {
                // Ошибка при обработке запроса
                throw new VKException(result.Error);
            }
            // Возврат результата
            return result.Response;
        }
    }
}
