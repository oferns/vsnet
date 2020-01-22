namespace VS.PostGres.Tests {

    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Net;

    [Description("A test table.")]
    public class TestTable {

        [Description("An Integer property.")]
        public int IntProp { get; set; }

        [MaxLength(24)]
        [Description("An String property. Can't be longer than 24 characters.")]
        public string StringProp { get; set; }

        [Description("A Floating number property.")]
        public float FloatProp { get; set; }

        [Description("A Double number property.")]
        public double DoubleProp { get; set; }

        public decimal DecimalProp { get; set; }

        public bool BoolProp { get; set; }

        public char CharacterProp { get; set; }

        public DateTime DateTimeProp { get; set; }

        public DateTimeOffset DateTimeOffsetProp { get; set; }
        
        public IPAddress  IPAddressProp { get; set; }

        public ValueTuple<IPAddress, int> CIDRAddressProp { get; set; }

    }
}