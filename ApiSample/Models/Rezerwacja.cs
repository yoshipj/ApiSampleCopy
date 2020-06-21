﻿using System;
using System.Collections.Generic;

namespace ApiSample.Models
{
    public partial class Rezerwacja
    {
        public int IdRezerwacja { get; set; }
        public DateTime DataOd { get; set; }
        public DateTime DataDo { get; set; }
        public int IdGosc { get; set; }
        public int NrPokoju { get; set; }
        public bool Zaplacona { get; set; }

        public virtual Gosc IdGoscNavigation { get; set; }
        public virtual Pokoj NrPokojuNavigation { get; set; }
    }
}
