using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Data;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        //Перепишите код на использование Fluent Assertions.
        actualTsar.Should().BeEquivalentTo(expectedTsar, options =>
            options.Excluding(memberInfo => 
                memberInfo.DeclaringType == typeof(Person) 
                && memberInfo.Name == nameof(Person.Id)));

        //С таким подходом код стал компактее и читабельнее.

        // upd: Теперь мы отсекаем Id на любой глубине и при добалении предков тест не провалится
        // Про "практически ничего не придется менять"
        // При добавлении свойства в Person тест потребует изменений обновить
        // expectedTsar или дополнить Excluding (для уникальных свойств по типу id).
        // При этом AreEqual просто перестаёт проверять новые свойства тогда как
        // BeEquivalentTo заставляет обновить тест

        //upd2: Теперь если переименуем поле IDE сообщит нам или само переименует 

        //ClassicAssert.AreEqual(actualTsar.Name, expectedTsar.Name);
        //ClassicAssert.AreEqual(actualTsar.Age, expectedTsar.Age);
        //ClassicAssert.AreEqual(actualTsar.Height, expectedTsar.Height);
        //ClassicAssert.AreEqual(actualTsar.Weight, expectedTsar.Weight);

        //ClassicAssert.AreEqual(expectedTsar.Parent!.Name, actualTsar.Parent!.Name);
        //ClassicAssert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
        //ClassicAssert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
        //ClassicAssert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));

        //Основной недостаток этого теста это его плохая обратная связь.
        //В предыдущем и моем тесте при провале теста в обозревателе тестов
        //видно где была ошибка или в каком свойстве было расхождение.
        //Здесь же при провале теста мы получаем то что ожидали True но получили False и не понятно где и что не правильно.
        //
        //upd: AreEqual просто перестаёт проверять новые свойства тогда как
        // BeEquivalentTo заставляет обновить тест

    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
