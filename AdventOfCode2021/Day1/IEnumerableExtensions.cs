using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Day1
{
    public static class IEnumerableExtensions
    {
        /// <exception cref="ArgumentNullException">Evaluating the return result can throw this exception if
        /// <paramref name="integersAsStrings"/> contains null.</exception>
        /// <exception cref="FormatException">Evaluating the return result can throw this exception if
        /// <paramref name="integersAsStrings"/> contains text that cannot be parsed into an <see cref="int"/>.</exception>
        /// <exception cref="OverflowException">Evaluating the return result can throw this exception if
        /// <paramref name="integersAsStrings"/> contains text that represents a number that is too big to be represented
        /// as <see cref="int"/>.</exception>
        public static IEnumerable<int> ToIntegers(this IEnumerable<string> integersAsStrings)
        {
            return integersAsStrings.Select(int.Parse);
        }

        public static int CountNumberOfTimesCurrentElementIsHigherThanPreviousElement(
            this IEnumerable<int> source)
        {
            bool isFirstElement = true;
            int previousElement = default;
            int numberOfTimesCurrentElementWasHigherThanPrevious = 0;
                
            foreach (int element in source)
            {
                if (isFirstElement)
                {
                    isFirstElement = false;
                }
                else
                {
                    if (element > previousElement)
                    {
                        numberOfTimesCurrentElementWasHigherThanPrevious += 1;
                    }
                }

                previousElement = element;
            }

            return numberOfTimesCurrentElementWasHigherThanPrevious;
        }

        public static IEnumerable<int> ToSumsOfSlidingWindowOfSize3(
            this IEnumerable<int> source) =>
            new SlidingWindowOf3SummationIterator(source);
    }
}