using System.Diagnostics.CodeAnalysis;

namespace Willmsy.AsyncTryResult;



/// <summary>
/// Intended to be used as the return type of <see langword="async"/> methods that may return a value or an error.
/// </summary>
/// <typeparam name="TValue">The type of the value to be returned. Must be a value type.</typeparam>
/// <typeparam name="TError">The type of the error to be returned. Must be a reference type.</typeparam>
public record AsyncTryValueResult<TValue, TError>
	where TValue : struct
	where TError : class {

	/// <summary>
	/// If this <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see> represents a success, returns a value, otherwise returns <see langword="null"/>.<br/>
	/// This property is not <see langword="null"/> if and only if <see cref="IsSuccess">IsSuccess</see> is <see langword="true"/>.<br/>
	/// This property is not <see langword="null"/> if and only if <see cref="IsSuccess">IsFailure</see> is <see langword="false"/>.
	/// </summary>
	public Box<TValue>? Value { get; }

	/// <summary>
	/// If this <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see> represents an error, returns the error, otherwise returns <see langword="null"/>.<br/>
	/// This property is not <see langword="null"/> if and only if <see cref="IsSuccess">IsSuccess</see> is <see langword="false"/>.<br/>
	/// This property is not <see langword="null"/> if and only if <see cref="IsSuccess">IsFailure</see> is <see langword="true"/>.
	/// </summary>
	public TError? Error { get; }

	/// <summary>
	/// Indicates if this <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see> represents a success.<br/>
	/// When <see langword="true"/>, <see cref="Value">Value</see> is not <see langword="null"/> and <see cref="Error">Error</see> is <see langword="null"/>.<br/>
	/// When <see langword="false"/>, <see cref="Value">Value</see> is <see langword="null"/> and <see cref="Error">Error</see> is not <see langword="null"/>.<br/>
	/// </summary>
	[MemberNotNullWhen(true, nameof(Value))]
	[MemberNotNullWhen(false, nameof(Error))]
	public bool IsSuccess { get; }

	/// <summary>
	/// Indicates if this <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see> represents an error.<br/>
	/// When <see langword="true"/>, <see cref="Value">Value</see> is <see langword="null"/> and <see cref="Error">Error</see> is not <see langword="null"/>.<br/>
	/// When <see langword="false"/>, <see cref="Value">Value</see> not is <see langword="null"/> and <see cref="Error">Error</see> is <see langword="null"/>.<br/>
	/// </summary>
	[MemberNotNullWhen(false, nameof(Value))]
	[MemberNotNullWhen(true, nameof(Error))]
	public bool IsFailure { get; }

	/// <summary>
	/// Initializes a new <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see> record that represents a success.
	/// </summary>
	/// <param name="value">The value to be stored by the <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see>.</param>
	public AsyncTryValueResult(TValue value) {
		Value = value;
		IsSuccess = true;
		IsFailure = false;
	}

	/// <summary>
	/// Initializes a new <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see> record that represents an error.
	/// </summary>
	/// <param name="error">The error to be stored by the <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see>.</param>
	public AsyncTryValueResult(TError error) {
		Error = error;
		IsSuccess = false;
		IsFailure = true;
	}

	/// <summary>
	/// Implicit conversion from an <see langword="object"/> of type <typeparamref name="TValue"/> to an <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see> representing a success.
	/// </summary>
	/// <param name="value">The value to be stored by the <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see>.</param>
	public static implicit operator AsyncTryValueResult<TValue, TError>(TValue value) {
		return new(value);
	}

	/// <summary>
	/// Implicit conversion from an <see langword="object"/> of type <typeparamref name="TError"/> to an <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see> representing an error.
	/// </summary>
	/// <param name="error">The error to be stored by the <see cref="AsyncTryValueResult&lt;TValue, TError&gt;">AsyncTryValueResult</see>.</param>
	public static implicit operator AsyncTryValueResult<TValue, TError>(TError error) {
		return new(error);
	}

}