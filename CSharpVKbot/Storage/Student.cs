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
    /// Студент
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Уникальный идентификатор студента
        /// </summary>
        [Key()]
        public Guid ID { get; set; }

        /// <summary>
        /// Идентификатор пользователя vk.com
        /// </summary>
        [Index("IX_VKID", IsUnique = true)]
        public int VKID { get; set; }

        /// <summary>
        /// Имя студента
        /// </summary>
        [MaxLength(255)]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия студента
        /// </summary>
        [MaxLength(255)]
        public string LastName { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [EmailAddress()]
        [MaxLength(255)]
        [Required(AllowEmptyStrings = false)]
        [Index("IX_EMAIL", IsUnique = true)]
        public string Email { get; set; }

        /// <summary>
        /// Занятия, на которых был студент
        /// </summary>
        public HashSet<Attendance> Attendances { get; set; }
    }
}
