namespace Examples;



public record Error {
	
	public required string Message { get; init; }

	public string FancyPrint() {
		return $"Very Serious Error (╯°□°）╯︵ ┻━┻: ~~~ {Message} ~~~";
	}

}