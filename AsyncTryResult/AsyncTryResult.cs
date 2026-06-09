using System.Diagnostics.CodeAnalysis;

namespace AsyncTryResult;



public record AsyncTryResult<TValue, TError> {

	public TValue? Value { get; }

	public TError? Error { get; }

	[MemberNotNullWhen(true, nameof(Value))]
	[MemberNotNullWhen(false, nameof(Error))]
	public bool IsSuccess { get; }

	[MemberNotNullWhen(false, nameof(Value))]
	[MemberNotNullWhen(true, nameof(Error))]
	public bool IsError { get; }

	public AsyncTryResult(TValue value) {

		Value = value;
		IsSuccess = true;
		IsError = false;
	}

	public AsyncTryResult(TError error) {

		Error = error;
		IsSuccess = false;
		IsError = true;
	}

	public static implicit operator AsyncTryResult<TValue, TError>(TValue success) {
		return new(success);
	}

	public static implicit operator AsyncTryResult<TValue, TError>(TError error) {
		return new(error);
	}

}

public record AsyncTryResult<TValue> {

	public TValue? Value { get; }

	[MemberNotNullWhen(true, nameof(Value))]
	public bool IsSuccess { get; }

	[MemberNotNullWhen(false, nameof(Value))]
	public bool IsError { get; }

	public AsyncTryResult(TValue value) {
		Value = value;
		IsSuccess = true;
		IsError = false;
	}

	private AsyncTryResult() {
		IsSuccess = false;
		IsError = true;
	}

	public static readonly AsyncTryResult<TValue> Failure = new();

	public static implicit operator AsyncTryResult<TValue>(TValue success) {
		return new(success);
	}

}