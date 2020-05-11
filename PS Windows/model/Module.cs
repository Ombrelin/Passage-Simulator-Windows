using System;
using System.Collections.Generic;

namespace PS_Windows.model
{
    public class Module
    {
        public String id { get; set; }
        public String name { get; set; }
        public float score { get; set; }

        public float coefficient { get; set; }

        public List<Grade> grades { get; set; }
    }
}