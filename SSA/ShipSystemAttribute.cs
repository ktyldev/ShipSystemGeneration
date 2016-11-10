using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSA {
    public abstract class ShipSystemAttribute {
        public enum Tag {
            shift,
            burnout,
            mass,
            none
        }

        public string name { get; private set; }
        public ShipSystemType[] systemTypes { get; private set; }
        public double pointsModifier { get; private set; }
        public Tag tag { get; private set; }

        public ShipSystemAttribute(string name, ShipSystemType[] systemTypes, double pointsModifier) 
            : this(name, systemTypes, pointsModifier, Tag.none) { }

        public ShipSystemAttribute(string name, ShipSystemType[] systemTypes, double pointsModifier, Tag tag) {
            this.name = name;
            this.systemTypes = systemTypes;
            this.pointsModifier = pointsModifier;
            this.tag = tag;
        }
    }
}
