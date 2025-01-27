﻿using Cosmos.Core;
using System;

namespace PrismOS.Libraries.Text.INI
{
    public struct INIEntry : IDisposable
    {
        public INIEntry(Type Type, string Name, object Value)
        {
            this.Type = Type;
            this.Name = Name;
            this.Value = Value;
        }

        public Type Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        public void Dispose()
        {
            GCImplementation.Free(Type);
            GCImplementation.Free(Name);
            GCImplementation.Free(Value);
            GC.SuppressFinalize(this);
        }
    }
}