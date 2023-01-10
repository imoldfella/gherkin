

using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
public class GherkinBuilder
{
    public static void build(StreamWriter fs)
    {
        Assembly currentAssem = Assembly.GetExecutingAssembly();
        foreach (var o in currentAssem.GetTypes())
        {
            build(fs, o);
        }
    }
    public static void build(StreamWriter fs, Type t)
    {
        var ca = (Feature?)t.GetCustomAttribute(typeof(Feature), false);
        if (ca == null) return;
        if (ca != null)
        {
            fs.WriteLine($@"
Feature: {ca.feature}

{ca.description}
");
        }
        foreach (var m in t.GetMethods())
        {
            foreach (var tc in m.GetCustomAttributes(typeof(TestCategoryAttribute), false))
            {
                var tcx = (TestCategoryAttribute)tc;
                foreach (var o in tcx.TestCategories)
                {
                    fs.Write($"@{o} ");
                }
            }
            foreach (var a in m.GetCustomAttributes(typeof(Te), false))
            {

                var ax = (Te)a;

                fs.WriteLine(@$"
Scenario: {ax.scenario}
Given: {ax.given}
When: {ax.when}
Then: {ax.then}
                ");
                Type? tx = ax.example;
                if (tx != null)
                {
                    fs.WriteLine(@"Examples:
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
                                fs.Write($"|{me.Name}");
                            }
                            fs.WriteLine("|");

                            foreach (var e in lst)
                            {
                                foreach (var v in fst.GetFields())
                                {
                                    fs.Write($"|{v.GetValue(e)}");
                                }
                                fs.WriteLine("|");
                            }
                         
                        }
                    }
                    else
                    {
                        fs.WriteLine("example not implemented");
                    }

                }
            }
        }
    }
}

