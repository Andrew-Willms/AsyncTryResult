using System.Diagnostics.CodeAnalysis;
using AsyncTryResult;

namespace Examples;



public static class Foo {

	private static readonly Random Random = new();

	////////////////////////////////////////////////////////////////////////////////
	//    Return values for success and failure.                                  //
	////////////////////////////////////////////////////////////////////////////////

	public static bool Foo1([NotNullWhen(true)] out int? value, [NotNullWhen(false)] out Error? error) {

		if (Random.Next(2) == 2) {
			value = null;
			error = new() { Message = "An error occurred." };
			return false;
		}

		value = 1;
		error = null;
		return true;
	}

	public static async Task<AsyncTryResult<int, Error>> Foo1Async() {

		await Task.Delay(100);

		return Random.Next(2) == 2
			? new Error { Message = "An error occurred." }
			: 1;
	}

	////////////////////////////////////////////////////////////////////////////////
	//    Return values for success only.                                         //
	////////////////////////////////////////////////////////////////////////////////

	public static bool Foo2([NotNullWhen(true)] out int? value) {

		if (Random.Next(2) == 2) {
			value = null;
			return false;
		}

		value = 1;
		return true;
	}

	public static async Task<AsyncTryResult<int>> Foo2Async() {

		await Task.Delay(100);

		return Random.Next(2) == 2
			? AsyncTryResult<int>.Failure
			: 1;
	}

	////////////////////////////////////////////////////////////////////////////////
	//    Return values for failure only.                                         //
	////////////////////////////////////////////////////////////////////////////////

	public static async Task<AsyncTryError<Error>> Foo3Async() {

		await Task.Delay(100);

		return Random.Next(2) == 2
			? new Error { Message = "An error occurred." }
			: AsyncTryError<Error>.Success;
	}
	public static bool Foo3([NotNullWhen(false)] out Error? error) {

		if (Random.Next(2) == 2) {
			error = new() { Message = "An error occurred." };
			return false;
		}

		error = null;
		return true;
	}

}