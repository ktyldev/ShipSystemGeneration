using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSA {
    class Program {
        static void Main(string[] args) {
            var allTypes = (ShipSystemType[])Enum.GetValues(typeof(ShipSystemType));

            var factory = new ShipSystemFactory();
            factory.advantages.AddRange(new[] {
                new Advantage("fast shift", new[] { ShipSystemType.reactor }, 0.1),
                new Advantage("reduced mass", allTypes, 0.15),
                new Advantage("decreased burnout", new [] { ShipSystemType.reactor }, 0.1)
            });
            factory.limitations.AddRange(new[] {
                new Limitation("slow shift", new [] { ShipSystemType.reactor }, 0.1),
                new Limitation("extra mass", allTypes, 0.15),
                new Limitation("increased burnout", new [] { ShipSystemType.reactor }, 0.1)
            });

            Console.WriteLine("Available advantages:");
            foreach (var advantage in factory.advantages) {
                Console.WriteLine("  " + advantage.name);
            }
            Console.WriteLine("Available limitations:");
            foreach (var limitation in factory.limitations) {
                Console.WriteLine("  " + limitation.name);
            }
            Console.WriteLine();

            var reactor = factory.CreateSystem(ShipSystemType.reactor, 50);
            var sensor = factory.CreateSystem(ShipSystemType.sensor, 25);
            LogSystemDetails(reactor);
            LogSystemDetails(sensor);
            Console.ReadLine();
        }

        static void LogSystemDetails(ShipSystem system) {
            Console.WriteLine("Created system of type: " + system.type);
            Console.WriteLine("  Cost: " + system.realCost);
            Console.WriteLine("  Advantages:");
            foreach (var adv in system.advantages)
                Console.WriteLine("    " + adv.name);
            Console.WriteLine("  Limitations:");
            foreach (var lim in system.limitations)
                Console.WriteLine("    " + lim.name);
        }
    }

    class ShipSystemFactory {
        Dictionary<ShipSystemType, Func<double, List<Advantage>, List<Limitation>, ShipSystem>> _ctors;
        Random _r = new Random();

        public List<Advantage> advantages = new List<Advantage>();
        public List<Limitation> limitations = new List<Limitation>();

        public ShipSystemFactory() {
            _ctors.Add(ShipSystemType.reactor, (c, a, l) => new Reactor(c, a, l));
            _ctors.Add(ShipSystemType.sensor, (c, a, l) => new Sensor(c, a, l));
            _ctors.Add(ShipSystemType.hold, (c, a, l) => new Hold(c, a, l));
            _ctors.Add(ShipSystemType.cloak, (c, a, l) => new Cloak(c, a, l));
            _ctors.Add(ShipSystemType.quarters, (c, a, l) => new Quarters(c, a, l));
            _ctors.Add(ShipSystemType.thruster, (c, a, l) => new Thruster(c, a, l));
            _ctors.Add(ShipSystemType.shield, (c, a, l) => new Shield(c, a, l));
            _ctors.Add(ShipSystemType.armour, (c, a, l) => new Armour(c, a, l));
            _ctors.Add(ShipSystemType.weapon, (c, a, l) => new Weapon(c, a, l));
        }

        public ShipSystem CreateSystem(ShipSystemType type, int baseCost) {
            int numberOfAdvantages = _r.Next(0, 3); // 0-2 advantages
            int numberOfLimitations = _r.Next(0, 3); // 0-2 limitations

            Console.WriteLine("Creating system of type: " + type);
            Console.WriteLine("  Adv count: " + numberOfAdvantages);
            Console.WriteLine("  Lim count: " + numberOfLimitations);
            Console.WriteLine();

            var advs = new List<Advantage>();
            var lims = new List<Limitation>();

            var advsOfType = advantages
                .Where(a => a.systemTypes.Contains(type))
                .ToArray();
            for (int i = 0; i < numberOfAdvantages; i++) {
                int index = _r.Next(advsOfType.Length);
                advs.Add(advsOfType[index]);
            }

            var activeCost = baseCost * (1 + advs.Sum(a => a.pointsModifier));

            var limsOfType = limitations
                .Where(l => l.systemTypes.Contains(type))
                .ToArray();
            for (int i = 0; i < numberOfLimitations; i++) {
                int index = _r.Next(limsOfType.Length);
                lims.Add(limsOfType[index]);
            }

            var realCost = activeCost / (1 + lims.Sum(l => l.pointsModifier));

            return _ctors[type](realCost, advs, lims); ;
        }
    }
}
