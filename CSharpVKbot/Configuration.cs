using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace CSharpVKbot
{
    /// <summary>
    /// Конфигурация бота
    /// </summary>
    [XmlRoot(Namespace = "http://www.croc.ru/sbannikov", ElementName = "Configuration")]
    public class Configuration
    {
        /// <summary>
        /// Идентификатор группы (сообщества) vk.com
        /// </summary>
        [XmlElement()]
        public string CommunityID;

        /// <summary>
        /// Идентификатор бота в Dialogflow
        /// </summary>
        [XmlElement()]
        public string AIToken;

        /// <summary>
        /// Описание группы vk.com
        /// </summary>
        [XmlElement()]
        public string GroupId;

        /// <summary>
        /// Время срабатывания таймера
        /// </summary>
        [XmlElement()]
        public int Timer;

        /// <summary>
        /// Время оповещения пользователей перед уроком (в часах)
        /// </summary>
        [XmlElement()]
        public int HoursBeforeLesson;

        /// <summary>
        /// Отладочная трассировка в консоль
        /// </summary>
        [XmlAttribute()]
        public bool ConsoleTrace;

        /// <summary>
        /// Диаграмма переходов конечного автомата
        /// </summary>
        [XmlElement(ElementName ="State")]
        public MachineState[] State;

        /// <summary>
        /// Загрузка конфигурации из файла
        /// </summary>
        /// <param name="name">Имя файла конфигурации</param>
        /// <returns></returns>
        static public Configuration Load(string name)
        {
            // Имя исполняемого файла
            string exe = Assembly.GetExecutingAssembly().Location;
            // Имя XML-файла конфигурации
            string xml = string.Format(@"{0}\{1}", System.IO.Path.GetDirectoryName(exe), name);
            Configuration cfg;
            // Сериализатор
            XmlSerializer ser = new XmlSerializer(typeof(Configuration));
            // Читатель файла
            using (XmlReader rdr = XmlReader.Create(xml))
            {
                // Десериализация!
                cfg = (Configuration)ser.Deserialize(rdr);
            }
            // Возврат результата
            return cfg;
        }

    }
}
