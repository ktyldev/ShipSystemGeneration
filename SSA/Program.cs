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
                new Advantage("fast shift", new[] { ShipSystemType.reactor }, 0.1, ShipSystemAttribute.Tag.shift),
                new Advantage("reduced mass", allTypes, 0.15, ShipSystemAttribute.Tag.mass),
                new Advantage("decreased burnout", new [] { ShipSystemType.reactor }, 0.1, ShipSystemAttribute.Tag.burnout),
                new Advantage("test advantage 1", allTypes, 0.1),
                new Advantage("test advantage 2", allTypes, 0.1),
                new Advantage("test advantage 3", allTypes, 0.1)
            });
            factory.limitations.AddRange(new[] {
                new Limitation("slow shift", new [] { ShipSystemType.reactor }, 0.1, ShipSystemAttribute.Tag.shift),
                new Limitation("extra mass", allTypes, 0.15, ShipSystemAttribute.Tag.mass),
                new Limitation("increased burnout", new [] { ShipSystemType.reactor }, 0.1, ShipSystemAttribute.Tag.burnout)
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
        Random _random = new Random();

        // To be set in inspector
        public List<Advantage> advantages;
        public List<Limitation> limitations;

        public ShipSystemFactory() {
            advantages = new List<Advantage>();
            limitations = new List<Limitation>();
            _ctors = new Dictionary<ShipSystemType, Func<double, List<Advantage>, List<Limitation>, ShipSystem>>();

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
            int numberOfAdvantages = _random.Next(0, 3); // 0-2 advantages
            int numberOfLimitations = _random.Next(0, 3); // 0-2 limitations

            var advs = GetDistinctAttributes(advantages, type, numberOfAdvantages);

            var availableTags = ((ShipSystemAttribute.Tag[])Enum.GetValues(typeof(ShipSystemAttribute.Tag))).ToList();
            foreach (var advantage in advs) {
                if (advantage.tag == ShipSystemAttribute.Tag.none) 
                    continue;

                if (availableTags.Contains(advantage.tag)) {
                    availableTags.Remove(advantage.tag);
                }
            }

            var availableLimitations = limitations
                .Where(l => availableTags.Contains(l.tag))
                .ToList();

            var lims = GetDistinctAttributes(availableLimitations, type, numberOfLimitations);
            
            return CreateSystem(type, baseCost, advs, lims);
        }

        private ShipSystem CreateSystem(ShipSystemType type, int baseCost, List<Advantage> advantages, List<Limitation> limitations) {
            advantages
                .Select(a => (ShipSystemAttribute)a)
                .Concat(limitations
                .Select(l => (ShipSystemAttribute)l))
                .ToList().ForEach(ssa => {
                    if (!ssa.systemTypes.Contains(type))
                        throw new ArgumentException(ssa.name + " is not compatible with system type: " + type);
                });

            var activeCost = baseCost * (1 + advantages.Sum(a => a.pointsModifier));
            var realCost = activeCost / (1 + limitations.Sum(l => l.pointsModifier));

            return _ctors[type](realCost, advantages, limitations);
        }

        private List<T> GetDistinctAttributes<T>(List<T> attributes, ShipSystemType type, int count) where T : ShipSystemAttribute {
            var attributeArray = attributes
                .Where(a => a.systemTypes.Contains(type))
                .ToArray();

            if (count > attributeArray.Length)
                throw new Exception(count + " less than number of possible choices");

            Shuffle(attributeArray);

            var results = new List<T>();
            for (int i = 0; i < count; i++) {
                results.Add(attributeArray[i]);
            }
            return results;
        }

        private void Shuffle<T>(T[] array) {
            int n = array.Length;
            for (int i = 0; i < n; i++) {
                int r = i + (int)(_random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }
    }
}
