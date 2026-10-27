

namespace TDD.Task;
public class Game
{
    private readonly List<Frame> _frames = new List<Frame>();

    private Frame? CurrentFrame => _frames.LastOrDefault(f => !f.IsOver);
    private Frame? PreviousFrame => _frames.Count >= 2 ? _frames[^2] : null;

    public void Roll(int pins)
    {
        
        if (CurrentFrame == null || CurrentFrame.IsOver)
        {
            CreateNewFrame();
        }
        var currentFrame = CurrentFrame;
        if (currentFrame.FirstRoll.HasValue)
        {
            currentFrame.SecondRoll = pins;
            if (currentFrame.FirstRoll + currentFrame.SecondRoll == 10)
            {
                currentFrame.Bonus = BonusType.Spare;
            }
        }
        else
        {
            currentFrame.FirstRoll = pins;
            if (pins == 10)
            {
                currentFrame.Bonus = BonusType.Strike;
            }
        }

        if (PreviousFrame != null)
        {
            if (PreviousFrame.Bonus == BonusType.Spare)
            {
                PreviousFrame.BonusPoint = currentFrame.FirstRoll;
            }
            else if (PreviousFrame.Bonus == BonusType.Strike)
            {
                PreviousFrame.BonusPoint = currentFrame.FirstRoll + currentFrame.SecondRoll ?? 0;
            }
        }
    }

    public int GetScore()
    {
        return _frames.Sum(f => f.FirstRoll ?? 0 + f.SecondRoll ?? 0 + f.BonusPoint ?? 0);
    }

    private Frame CreateNewFrame()
    {
        _frames.Add(new Frame());
        return _frames.Last();
    }
}

public enum BonusType
{
    None,
    Spare,
    Strike
}

public class Frame
{
    public int? FirstRoll { get; set; }
    public int? SecondRoll { get; set; }
    public BonusType Bonus { get; set; }
    public bool IsOver => FirstRoll.HasValue && (FirstRoll == 10 || SecondRoll.HasValue);
    public int? BonusPoint { get; set; }
}


