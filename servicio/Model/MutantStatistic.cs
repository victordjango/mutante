using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myMicroservice.Model
{
    public class MutantStatistic
    {
         public int CountMutantDna { set; get; }
         public int CountHumanDna { set; get; }
         public float Ratio {
            get{
                if (CountHumanDna != 0)
                    return CountMutantDna / (float)CountHumanDna;
                else
                    return 0;
            }
        }
    }
}
