namespace TestRunner.SimpleTestStrategies;

[ExtendedStrategy(Dependencies = new[] {
	typeof(TestStrategyOne),
	typeof(TestStrategyTwo)
})]
public class TestStrategyThree : ISimpleTestStrategy {
	public void DoSomething() {
		Console.WriteLine(GetType().Name);
	}
}