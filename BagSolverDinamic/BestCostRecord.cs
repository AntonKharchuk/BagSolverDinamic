
using System.Security.Principal;

namespace BagSolverDinamic
{
    public class BestCostRecord
    {
        public List<ResultVDECombination> SetOfSelectedVDEs { get; set; }

        public BestCostRecord()
        {
            SetOfSelectedVDEs = new List<ResultVDECombination>();
        }
    }
}
