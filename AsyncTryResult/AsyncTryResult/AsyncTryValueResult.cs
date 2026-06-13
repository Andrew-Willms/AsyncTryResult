using System.Diagnostics.CodeAnalysis;

namespace AsyncTryResult;



public record AsyncTryValueResult<TValue, TError>
	where TValue : struct
	where TError : class {

	public Box<TValue>? Value { get; }

	public TError? Error { get; }

	[MemberNotNullWhen(true, nameof(Value))]
	[MemberNotNullWhen(false, nameof(Error))]
	public bool IsSuccess { get; }

	[MemberNotNullWhen(false, nameof(Value))]
	[MemberNotNullWhen(true, nameof(Error))]
	public bool IsFailure { get; }

	public AsyncTryValueResult(TValue value) {

		Value = value;
		IsSuccess = true;
		IsFailure = false;
	}

	public AsyncTryValueResult(TError error) {

		Error = error;
		IsSuccess = false;
		IsFailure = true;
	}

	public static implicit operator AsyncTryValueResult<TValue, TError>(TValue value) {
		return new(value);
	}

	public static implicit operator AsyncTryValueResult<TValue, TError>(TError error) {
		return new(error);
	}

}