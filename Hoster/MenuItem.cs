﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoster
{
   public class MenuItem
    {
        private string _name;
            public string  Name{
            get{
            return _name;
            }
            set
            {
                _name = value;
            }
}

        public MenuItem(string name)
        {
            _name = name;
        }
    }
}
