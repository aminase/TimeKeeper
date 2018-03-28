﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class BaseModel<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}