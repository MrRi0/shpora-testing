using Basic.Task.WordsStatistics.WordsStatistics;
using FluentAssertions;
using NUnit.Framework;

namespace Basic.Task.WordsStatistics;

// Документация по FluentAssertions с примерами : https://github.com/fluentassertions/fluentassertions/wiki

[TestFixture]
public class WordsStatisticsTests
{

    private IWordsStatistics wordsStatistics;

    [SetUp]
    public void SetUp()
    {

        wordsStatistics = CreateStatistics();
    }

    public virtual IWordsStatistics CreateStatistics()
    {
        // меняется на разные реализации при запуске exe
        return new WordsStatisticsImpl();
    }


    [Test]
    public void GetStatistics_IsEmpty_AfterCreation()
    {
        wordsStatistics.GetStatistics().Should().BeEmpty();
    }

    [Test]
    public void GetStatistics_ContainsItem_AfterAddition()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
    }

    [Test]
    public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
    {
        wordsStatistics.AddWord("abc");
        wordsStatistics.AddWord("def");
        wordsStatistics.GetStatistics().Should().HaveCount(2);
    }

    [Test]
    public void Getstatistics_ThrowArgumentNullException_AfterAddingNullWord()
    {
        var addNullWordAction = () => wordsStatistics.AddWord(null);
        addNullWordAction.Should().Throw<ArgumentNullException>();
    }

    [TestCase("", TestName = "EmptyString")]
    [TestCase(" ", TestName = "WhitespaceString")]
    public void Getstatistics_Ignore_AfterAddingWhitespace(string word)
    {
        var countBeforeAdding = wordsStatistics.GetStatistics().Count();
        wordsStatistics.AddWord(word);
        wordsStatistics.GetStatistics().Should().HaveCount(countBeforeAdding);
    }


    [TestCase("1234567890", TestName = "Слово длиной 10 добавляяется полностью")]
    [TestCase("12345678901", TestName = "Слово длиной больше 10 обрезается")]
    public void Getstatistics_CutWords_IfMoreThan10Letters(string word)
    {
        wordsStatistics.AddWord(word);
        wordsStatistics.GetStatistics().FirstOrDefault(x => x.Word == word.Substring(0, 10)).Should().NotBe(null);
    }

    [Test]
    public void GetStatistics_IncreaseCount_AfterAddingSameWords()
    {
        var word = "word";
        wordsStatistics.AddWord(word);
        wordsStatistics.AddWord(word);
        wordsStatistics.GetStatistics().FirstOrDefault(x => x.Word == word).Should().Be(new WordCount(word, 2));
    }

    [Test]
    public void GetStatistics_ReturnsWordsByDescendingCountThenAlphabetically()
    {
        wordsStatistics.AddWord("beta");
        wordsStatistics.AddWord("beta");
        wordsStatistics.AddWord("gamma");
        wordsStatistics.AddWord("gamma");
        wordsStatistics.AddWord("alpha");
        wordsStatistics.AddWord("alpha");
        wordsStatistics.AddWord("alpha");

        wordsStatistics.GetStatistics().Should().Equal(
        new WordCount("alpha", 3),
        new WordCount("beta", 2),
        new WordCount("gamma", 2));
    }

    [Test]
    public void GetStatistics_ContainsWordToLower_AfterAddingWordToUpper()
    {
        var word = "ABC";
        wordsStatistics.AddWord(word);
        wordsStatistics.GetStatistics().Should().Contain(new WordCount(word.ToLower(), 1));
    }

    [Test]
    public void GetStatistics_IncreaseCount_AfterAddingWordMoreThan10Letters()
    {
        var word1 = "1234567890";
        var word2 = "12345678901";
        wordsStatistics.AddWord(word1);
        wordsStatistics.AddWord(word2);
        wordsStatistics.GetStatistics().FirstOrDefault(x => x.Word == word1).Should().Be(new WordCount(word1, 2));
    }

    [Test]
    [Timeout(1000)]
    public void AddWord_ManyDifferentWords_CompletesWithinTimeLimit()
    {
        for (int i = 0; i < 10_000; i++)
        {
            wordsStatistics.AddWord($"word{i}");
        }
    }

    [Test]
    public void AddWord_LargeNumberOfUniqueWords_AddsAllWords()
    {
        for (int i = 0; i < 10_000; i++)
        {
            wordsStatistics.AddWord($"word{i}");
        }

        wordsStatistics.GetStatistics().Should().HaveCount(10_000);
    }

    //- E - Null, Empty, Whitespaces
    //- CR - Whitespaces
    //- C - Case
    //- O - Order
    //- L - Length
    //- 123 - Hash
    //- 998 - Add Different Performance
    //- 999 - Add Same Performance
    //- QWE - All chars lower case
    //- STA - Static
    //- EN - To cache or not to cache

}