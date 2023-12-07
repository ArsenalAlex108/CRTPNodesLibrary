namespace CRTPNodesLibrary.Iterables;
public static partial class EnumerableExtensions
{
    private struct NonRefStructIntEnumerator
    {
        private readonly int _end;
        private readonly bool _forward;

        public NonRefStructIntEnumerator(Range range)
        {
            if (range.Start.IsFromEnd)
            {
                throw new ArgumentException($"{nameof(range)} must be from start.");
            }

            _forward = Current >= _end;

            Current = range.Start.Value + (_forward ? -1 : 1);
            _end = range.End.Value;

        }

        public int Current { get; private set; }

        public bool MoveNext()
        {
            if (_forward)
            {
                if (Current >= _end - 1) return false;

                Current++;
                return true;
            }
            else
            {
                if (Current <= _end + 1) return false;

                Current--;
                return true;
            }
        }
    }
}
