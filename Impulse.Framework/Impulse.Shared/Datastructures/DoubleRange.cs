// <copyright file="DoubleRange.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Impulse.Shared.Datastructures
{
    // :TODO: I don't know if 'DoubleRange' is appropriate just because the range is over
    // a double value, does it make sense to have additional range types? Discrete int or Vector?
    public class DoubleRange<T> : ValueObject
    {
        public DoubleRange(double from, double to, T value)
        {
            Range = (from, to);
            Midpoint = from + ((to - from) / 2);
            Value = value;
        }

        public (double From, double To) Range { get; }

        public double Midpoint { get; }

        public T Value { get; }

        public bool Contains(double value) => value >= Range.From && value < Range.To;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Range;
            yield return Value;
        }
    }
}
