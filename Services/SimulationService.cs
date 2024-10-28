using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Services
{
    public static class SimulationService
    {
        public static bool IsSimulation { get; private set; }

        public static void BeginSimulation()
        {
            IsSimulation = true;
        }

        public static void EndSimulation()
        {
            IsSimulation = false;
        }
    }
}
