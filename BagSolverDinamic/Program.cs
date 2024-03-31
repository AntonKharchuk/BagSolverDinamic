// See https://aka.ms/new-console-template for more information
using BagSolverDinamic;

using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");


InputVDEData inputVDEData = new InputVDEData();

inputVDEData.AddAntonDataToVDE();

inputVDEData.ShowVDEInfos();

Console.WriteLine("--------------");


BrootForceSolver brootForceSolver = new BrootForceSolver(inputVDEData, 10, 100, 5);

brootForceSolver.GenerateAllVariants();

brootForceSolver.ShowVDEInfos(brootForceSolver.ResultVDECombinations);

var bestPowerValue = brootForceSolver.MaxCombinationPower();

Console.WriteLine("--------------");
Console.WriteLine($"Best Power: {bestPowerValue}");

var CombinationsWithBestPower = brootForceSolver.GetAllResultCombinationWithThisPower(bestPowerValue);

Console.WriteLine("--------------");
Console.WriteLine("Best of the best:");

brootForceSolver.ShowVDEInfos(CombinationsWithBestPower);


