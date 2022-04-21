using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Snowball.Bookshelf.Domain
{
    public enum BookCategory
    {
        [Display(Name = "金融投资")]
        FinancialInvestment = 1
    }
}
