using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PttLib.CaptchaBreaker.Coral.Guesser
{
    class IntVector : IEnumerable
    {
        private List<int> _items;
        public IntVector()
        {
            _items = new List<int>();
        }
        public IntVector(List<int> elements)
        {
            _items = elements;
        }
        public void Add(int element)
        {
            _items.Add(element);
        }
        public int Count()
        {
            return _items.Count();
        }
        public bool Any()
        {
            return _items.Any();
        }
        public double Magnitude()
        {
            double sumOfSquares = 0;
            foreach (var item in _items)
            {
                sumOfSquares += item * item;
            }
            return Math.Sqrt(sumOfSquares);
        }
        public int this[int index]
        {
            get
            {
                return _items[index];
            }
        }

        public int DotProduct(IntVector otherIntVector)
        {
            if (this.Count() != otherIntVector.Count())
            {
                throw new ArgumentException("vectors need to have equal elements!");
            }
            if (!this.Any()) return 0;

            var result = 0;
            for (int posCounter = 0; posCounter < _items.Count; posCounter++)
            {
                result += _items[posCounter] * otherIntVector[posCounter];
            }
            return result;
        }

        public double NormalizedProduct(IntVector otherIntVector)
        {
            if (this.Count() != otherIntVector.Count())
            {
                throw new ArgumentException("vectors need to have equal elements!");
            }
            if (!this.Any()) return 0;
            var dotProduct = this.DotProduct(otherIntVector);
            var magnitudeProduct = this.Magnitude() * otherIntVector.Magnitude();
            if (magnitudeProduct == 0) return 0;
            return (double)dotProduct / magnitudeProduct;
        }

        #region Implementation of IEnumerable

        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion
    }
}