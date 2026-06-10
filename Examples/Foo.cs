using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AsyncTryResult;

namespace Examples;



public static class Foo {

	private static readonly Random Random = new();

	////////////////////////////////////////////////////////////////////////////////
	//    Return values for success and failure.                                  //
	////////////////////////////////////////////////////////////////////////////////

	public static bool Foo1([NotNullWhen(true)] out MyData? value, [NotNullWhen(false)] out MyError? error) {

		if (Random.Next(1) == 1) {
			value = null;
			error = new() { Message = "An error occurred." };
			return false;
		}

		value = new() { Number = 1 };
		error = null;
		return true;
	}

	public static async Task<AsyncTryResult<MyData, MyError>> Foo1Async() {

		await Task.Delay(100);

		return Random.Next(1) == 1
			? new MyError { Message = "An error occurred." } // Implicit conversion to AsyncTryResult.
			: new MyData { Number = 1 }; // Implicit conversion to AsyncTryResult.
	}

	////////////////////////////////////////////////////////////////////////////////
	//    Return values for success only.                                         //
	////////////////////////////////////////////////////////////////////////////////

	public static bool Foo2([NotNullWhen(true)] out MyData? value) {

		if (Random.Next(1) == 1) {
			value = null;
			return false;
		}

		value = new() { Number = 1 };
		return true;
	}

	public static async Task<AsyncTryResult<MyData>> Foo2Async() {

		await Task.Delay(100);

		return Random.Next(1) == 1
			? AsyncTryResult<MyData>.Failure
			: new MyData { Number = 1 };
	}

	////////////////////////////////////////////////////////////////////////////////
	//    Return values for failure only.                                         //
	////////////////////////////////////////////////////////////////////////////////

	public static async Task<AsyncTryError<MyError>> Foo3Async() {

		await Task.Delay(100);

		return Random.Next(1) == 1
			? new MyError { Message = "An error occurred." }
			: AsyncTryError<MyError>.Success;
	}
	public static bool Foo3([NotNullWhen(false)] out MyError? error) {

		if (Random.Next(1) == 1) {
			error = new() { Message = "An error occurred." };
			return false;
		}

		error = null;
		return true;
	}



	////////////////////////////////////////////////////////////////////////////////
	//    Value type example.                                                     //
	////////////////////////////////////////////////////////////////////////////////

	public static bool ValueFoo([NotNullWhen(true)] out int? value, [NotNullWhen(false)] out MyError? error) {

		if (Random.Next(1) == 1) {
			value = null;
			error = new() { Message = "An error occurred." };
			return false;
		}

		value = 1;
		error = null;
		return true;
	}

	public static bool ValueFooBoxed([NotNullWhen(true)] out Box<int>? value, [NotNullWhen(false)] out MyError? error) {

		if (Random.Next(1) == 1) {
			value = null;
			error = new() { Message = "An error occurred." };
			return false;
		}

		value = 1;
		error = null;
		return true;
	}

	public static async Task<UnrestrictedAsyncTryResult<int, MyError>> ValueFooAsyncUnrestricted() {

		await Task.Delay(100);

		return Random.Next(1) == 1
			? new MyError { Message = "An error occurred." }
			: 1;
	}

	public static async Task<AsyncTryValueResult<int, MyError>> ValueFooAsync() {

		await Task.Delay(100);

		return Random.Next(1) == 1
			? new MyError { Message = "An error occurred." }
			: 1;
	}

}