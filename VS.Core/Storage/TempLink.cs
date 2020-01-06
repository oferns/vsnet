namespace VS.Core.Storage {

    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TempLink {


        // [Required]
        public int AccessLevel { get; set; }

        // [Required]
        public Uri Path { get; set; }

        // [Required]
        public DateTime Starts { get; set; }

        // [Required]
        public DateTime Expires { get; set; }

    }
}
