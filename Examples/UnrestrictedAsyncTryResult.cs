using System.Diagnostics.CodeAnalysis;

namespace Examples;



public record UnrestrictedAsyncTryResult<TValue, TError> where TError : class {

	public TValue? Value { get; }

	public TError? Error { get; }

	[MemberNotNullWhen(true, nameof(Value))]
	[MemberNotNullWhen(false, nameof(Error))]
	public bool IsSuccess { get; }

	[MemberNotNullWhen(false, nameof(Value))]
	[MemberNotNullWhen(true, nameof(Error))]
	public bool IsFailure { get; }

	public UnrestrictedAsyncTryResult(TValue value) {

		Value = value;
		IsSuccess = true;
		IsFailure = false;
	}

	public UnrestrictedAsyncTryResult(TError error) {

		Error = error;
		IsSuccess = false;
		IsFailure = true;
	}

	public static implicit operator UnrestrictedAsyncTryResult<TValue, TError>(TValue success) {
		return new(success);
	}

	public static implicit operator UnrestrictedAsyncTryResult<TValue, TError>(TError error) {
		return new(error);
	}

}