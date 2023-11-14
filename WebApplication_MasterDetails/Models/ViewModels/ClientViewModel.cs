using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WebApplication_MasterDetails.Models.ViewModels
{
    public class ClientViewModel
    {
        public ClientViewModel()
        {
            this.SpotList = new List<int>();
        }
        public int ClientId { get; set; }
        [Required, StringLength(50), Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Required, Column(TypeName ="date"), Display(Name = "Date of Birth"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        public HttpPostedFileBase PictureFile { get; set; }
        [Display(Name = "Marital Status")]
        public bool MaritalStatus { get; set; }
        public List<int> SpotList { get; set; }
    }
}