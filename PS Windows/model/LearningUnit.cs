using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS_Windows.model
{
    public class LearningUnit
    {
        public LearningUnit(string name) {
            this.name = name;
        }

        public String id { get; set; }
        public String name { get; set; }
        public float score { get; set; }

        public List<Module> modules { get; set; }
    }
}