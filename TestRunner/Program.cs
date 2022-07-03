global using ExtendedStrategyPattern;
using TestRunner.CyclicTestStrategies;
using TestRunner.SimpleTestStrategies;

namespace TestRunner;

internal class Program {
	private static void Main() {
		TestSimpleSetup();
		TestcyclicSetup();
	}

	private static void TestSimpleSetup() {
		RunTest(new ExtendedStrategyHandler<ISimpleTestStrategy>());
	}

	private static void TestcyclicSetup() {
		try {
			RunTest(new ExtendedStrategyHandler<ICyclicTestStrategy>());
		} catch (InvalidOperationException ex) {
			if (!ex.Message.Contains("cyclic dependency")) {
				throw ex;
			}
		}
	}

	private static void RunTest<T>(ExtendedStrategyHandler<T> handler)
	where T: ITestStrategy {
		handler.AddImplementingStrategies();

		foreach (var strat in handler.IterateStrategies()) {
			strat.DoSomething();
		}
	}
}