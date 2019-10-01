using System.Collections.Generic;

namespace SwissAcademic.Addons.TomatoTimer
{
    internal class TomatoTimerStates : List<TomatoTimerState>
    {
        #region Fields

        int _counter;

        #endregion

        #region Constructors

        public TomatoTimerStates(TomatoTimerState defaultTimer)
        {
            _counter = 0;
            Default = defaultTimer;
        }

        #endregion

        #region Properties

        public TomatoTimerState Current => this[_counter];

        public TomatoTimerState Default { get; }

        #endregion

        #region Methods

        public TomatoTimerState Next()
        {
            _counter++;
            if (_counter == this.Count) _counter = 0;
            return Current;
        }

        public void Reset()
        {
            _counter = 0;
        }

        #endregion
    }
}
