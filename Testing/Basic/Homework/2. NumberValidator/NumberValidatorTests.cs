
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1)]
    [TestCase(0)]
    public void Constructor_ThrowArgumentException_ForNegativePrecision(int precision)
    {
        var exception = () => new NumberValidator(precision, 0);
        exception.Should().Throw<ArgumentException>("precision must be a positive number");
    }

    [TestCase(1, -1, TestName = "Scale less than zero")]
    [TestCase(1, 2, TestName = "Scale more than precision")]
    [TestCase(2, 2, TestName = "Scale equals precision")]
    public void Constructor_ThrowArgumentException_ForInvalidScale(int precision, int scale)
    {
        var exception = () => new NumberValidator(precision, scale);
        exception.Should().Throw<ArgumentException>("precision must be a non-negative number less or equal than precision");
    }

    [TestCase(null, TestName = "number is null")]
    [TestCase("", TestName = "number is empty string")]
    public void IsValidNumber_ReturnsFalse_ForNullOrEmpty(string value)
    {
        var numberValidator = new NumberValidator(10);
        numberValidator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase("abc")]
    [TestCase("1.2.3")]
    [TestCase("1.")]
    [TestCase(".1")]
    [TestCase("++1")]
    [TestCase(".")]
    public void IsValidNumber_ReturnsFalse_ForInvalidFormat(string value)
    {
        var numberValidator = new NumberValidator(10, 2);
        numberValidator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase(10, 2, "0.000")]
    [TestCase(10, 2, "1.234")]
    [TestCase(10, 1, "123.45")]
    public void IsValidNumber_ReturnsFalse_WhenScaleOverflow(int precision, int scale, string value)
    {
        var numberValidator = new NumberValidator(precision, scale);
        numberValidator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase(3, 2, "00.00")]
    [TestCase(3, 2, "-0.00")]
    [TestCase(3, 2, "+0.00")]
    public void IsValidNumber_ReturnsFalse_WhenPrecisionOverflow(int precision, int scale, string value)
    {
        var numberValidator = new NumberValidator(precision, scale);
        numberValidator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase("-1")]
    [TestCase("-1.23")]
    [TestCase("-0")]
    public void IsValidNumber_ReturnsFalse_WhenValueNegativeButOnlyPositiveTrue(string value)
    {
        var numberValidator = new NumberValidator(10, 2, onlyPositive: true);
        numberValidator.IsValidNumber(value).Should().BeFalse();
    }

    [TestCase(17, 2, false, "0", TestName = "Integer zero")]
    [TestCase(17, 2, true, "0", TestName = "Zero with onlyPositive true")]
    [TestCase(17, 2, true, "+0", TestName = "Explicit plus zero with onlyPositive true")]
    [TestCase(17, 2, false, "0.0", TestName = "Zero with decimal")]
    [TestCase(17, 2, false, "0,0", TestName = "Zero with comma")]
    [TestCase(17, 2, false, "123", TestName = "plain integer")]
    [TestCase(17, 2, false, "123.45", TestName = "Plain decimal")]
    [TestCase(17, 2, false, "123,45", TestName = "plain decimal with comma")]
    [TestCase(3, 2, false, "1.23", TestName = "total length = precision = 3")]
    [TestCase(5, 2, false, "+1234", TestName = "plus sign precision = 5")]
    [TestCase(5, 2, false, "00", TestName = "Two zeros precision = 5")]
    [TestCase(5, 2, false, "00123", TestName = "First zeros precision = 5")]
    [TestCase(5, 2, false, "00.12", TestName = "first zeros with decimals precision = 5")]
    public void IsValidNumber_ReturnsTrue_ForValidInput(int precison, int scale, bool onlyPositive, string value)
    {
        var numberValidator = new NumberValidator(precison, scale, onlyPositive);
        numberValidator.IsValidNumber(value).Should().BeTrue();
    }
}