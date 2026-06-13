namespace Willmsy.AsyncTryResult;



/// <summary>
/// A simple reference type to box a value type.
/// </summary>
/// <typeparam name="T">The type of the <see langword="object"/> to be boxed.</typeparam>
public record Box<T> where T : struct {
	
	/// <summary>
	/// Returns the boxed value.
	/// </summary>
	public T Value { get; }

	/// <summary>
	/// Initializes a new <see cref="Box&lt;T&gt;">Box</see>.
	/// </summary>
	/// <param name="value">The value to be stored by the <see cref="Box&lt;T&gt;">Box</see>.</param>
	public Box(T value) {
		Value = value;
	}

	/// <summary>
	/// Implicit conversion from an <see langword="object"/> of type <typeparamref name="T"/> to a <see cref="Box&lt;T&gt;">Box</see>.
	/// </summary>
	/// <param name="value">The value to be stored by the <see cref="Box&lt;T&gt;">Box</see>.</param>
	public static implicit operator Box<T>(T value) {
		return new(value);
	}

	/// <summary>
	/// Implicit conversion from a <see cref="Box&lt;T&gt;">Box</see> to an <see langword="object"/> of type <typeparamref name="T"/>.
	/// </summary>
	/// <param name="box">The <see cref="Box&lt;T&gt;">Box</see> the value will be copied from.</param>
	public static implicit operator T(Box<T> box) {
		return box.Value;
	}

}