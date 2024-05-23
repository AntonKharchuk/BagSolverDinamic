
using System.Runtime.CompilerServices;
using BagSolverDinamic.MyModels;

namespace BagSolverDinamic
{
    public class Solver1
    {

        private List<BestCostRecord> _eachCostBestRecord;
        private int _maxCost;
        private int _minPointDistance;

        public List<BestCostRecord> EachCostBestRecord
        {
            get { return _eachCostBestRecord; }
            private set {}
        }

        private InputVDEData _inputVDEData;

        public Solver1(InputVDEData inputVDEData, int maxCost, int minPointDistance)
        {
            _inputVDEData = inputVDEData;
            _eachCostBestRecord = new List<BestCostRecord>();
            _maxCost = maxCost;
            _minPointDistance = minPointDistance;
        }


        public void CalculateEachCostBestRecord()
        {
            _eachCostBestRecord.Add(new());//create Best record for 0 cost
            Helper(0,0,0,0, new ResultVDECombination(0, _minPointDistance));

            void Helper(int currentCost, int leftCost, int currentLowerThanLeftCost, int currentVDEIndex, ResultVDECombination currentBestCombination)
            {
                if (leftCost == 0| currentLowerThanLeftCost==0)
                {
                    if (_eachCostBestRecord[currentCost].SetOfSelectedVDEs.Count ==0)//if record is not set
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
                   
                    if (currentVDEIndex == _inputVDEData.VDEInfos.Count-1)//if this is last VDE example in a input list
                    {
                        if (currentCost <= _maxCost)//if cost is not out of range
                        {
                            _eachCostBestRecord.Add(new());//create Best record for next cost
                            currentCost++;//set bigger cost
                            //_eachCostBestRecord[currentCost+1] = new();//create Best record for next cost
                            Helper(currentCost, leftCost: currentCost, currentLowerThanLeftCost: currentCost, currentVDEIndex:0, new ResultVDECombination(currentCost, _minPointDistance));
                        }//go to next cost
                        else//if cost is out of range
                        {
                            return;//stop
                        }
                    }
                    else//if this is not last VDE example in a input list
                    {
                        Helper(currentCost, leftCost:currentCost, currentLowerThanLeftCost: currentCost, currentVDEIndex+1, new ResultVDECombination(currentCost, _minPointDistance));
                    }//move to next VDE
                }
                else
                {
                    if (currentLowerThanLeftCost == currentCost)
                    {
                        if (currentBestCombination.CanAddInfo(_inputVDEData.VDEInfos[currentVDEIndex]))//if can buy current VDE
                        {
                            currentBestCombination.AddInfo(_inputVDEData.VDEInfos[currentVDEIndex]);//Add VDE to combination
                            var left = leftCost - _inputVDEData.VDEInfos[currentVDEIndex].Cost;//make left cost
                            Helper(currentCost, leftCost: (int)left, currentLowerThanLeftCost: (int)left, currentVDEIndex, currentBestCombination);
                        }//continue recurtion with left cost
                        else
                        {
                            Helper(currentCost, leftCost: leftCost, currentLowerThanLeftCost: leftCost-1, currentVDEIndex, currentBestCombination);
                        }//lower the currentLowerCost and try again
                    }
                    else
                    {
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
                                Helper(currentCost, leftCost: leftCost, currentLowerThanLeftCost: leftCost, currentVDEIndex, currentBestCombination);
                                //continue recurtion with left cost
                                recordWithThisCostFound = true;//set that pair is found
                            }
                        }
                        if (!recordWithThisCostFound)//if pair is not found 
                        {
                            Helper(currentCost, leftCost: leftCost, currentLowerThanLeftCost: currentLowerThanLeftCost - 1, currentVDEIndex, currentBestCombination);
                        }//lower the currentLowerCost and try again
                    }
                    //chack if we can add items from current record to previos record with currentLowerThanLeftCost
                    //if yes than
                    //{
                    //  add current record to previos record with currentLowerThanLeftCost
                    //  
                    // leftCost= leftCost - currentLowerThanLeftCost
                    //  
                    // Helper(currentCost, leftCost:leftCost, currentLowerThanLeftCost: leftCost, currentVDEIndex, current record);
                    //  
                    //}
                    //else
                    //than lover currentLowerThanLeftCost -1 and try again

                }
            }
        }

    }
}
