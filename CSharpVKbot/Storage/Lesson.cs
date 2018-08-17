using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSharpVKbot.Storage
{
    /// <summary>
    /// Занятие
    /// </summary>
    public class Lesson
    {
        /// <summary>
        /// Уникальный идентификатор занятия
        /// </summary>
        [Key()]
        public Guid ID { get; set; }

        /// <summary>
        /// Описание занятия
        /// </summary>
        [MaxLength(255)]
        [Required]
        public string Topic { get; set; }

        /// <summary>
        /// Начало занятия
        /// </summary>
        [Required]
        public DateTime BeginLesson { get; set; }

        /// <summary>
        /// Конец занятия
        /// </summary>
        [Required]
        public DateTime EndLesson { get; set; }

        /// <summary>
        /// Занятия, на которых был студент
        /// </summary>
        public HashSet<Attendance> Attendances { get; set; }
    }
}
