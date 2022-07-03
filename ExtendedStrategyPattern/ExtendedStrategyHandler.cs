namespace ExtendedStrategyPattern;

public class ExtendedStrategyHandler<TStrategy>
{
	private Stack<Type> pending = new();
	private List<Type> strategyTypes = new();

	public  void AddImplementingStrategies() {
		var candidates = AppDomain.CurrentDomain
			.GetAssemblies()
			.SelectMany(ass => ass.GetTypes())
			.Where(t => !t.IsInterface)
			.Where(t => !t.IsAbstract)
			.Where(typeof(TStrategy).IsAssignableFrom)
			.Where(HasExtendedStrategyAttribute);

		foreach (var candidate in candidates)
		{
			AddStrategy(candidate);
		}
	}

	private bool HasExtendedStrategyAttribute(Type strat) {
		var attrType = typeof(ExtendedStrategyAttribute);
		var result = strat.GetCustomAttributes(inherit: false)
			.Any(it => attrType.IsAssignableFrom(it.GetType()));

		Console.WriteLine($"{strat.Name}: {result}");
		return result;
	}

	public void AddStrategy<T>() where T: TStrategy => AddStrategy(typeof(T));
	public void AddStrategy(Type type) {
		if (!typeof(TStrategy).IsAssignableFrom(type)) {
			throw new ArgumentException(
				$"type {type.Name} is no strategy of type {typeof(TStrategy).Name}"
			);
		}

		var defaultConstructor = type.GetConstructor(Type.EmptyTypes);
		if (defaultConstructor == null) throw new ArgumentException(
			$"type {type.Name} has no default constructor"
		);

		Console.WriteLine("initial push: " + type.Name);
		pending.Push(type);
		AddPendingStrategies();
		Console.WriteLine("final order: " + string.Join(" -> ", strategyTypes.Select(t => t.Name)));
	}

	public IEnumerable<TStrategy> IterateStrategies() {
		if (pending.Any()) AddPendingStrategies();
		Console.WriteLine($"iterating {strategyTypes.Count} strategies");
		return strategyTypes
			.Select(Activator.CreateInstance)
			.Cast<TStrategy>();
	}

	private void AddPendingStrategies() {
		while (pending.Any()) {
			var candidate = pending.Pop();
			if (strategyTypes.Contains(candidate)) continue;
			
			if (HasMissingDependencies(candidate)) {
				pending.Push(candidate);
				PushMissingDependencies(candidate);
				continue;
			}

			strategyTypes.Add(candidate);
		}
	}

	private bool HasMissingDependencies(Type strat) {
		var stratAttr = GetExtendedStrategyAttribute(strat);
		foreach (var dep in stratAttr.Dependencies)
		{
			if (!strategyTypes.Contains(dep)) return true;
		}

		return false;
	}

	private void PushMissingDependencies(Type strat) {
		var stratAttr = GetExtendedStrategyAttribute(strat);
		if (stratAttr.Dependencies.Contains(strat)) {
			throw new InvalidOperationException(
				$"type {strat.Name} can not have itself as dependency"
			);
		}

		var toPush = stratAttr.Dependencies
			.Where(dep => !strategyTypes.Contains(dep));

		foreach (var dep in toPush)
		{
			if (pending.Contains(dep)) throw new InvalidOperationException(
				$"cyclic dependencies detected on {strat.Name}, " +
				$"dependency: {dep.Name}"
			);

			Console.WriteLine("  push dependency: " + dep.Name);
			pending.Push(dep);
		}
	}

	private ExtendedStrategyAttribute GetExtendedStrategyAttribute(Type strat) {
		var attrType = typeof(ExtendedStrategyAttribute);
		var attr = strat.GetCustomAttributes(inherit: false).FirstOrDefault(
			it => attrType.IsAssignableFrom(it.GetType()
		)) as ExtendedStrategyAttribute;

		if (attr == null) throw new InvalidOperationException(
			$"no {nameof(ExtendedStrategyAttribute)} on type {strat.Name}"
		);

		return attr;
	}

}