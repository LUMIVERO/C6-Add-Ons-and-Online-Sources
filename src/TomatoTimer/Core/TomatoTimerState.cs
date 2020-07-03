using System;
using System.Drawing;

namespace SwissAcademic.Addons.TomatoTimerAddon
{
    internal class TomatoTimerState
    {
        // Constructors

        public TomatoTimerState(int period) => Minutes = period;

        // Properties

        public int Minutes { get; }

        public virtual TimerState State { get; }

        public virtual Image Image { get; }

        public string Message => MessageResolver.Invoke();

        public Func<string> MessageResolver { get; set; } = () => null;

        public virtual string Status { get; }
    }

    internal class StoppedTimer : TomatoTimerState
    {
        // Constructors

        public StoppedTimer(int period) : base(period) { }

        // Properties

        public override TimerState State => TimerState.Stopped;

        public override Image Image => Properties.Resources.tomato_stop;

        public override string Status => Properties.Resources.StartMessage;

    }

    internal class RunningTimer : TomatoTimerState
    {
        // Constructors

        public RunningTimer(int period) : base(period) { }

        // Properties

        public override TimerState State => TimerState.Running;

        public override Image Image => Properties.Resources.tomato;

        public override string Status => Properties.Resources.Active;
    }

    internal class WalkingTimer : TomatoTimerState
    {
        // Constructors

        public WalkingTimer(int period) : base(period) { }

        // Properties

        public override TimerState State => TimerState.Walking;

        public override Image Image => Properties.Resources.walk;

        public override string Status => Properties.Resources.Rest;
    }

    internal class PausingTimer : TomatoTimerState
    {
        // Constructors

        public PausingTimer(int period) : base(period) { }

        // Properties

        public override TimerState State => TimerState.Pausing;

        public override Image Image => Properties.Resources.pause;

        public override string Status => Properties.Resources.Rest;
    }
}
