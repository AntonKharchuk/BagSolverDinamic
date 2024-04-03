
using System.Net.Http.Headers;

namespace BagSolverDinamic
{
    public class LoopSolver
    {
        private List<BestCostRecord> _eachCostBestRecord;
        private int _maxCost;
        private int _minPointDistance;

        public List<BestCostRecord> EachCostBestRecord
        {
            get { return _eachCostBestRecord; }
            private set { }
        }

        private InputVDEData _inputVDEData;

        public LoopSolver(InputVDEData inputVDEData, int maxCost, int minPointDistance)
        {
            _inputVDEData = inputVDEData;
            _eachCostBestRecord = new List<BestCostRecord>();
            _maxCost = maxCost;
            _minPointDistance = minPointDistance;
        }

        public void CalculateEachCostBestRecord()
        {
            //int currentCost, int leftCost, int currentLowerThanLeftCost, int currentVDEIndex

            int currentCost = 0;

            while (currentCost <= _maxCost) //while cost is smaller than max
            {
                int currentVDEIndex = 0;//start trying to add new VDEs from the firs one
                _eachCostBestRecord.Add(new());//create Best record for next cost

                while (currentVDEIndex < _inputVDEData.VDEInfos.Count)//while do not try to add all VDEs
                {
                    int leftCost = currentCost;
                    int currentLowerThanLeftCost = leftCost;//set cost to waste

                    var currentBestCombination = new ResultVDECombination(currentCost, _minPointDistance);

                    if (currentBestCombination.CanAddInfo(_inputVDEData.VDEInfos[currentVDEIndex]))//if can buy current VDE
                    {
                        if (_inputVDEData.VDEInfos[currentVDEIndex].Cost==9)
                        {

                        }
                        currentBestCombination.AddInfo(_inputVDEData.VDEInfos[currentVDEIndex]);//Add VDE to combination
                        leftCost = (int)(leftCost - _inputVDEData.VDEInfos[currentVDEIndex].Cost);//make left cost
                        currentLowerThanLeftCost = leftCost;//set rest cost = left cost
                    }
                    if (currentLowerThanLeftCost ==17)
                    {

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
                                    if (info.Cost == 5)
                                    {

                                    }
                                    currentBestCombination.AddInfo(info);
                                }
                                leftCost = leftCost - (int)record.CurrentCost;//minus total cost from leftCost
                                currentLowerThanLeftCost = leftCost;//set rest cost = left cost
                                recordWithThisCostFound = true;//set that pair is found
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
        }

    }
}
