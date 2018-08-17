using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVKbot
{
    /// <summary>
    /// Состояние бота для пользователя
    /// </summary>
    public enum BotState
    {
        /// <summary>
        /// Исходное состояние
        /// </summary>
        None,
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        Register,
        /// <summary>
        /// Добавление урока
        /// </summary>
        AddLesson,
        /// <summary>
        /// Добавление названия урока
        /// </summary>
        AddTopic,
        /// <summary>
        /// Добавление начала урока
        /// </summary>
        AddBeginTime,
        /// <summary>
        /// Добавление продолжительности урока
        /// </summary>
        AddDuration
    }
}
