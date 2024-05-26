using BagSolverDinamic;

Console.WriteLine("Choose mode (1 - data entered in program, 2 - generated data, 3 - data from file, 4 - generated data in the range):");
var answer = Console.ReadLine();

const double evaporationRate = 0.05; // Швидкість випаровування феромону

switch (answer)
{
    case "1":
        {
            Console.WriteLine("Enter number of ants");
            int ants = 0;
            try
            {
                ants = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter number of iterations");
            int iterations = 0;
            try
            {
                iterations = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }


            Console.WriteLine("Enter number of locations:");
            int locationsNum = 0;
            try
            {
                locationsNum = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }
            var locations = new double[locationsNum, 2];
            for (int i = 0; i < locationsNum; i++)
            {
                Console.WriteLine($"Enter x for location {locationsNum}");
                try
                {
                    locations[i, 0] = double.Parse(Console.ReadLine()!);
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong input");
                }

                Console.WriteLine($"Enter y for location {locationsNum}");
                try
                {
                    locations[i, 1] = double.Parse(Console.ReadLine()!);
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong input");
                }
            }

            Console.WriteLine("Enter number of units:");
            var units = 0;
            try
            {
                units = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }
            var costs = new double[locationsNum, units];
            for (int i = 0; i < locationsNum; i++)
            {
                for (int j = 0; j < units; j++)
                {
                    Console.WriteLine($"Enter instalation cost for location {i}, unit {j}");
                    try
                    {
                        costs[i, j] = double.Parse(Console.ReadLine()!);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Wrong input");
                    }

                }
            }

            var powers = new double[locationsNum, units];
            for (int i = 0; i < locationsNum; i++)
            {
                for (int j = 0; j < units; j++)
                {
                    Console.WriteLine($"Enter power for location {i}, unit {j}");
                    try
                    {
                        costs[i, j] = double.Parse(Console.ReadLine()!);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Wrong input");
                    }

                }
            }

            Console.WriteLine("Enter budget:");
            int budget = 0;
            try
            {
                budget = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter minimal distance between VDEs:");
            int minDist = 0;
            try
            {
                minDist = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }


            var selfSetTask = new BagSolverDinamic.DenModels.Task
            {
                Locations = locations,
                Costs = costs,
                Powers = powers,
                Budget = budget,
                MinDist = minDist,
            };

            RunSolvers(evaporationRate, selfSetTask, ants, iterations);
            break;
        }
    case "2":
        {
            Console.WriteLine("Enter number of ants");
            int ants = 0;
            try
            {
                ants = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter number of iterations");
            int iterations = 0;
            try
            {
                iterations = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter number of tasks: ");

            int tasks = 0;

            try
            {
                tasks = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter number of locations:");
            int locations = 0;
            try
            {
                locations = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter number of units:");
            int units = 0;
            try
            {
                units = int.Parse(Console.ReadLine()!);
                if (units < 2)
                {
                    throw new IndexOutOfRangeException();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter budget:");
            int budget = 0;
            try
            {
                budget = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter minimal distance between VDEs:");
            var minDist = 0;
            try
            {
                minDist = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            double avgError = 0;

            var generator = new TaskGenerator(locations, units, budget, minDist);

            for (int i = 0; i < tasks; i++)
            {

                var task = generator.Generate();
                RunSolvers(evaporationRate, task, ants, iterations);
            }

            break;
        }
    case "3":
        {
            Console.WriteLine("Enter file name:");
            var fileName = Console.ReadLine();

            if (fileName is null)
            {
                Console.WriteLine("File name is empty.");
                return;
            }

            var ants = 0;
            var iterations = 0;
            double[,] locations = null!;
            double[,] costs = null!;
            double[,] powers = null!;
            int budget = 0;
            int minDist = 0;

            try
            {
                string[] lines = File.ReadAllLines(fileName);
                for (int i = 0; i < lines.Length; i++)
                {
                    string trimmedLine = lines[i].Trim();
                    if (string.IsNullOrEmpty(trimmedLine))
                        continue;

                    switch (trimmedLine)
                    {
                        case "Ants":
                            ants = int.Parse(lines[++i].Trim());
                            break;
                        case "Iterations":
                            iterations = int.Parse(lines[++i].Trim());
                            break;
                        case "Location coordinates":
                            var coordinatesList = new List<double[]>();
                            while (++i < lines.Length && int.TryParse(lines[i][0].ToString(), out int dummyInt))
                            {
                                var coords = Array.ConvertAll(lines[i].Trim().Split(' '), double.Parse);
                                coordinatesList.Add(coords);
                            }
                            locations = new double[coordinatesList.Count, 2];
                            for (int j = 0; j < coordinatesList.Count; j++)
                            {
                                locations[j, 0] = coordinatesList[j][0];
                                locations[j, 1] = coordinatesList[j][1];
                            }
                            i--;
                            break;
                        case "Costs matrix":
                            costs = ReadMatrix(ref i, lines);
                            break;
                        case "Powers matrix":
                            powers = ReadMatrix(ref i, lines);
                            break;
                        case "Budget":
                            budget = int.Parse(lines[++i].Trim());
                            break;
                        case "Min dist":
                            minDist = int.Parse(lines[++i].Trim());
                            break;
                    }
                }

                var selfSetTask = new BagSolverDinamic.DenModels.Task
                {
                    Locations = locations,
                    Costs = costs,
                    Powers = powers,
                    Budget = budget,
                    MinDist = minDist,
                };

                RunSolvers(evaporationRate, selfSetTask, ants, iterations);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            break;
        }
    case "4":
        {
            Console.WriteLine("Enter number of ants");
            int ants = 0;
            try
            {
                ants = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter number of iterations");
            int iterations = 0;
            try
            {
                iterations = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter number of tasks: ");

            int tasks = 0;

            try
            {
                tasks = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter min number of locations:");
            int locationsFrom = 0;
            try
            {
                locationsFrom = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }
            Console.WriteLine("Enter max number of locations:");

            int locationsTo = 0;
            try
            {
                locationsTo = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }
            Console.WriteLine("Enter locations change step:");

            int locationsStep = 0;
            try
            {
                locationsStep = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter min number of units:");
            int unitsFrom = 0;
            try
            {
                unitsFrom = int.Parse(Console.ReadLine()!);
                if (unitsFrom < 2)
                {
                    throw new IndexOutOfRangeException();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }
            Console.WriteLine("Enter max number of units:");

            int unitsTo = 0;
            try
            {
                unitsTo = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }
            Console.WriteLine("Enter units change step:");

            int unitsStep = 0;
            try
            {
                unitsStep = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter budget:");
            int budget = 0;
            try
            {
                budget = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            Console.WriteLine("Enter minimal distance between VDEs:");
            int minDist = 0;
            try
            {
                minDist = int.Parse(Console.ReadLine()!);
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input");
            }

            double avgError = 0;

            for (int loc = locationsFrom; loc < locationsTo; loc += locationsStep)
            {
                for (int unit = unitsFrom; unit < unitsTo; unit += unitsStep)
                {

                    for (int i = 0; i < tasks; i++)
                    {
                        var generator = new TaskGenerator(loc, unit, budget, minDist);

                        var task = generator.Generate();
                        RunSolvers(evaporationRate, task, ants, iterations);
                    }
                }
            }

            break;
        }
    default:
        Console.WriteLine("Wrong input");
        break;
}

static double[,] ReadMatrix(ref int i, string[] lines)
{
    var matrixList = new List<int[]>();
    while (++i < lines.Length && int.TryParse(lines[i][0].ToString(), out int dummyInt))
    {
        var row = Array.ConvertAll(lines[i].Trim().Split(' '), int.Parse);
        matrixList.Add(row);
    }
    var matrix = new double[matrixList.Count, matrixList[0].Length];
    for (int j = 0; j < matrixList.Count; j++)
    {
        for (int k = 0; k < matrixList[j].Length; k++)
        {
            matrix[j, k] = matrixList[j][k];
        }
    }
    i--;
    return matrix;
}

static void RunSolvers(double evaporationRate, BagSolverDinamic.DenModels.Task task, int ants, int antsIterations)
{
    using var streamWriter = new StreamWriter("Result.txt", true);

    Console.WriteLine(task.ToString());
    streamWriter.WriteLine(task.ToString());

    var denInputBagSolver = new BagSolver(task.Locations, task.Costs, task.Powers, task.Budget, task.MinDist);

    var bagResult = denInputBagSolver.CalculateEachCostBestRecord();

    Console.WriteLine(bagResult.ToString());
    streamWriter.WriteLine(bagResult.ToString());

    var antSlver = new AntColonyOptimizator(task.Locations, task.Costs, task.Powers, task.Budget, task.MinDist, evaporationRate);

    var antResult = antSlver.Optimize(ants, antsIterations);

    Console.WriteLine(antResult.ToString());
    streamWriter.WriteLine(antResult.ToString());


    var evoSolver = new EvoSolver(task.Locations, task.Costs, task.Powers, task.Budget, task.MinDist);

    var evoResult = evoSolver.Solve();
    Console.WriteLine(evoResult.ToString());
    streamWriter.WriteLine(evoResult.ToString());


    var greSolver = new GreSolver(task.Locations, task.Costs, task.Powers, task.Budget, task.MinDist);
    var greResult = greSolver.Solve();
    Console.WriteLine(greResult.ToString());
    streamWriter.WriteLine(greResult.ToString());
}