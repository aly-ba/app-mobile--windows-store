﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    class Demo
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
        public override string ToString()
        {
            return Title;
        }
    }
}
