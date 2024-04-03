
namespace BagSolverDinamic
{
    public class ResultVDECombination
    {
        public double MaxCost { get; private set; }
        public double CurrentCost { get; private set; }
        public double CurrentPower { get; private set; }
        public double MinPointDistance { get; private set; }

        private List<VDEInfo> _resultInfos;

        public List<VDEInfo> ResultInfos
        {
            get { return _resultInfos; }
            private set { }
        }

        public ResultVDECombination(int maxCost, int minPointDistance)
        {
            MaxCost = maxCost;
            CurrentCost = 0;
            CurrentPower = 0;
            MinPointDistance = minPointDistance;
            _resultInfos = new List<VDEInfo> { };
        }
        public bool CanAddInfo(List<VDEInfo> infos)
        {
            if (infos.Count ==0)
            {
                return false;
            }
            foreach (var info in infos)
            {
                if (CanAddInfo(info) == false)
                    return false;
            }
            return true;
        }

        public bool CanAddInfo(VDEInfo info)
        {
            if (IsPDEAlreadySet(info.Place.Id))
                return false;
            if (CurrentCost + info.Cost > MaxCost)
                return false;
            if (!CheckPointMatch(info))
                return false;
            return true;
        }
        private bool IsPDEAlreadySet(int placeId)
        {
            foreach (VDEInfo info in _resultInfos)
            {
                if (info.Place.Id == placeId)
                    return true;
            }
            return false;
        }
        private bool CheckPointMatch(VDEInfo info)
        {
            if (info.Id ==0)
                return true;

            foreach (VDEInfo info1 in _resultInfos)
            {
                if (MinPointDistance > CalculateDistance(info.Place.Point, info1.Place.Point))
                    return false;
            } 
            return true;
        }
        private double CalculateDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
        public void AddInfo(VDEInfo info)
        {
            if (!CanAddInfo(info))
                throw new ArgumentException($"Can not add {info.Id}");
            else
            {
                CurrentCost += info.Cost;
                CurrentPower += info.Power;
                _resultInfos.Add(info);
            }
        }

        public void ShowVDEInfos()
        {
            foreach (var vdeInfo in _resultInfos)
            {
                Console.WriteLine(vdeInfo.ToString());
            }
        }

        public override string ToString()
        {
            var resStr = string.Empty;

            foreach (var vdeInfo in _resultInfos) { resStr += (vdeInfo.ToString() + " ");}

            return resStr;
        }
    }
}
