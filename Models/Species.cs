using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Web_Lab1_Cats
{
    public partial class Species
    {
        public Species()
        {
           Cats = new HashSet<Cats>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Назва породи")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Країна")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Шерсть")]
        public string Wool { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Розмір")]
        public string Size { get; set; }
        [Required(ErrorMessage = "Поле не може бути порожнім")]
        [Display(Name = "Середня тривалість життя")]
        public int Lifetime { get; set; }
        public string Photo { get; set; }
        //[Required(ErrorMessage = "Поле не може бути порожнім")]
        public virtual ICollection<Cats> Cats { get; set; }
    }
}
