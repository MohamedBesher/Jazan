using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Saned.Jazan.Api.Models
{
    public class ConfirmViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Code { get; set; }
    }
}