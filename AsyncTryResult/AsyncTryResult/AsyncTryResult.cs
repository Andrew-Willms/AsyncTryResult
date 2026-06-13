using System.Diagnostics.CodeAnalysis;

namespace AsyncTryResult;



/// <summary>
/// Intended to be used as the return type of asynchronous methods that may return a value or an error.
/// </summary>
/// <typeparam name="TValue">The type of the value to be returned. Must be a reference type.</typeparam>
/// <typeparam name="TError">The type of the error to be returned. Must be a reference type.</typeparam>
public record AsyncTryResult<TValue, TError>
	where TValue : class 
	where TError : class {

	/// <summary>
	/// If this <see cref="AsyncTryResult&lt;TValue, TError&gt;">AsyncTryResult</see> represents a success, returns a value, otherwise <see langword="null"/>.<br/>
	/// This property is not <see langword="null"/> if and only if <see cref="IsSuccess">IsSuccess</see> is <see langword="true"/>.<br/>
	/// This property is not <see langword="null"/> if and only if <see cref="IsSuccess">IsFailure</see> is <see langword="false"/>.
	/// </summary>
	public TValue? Value { get; }

	/// <summary>
	/// If this <see cref="AsyncTryResult&lt;TValue, TError&gt;">AsyncTryResult</see> represents an error, returns the error, otherwise <see langword="null"/>.<br/>
	/// This property is not <see langword="null"/> if and only if <see cref="IsSuccess">IsSuccess</see> is <see langword="false"/>.<br/>
	/// This property is not <see langword="null"/> if and only if <see cref="IsSuccess">IsFailure</see> is <see langword="true"/>.
	/// </summary>
	public TError? Error { get; }

	/// <summary>
	/// Indicates if the <see cref="AsyncTryResult&lt;TValue, TError&gt;">AsyncTryResult</see> represents a success.<br/>
	/// When <see langword="true"/>, <see cref="Value">Value</see> is not <see langword="null"/> and <see cref="Error">Error</see> is <see langword="null"/>.<br/>
	/// When <see langword="false"/>, <see cref="Value">Value</see> is <see langword="null"/> and <see cref="Error">Error</see> is not <see langword="null"/>.<br/>
	/// </summary>
	[MemberNotNullWhen(true, nameof(Value))]
	[MemberNotNullWhen(false, nameof(Error))]
	public bool IsSuccess { get; }

	/// <summary>
	/// Indicates if the <see cref="AsyncTryResult&lt;TValue, TError&gt;">AsyncTryResult</see> represents an error.<br/>
	/// When <see langword="true"/>, <see cref="Value">Value</see> is <see langword="null"/> and <see cref="Error">Error</see> is not <see langword="null"/>.<br/>
	/// When <see langword="false"/>, <see cref="Value">Value</see> not is <see langword="null"/> and <see cref="Error">Error</see> is <see langword="null"/>.<br/>
	/// </summary>
	[MemberNotNullWhen(false, nameof(Value))]
	[MemberNotNullWhen(true, nameof(Error))]
	public bool IsFailure { get; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="value"></param>
	public AsyncTryResult(TValue value) {

		Value = value;
		IsSuccess = true;
		IsFailure = false;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="error"></param>
	public AsyncTryResult(TError error) {

		Error = error;
		IsSuccess = false;
		IsFailure = true;
	}

	/// <summary>
	/// Implicit conversion from
	/// </summary>
	/// <param name="value"></param>
	public static implicit operator AsyncTryResult<TValue, TError>(TValue value) {
		return new(value);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="error"></param>
	public static implicit operator AsyncTryResult<TValue, TError>(TError error) {
		return new(error);
	}


}