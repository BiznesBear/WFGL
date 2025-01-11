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
    public virtual void Dispose() { timeMaster.Update -= Frame; GC.SuppressFinalize(this); }
}

public class Counter(Time time, float duration, bool loopAfterEnd, Action act) : BaseCounter(time, duration, loopAfterEnd)
{
    public Counter(Time time, CounterEvent cevent, bool loopAfterEnd) : this(time, cevent.Duration, loopAfterEnd, cevent.Action) { }

    public override void OnCountingEnd()
    {
        base.OnCountingEnd();
        act?.Invoke();
    }
}

public record CounterEvent(float Duration, Action Action);
public class SequanceCounter : BaseCounter
{
    public List<CounterEvent> events;
    private int current;

    public SequanceCounter(Time time, bool loop = true, params List<CounterEvent> cEvents) : base(time, 1, loop)
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

        // check if current is ending sequance
        if (current > events.Count) current = 0;
    }
}
