﻿namespace VS.Abstractions.Storage {

    using System;

    [Flags]
    public enum AccessLevel {
        None = 0,
        Read = 1,
        Write = 2,
        Delete = 4,
        List = 8,
        Add = 16,
        Create = 32
    }
}