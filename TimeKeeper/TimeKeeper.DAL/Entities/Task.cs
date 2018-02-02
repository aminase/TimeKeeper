﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{
    public class Task : BaseClass<string>
    { 
        public string Description { get; set; }
        public decimal Hours { get; set; }

        public virtual Project Project { get; set; }
        public virtual Calendar DayId { get; set; }
    }
}