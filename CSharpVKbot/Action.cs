using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVKbot
{
    /// <summary>
    /// Действие пользователя
    /// </summary>
    public enum Action
    {
        /// <summary>
        /// Действие не определено
        /// </summary>
        Unknown,
        /// <summary>
        /// Приветствие нового пользователя
        /// </summary>
        Welcome,
        /// <summary>
        /// Общение
        /// </summary>
        Chat,
        /// <summary>
        /// Ответ на благодарность
        /// </summary>
        Thanks,
        /// <summary>
        /// Прощание
        /// </summary>
        Bye,
        /// <summary>
        /// Вывод пользователю "инструкции" по боту
        /// </summary>
        Help,
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        Register,
        /// <summary>
        /// Обработка адрес электронной почты
        /// </summary>
        Email,
        /// <summary>
        /// Отметка пользователя на занятии
        /// </summary>
        Checkin,
        /// <summary>
        /// Добавление урока
        /// </summary>
        addLesson,
        /// <summary>
        /// Добавление названия урока
        /// </summary>
        addTopic,
        /// <summary>
        /// Добавление начала урока
        /// </summary>
        addBeginTime,
        /// <summary>
        /// Добавление продолжительности урока
        /// </summary>
        addDuration,
        /// <summary>
        /// Документ
        /// </summary>
        Document
    }
}
