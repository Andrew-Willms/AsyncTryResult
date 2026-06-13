using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AsyncTryResult;
using Examples;
// ReSharper disable MoveLocalFunctionAfterJumpStatement



Random random = new();

////////////////////////////////////////////////////////////////////////////////
//    Synchronous example.                                                    //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis attributes.
bool Foo1([NotNullWhen(true)] out MyData? value, [NotNullWhen(false)] out MyError? error) {

	if (random.Next(1) == 1) {
		value = null;
		error = new() { Message = "An error occurred." };
		return false;
	}

	value = new() { Number = 1 };
	error = null;
	return true;
}

if (!Foo1(out MyData? value1, out MyError? error1)) {
	// The compiler knows error1 is not null within the if block.
	return error1.ExampleFunction();
}

// The compiler knows error1 may be null outside the if block.
Console.WriteLine(error1.FancyPrint()); // CS8602: Dereference of a possible null reference.

// The compiler knows value1 will not be null outside the if block.
Console.WriteLine(value1.ExampleFunction());



////////////////////////////////////////////////////////////////////////////////
//    Asynchronous example.                                                   //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using AsyncTryResult<MyData, MyError>.
async Task<AsyncTryResult<MyData, MyError>> Foo2() {

	await Task.Yield();

	return random.Next(1) == 1
		? new MyError { Message = "An error occurred." } // Implicit conversion to AsyncTryResult.
		: new MyData { Number = 1 }; // Implicit conversion to AsyncTryResult.
}

AsyncTryResult<MyData, MyError> result2 = await Foo2();
if (result2.IsFailure) {
	// The compiler knows result2.Error is not null within the if block.
	return result2.Error.ExampleFunction();
}

// The compiler knows result2.Error may be null outside the if block.
Console.WriteLine(result2.Error.FancyPrint()); // CS8602: Dereference of a possible null reference.

// The compiler knows result2.Value will not be null outside the if block.
Console.WriteLine(result2.Value.ExampleFunction());



////////////////////////////////////////////////////////////////////////////////
//    Concise error handling example.                                         //
////////////////////////////////////////////////////////////////////////////////

async Task<AsyncTryResult<MyData, MyError>> Foo3() {

	await Task.Yield();

	return random.Next(1) == 1
		? new MyError { Message = "An error occurred." }
		: new MyData { Number = 1 };
}

AsyncTryResult<MyData, MyError> result3 = await Foo3();
if (result3.IsFailure) {
	return result3.Error.ExampleFunction();
}



////////////////////////////////////////////////////////////////////////////////
//    Synchronous example with nullable value types.                          //
////////////////////////////////////////////////////////////////////////////////

// Attempt to provide null awareness possible using System.Diagnostics.CodeAnalysis attributes.
bool ValueFoo([NotNullWhen(true)] out int? value, [NotNullWhen(false)] out MyError? error) {

	if (random.Next(1) == 1) {
		value = null;
		error = new() { Message = "An error occurred." };
		return false;
	}

	value = 1;
	error = null;
	return true;
}

if (!ValueFoo(out int? value4, out MyError? error4)) {
	return error4.ExampleFunction();
}

// The compiler should know that value4 is not null outside the if block.
//int notNullValue4 = value4; // CS0266 Cannot implicitly convert type 'int?' to 'int'.
Console.WriteLine(value4); // No error but this is using the Console.WriteLine(object? value) overload.



////////////////////////////////////////////////////////////////////////////////
//    Asynchronous example with nullable value types.                         //
////////////////////////////////////////////////////////////////////////////////

// UnrestrictedAsyncTryResult<TValue, TError> was created just for this example.
// Unlike AsyncTryResult<TValue, TError> it does not constrain TValue to reference types.
async Task<UnrestrictedAsyncTryResult<int, MyError>> ValueFooAsyncUnrestricted() {

	await Task.Yield();

	return random.Next(1) == 1
		? new MyError { Message = "An error occurred." } // Implicit conversion to AsyncTryResult.
		: 1;
}

UnrestrictedAsyncTryResult<int, MyError> result5 = await ValueFooAsyncUnrestricted();
if (result5.IsFailure) {

	// result5.Value has type int? but the compiler represents it as just int.
	// No warning raised. If result5.Value is null this silently fails and evaluates to 0.
	int notNullResult5Value = result5.Value;

	return result5.Error.ExampleFunction();
}

// MyError is a reference types and the appropriate warning is raised for an unchecked access.
Console.WriteLine(result5.Error.FancyPrint()); // CS8602: Dereference of a possible null reference.

Console.WriteLine(result5.Value);



////////////////////////////////////////////////////////////////////////////////
//    Asynchronous example with boxed value types.                            //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using AsyncTryValueResult<int, Error>.
async Task<AsyncTryValueResult<int, MyError>> ValueFooAsync() {

	await Task.Yield();

	return random.Next(1) == 1
		? new MyError { Message = "An error occurred." } // Implicit conversion to AsyncTryResult.
		: 1;
}

AsyncTryValueResult<int, MyError> result6 = await ValueFooAsync();
if (result6.IsFailure) {

	// The compiler knows that result6.Value may be null here.
	Console.WriteLine(result6.Value); // CS8604: Possible null reference argument...

	return result6.Error.ExampleFunction();
}

// The compiler knows result6.Error may be null outside the if block.
Console.WriteLine(result6.Error.FancyPrint()); // CS8602: Dereference of a possible null reference.

// The compiler knows that result6.Value will not be null here.
Console.WriteLine(result6.Value); // Box<int> is implicitly cast to int.



////////////////////////////////////////////////////////////////////////////////
//    Synchronous example with boxed value types.                             //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis attributes and the Box<T> type.
bool ValueFooBoxed([NotNullWhen(true)] out Box<int>? value, [NotNullWhen(false)] out MyError? error) {

	if (random.Next(1) == 1) {
		value = null;
		error = new() { Message = "An error occurred." };
		return false;
	}

	value = 1;
	error = null;
	return true;
}

if (!ValueFooBoxed(out Box<int>? value7, out MyError? error5)) {

	// The compiler knows that value7 may be null here.
	Console.WriteLine(value7); // CS8604: Possible null reference argument...

	return error5.ExampleFunction();
}

// The compiler knows value7 is not null outside the if block.
int notNullValue7 = value7; // Implicit cast from Box<int> to int.
Console.WriteLine(value7); // This uses the Console.WriteLine(int value) overload.



////////////////////////////////////////////////////////////////////////////////
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

return 0;