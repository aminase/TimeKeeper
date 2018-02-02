﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{
    public class Team : BaseClass<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}