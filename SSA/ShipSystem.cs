using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSA {
    public enum ShipSystemType {
        reactor,
        sensor,
        hold,
        cloak,
        quarters,
        thruster,
        shield,
        armour,
        weapon
    }

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

    public class Reactor : ShipSystem {
        public override ShipSystemType type {
            get { return ShipSystemType.reactor; }
        }

        public Reactor(double realCost, List<Advantage> advantages, List<Limitation> limitations) : base(realCost, advantages, limitations) { }
    }

    public class Sensor : ShipSystem {
        public override ShipSystemType type {
            get { return ShipSystemType.sensor; }
        }

        public Sensor(double realCost, List<Advantage> advantages, List<Limitation> limitations) : base(realCost, advantages, limitations) { }
    }

    public class Hold : ShipSystem {
        public override ShipSystemType type {
            get { return ShipSystemType.hold; }
        }

        public Hold(double realCost, List<Advantage> advantages, List<Limitation> limitations) : base(realCost, advantages, limitations) { }
    }

    public class Cloak : ShipSystem {
        public override ShipSystemType type {
            get { return ShipSystemType.cloak; }
        }

        public Cloak(double realCost, List<Advantage> advantages, List<Limitation> limitations) : base(realCost, advantages, limitations) { }
    }

    public class Quarters : ShipSystem {
        public override ShipSystemType type {
            get { return ShipSystemType.quarters; }
        }

        public Quarters(double realCost, List<Advantage> advantages, List<Limitation> limitations) : base(realCost, advantages, limitations) { }
    }

    public class Thruster : ShipSystem {
        public Thruster(double realCost, List<Advantage> advantages, List<Limitation> limitations) : base(realCost, advantages, limitations) { }

        public override ShipSystemType type {
            get {
                return ShipSystemType.thruster;
            }
        }
    }

    public class Shield : ShipSystem {
        public Shield(double realCost, List<Advantage> advantages, List<Limitation> limitations) : base(realCost, advantages, limitations) { }

        public override ShipSystemType type {
            get {
                return ShipSystemType.shield;
            }
        }
    }

    public class Armour : ShipSystem {
        public Armour(double realCost, List<Advantage> advantages, List<Limitation> limitations) : base(realCost, advantages, limitations) { }

        public override ShipSystemType type {
            get { return ShipSystemType.armour; }
        }
    }

    public class Weapon : ShipSystem {
        public Weapon(double realCost, List<Advantage> advantages, List<Limitation> limitations) : base(realCost, advantages, limitations) { }

        public override ShipSystemType type {
            get { return ShipSystemType.weapon; }
        }
    }
}
