
using System.Text;

namespace BagSolverDinamic.DenModels
{
    public class AntSolution : Solution
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("AntSolution");
            sb.AppendLine($"Founded solution is {Power} units of power. Placement:");
            sb.Append(" ");
            for (int unit = 0; unit < LocAndUnit.GetLength(1); unit++)
            {
                sb.Append($"  U{unit + 1}");
            }

            for (int loc = 0; loc < LocAndUnit.GetLength(0); loc++)
            {

                sb.AppendLine();

                sb.Append($"L{loc + 1}  ");
                for (int unit = 0; unit < LocAndUnit.GetLength(1); unit++)
                {
                    if (LocAndUnit[loc, unit] == 1)
                    {
                        sb.Append("X   ");
                    }
                    else
                    {
                        sb.Append("-   ");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
    public class BagSolution: Solution
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BagSolution");

            sb.AppendLine($"Founded solution is {Power} units of power. Placement:");

            for (int i = 0; i < LocAndUnit.GetLength(0); i++)
            {
                sb.AppendLine($"Location: {LocAndUnit[i, 0]}  VDEid: {LocAndUnit[i, 1]}");
            }

            return sb.ToString();
        }
    }

    public class Solution
    {
        public int[,] LocAndUnit { get; set; } = null!;
        public double Power { get; set; }
    }
}
