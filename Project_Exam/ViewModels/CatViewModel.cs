using Microsoft.AspNetCore.Http;
using Project_Exam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Exam.ViewModels
{
    public class CatViewModel
    {
        public int CatId { get; set; }

        [Required, StringLength(50), Display(Name = "Cat Name")]
        public string CatName { get; set; }

        [Required, Column(TypeName = "date")
            , Display(Name = "Date of Birth")
            , DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Dob { get; set; }

        [Required, EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        public bool Available { get; set; }

        public IFormFile Picture { get; set; }

        [Required, ForeignKey("Breed")]
        public int BreedId { get; set; }

        //Navigation
        public virtual Breed Breed { get; set; }
    }
}
