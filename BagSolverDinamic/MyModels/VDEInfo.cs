
using BagSolverDinamic.MyModels;

namespace BagSolverDinamic
{
    public  class VDEInfo
    {
        public  int   Id { get; set; }
        public Place Place { get; set; }
        public double Cost { get; set; }
        public double Power { get; set; }

        public override string ToString()
        {
            return $"VDEInfo ID: {Id}, Place ID: {Place.Id}, Cost: {Cost}, Power: {Power}";
        }
    }
}
