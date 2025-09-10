using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.domain.model
{
    internal class Vital_data
    {
        private int pressure;
        private int temperature;
        private int pulse;
        private int blood_oxygen_level;

        public int Pressure { get => pressure; set => pressure = value; }
        public int Temperature { get => temperature; set => temperature = value; }
        public int Pulse { get => pulse; set => pulse = value; }
        public int Blood_oxygen_level { get => blood_oxygen_level; set => blood_oxygen_level = value; }
    }
}
