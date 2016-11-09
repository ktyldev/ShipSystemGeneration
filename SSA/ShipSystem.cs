using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSA {
    public abstract class ShipSystem {
        public int realCost { get { return (int)_realCost; } }
        private double _realCost;

        public List<Advantage> advantages;
        public List<Limitation> limitations;

        public abstract ShipSystemType type { get; }

        public ShipSystem(double realCost, List<Advantage> advantages, List<Limitation> limitations) {
            _realCost = realCost;
            this.advantages = advantages;
            this.limitations = limitations;
        }

        public void AddAdvantage(Advantage advantage) {
            advantages.Add(advantage);
        }

        public void AddLimitation(Limitation limitation) {
            limitations.Add(limitation);
        }
    }
}
