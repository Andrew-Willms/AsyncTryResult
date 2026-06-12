namespace AsyncTryResult;



public record Box<T> where T : struct {
	
	public T Value { get; }

	public Box(T value) {
		Value = value;
	}

	public static implicit operator Box<T>(T value) {
		return new(value);
	}

	public static implicit operator T(Box<T> box) {
		return box.Value;
	}

}