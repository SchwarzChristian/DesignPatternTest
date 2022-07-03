namespace TestRunner.SimpleTestStrategies;

[ExtendedStrategy(Dependencies = new[] { typeof(TestStrategyOne) })]
public class TestStrategyTwo : ISimpleTestStrategy {
	public void DoSomething() {
		Console.WriteLine(GetType().Name);
	}
}