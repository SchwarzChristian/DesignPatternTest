namespace TestRunner.CyclicTestStrategies;

[ExtendedStrategy(Dependencies = new[] { typeof(TestStrategyOne) })]
public class TestStrategyTwo : ITestStrategy {
	public void DoSomething() {
		Console.WriteLine(GetType().Name);
	}
}