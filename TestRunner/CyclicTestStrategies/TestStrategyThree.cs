namespace TestRunner.CyclicTestStrategies;

[ExtendedStrategy(Dependencies = new[] { typeof(TestStrategyTwo) })]
public class TestStrategyThree : ITestStrategy {
	public void DoSomething() {
		Console.WriteLine(GetType().Name);
	}
}