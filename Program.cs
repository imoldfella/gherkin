using System;
using System.Reflection;
//using CustomCodeAttributes;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class Feature : Attribute
{
    public string feature, description;
    public Feature(string feature, string description)
    {
        this.feature = feature;
        this.description = description;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class Te : Attribute
{

    public string scenario, given, when, then;
    public Type? example;

    public Te(string scenario, string given, string when, string then, Type? example = null)
    {
        this.scenario = scenario;
        this.given = given;
        this.when = when;
        this.then = then;
        this.example = example;
    }
}
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


[Feature("Calculator", "Simple calculator")]
public class Test
{
    [TestMethod]
    [TestCategory("sometag")]  // use mstest tag
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

        }
    }
}

public class GherkinBuilder
{
    public static void build(Type t)
    {
        var ca = (Feature?)t.GetCustomAttribute(typeof(Feature), false);
        if (ca != null)
        {
            Console.Write($@"
Feature: {ca.feature}

{ca.description}
            ");
        }
        foreach (var m in t.GetMethods())
        {
            foreach (var a in m.GetCustomAttributes(typeof(Te), false))
            {

                var ax = (Te)a;

                Console.WriteLine(@$"
Scenario: {ax.scenario}
Given: {ax.given}
When: {ax.when}
Then: {ax.then}
                ");
                Type? tx = ax.example;
                if (tx != null)
                {
                    Console.WriteLine(@"Examples:
                    ");
                    // we need to execute the static method "example" to return a list of examples
                    var ex = tx.GetMethod("examples");
                    if (ex != null)
                    {
                        var o = ex.Invoke(null, null);
                        var lst = (dynamic[]?)o;
                        if (o != null && lst != null)
                        {
                            Type? fst = lst[0].GetType();

                            foreach (var me in fst.GetFields())
                            {
                                Console.Write($"|{me.Name}");
                            }
                            Console.WriteLine("|");

                            foreach (var e in lst)
                            {
                                foreach (var v in fst.GetFields())
                                {
                                    Console.Write($"|{v.GetValue(e)}");
                                }
                            }
                            Console.WriteLine("|");
                        }
                    }
                    else
                    {
                        Console.WriteLine("example not implemented");
                    }

                }
            }
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        GherkinBuilder.build(typeof(Test));
    }
}
