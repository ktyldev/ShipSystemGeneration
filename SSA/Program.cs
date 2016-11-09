using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSA {
    class Program {
        static void Main(string[] args) {
            var factory = new ShipSystemFactory();
            factory.advantages.AddRange(new[] {
                new Advantage("fast shift", new[] { ShipSystemType.reactor }, 0.1),
                new Advantage("reduced mass", new [] { ShipSystemType.armour, ShipSystemType.cloak, ShipSystemType.reactor }, 0.15),
                new Advantage("decreased burnout", new [] { ShipSystemType.reactor }, 0.1)
            });
            factory.limitations.AddRange(new[] {
                new Limitation("slow shift", new [] { ShipSystemType.reactor }, 0.1),
                new Limitation("extra mass", new [] { ShipSystemType.armour, ShipSystemType.cloak, ShipSystemType.reactor }, 0.15),
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
            Console.WriteLine("Created sytem of type: " + reactor.type);
            Console.WriteLine("  Cost: " + reactor.realCost);
            Console.WriteLine("  Advantages:");
            foreach (var adv in reactor.advantages)
                Console.WriteLine("    " + adv.name);
            Console.WriteLine("  Limitations:");
            foreach (var lim in reactor.limitations)
                Console.WriteLine("    " + lim.name);

            Console.ReadLine();
        }
    }

    class Reactor : ShipSystem {
        public override ShipSystemType type {
            get { return ShipSystemType.reactor; }
        }

        public Reactor(double realCost, List<Advantage> advantages, List<Limitation> limitations) : base(realCost, advantages, limitations) { }
    }

    class ShipSystemFactory {
        Random _r = new Random();

        public List<Advantage> advantages = new List<Advantage>();
        public List<Limitation> limitations = new List<Limitation>();

        private ShipSystem CreateSystem(ShipSystemType type, double realCost, List<Advantage> advantages, List<Limitation> limitations) {
            switch (type) {
                case ShipSystemType.reactor:
                    return new Reactor(realCost, advantages, limitations);
                case ShipSystemType.sensor:
                case ShipSystemType.hold:
                case ShipSystemType.cloak:
                case ShipSystemType.quarters:
                case ShipSystemType.thruster:
                case ShipSystemType.shield:
                case ShipSystemType.armour:
                case ShipSystemType.weapon:
                default:
                    throw new NotImplementedException();
            }
        }

        public ShipSystem CreateSystem(ShipSystemType type, int baseCost) {
            int numberOfAdvantages = _r.Next(0, 3); // 0-2 advantages
            int numberOfLimitations = _r.Next(0, 3); // 0-2 limitations

            Console.WriteLine("Creating system of type: " + type);
            Console.WriteLine("  Adv count: " + numberOfAdvantages);
            Console.WriteLine("  Lim count: " + numberOfLimitations);

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

            return CreateSystem(type, realCost, advs, lims);
        }
    }
}
