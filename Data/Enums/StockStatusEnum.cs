using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Enums
{
    public enum StockStatusEnum
    {
        InStock = 1,
        OutOfStock = 2,
        PhasedOut = 3
    }
}
