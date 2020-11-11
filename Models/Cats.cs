using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_Lab1_Cats
{
    public partial class Cats
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Ім'я котика")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Фото котика")]
        public string Photo { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
       
        [Display(Name = "Вік")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Опис")]
        public string Info { get; set; }

        [Display(Name = "Порода")]
        public int SpeciesId { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Порода ")]
        public virtual Species Species { get; set; }
    }
}
