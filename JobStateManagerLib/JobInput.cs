using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobStateManagerLib
{
    public class JobInput
    {
        public int Job { get; set; }
        public int DependsOn { get; set; }
    }
}
