namespace Examples;



public record MyData {
	
	public required int Number { get; init; }

	public string ExampleFunction() {
		return ToString();
	}

}