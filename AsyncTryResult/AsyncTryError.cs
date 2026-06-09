using System.Diagnostics.CodeAnalysis;

namespace AsyncTryResult;



public record AsyncTryError<TError> {

	public TError? Error { get; }

	[MemberNotNullWhen(false, nameof(Error))]
	public bool IsSuccess { get; }

	[MemberNotNullWhen(true, nameof(Error))]
	public bool IsError { get; }

	public AsyncTryError(TError? error) {
		Error = error;
		IsSuccess = false;
		IsError = true;
	}

	private AsyncTryError() {
		IsSuccess = true;
		IsError = false;
	}

	public static readonly AsyncTryError<TError> Success = new();

	public static implicit operator AsyncTryError<TError>(TError error) {
		return new(error);
	}

}