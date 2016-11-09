using System.Collections.Generic;

namespace SSA {
    public class Limitation : ShipSystemAttribute {
        public Limitation(string name, ShipSystemType[] systemTypes, double value) : base(name, systemTypes, value) { }
    }
}