using System.Collections.Generic;

namespace SwissAcademic.Addons.TomatoTimerAddon
{
    internal class TomatoTimerStates : List<TomatoTimerState>
    {
        // Fields

        int _counter;

        // Constructors

        public TomatoTimerStates(TomatoTimerState defaultTimer) => (_counter, Default) = (0, defaultTimer);

        // Properties

        public TomatoTimerState Current => this[_counter];

        public TomatoTimerState Default { get; }

        // Methods

        public TomatoTimerState Next()
        {
            _counter++;
            if (_counter == Count)
            {
                _counter = 0;
            }

            return Current;
        }

        public void Reset() => _counter = 0;
    }
}
