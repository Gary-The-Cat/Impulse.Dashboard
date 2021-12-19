// <copyright file="SingleCoverageValue.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Impulse.Shared.Datastructures
{
    public class SingleCoverageValue<T> : IEnumerable<T>
    {
        private static Random random = new Random();
        private List<double> keys;
        private Dictionary<double, DoubleRange<T>> spans;

        public SingleCoverageValue(IEnumerable<DoubleRange<T>> values)
        {
            spans = new Dictionary<double, DoubleRange<T>>();

            foreach (var value in values)
            {
                spans.Add(value.Midpoint, value);
            }

            keys = spans.Keys.OrderBy(k => k).ToList();
        }

        public double Earliest => keys.First();

        public double Latest => keys.Last();

        public T Sample => this[Earliest + (random.NextDouble() * (Latest - Earliest))];

        public T this[double x] => this.GetValue(x);

        public T GetValue(double x)
        {
            var key = this.GetKey(x);
            return spans[key].Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var key in keys)
            {
                yield return this[key];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private double GetKey(double value)
        {
            int index;

            if (value < Earliest)
            {
                return keys.First();
            }
            else if (value > Latest)
            {
                return keys.Last();
            }
            else
            {
                index = keys.BinarySearch(value);
            }

            if (index < 0)
            {
                // Bitwise complement
                index = -index - 1;
            }

            if (spans[keys[index]].Contains(value))
            {
                return keys[index];
            }

            if (index > 0 && spans[keys[index - 1]].Contains(value))
            {
                return keys[index - 1];
            }

            if (index < (keys.Count - 1) && spans[keys[index]].Contains(value))
            {
                return keys[index];
            }

            throw new Exception("Invalid Span Value Requested");
        }
    }
}
