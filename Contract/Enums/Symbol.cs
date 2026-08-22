using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Enums
{
    public enum Symbol
    {
        [Description("Tesla")]
        TSLA,
        [Description("Alphabet")]
        GOOGL,
        [Description("NVIDIA")]
        NVDA,
        [Description("Apple")]
        AAPL,
        [Description("Meta")]
        META,
        [Description("Amazon")]     
        AMZN,
        [Description("Microsoft")]
        MSFT
    }
}
