// See https://aka.ms/new-console-template for more information
using BagSolverDinamic;
using BagSolverDinamic.MyModels;
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");

var maxCost = 16;
var minDistance = 0;

double[,] locations = new double[2, 2];

locations[0, 0] = 1;
locations[0, 1] = 2;
locations[1, 0] = 1;
locations[1, 1] = 1;

double[,] costs = new double[2, 2];

costs[0, 0] = 5;
costs[0, 1] = 10;
costs[1, 0] = 5;
costs[1, 1] = 10;


double[,] powers = new double[2, 2];

powers[0, 0] = 100;
powers[0, 1] = 100;
powers[1, 0] = 100;
powers[1, 1] = 100;

using var streamWriter = new StreamWriter("Result.txt");


var denInputBagSolver = new BagSolver(locations, costs, powers, maxCost, minDistance, streamWriter);


Console.WriteLine("LoopSolver");
Console.WriteLine("--------------");


denInputBagSolver.CalculateEachCostBestRecord();



Console.WriteLine("--------------");
Console.WriteLine("Best of the best:");


ShowVDEInfos(denInputBagSolver.EachCostBestRecord[maxCost].SetOfSelectedVDEs);

void ShowVDEInfos(List<ResultVDECombination> resultVDECombinations)
{
    foreach (var resultVDECombination in resultVDECombinations)
    {
        Console.WriteLine($"Max Power = {resultVDECombination.CurrentPower}");
        Console.WriteLine(resultVDECombination.ToString());
    }
}


//InputVDEData inputVDEData = new InputVDEData();

//inputVDEData.AddAntonDataToVDE();

//inputVDEData.ShowVDEInfos();

//Console.WriteLine("--------------");


//Console.WriteLine("brootForceSolver");
//Console.WriteLine("--------------");
//BrootForceSolver brootForceSolver = new BrootForceSolver(inputVDEData, 10, maxCost, minDistance);

//brootForceSolver.GenerateAllVariants();

//var bestPowerValue = brootForceSolver.MaxCombinationPower();

//Console.WriteLine("--------------");
//Console.WriteLine($"Best Power: {bestPowerValue}");

//var CombinationsWithBestPower = brootForceSolver.GetAllResultCombinationWithThisPower(bestPowerValue);

//Console.WriteLine("--------------");
//Console.WriteLine("Best of the best:");

//brootForceSolver.ShowVDEInfos(CombinationsWithBestPower);


//InputVDEData inputVDEDataWithoutZeros = new InputVDEData();

//inputVDEDataWithoutZeros.AddAntonDataToVDE();

//for (int i = 0; i < inputVDEDataWithoutZeros.VDEInfos.Count; i++)
//{
//	if (inputVDEDataWithoutZeros.VDEInfos[i].Id==0)
//	{
//        inputVDEDataWithoutZeros.VDEInfos.RemoveAt(i);
//    }
//}

//Console.WriteLine("LoopSolver"); 
//Console.WriteLine("--------------");

//var recurtionDinamicSolver = new LoopSolver(inputVDEDataWithoutZeros, maxCost, minDistance);

//recurtionDinamicSolver.CalculateEachCostBestRecord();



//Console.WriteLine("--------------");
//Console.WriteLine("Best of the best:");


//recurtionDinamicSolver.ShowVDEInfos(recurtionDinamicSolver.EachCostBestRecord[maxCost].SetOfSelectedVDEs);
