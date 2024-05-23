// See https://aka.ms/new-console-template for more information
using BagSolverDinamic;
using BagSolverDinamic.MyModels;
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");

var maxCost =100;
var minDistance = 1;


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


InputVDEData inputVDEDataWithoutZeros = new InputVDEData();

inputVDEDataWithoutZeros.AddAntonDataToVDE();

for (int i = 0; i < inputVDEDataWithoutZeros.VDEInfos.Count; i++)
{
	if (inputVDEDataWithoutZeros.VDEInfos[i].Id==0)
	{
        inputVDEDataWithoutZeros.VDEInfos.RemoveAt(i);
    }
}

Console.WriteLine("LoopSolver"); 
Console.WriteLine("--------------");

var recurtionDinamicSolver = new LoopSolver(inputVDEDataWithoutZeros, maxCost, minDistance);

recurtionDinamicSolver.CalculateEachCostBestRecord();



Console.WriteLine("--------------");
Console.WriteLine("Best of the best:");


recurtionDinamicSolver.ShowVDEInfos(recurtionDinamicSolver.EachCostBestRecord[maxCost].SetOfSelectedVDEs);
