using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSA {
    public class Advantage : ShipSystemAttribute {
        public Advantage(string name, ShipSystemType[] systemTypes, double value) : base(name, systemTypes, value) { }
    }
}
