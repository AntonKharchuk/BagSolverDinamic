
namespace BagSolverDinamic
{
    public class OutputHelper
    {
        private readonly StreamWriter _writer;

        public OutputHelper(StreamWriter writer)
        {
            _writer = writer;
        }

        public void WriteBadLocation(int badLocation, int goodLocation)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Bad location is {badLocation + 1} with good {goodLocation + 1}");
            _writer.WriteLine($"Bad location is {badLocation + 1} with good {goodLocation + 1}");
            Console.ResetColor();
        }

        public void WriteCoordinates(double[] x, double[] y)
        {
            Console.WriteLine("Coordinates of possible locations (x, y):");
            _writer.WriteLine("Coordinates of possible locations (x, y):");
            for (int i = 0; i < x.Length; i++)
            {
                Console.WriteLine($"Location {i + 1}: ({x[i]}, {y[i]})");
                _writer.WriteLine($"Location {i + 1}: ({x[i]}, {y[i]})");
            }
        }

        public void WriteCostMatrix(double[,] cost)
        {
            Console.WriteLine("\nCost of VDE:");
            _writer.WriteLine("\nCost of VDE:");
            PrintMatrix(cost);
        }

        public void WritePowerMatrix(double[,] power)
        {
            Console.WriteLine("\nPower of VDE:");
            _writer.WriteLine("\nPower of VDE:");
            PrintMatrix(power);
        }

        public void WriteSummary(int budget, double minDistance, double totalPower, double totalPrice)
        {
            Console.WriteLine($"\nTotal budget: {budget}");
            _writer.WriteLine($"\nTotal budget: {budget}");
            Console.WriteLine($"Minimum distance required: {minDistance} units");
            _writer.WriteLine($"Minimum distance required: {minDistance} units");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nTotal power: {totalPower}");
            _writer.WriteLine($"\nTotal power: {totalPower}");
            Console.WriteLine($"Total cost: {totalPrice}");
            _writer.WriteLine($"Total cost: {totalPrice}");
            Console.ResetColor();
        }

        public void WriteOptimalLocations((int location, int unit)[] optimalLocations)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Optimal location and VDE for maximum energy production:");
            _writer.WriteLine("Optimal location and VDE for maximum energy production:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var loc in optimalLocations)
            {
                Console.WriteLine($"Location {loc.location + 1}, VDE {loc.unit + 1}");
                _writer.WriteLine($"Location {loc.location + 1}, VDE {loc.unit + 1}");
            }
            Console.ResetColor();
        }

        private void PrintMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                    _writer.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
                _writer.WriteLine();
            }
        }
    }

}
