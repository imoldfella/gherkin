//using CustomCodeAttributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;


public class AddExample
{
    public double first, second, result;
    public AddExample(double first, double second, double result)
    {
        this.first = first;
        this.second = second;
        this.result = result;
    }
    public static AddExample[] examples()
    {
        return new AddExample[]{
            new AddExample(2,40, 42)
        };
    }
}

[TestClass]
[Feature("Calculator", "Simple calculator")]
public class Test
{
    [TestMethod]
    [TestCategory("sometag")]
    [TestCategory("mytag")] // use mstest tag
    [Te(scenario: "Add two numbers",
        given: "I have entered <First> in the calculator",
        when: "I press add AND I have entered <Second> into the calculator",
        then: "the result should be <Result> on the screen",
        example: typeof(AddExample))]
    public void test1()
    {

        var ex = AddExample.examples();
        foreach (var o in ex)
        {
            // use examples
            Assert.AreEqual(o.first + o.second, o.result);
        }
    }
}



[TestClass]
[Feature("Calculator", "Simple calculator")]
public class Test2
{
    [TestMethod]
    [TestCategory("sometag")]
    [TestCategory("mytag")] // use mstest tag
    [Te(scenario: "Add two numbers",
        given: "I have entered <First> in the calculator",
        when: "I press add AND I have entered <Second> into the calculator",
        then: "the result should be <Result> on the screen",
        example: typeof(AddExample))]
    public void test1()
    {

        var ex = AddExample.examples();
        foreach (var o in ex)
        {
            // use examples
            Assert.AreEqual(o.first + o.second, o.result);
        }
    }
}


public class Foo
{
    public static void Main()
    {
        var sw = new StreamWriter(Console.OpenStandardOutput());
        sw.AutoFlush = true;
        GherkinBuilder.build(sw);

    }
}