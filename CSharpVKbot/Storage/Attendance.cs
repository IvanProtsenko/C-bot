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
    /// Присутствие на занятии
    /// </summary>
    public class Attendance
    {
        /// <summary>
        /// Уникальный идентификатор занятия
        /// </summary>
        [Key()]
        public Guid ID { get; set; }

        /// <summary>
        /// Время проведения занятия
        /// </summary>
        [Required]
        public DateTime timestamp { get; set; }

        /// <summary>
        /// Присутствующий студент
        /// </summary>
        [Required]
        public virtual Student Attendant { get; set; }

        /// <summary>
        /// Занятие, на котором студент присутствует
        /// </summary>
        [Required]
        public virtual Lesson Lesson { get; set; }
    }
}
