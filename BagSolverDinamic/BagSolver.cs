
using BagSolverDinamic.DenModels;

namespace BagSolverDinamic
{
    public class BagSolver
    {

        private List<BestCostRecord> _eachCostBestRecord;
        private double _maxCost;
        private double _minPointDistance;

        private readonly StreamWriter _writer;
        private readonly List<VDEInfo> _inputVDEInfos;

        public List<BestCostRecord> EachCostBestRecord
        {
            get { return _eachCostBestRecord; }
            private set { }
        }


        public BagSolver(double[,] locations,
                                double[,] costs,
                                double[,] powers,
                                double totalBudget,
                                double minDist,
                                StreamWriter writer
                                )
        {
            if (costs.GetLength(0) != powers.GetLength(0) || costs.GetLength(1) != powers.GetLength(1))
            {
                throw new ArgumentException("costs and powers arrays must be of the same size.");
            }

            var  numLocations = locations.GetLength(0);
            var  numUnits = costs.GetLength(1);
            _minPointDistance = minDist;
            _maxCost = totalBudget;
            _writer = writer;

            _inputVDEInfos = new List<VDEInfo>();

            for (int i = 0; i < numLocations; i++)
            {
                for (int j = 0; j < numUnits; j++)
                {

                    var vDEInfo = new VDEInfo() {

                        Id = j+1,
                        Place = new MyModels.Place()
                        {
                            Id = i+1,
                            Point = new Point()
                            {
                                X = locations[i,0],
                                Y = locations[i,1]
                            }
                        },
                        Cost = costs[i, j],
                        Power = powers[i, j],
                    };
                    _inputVDEInfos.Add(vDEInfo);
                }
            }

            for (int i = 0; i < numUnits; i++)
            {

            }

            _eachCostBestRecord = new List<BestCostRecord>();
        }

        public BagSolution CalculateEachCostBestRecord()
        {
            int currentCost = 0;

            while (currentCost <= _maxCost) //while cost is smaller than max
            {
                int currentVDEIndex = 0;//start trying to add new VDEs from the firs one
                _eachCostBestRecord.Add(new());//create Best record for next cost

                while (currentVDEIndex < _inputVDEInfos.Count)//while do not try to add all VDEs
                {
                    int leftCost = currentCost;
                    int currentLowerThanLeftCost = leftCost;//set cost to waste

                    var currentBestCombination = new ResultVDECombination(currentCost, _minPointDistance);

                    if (currentBestCombination.CanAddInfo(_inputVDEInfos[currentVDEIndex]))//if can buy current VDE
                    {
                        currentBestCombination.AddInfo(_inputVDEInfos[currentVDEIndex]);//Add VDE to combination
                        leftCost = (int)(leftCost - _inputVDEInfos[currentVDEIndex].Cost);//make left cost
                        currentLowerThanLeftCost = leftCost;//set rest cost = left cost
                    }

                    while (currentLowerThanLeftCost > 0)//while there is money to waste
                    {//try to get record we can add to combination
                        bool recordWithThisCostFound = false;//add variable that represents if recordWithThisCostFound
                        foreach (var record in _eachCostBestRecord[(int)currentLowerThanLeftCost].SetOfSelectedVDEs)//for every record in this cost
                        {
                            if (currentBestCombination.CanAddInfo(record.ResultInfos))//if can add record infos for this cost
                            {
                                foreach (var info in record.ResultInfos)//add all record infos for this cost
                                {
                                    currentBestCombination.AddInfo(info);
                                }
                                leftCost = leftCost - (int)record.CurrentCost;//minus total cost from leftCost
                                currentLowerThanLeftCost = leftCost;//set rest cost = left cost
                                recordWithThisCostFound = true;//set that pair is found
                                break;
                            }
                        }
                        if (!recordWithThisCostFound)//if pair is not found 
                        {
                            currentLowerThanLeftCost--;
                        }
                    }

                    if (_eachCostBestRecord[currentCost].SetOfSelectedVDEs.Count == 0)//if record is not set
                    {
                        _eachCostBestRecord[currentCost].SetOfSelectedVDEs.Add(currentBestCombination);//add first record
                    }
                    else
                    {
                        if (_eachCostBestRecord[currentCost].SetOfSelectedVDEs[0].CurrentPower
                            < currentBestCombination.CurrentPower)//compare with record. If our record is bigger
                        {
                            _eachCostBestRecord[currentCost].SetOfSelectedVDEs = new();//set new record list
                            _eachCostBestRecord[currentCost].SetOfSelectedVDEs.Add(currentBestCombination);//add first record
                        }
                        else if (_eachCostBestRecord[currentCost].SetOfSelectedVDEs[0].CurrentPower
                            == currentBestCombination.CurrentPower)//compare with record. If our record equeals previous record
                        {
                            _eachCostBestRecord[currentCost].SetOfSelectedVDEs.Add(currentBestCombination);//add alternative record
                        }
                    }

                    currentVDEIndex++;
                }
                currentCost++;
            }

            //actions after calculation


            var FirstBestRecord = EachCostBestRecord[(int)_maxCost].SetOfSelectedVDEs[0].ResultInfos;

            int[,] locAndUnit = new int[FirstBestRecord.Count, 2];

            for (int i = 0; i < locAndUnit.GetLength(0); i++)
            {
                locAndUnit[i, 0] = FirstBestRecord[i].Place.Id;
                locAndUnit[i, 1] = FirstBestRecord[i].Id;
            }


            var Solution = new BagSolution()
            {
                LocAndUnit = locAndUnit,
                Power = EachCostBestRecord[(int)_maxCost].SetOfSelectedVDEs[0].CurrentPower
            };

            return Solution;
        }




    }
}
