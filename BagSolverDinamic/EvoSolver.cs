
using BagSolverDinamic.DenModels;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BagSolverDinamic
{
    public class EvoSolver
    {
        private  int locationsCount = 9; // Locations quantity
        private  int vdeCount = 10; // VDE quantity
        private  double d = 15; // Minimum distance
        private  double C = 300; // Budget

        private readonly Random random = new Random(69); // Seed for reproducibility
        private double[,] coordinates;
        private double[,] c_ij; // Cost
        private double[,] p_ij; // Capacity
        private List<int> validLocations;
        private double[,] filtered_c_ij;
        private double[,] filtered_p_ij;
        private double[,] filtered_coordinates;

        public EvoSolver(double[,] locations,
                                double[,] costs,
                                double[,] powers,
                                double totalBudget,
                                double minDist)
        {
            locationsCount = powers.GetLength(0);
            vdeCount = powers.GetLength(1);

            coordinates = locations;
             c_ij= costs; // Cost
             p_ij = powers; // Capacity
            FilterValidLocations();
        }

        public EvoSolution Solve()
        {
            var (bestIndividual, maxCapacity) = GeneticMain(filtered_c_ij, filtered_p_ij, validLocations.Count);

            //parse to EvoSolution

            var setVDECount = bestIndividual.Where(vdeNum => vdeNum != -1).Select(v => v);

            int[,] locAndUnit = new int[setVDECount.Count(), 2];
            int locAndUnitIndex = 0;
            for (int i = 0; i < bestIndividual.Count; i++)
            {
                if (bestIndividual[i]!=-1)
                {
                    locAndUnit[locAndUnitIndex, 0] = i;
                    locAndUnit[locAndUnitIndex, 1] = bestIndividual[i];
                    locAndUnitIndex++;
                }
            }


            var solution = new EvoSolution()
            {
                LocAndUnit = locAndUnit,
                Power = maxCapacity
            };

            return solution;

            //Console.WriteLine("Best solution: " + string.Join(", ", bestIndividual));
            //Console.WriteLine("Best capacity: " + maxCapacity);
        }

        

        private void FilterValidLocations()
        {
            bool[,] isTooClose = new bool[locationsCount, locationsCount];
            validLocations = new List<int>();
            HashSet<int> excluded = new HashSet<int>();

            for (int i = 0; i < locationsCount; i++)
            {
                for (int j = i + 1; j < locationsCount; j++)
                {
                    double dist = Math.Sqrt(Math.Pow(coordinates[i, 0] - coordinates[j, 0], 2) +
                                            Math.Pow(coordinates[i, 1] - coordinates[j, 1], 2));
                    if (dist < d)
                    {
                        isTooClose[i, j] = isTooClose[j, i] = true;
                    }
                }
            }

            for (int i = 0; i < locationsCount; i++)
            {
                if (!excluded.Contains(i))
                {
                    validLocations.Add(i);
                    for (int j = 0; j < locationsCount; j++)
                    {
                        if (isTooClose[i, j])
                        {
                            excluded.Add(j);
                        }
                    }
                }
            }

            filtered_c_ij = new double[validLocations.Count, vdeCount];
            filtered_p_ij = new double[validLocations.Count, vdeCount];
            filtered_coordinates = new double[validLocations.Count, 2];

            for (int i = 0; i < validLocations.Count; i++)
            {
                int loc = validLocations[i];
                filtered_coordinates[i, 0] = coordinates[loc, 0];
                filtered_coordinates[i, 1] = coordinates[loc, 1];
                for (int j = 0; j < vdeCount; j++)
                {
                    filtered_c_ij[i, j] = c_ij[loc, j];
                    filtered_p_ij[i, j] = p_ij[loc, j];
                }
            }
        }

        private (List<int> bestIndividual, double maxCapacity) GeneticMain(double[,] c_ij, double[,] p_ij, int m)
        {
            List<List<int>> population = new List<List<int>>();
            for (int i = 0; i < 50; i++)
            {
                population.Add(CreateIndividual(c_ij, p_ij, m));
            }

            List<double> capacities = population.Select(ind => CalculateCapacity(ind, p_ij, m)).ToList();
            for (int generation = 0; generation < 500; generation++)
            {
                List<List<int>> newGeneration = new List<List<int>>();
                while (newGeneration.Count < population.Count)
                {
                    newGeneration.AddRange(CrossoverAndMutate(population, c_ij, p_ij, m));
                }
                population = newGeneration;
                capacities = population.Select(ind => CalculateCapacity(ind, p_ij, m)).ToList();
            }

            double maxCapacity = capacities.Max();
            List<int> bestIndividual = population[capacities.IndexOf(maxCapacity)];

            return (bestIndividual, maxCapacity);
        }

        private List<int> CreateIndividual(double[,] c_ij, double[,] p_ij, int m)
        {
            List<int> individual = new List<int>();
            double totalCost = 0;
            for (int i = 0; i < m; i++)
            {
                int chosenRES = -1;
                for (int j = 0; j < vdeCount; j++)
                {
                    int res = random.Next(vdeCount);
                    if (totalCost + c_ij[i, res] <= C)
                    {
                        chosenRES = res;
                        totalCost += c_ij[i, res];
                        break;
                    }
                }
                individual.Add(chosenRES);
            }
            return individual;
        }

        private double CalculateCapacity(List<int> individual, double[,] p_ij, int m)
        {
            double capacity = 0;
            for (int i = 0; i < individual.Count; i++)
            {
                if (individual[i] >= 0)
                {
                    capacity += p_ij[i, individual[i]];
                }
            }
            return capacity;
        }

        private List<List<int>> CrossoverAndMutate(List<List<int>> population, double[,] c_ij, double[,] p_ij, int m)
        {
            List<List<int>> newGeneration = new List<List<int>>();
            for (int i = 0; i < population.Count - 1; i += 2)
            {
                List<int> parent1 = population[i];
                List<int> parent2 = population[i + 1];

                int crossoverPoint = random.Next(1, m);
                List<int> child1 = new List<int>(parent1.Take(crossoverPoint).Concat(parent2.Skip(crossoverPoint)));
                List<int> child2 = new List<int>(parent2.Take(crossoverPoint).Concat(parent1.Skip(crossoverPoint)));

                child1 = Mutate(child1, c_ij, m);
                child2 = Mutate(child2, c_ij, m);

                newGeneration.Add(child1);
                newGeneration.Add(child2);
            }
            return newGeneration;
        }

        private List<int> Mutate(List<int> individual, double[,] c_ij, int m)
        {
            int mutationPoint = random.Next(m);
            int newRES = random.Next(vdeCount);
            double currentCost = individual.Where(index => index != -1).Sum(index => c_ij[mutationPoint, index]);
            if (currentCost + c_ij[mutationPoint, newRES] <= C)
            {
                individual[mutationPoint] = newRES;
            }
            else
            {
                individual[mutationPoint] = -1;
            }
            return individual;
        }

        public void PrintCoordinates(StreamWriter writer, double[,] coords, int count)
        {
            for (int i = 0; i < count; i++)
            {
                writer.WriteLine($"({coords[i, 0]:F2}, {coords[i, 1]:F2})");
            }
        }

        public void PrintMatrix<T>(StreamWriter writer, T[,] matrix, int rows, int cols)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    writer.Write(matrix[i, j] + " ");
                }
                writer.WriteLine();
            }
        }
    }
}
