using System;
using System.Collections.Generic;
using System.Text;

namespace HomeWork_2
{
    class Town
    {
        public List<Disk> disk = new List<Disk>();
        public int x { get; set; }
        public int lengthTown { get;  } = 150;
        public int CountDisk { get; set; } = 0;

    }

    class Disk
    {
        public int size { get; set; }
    }
}
