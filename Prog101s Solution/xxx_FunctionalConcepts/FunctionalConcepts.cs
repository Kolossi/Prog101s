using System;
using System.Collections.Generic;
using System.Linq;

namespace Prog101s.FunctionalConcepts
{
    public static class Concepts
    {
        public interface ISetoid<T>
        {
            Func<T, T, bool> IsEquivalent { get; set; }
        }

        public interface ISemigroup<T> : ISetoid<T>
        {
            Func<T, T, T> AssociativeOperation { get; set; }
        }

        public class Semigroup<T> : ISemigroup<T>
        {
            public Func<T, T, bool> IsEquivalent { get; set; }
            public Func<T, T, T> AssociativeOperation { get; set; }

            public Semigroup(Func<T, T, bool> isEquivalent, Func<T, T, T> associativeOperation)
            {
                IsEquivalent = isEquivalent;
                AssociativeOperation = associativeOperation;
            }

            public bool IsValidSemigroup(ICollection<T> set)
            {
                foreach (var x in set)
                {
                    foreach (var y in set.Where(s => !IsEquivalent(s, x)))
                    {
                        foreach (var z in set.Where(s => !IsEquivalent(s,x) && !IsEquivalent(s, y)))
                        {
                            if (!IsEquivalent(
                                    AssociativeOperation(AssociativeOperation(x, y), z),
                                    AssociativeOperation(x, AssociativeOperation(y, z))))
                                return false;
                        }
                    }
                }
                return true;
            }
        }




        public static bool IntEquivalent(int x, int y)
        {
            return x == y;
        }

        public static int IntAdd(int x, int y)
        {
            return x + y;
        }

        public static int IntSubtract(int x, int y)
        {
            return x - y;
        }

        public static void CheckIntAddSemigroup()
        {
            var intSemigroup = new Semigroup<int>(IntEquivalent, IntAdd);

            var positiveInts = Enumerable.Range(1, 100).ToArray();

            var semigroupValid = intSemigroup.IsValidSemigroup(positiveInts);

            Console.WriteLine(string.Format("Positive ints with add is {0}a semigroup",
                 !semigroupValid ? "NOT " : ""));
        }

        public static void CheckIntSubtractSemigroup()
        {
            var intSemigroup = new Semigroup<int>(IntEquivalent, IntSubtract);

            var positiveInts = Enumerable.Range(1, 100).ToArray();

            var semigroupValid = intSemigroup.IsValidSemigroup(positiveInts);

            Console.WriteLine(string.Format("Positive ints with subtract is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }


        public static bool FloatEquivalent(float x, float y)
        {
            return x == y;
        }

        public static bool FloatEqualish(float x, float y)
        {
            return Math.Abs(x - y) < 0.00001;
        }

        public static float FloatAdd(float x, float y)
        {
            return x + y;
        }

        public static void CheckFloatAddSemigroup()
        {
            var floatSemigroup = new Semigroup<float>(FloatEquivalent, FloatAdd);

            var positiveFloats = Enumerable.Range(1, 100)
                .Select(i => (float)i / 1000f).ToArray(); //0.001, 0.002, ...0.100

            var semigroupValid = floatSemigroup.IsValidSemigroup(positiveFloats);

            Console.WriteLine(string.Format("Positive floats with add is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }

        public static void CheckFloatAddEqualishSemigroup()
        {
            var floatSemigroup = new Semigroup<float>(FloatEqualish, FloatAdd);

            var positiveFloats = Enumerable.Range(1, 100)
                .Select(i => (float)i / 1000f).ToArray(); //0.001, 0.002, ...0.100

            var semigroupValid = floatSemigroup.IsValidSemigroup(positiveFloats);

            Console.WriteLine(string.Format("Positive floats with add and equalish is {0}a semigroup",
                !semigroupValid ? "NOT " : ""));
        }

        public static void Process()
        {
            CheckIntAddSemigroup();

            CheckIntSubtractSemigroup();

            CheckFloatAddSemigroup();

            CheckFloatAddEqualishSemigroup();
        }
    }
}
