using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyseVisitorsTool.Models.AccountViewModels
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Diesen Browser erinnern?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Erinnern?")]
        public bool RememberMe { get; set; }
    }
}
