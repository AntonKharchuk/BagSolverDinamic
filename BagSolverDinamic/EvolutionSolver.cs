
using BagSolverDinamic.DenModels;

namespace BagSolverDinamic
{
    class EvolutionSolver
    {
        static Random random = new Random();

         int m = 30; // locations quantity
         int n = 50; // RES quantity
         double d = 5; // minimum distance
         double C = 5000; // budget

         int populationSize = 50;
         int generationsNumber = 100;
         double crossoverRate = 0.7;

        static double[,] coordinates; // locations 
        static double[,] c_ij; // cost
        static double[,] p_ij; // capacity

        public EvolutionSolver(double[,] locations,
                              double[,] costs,
                              double[,] powers,
                              double totalBudget,
                              double minDist)
        {
            m = powers.GetLength(0);
            n = powers.GetLength(1);

            coordinates = locations;
            c_ij = costs; // Cost
            p_ij = powers; // Capacity
            //FilterValidLocations();
        }

        public EvoSolution Solve()
        {
            InitializeVariables();


            bool[,] isTooClose = new bool[m, m];
            List<int> validLocations = new List<int>();

            // calculate the distance matrix
            for (int i = 0; i < m; i++)
            {
                for (int j = i + 1; j < m; j++)
                {
                    double dist = Math.Sqrt(Math.Pow(coordinates[i, 0] - coordinates[j, 0], 2) + Math.Pow(coordinates[i, 1] - coordinates[j, 1], 2));
                    if (dist < d)
                    {
                        isTooClose[i, j] = isTooClose[j, i] = true;
                    }
                }
            }

            // check if exceeding minimal distance
            HashSet<int> excluded = new HashSet<int>();
            for (int i = 0; i < m; i++)
            {
                if (!excluded.Contains(i))
                {
                    validLocations.Add(i);
                    for (int j = 0; j < m; j++)
                    {
                        if (isTooClose[i, j])
                        {
                            excluded.Add(j);
                        }
                    }
                }
            }

            // adjust c_ij and p_ij according to filtering
            double[,] filtered_c_ij = new double[validLocations.Count, n];
            double[,] filtered_p_ij = new double[validLocations.Count, n];
            double[,] filtered_coordinates = new double[validLocations.Count, 2];

            for (int i = 0; i < validLocations.Count; i++)
            {
                int loc = validLocations[i];
                filtered_coordinates[i, 0] = coordinates[loc, 0];
                filtered_coordinates[i, 1] = coordinates[loc, 1];
                for (int j = 0; j < n; j++)
                {
                    filtered_c_ij[i, j] = c_ij[loc, j];
                    filtered_p_ij[i, j] = p_ij[loc, j];
                }
            }

            // output adjusted variables after filtering
            Console.WriteLine("Valid Locations:");
            PrintCoordinates(filtered_coordinates, validLocations.Count);
            Console.WriteLine("Filtered Cost:");
            PrintMatrix(filtered_c_ij, validLocations.Count, n);
            Console.WriteLine("Filtered Capacity:");
            PrintMatrix(filtered_p_ij, validLocations.Count, n);

            Console.WriteLine("Total Budget: " + C);
            Console.WriteLine("Minimal distance: " + d);

            // create a solution
            var (bestIndividual, maxCapacity) = GeneticMain(filtered_c_ij, filtered_p_ij, validLocations.Count, crossoverRate);

            var setVDECount = bestIndividual.Where(vdeNum => vdeNum != -1).Select(v => v);

            int[,] locAndUnit = new int[setVDECount.Count(), 2];
            int locAndUnitIndex = 0;
            for (int i = 0; i < bestIndividual.Count; i++)
            {
                if (bestIndividual[i] != -1)
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



         void InitializeVariables()
        {
            coordinates = new double[m, 2];
            c_ij = new double[m, n];
            p_ij = new double[m, n];

            for (int i = 0; i < m; i++)
            {
                coordinates[i, 0] = random.Next(0, 50);
                coordinates[i, 1] = random.Next(0, 50);
                for (int j = 0; j < n; j++)
                {
                    c_ij[i, j] = random.Next(50, 200);
                    p_ij[i, j] = random.Next(100, 500);
                }
            }
        }

         int participantsNumber = 3;

         List<int> TournamentSelection(List<List<int>> population, List<double> capacities, int participantsNumber)
        {
            // combine population and capacities into a list of participants
            var combinedList = population.Zip(capacities, (individual, capacity) => new { Individual = individual, Capacity = capacity }).ToList();

            // shuffle the list to ensure random selection
            var shuffledParticipants = combinedList.OrderBy(x => random.Next()).ToList();

            // select a participants comparing their capacity values
            var tournamentParticipants = shuffledParticipants.Take(participantsNumber).ToList();
            var winner = tournamentParticipants.Aggregate((best, next) => next.Capacity > best.Capacity ? next : best).Individual;

            return winner;
        }

         Tuple<List<int>, double> GeneticMain(double[,] c_ij, double[,] p_ij, int m, double crossoverRate)
        {
            List<List<int>> population = new List<List<int>>();
            for (int i = 0; i < populationSize; i++)
            {
                population.Add(CreateIndividual(c_ij, p_ij, m));
            }

            List<double> capacities = population.Select(ind => CalculateCapacity(ind, p_ij, m)).ToList();
            for (int generation = 0; generation < generationsNumber; generation++)
            {
                List<List<int>> newGeneration = new List<List<int>>();
                while (newGeneration.Count < population.Count)
                {
                    List<int> parent1 = TournamentSelection(population, capacities, participantsNumber);
                    List<int> parent2 = TournamentSelection(population, capacities, participantsNumber);
                    newGeneration.AddRange(CrossoverAndMutate(new List<List<int>> { parent1, parent2 }, c_ij, p_ij, m, crossoverRate));
                }
                population = newGeneration;
                capacities = population.Select(ind => CalculateCapacity(ind, p_ij, m)).ToList();
            }

            double maxCapacity = capacities.Max();
            List<int> bestIndividual = population[capacities.IndexOf(maxCapacity)];

            return Tuple.Create(bestIndividual, maxCapacity);
        }

         List<int> CreateIndividual(double[,] c_ij, double[,] p_ij, double m)
        {
            List<int> individual = new List<int>();
            double totalCost = 0;
            for (int i = 0; i < m; i++)
            {
                // if no RES placed on current location
                int chosenRES = -1;

                for (int j = 0; j < n; j++)
                {
                    int res = random.Next(n);
                    if (totalCost + c_ij[i, res] <= C)
                    {
                        chosenRES = res;
                        totalCost += c_ij[i, res];

                        // if RES placed on current location
                        break;
                    }
                }
                // add to a solution
                individual.Add(chosenRES);
            }
            return individual;
        }

         double CalculateCapacity(List<int> individual, double[,] p_ij, double m)
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

         List<List<int>> CrossoverAndMutate(List<List<int>> population, double[,] c_ij, double[,] p_ij, int m, double crossoverRate)
        {
            List<List<int>> newGeneration = new List<List<int>>();
            for (int i = 0; i < population.Count - 1; i += 2)
            {
                List<int> parent1 = population[i];
                List<int> parent2 = population[i + 1];
                double rand = random.NextDouble();

                if (rand < crossoverRate)
                {
                    // crossover
                    int crossoverPoint = random.Next(1, m);
                    List<int> child1 = new List<int>(parent1.Take(crossoverPoint).Concat(parent2.Skip(crossoverPoint)));
                    List<int> child2 = new List<int>(parent2.Take(crossoverPoint).Concat(parent1.Skip(crossoverPoint)));
                    newGeneration.Add(child1);
                    newGeneration.Add(child2);
                }
                else
                {
                    // mutation
                    List<int> mutatedParent1 = Mutate(new List<int>(parent1), c_ij, m);
                    List<int> mutatedParent2 = Mutate(new List<int>(parent2), c_ij, m);
                    newGeneration.Add(mutatedParent1);
                    newGeneration.Add(mutatedParent2);
                }
            }
            return newGeneration;
        }

         List<int> Mutate(List<int> individual, double[,] c_ij, int m)
        {
            int mutationPoint = random.Next(m);
            int newRES = random.Next(n);

            // current cost
            double currentCost = individual.Where(index => index != -1).Sum(index => c_ij[mutationPoint, index]);

            // check whether the budget constraint is met
            if (currentCost + c_ij[mutationPoint, newRES] <= C)
            {
                individual[mutationPoint] = newRES; // mutate
            }
            else
            {
                individual[mutationPoint] = -1; // choose nothing if the mutation would exceed the budget
            }
            return individual;
        }

         void PrintCoordinates(double[,] coords, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"({coords[i, 0]:F2}, {coords[i, 1]:F2})");
            }
        }

         void PrintMatrix<T>(T[,] matrix, int rows, int cols)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }

}

