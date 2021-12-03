using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode2021.Day1
{
    public class SlidingWindowOf3SummationIterator : IEnumerable<int>, IEnumerator<int>
    {
        private IEnumerator<int>? _sourceIterator;
        private State _state = State.NoNumbersInSlidingWindow;
        private int _secondPreviousElement;
        private int _previousElement;
        private int _currentElement;

        public SlidingWindowOf3SummationIterator(IEnumerable<int> source)
        {
            _sourceIterator = source.GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool MoveNext()
        {
            while (_state != State.EndReached)
            {
                if (_sourceIterator.MoveNext())
                {
                    var currentValue = _sourceIterator.Current;
                    switch (_state)
                    {
                        case State.NoNumbersInSlidingWindow:
                            _currentElement = currentValue;
                            _state = State.OnlyOneNumberInSlidingWindow;
                            break;
                        case State.OnlyOneNumberInSlidingWindow:
                            _previousElement = _currentElement;
                            _currentElement = currentValue;
                            _state = State.OnlyTwoNumbersInSlidingWindow;
                            break;
                        case State.OnlyTwoNumbersInSlidingWindow:
                        case State.SlidingWindowFullyFilled:
                            _secondPreviousElement = _previousElement;
                            _previousElement = _currentElement;
                            _currentElement = currentValue;
                            Current = _currentElement + _previousElement + _secondPreviousElement;
                            _state = State.SlidingWindowFullyFilled;
                            return true;
                    }
                }
                else
                {
                    _state = State.EndReached;
                    return false;
                }
            }

            return false;
        }

        public void Reset()
        {
            throw new InvalidOperationException("This iterator cannot be reset.");
        }

        public int Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _state = State.EndReached;
            if (_sourceIterator != null)
            {
                _sourceIterator.Dispose();
                _sourceIterator = null;
            }
        }

        private enum State
        {
            NoNumbersInSlidingWindow,
            OnlyOneNumberInSlidingWindow,
            OnlyTwoNumbersInSlidingWindow,
            SlidingWindowFullyFilled,
            EndReached
        }
    }
}