using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Enums
{
    public enum StatusEnum
    {
        [Description("In-Stock")]
        InStock = 1,
        [Description("Out-Of-Stock")]
        OutOfStock = 2,
        [Description("Phased-Out")]
        PhasedOut = 3
    }
}
