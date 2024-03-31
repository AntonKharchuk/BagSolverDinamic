using System;
using System.Collections.Generic;
using System.Linq;

namespace BagSolverDinamic
{
    public class BrootForceSolver
    {
        private InputVDEData _inputVDEData;
        private List<ResultVDECombination> _resultVDECombinations;

        public List<ResultVDECombination> ResultVDECombinations
        {
            get { return _resultVDECombinations; }
            private set { }
        }


        private int _numOfVDETypesOnEachPlace;
        private int _maxCost;
        private int _minPointDistance;

        public BrootForceSolver(InputVDEData inputVDEData, int numOfVDETypesOnEachPlace, int maxCost, int minPointDistance)
        {
            _maxCost = maxCost;
            _minPointDistance = minPointDistance;
            _inputVDEData = inputVDEData;
            _resultVDECombinations = new List<ResultVDECombination>();
            _numOfVDETypesOnEachPlace = numOfVDETypesOnEachPlace;
        }

        public void ShowVDEInfos(List<ResultVDECombination> resultVDECombinations)
        {
            foreach (var resultVDECombination in resultVDECombinations)
            {
                Console.WriteLine(resultVDECombination.ToString());
            }
        }

        public double MaxCombinationPower()
        {
            double maxPower = -1;
            foreach (var combination in _resultVDECombinations)
            {
                if (combination.CurrentPower > maxPower)
                    maxPower = combination.CurrentPower;
            }
            return maxPower;
        }

        public List<ResultVDECombination> GetAllResultCombinationWithThisPower(double power)
        {
            var result = new List<ResultVDECombination>();

            foreach (var combination in _resultVDECombinations)
            {
                if (combination.CurrentPower == power)
                    result.Add(combination);
            }
            return result;
        }


        public void GenerateAllVariants()
        {
            List<VDEInfo> vdeInfos = _inputVDEData.VDEInfos;

            // Get distinct place IDs
            var distinctPlaces = vdeInfos.Select(v => v.Place.Id).Distinct().ToList();

            // Generate combinations recursively
            GenerateCombinations(vdeInfos, distinctPlaces, new List<VDEInfo>());
        }

        private void GenerateCombinations(List<VDEInfo> vdeInfos, List<int> places, List<VDEInfo> currentCombination)
        {
            // If we have filled all places, add the combination to the results
            if (currentCombination.Count == places.Count)
            {
                var combination = new ResultVDECombination(_maxCost, _minPointDistance);
                foreach (var info in currentCombination)
                {
                    if (combination.CanAddInfo(info))
                        combination.AddInfo(info);
                    else
                        return;
                }
                _resultVDECombinations.Add(combination);
                return;
            }

            // Get the next place to fill
            var placeId = places[currentCombination.Count];

            // Get available VDEInfos for the current place
            var availableInfos = vdeInfos.Where(info => info.Place.Id == placeId).ToList();

            // If there are more VDEInfos than required, take only the first _numOfVDETypesOnEachPlace
            if (availableInfos.Count > _numOfVDETypesOnEachPlace)
                availableInfos = availableInfos.Take(_numOfVDETypesOnEachPlace).ToList();

            // Try each available VDEInfo for the current place
            foreach (var info in availableInfos)
            {
                // Add the VDEInfo to the current combination
                currentCombination.Add(info);

                // Recursively generate combinations for the next places
                GenerateCombinations(vdeInfos, places, currentCombination);

                // Backtrack: Remove the last added VDEInfo to try the next one
                currentCombination.Remove(info);
            }
        }
    }
}
