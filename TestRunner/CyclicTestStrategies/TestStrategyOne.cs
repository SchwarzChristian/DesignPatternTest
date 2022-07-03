namespace TestRunner.CyclicTestStrategies;

[ExtendedStrategy(Dependencies = new[] { typeof(TestStrategyThree) })]
public class TestStrategyOne : ITestStrategy {
	public void DoSomething() {
		Console.WriteLine(GetType().Name);
	}
}