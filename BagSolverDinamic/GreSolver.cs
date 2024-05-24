
using BagSolverDinamic.DenModels;

namespace BagSolverDinamic
{
    public class GreSolver
    {
        private readonly int numLocations;
        private readonly int numUnits;
        private readonly double minDist;
        private  double[,] costs;
        private readonly double[,] powers;
        private readonly double[,] locations;
        private readonly double budget;

        public GreSolver(double[,] locations,
                               double[,] costs,
                               double[,] powers,
                               double totalBudget,
                               double minDist)
        {
            numLocations = locations.GetLength(0);
            numUnits = costs.GetLength(1);
            this.costs = costs;
            this.powers = powers;
            this.locations = locations;
            this.minDist = minDist;
            budget = totalBudget;
        }

        public GreSolution Solve()
        {
            
            var locs = new List<int>();
            var units = new List<int>();

            double resC = 0;
            double resP = 0;

            while (true)
            {
                if (locs.Count == numLocations)
                {
                    break;
                }
                var minCostIndex = MinCostIndex();
                if (minCostIndex.loc==-1|| minCostIndex.unit == -1)
                    break;
                var minCost = minCostIndex.cost;

                if (resC+ minCost<=budget)
                {
                    if (IsDistanseCorrect(minCostIndex.loc, locs))
                    {
                        locs.Add(minCostIndex.loc);
                        units.Add(minCostIndex.unit);
                        resC += minCostIndex.cost;
                        resP += powers[minCostIndex.loc, minCostIndex.unit];
                    }
                }
                else
                {
                    break;
                }
            }

            int[,] locAndUnit = new int[locs.Count, 2];
            for (int i = 0; i < locAndUnit.GetLength(0); i++)
            {
                locAndUnit[i, 0] = locs[i];
                locAndUnit[i, 1] = units[i];
            }


            var solution = new GreSolution()
            {
                LocAndUnit = locAndUnit,
                Power = resP
            };

            return solution;
        }
        private bool IsDistanseCorrect(int loc ,List<int> locs)
        {
            var locX = locations[loc,0];
            var locY = locations[loc,1];

            foreach (var locIndex in locs)
            {
                if (locIndex == loc)
                {
                    return false;
                }
                var distanse = CalculateDistance(locX, locY, locations[locIndex, 0], locations[locIndex, 1]);
                if (distanse<minDist)
                {
                    return false;
                }
            }
            return true;
        }
        private double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - y1, 2) + Math.Pow(y1 - y2, 2));
        }
        private (int loc, int unit, double cost) MinCostIndex()
        {
            int minX = -1;
            int minY = -1;
            double minC = double.MaxValue;
            for (int i = 0; i < numLocations; i++)
            {
                for (int j = 0; j < numUnits; j++)
                {
                    if (costs[i,j]<minC)
                    {
                        minX = i; 
                        minY = j; 
                        minC = costs[i, j];
                    }
                }
            }
            double cost = -1;
            if (minX != -1 && minY!=-1)
            {
                cost = costs[minX, minY];
                costs[minX, minY] = double.MaxValue;
            }
            return (minX, minY, cost);    
        }
    }
}
