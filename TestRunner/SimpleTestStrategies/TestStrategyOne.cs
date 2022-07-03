namespace TestRunner.SimpleTestStrategies;

[ExtendedStrategy]
public class TestStrategyOne : ISimpleTestStrategy {
	public void DoSomething() {
		Console.WriteLine(GetType().Name);
	}
}