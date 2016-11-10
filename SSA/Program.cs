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
}
