using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class PollCreateViewModel
    {
        
        public string Title { get; set; }

       
        public string Option1Text { get; set; }

      
        public string Option2Text { get; set; }

        public string? Option3Text { get; set; }
    }
}

