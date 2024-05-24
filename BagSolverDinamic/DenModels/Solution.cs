
using System.Text;

namespace BagSolverDinamic.DenModels
{
    public class Solution
    {
        public int[,] LocAndUnit { get; set; } = null!;
        public double Power { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Founded solution is {Power} units of power. Placement:");

            sb.AppendLine();

            for (int i = 0; i < LocAndUnit.GetLength(0); i++)
            {
                sb.AppendLine();
                sb.Append($"Location: {LocAndUnit[i,0]}  VDEid: {LocAndUnit[i, 1]}");
            }

            return sb.ToString();
        }
    }
}
