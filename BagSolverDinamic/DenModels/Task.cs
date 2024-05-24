
using System.Text;

namespace BagSolverDinamic.DenModels
{
    public class Task
    {
        public double MinDist { get; set; }
        public double Budget { get; set; }
        public double[,] Locations { get; set; } = null!;
        public double[,] Costs { get; set; } = null!;
        public double[,] Powers { get; set; } = null!;
        public double ExpectedTotalPower { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Coordinates of possible locations (x, y):");
            for (int i = 0; i < Locations.GetLength(0); i++)
            {
                sb.AppendLine($"Location {i + 1}: ({Locations[i, 0]}, {Locations[i, 1]})");
            }

            sb.AppendLine("\nCost of VDE:");
            for (int i = 0; i < Costs.GetLength(0); i++)
            {
                for (int j = 0; j < Costs.GetLength(1); j++)
                {
                    sb.Append(Costs[i, j] + "\t");
                }
                sb.AppendLine();
            }

            sb.AppendLine("\nPower of VDE:");
            for (int i = 0; i < Powers.GetLength(0); i++)
            {
                for (int j = 0; j < Powers.GetLength(1); j++)
                {
                    sb.Append(Powers[i, j] + "\t");
                }
                sb.AppendLine();
            }

            sb.AppendLine($"\nTotal budget: {Budget}");
            sb.AppendLine($"Minimum distance required: {MinDist} units");

            return sb.ToString();
        }
    }

}
