using FluentAssertions;
using NUnit.Framework;
using TDD.Task;

namespace TDD;

[TestFixture]
public class GameTests
{
    [Test]
    [Explicit]
    public void HaveZeroScore_BeforeAnyRolls()
    {
        new Game()
            .GetScore()
            .Should().Be(0);
    }

    [Test]
    public void a()
    {
        var game = new Game();
        game.Roll(5);
        game.GetScore().Should().Be(5);
    }

    [Test]
    public void b()
    {
        var game = new Game();
        game.Roll(5);
        game.Roll(5);
        game.GetScore().Should().Be(10);
    }

    [Test]
    public void HaveCorrectScore_AfterRolling()
    {
        var game = new Game();
        game.Roll(5);
        game.Roll(3);
        game.GetScore().Should().Be(8);
    }

    [Test]
    public void HaveCorrectScore_AfterRolling_Strike()
    {
        var game = new Game();
        game.Roll(10);
        game.Roll(5);
        game.Roll(3);
        game.GetScore().Should().Be(10 + (5 + 3) * 2);
    }

    [Test]
    public void HaveCorrectScore_AfterRolling_Spare()
    {
        var game = new Game();
        game.Roll(9);
        game.Roll(1);
        game.Roll(5);
        game.Roll(3);
        game.GetScore().Should().Be(9 + 1 + 5 + 5 + 3);
    }
}