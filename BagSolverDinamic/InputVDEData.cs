
namespace BagSolverDinamic
{
    public class InputVDEData
    {
        public List<VDEInfo> VDEInfos { get; set; }
        public InputVDEData()
        {
            VDEInfos = new List<VDEInfo>();
        }
        public void ShowVDEInfos()
        {
            foreach (var vdeInfo in VDEInfos)
            {
                Console.WriteLine(vdeInfo.ToString());
            }
        }

        public void AddTestDataToVDE()
        {
            Point point1 = new Point
            {
                X = 2,
                Y = 14,
            };
            Point point2 = new Point
            {
                X = 3,
                Y = 19,
            };
            Point point3 = new Point
            {
                X = 5,
                Y = 12,
            };

            Place place1 = new Place
            {
                Id = 1,
                Point = point1,
            };
            Place place2 = new Place
            {
                Id = 2,
                Point = point2,
            }; 
            Place place3 = new Place
            {
                Id = 3,
                Point = point3,
            };

            VDEInfos.Add(new VDEInfo
            {
                Cost = 0,
                Id = 0,
                Place = place1,
                Power = 0,
            });
            VDEInfos.Add(new VDEInfo
            {
                Cost = 10,
                Id = 1,
                Place = place1,
                Power = 10,
            });
            VDEInfos.Add(new VDEInfo
            {
                Cost = 20,
                Id = 2,
                Place = place1,
                Power = 20,
            });
            VDEInfos.Add(new VDEInfo
            {
                Cost = 0,
                Id = 0,
                Place = place2,
                Power = 0,
            });
            VDEInfos.Add(new VDEInfo
            {
                Cost = 21,
                Id = 1,
                Place = place2,
                Power = 21,
            });
            VDEInfos.Add(new VDEInfo
            {
                Cost = 22,
                Id = 2,
                Place = place2,
                Power = 22,
            });
            VDEInfos.Add(new VDEInfo
            {
                Cost = 0,
                Id = 0,
                Place = place3,
                Power = 0,
            });
            VDEInfos.Add(new VDEInfo
            {
                Cost = 31,
                Id = 3,
                Place = place3,
                Power = 31,
            });
            VDEInfos.Add(new VDEInfo
            {
                Cost = 32,
                Id = 2,
                Place = place3,
                Power = 32,
            });

        }
        public void AddAntonDataToVDE()
        {
            List<Place> places = new List<Place>();

            // Hardcoded data
            var data = new List<Tuple<int, double, double>>
        {
            Tuple.Create(1, 2.0, 14.0),
            Tuple.Create(2, 3.0, 9.0),
            Tuple.Create(3, 5.0, 12.0),
            Tuple.Create(4, 7.0, 2.0),
            Tuple.Create(5, 8.0, 14.0),
            Tuple.Create(6, 10.0, 8.0),
            Tuple.Create(7, 13.0, 9.0),
            Tuple.Create(8, 20.0, 5.0)
        };

            // Create entities
            foreach (var item in data)
            {
                Place place = new Place
                {
                    Id = item.Item1,
                    Point = new Point { X = item.Item2, Y = item.Item3 }
                };
                places.Add(place);
            }

            List<VDEInfo> vdeInfos = new List<VDEInfo>();

            // Hardcoded data
            var data1 = new List<Tuple<int, int, double, double>>
            {
                Tuple.Create(0, 1, 0.0, 0.0),
                Tuple.Create(1, 1, 10.0, 70.0),
                Tuple.Create(2, 1, 12.0, 80.0),
                Tuple.Create(3, 1, 13.0, 90.0),
                Tuple.Create(4, 1, 15.0, 100.0),
                Tuple.Create(5, 1, 17.0, 110.0),
                Tuple.Create(6, 1, 18.0, 120.0),
                Tuple.Create(7, 1, 20.0, 140.0),
                Tuple.Create(8, 1, 22.0, 150.0),
                Tuple.Create(9, 1, 25.0, 160.0),

                Tuple.Create(0, 2, 0.0, 0.0),
                Tuple.Create(1, 2, 12.0, 75.0),
                Tuple.Create(2, 2, 13.0, 85.0),
                Tuple.Create(3, 2, 14.0, 95.0),
                Tuple.Create(4, 2, 16.0, 105.0),
                Tuple.Create(5, 2, 17.0, 115.0),
                Tuple.Create(6, 2, 19.0, 125.0),
                Tuple.Create(7, 2, 21.0, 145.0),
                Tuple.Create(8, 2, 23.0, 155.0),
                Tuple.Create(9, 2, 26.0, 165.0),

                Tuple.Create(0, 3, 0.0, 0.0),
                Tuple.Create(1, 3, 8.0, 40.0),
                Tuple.Create(2, 3, 9.0, 50.0),
                Tuple.Create(3, 3, 10.0, 70.0),
                Tuple.Create(4, 3, 11.0, 90.0),
                Tuple.Create(5, 3, 12.0, 100.0),
                Tuple.Create(6, 3, 15.0, 120.0),
                Tuple.Create(7, 3, 17.0, 140.0),
                Tuple.Create(8, 3, 19.0, 180.0),
                Tuple.Create(9, 3, 20.0, 200.0),

                Tuple.Create(0, 4, 0.0, 0.0),
                Tuple.Create(1, 4, 13.0, 100.0),
                Tuple.Create(2, 4, 14.0, 120.0),
                Tuple.Create(3, 4, 17.0, 130.0),
                Tuple.Create(4, 4, 19.0, 150.0),
                Tuple.Create(5, 4, 20.0, 180.0),
                Tuple.Create(6, 4, 23.0, 220.0),
                Tuple.Create(7, 4, 26.0, 230.0),
                Tuple.Create(8, 4, 28.0, 250.0),
                Tuple.Create(9, 4, 29.0, 300.0),

                Tuple.Create(0, 5, 0.0, 0.0),
                Tuple.Create(1, 5, 9.0, 65.0),
                Tuple.Create(2, 5, 11.0, 75.0),
                Tuple.Create(3, 5, 12.0, 85.0),
                Tuple.Create(4, 5, 14.0, 95.0),
                Tuple.Create(5, 5, 16.0, 105.0),
                Tuple.Create(6, 5, 17.0, 115.0),
                Tuple.Create(7, 5, 19.0, 135.0),
                Tuple.Create(8, 5, 21.0, 145.0),
                Tuple.Create(9, 5, 24.0, 155.0),

                Tuple.Create(0, 6, 0.0, 0.0),
                Tuple.Create(1, 6, 12.0, 90.0),
                Tuple.Create(2, 6, 14.0, 100.0),
                Tuple.Create(3, 6, 15.0, 110.0),
                Tuple.Create(4, 6, 17.0, 120.0),
                Tuple.Create(5, 6, 19.0, 130.0),
                Tuple.Create(6, 6, 20.0, 140.0),
                Tuple.Create(7, 6, 22.0, 160.0),
                Tuple.Create(8, 6, 24.0, 170.0),
                Tuple.Create(9, 6, 27.0, 180.0),

                Tuple.Create(0, 7, 0.0, 0.0),
                Tuple.Create(1, 7, 15.0, 85.0),
                Tuple.Create(2, 7, 16.0, 90.0),
                Tuple.Create(3, 7, 17.0, 95.0),
                Tuple.Create(4, 7, 19.0, 100.0),
                Tuple.Create(5, 7, 21.0, 120.0),
                Tuple.Create(6, 7, 25.0, 140.0),
                Tuple.Create(7, 7, 27.0, 160.0),
                Tuple.Create(8, 7, 29.0, 175.0),
                Tuple.Create(9, 7, 30.0, 190.0),

                Tuple.Create(0, 8, 0.0, 0.0),
                Tuple.Create(1, 8, 5.0, 40.0),
                Tuple.Create(2, 8, 6.0, 50.0),
                Tuple.Create(3, 8, 7.0, 70.0),
                Tuple.Create(4, 8, 9.0, 90.0),
                Tuple.Create(5, 8, 11.0, 110.0),
                Tuple.Create(6, 8, 12.0, 120.0),
                Tuple.Create(7, 8, 14.0, 140.0),
                Tuple.Create(8, 8, 15.0, 150.0),
                Tuple.Create(9, 8, 17.0, 160.0)
            };

            // Create entities
            foreach (var item in data1)
            {
                VDEInfo vdeInfo = new VDEInfo
                {
                    Id = item.Item1,
                    Place = places.Where(p => p.Id == item.Item2).First(),
                    Cost = item.Item3,
                    Power = item.Item4
                };
                vdeInfos.Add(vdeInfo);
            }
            VDEInfos = vdeInfos;
        }
    }
}
