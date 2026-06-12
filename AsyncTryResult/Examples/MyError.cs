namespace Examples;



public record MyError {
	
	public required string Message { get; init; }

	public string FancyPrint() {
		return $"Very Serious Error (╯°□°）╯︵ ┻━┻: ~~~ {Message} ~~~";
	}

	public int ExampleFunction() {
		return Message.Length;
	}

}