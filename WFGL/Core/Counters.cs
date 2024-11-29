using WFGL.Utilities;

namespace WFGL.Core;
public class BaseCounter : IDisposable
{
    protected float maxTime;
    protected Time timeMaster;
    protected bool loop;

    private float timer;
    public BaseCounter(Time time, float duration, bool loopAfterEnd)
    {
        maxTime = duration;

        timeMaster = time;
        loop = loopAfterEnd;

        timeMaster.Update += Frame;
    }

    private void Frame()
    {
        timer += timeMaster.DeltaTimeF;
        if (timer >= maxTime)
        {
            OnCountingEnd();
            timer = 0;
            if (!loop) Dispose();
        }
    }
    public virtual void OnCountingEnd() { }
    public virtual void Dispose() { timeMaster.Update -= Frame; }
}

public class Counter : BaseCounter
{
    private Action action;

    public Counter(Time time, float duration, bool loopAfterEnd, Action act) : base(time, duration, loopAfterEnd)
    {
        action = act;
    }
    public Counter(Time time, CounterEvent cevent, bool loopAfterEnd) : this(time, cevent.Duration, loopAfterEnd, cevent.Action) { }

    public override void OnCountingEnd()
    {
        base.OnCountingEnd();
        action?.Invoke();
    }
}

public record CounterEvent(float Duration, Action Action);
public class SequanceCounter : BaseCounter
{
    public List<CounterEvent> events;
    private int current;

    public SequanceCounter(Time time, params List<CounterEvent> cEvents) : base(time, 1, true)
    {
        events = cEvents;
        maxTime = events.First().Duration;

        if (cEvents.Count < 1)
        {
            Wrint.Error($"Empty {nameof(CounterEvent)} list in {nameof(SequanceCounter)}");
            Dispose();
        }

    }
    public override void OnCountingEnd()
    {
        base.OnCountingEnd();

        events[current].Action?.Invoke();
        current++;

        maxTime = events[current].Duration;

        // check if its last
        if (current > events.Count) current = 0;
    }
}
