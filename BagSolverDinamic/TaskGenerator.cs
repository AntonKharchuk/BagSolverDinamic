﻿

namespace BagSolverDinamic
{
    public class TaskGenerator
    {
        private readonly int locations;
        private readonly int units;
        private readonly int budget;
        private readonly double minDistance;

        public TaskGenerator(int locations, int units, int budget, int minDist)
        {
            this.locations = locations;
            this.units = units;
            this.budget = budget;
            this.minDistance = minDist;
        }

        public DenModels.Task Generate()
        {
            double[,] cost = new double[locations, units];
            double[,] power = new double[locations, units];

            var rand = new Random();

            int numberOfOptimalLocations = rand.Next(locations / 2, locations + 1);
            var optimalLocations = new (int location, int unit)[numberOfOptimalLocations];

            // Generate good location
            for (int i = 0; i < numberOfOptimalLocations; i++)
            {
                optimalLocations[i].location = -1;
                while (true)
                {
                    int index = rand.Next(locations);
                    if (!optimalLocations.Any(lu => lu.location == index))
                    {
                        optimalLocations[i].location = index;
                        break;
                    }
                }
            }

            int nuberOfBadLocsByDist = rand.Next(0, locations - numberOfOptimalLocations + 1);
            var badLocsIndexes = new int[nuberOfBadLocsByDist];

            var x = new double[locations];
            var y = new double[locations];

            // Generate coordinates
            int currentBadLoc = 0;
            for (int i = 0; i < locations; i++)
            {
                // if(optimalLocations.Any(lu => lu.location == i) && currentBadLoc < nuberOfBadLocsByDist)
                // {
                x[i] = rand.Next(0, 100 * locations / 5);
                y[i] = rand.Next(0, 100 * locations / 5);
                // }          
            }

            rand = new Random();

            int currentOptimalVDE = numberOfOptimalLocations;
            for (int i = 0; i < locations; i++)
            {
                if (!optimalLocations.Any(lu => lu.location == i) && currentBadLoc < nuberOfBadLocsByDist)
                {
                    int optimalLoc = rand.Next(0, numberOfOptimalLocations);

                    x[i] = rand.Next((int)(x[optimalLocations[optimalLoc].location] - minDistance), (int)(x[optimalLocations[optimalLoc].location] + minDistance));

                    double dSqrt = Math.Sqrt(Math.Pow(2 * y[optimalLocations[optimalLoc].location], 2)
                        - 4 * (Math.Pow(y[optimalLocations[optimalLoc].location], 2)
                        + Math.Pow(x[i] - x[optimalLocations[optimalLoc].location], 2) - Math.Pow(minDistance, 2)));

                    if (dSqrt >= y[i])
                    {
                        double min = (2 * y[optimalLocations[optimalLoc].location] - dSqrt) / 2;
                        double max = (2 * y[optimalLocations[optimalLoc].location] + dSqrt) / 2;
                        y[i] = min + rand.NextDouble() * (max - min);
                    }
                    else
                    {
                        double max = (2 * y[optimalLocations[optimalLoc].location] - dSqrt) / 2;
                        double min = (2 * y[optimalLocations[optimalLoc].location] + dSqrt) / 2;
                        y[i] = min + rand.NextDouble() * (max - min);
                    }


                    badLocsIndexes[currentBadLoc] = i;
                    currentOptimalVDE--;
                    currentBadLoc++;
                }
            }

            // Generate good units
            for (int i = 0; i < numberOfOptimalLocations; i++)
            {
                optimalLocations[i].unit = -1;
                while (true)
                {
                    int index = rand.Next(units);
                    if (!optimalLocations.Any(lu => lu.unit == index))
                    {
                        optimalLocations[i].unit = index;
                        break;
                    }
                }
            }

            var optimalVDEsCosts = GenerateOptimalVDEsCosts(budget, numberOfOptimalLocations);
            int currentOptimalLocation = 0;
            for (int i = 0; i < locations; i++)
            {
                for (int j = 0; j < units; j++)
                {
                    if (optimalLocations.Contains((i, j)))
                    {
                        cost[i, j] = optimalVDEsCosts[currentOptimalLocation];
                        power[i, j] = rand.Next(80, 100);

                        currentOptimalLocation++;
                    }
                    else if (badLocsIndexes.Contains(i))
                    {
                        cost[i, j] = rand.Next(100, 120);
                        power[i, j] = rand.Next(70, 80);
                    }
                    else
                    {
                        cost[i, j] = rand.Next(budget / locations, budget);
                        power[i, j] = rand.Next(10, 80);
                    }
                }
            }


            double totalPower = 0;
            double totalPrice = 0;
            for (int i = 0; i < optimalLocations.Length; i++)
            {
                totalPower += power[optimalLocations[i].location, optimalLocations[i].unit];
                totalPrice += cost[optimalLocations[i].location, optimalLocations[i].unit];
            }

            var coordinates = new double[locations, 2];
            for (int i = 0; i < locations; i++)
            {
                coordinates[i, 0] = x[i];
                coordinates[i, 1] = y[i];
            }

            return new DenModels.Task
            {
                Budget = budget,
                MinDist = minDistance,
                Locations = coordinates,
                Costs = cost,
                Powers = power,
                ExpectedTotalPower = totalPower
            };
        }

        

        private static int[] GenerateOptimalVDEsCosts(int budget, int numberOfOptimalVDEs)
        {
            var rand = new Random();
            int[] costs = new int[numberOfOptimalVDEs];
            int totalCost = 0;

            for (int i = 0; i < numberOfOptimalVDEs; i++)
            {
                int minCost = budget / numberOfOptimalVDEs / 2;

                int maxCost = (budget - totalCost) - (numberOfOptimalVDEs - i - 1) * minCost;
                costs[i] = rand.Next(minCost, maxCost + 1);
                totalCost += costs[i];
            }

            if (totalCost > budget)
            {
                for (int i = 0; i < numberOfOptimalVDEs; i++)
                {
                    int reducePriceOn = (totalCost - budget) < costs[numberOfOptimalVDEs - 1]
                        ? (totalCost - budget)
                        : costs[numberOfOptimalVDEs - 1] / 2;
                    costs[numberOfOptimalVDEs - 1] -= reducePriceOn;

                    totalCost -= reducePriceOn;

                    if (totalCost < budget)
                    {
                        break;
                    }
                }

            }

            return costs;
        }
    }
}
