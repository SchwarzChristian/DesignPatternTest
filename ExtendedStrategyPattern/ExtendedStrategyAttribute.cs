namespace ExtendedStrategyPattern;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ExtendedStrategyAttribute : Attribute
{
	public Type[] Dependencies { get; set; } = new Type[0];
}
