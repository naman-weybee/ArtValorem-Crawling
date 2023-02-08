using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phillips_Crawling_Task.Service
{
    public class XpathStrings
    {

        public static readonly string AcceptCoockiesXpath = "(//strong[.='Tout accepter'])[1]";
        public static readonly string NextPageXpath = "(//a[.='Suivant'])[2]";
    }
}
