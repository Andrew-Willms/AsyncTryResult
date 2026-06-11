using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AsyncTryResult;
using Examples;
// ReSharper disable MoveLocalFunctionAfterJumpStatement



Random random = new();

////////////////////////////////////////////////////////////////////////////////
//    Synchronous example with return values for success and failure.         //
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
//    Asynchronous example with return values for success and failure.        //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using AsyncTryResult<MyData, MyError>.
async Task<AsyncTryResult<MyData, MyError>> Foo1Async() {

	await Task.Yield();

	return random.Next(1) == 1
		? new MyError { Message = "An error occurred." } // Implicit conversion to AsyncTryResult.
		: new MyData { Number = 1 }; // Implicit conversion to AsyncTryResult.
}

AsyncTryResult<MyData, MyError> result1 = await Foo1Async();
if (result1.IsFailure) {
	// The compiler knows result1.Error is not null within the if block.
	return result1.Error.ExampleFunction();
}

// The compiler knows result1.Error may be null outside the if block.
Console.WriteLine(result1.Error.FancyPrint()); // CS8602: Dereference of a possible null reference.

// The compiler knows result1.Value will not be null outside the if block.
Console.WriteLine(result1.Value.ExampleFunction());



////////////////////////////////////////////////////////////////////////////////
//    Concise error handling example                                          //
////////////////////////////////////////////////////////////////////////////////

AsyncTryResult<MyData, MyError> result = await Foo1Async();
if (result.IsFailure) {
	return result.Error.ExampleFunction();
}



////////////////////////////////////////////////////////////////////////////////
//    Synchronous example with return value for success only.                 //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis attributes.
bool Foo2([NotNullWhen(true)] out MyData? value) {

	if (random.Next(1) == 1) {
		value = null;
		return false;
	}

	value = new() { Number = 1 };
	return true;
}

if (!Foo2(out MyData? value2)) {
	// The compiler knows value2 may be null within the if block.
	Console.WriteLine(value2.ExampleFunction());
	return 1;
}

// The compiler knows value2 will not be null outside the if block.
Console.WriteLine(value2.ExampleFunction());



////////////////////////////////////////////////////////////////////////////////
//    Asynchronous example with return value for success only.                //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using AsyncTryResult<MyData>.
async Task<AsyncTryResult<MyData>> Foo2Async() {

	await Task.Yield();

	return random.Next(1) == 1
		? AsyncTryResult<MyData>.Failure
		: new MyData { Number = 1 }; // Implicit conversion to AsyncTryResult.
}

AsyncTryResult<MyData> result2 = await Foo2Async();
if (result2.IsFailure) {
	// The compiler knows result2.Value may be null within the if block.
	Console.WriteLine(result2.Value.ExampleFunction());
	return 1;
}

// The compiler knows result2.Value will not be null outside the if block.
Console.WriteLine(result2.Value.ExampleFunction());



////////////////////////////////////////////////////////////////////////////////
//    Synchronous example with return value for failure only.                 //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis attributes.
bool Foo3([NotNullWhen(false)] out MyError? error) {

	if (random.Next(1) == 1) {
		error = new() { Message = "An error occurred." };
		return false;
	}

	error = null;
	return true;
}

if (!Foo3(out MyError? error3)) {
	// The compiler knows error3 is not null within the if block.
	return error3.ExampleFunction();
}

// The compiler knows error3 may be null outside the if block.
Console.WriteLine(error3.FancyPrint()); // CS8602: Dereference of a possible null reference.



////////////////////////////////////////////////////////////////////////////////
//    Asynchronous example with return value for failure only.                //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using AsyncTryError<MyError>.
async Task<AsyncTryError<MyError>> Foo3Async() {

	await Task.Yield();

	return random.Next(1) == 1
		? new MyError { Message = "An error occurred." } // Implicit conversion to AsyncTryResult.
		: AsyncTryError<MyError>.Success;
}

AsyncTryError<MyError> result3 = await Foo3Async();
if (result3.IsFailure) {
	// The compiler knows result3.Error is not null within the if block.
	return result3.Error.ExampleFunction();
}

// The compiler knows result3.Error may be null outside the if block.
Console.WriteLine(result3.Error.FancyPrint()); // CS8602: Dereference of a possible null reference.



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
int notNullValue4 = value4; // CS0266 Cannot implicitly convert type 'int?' to 'int'.
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

UnrestrictedAsyncTryResult<int, MyError> result4 = await ValueFooAsyncUnrestricted();
if (result4.IsFailure) {

	// result4.Value has type int? but the compiler represents it as just int.
	// No warning raised. If result4.Value is null this silently fails and evaluates to 0.
	int notNullResult4Value = result4.Value;

	return result4.Error.ExampleFunction();
}

// MyError is a reference types and the appropriate warning is raised for an unchecked access.
Console.WriteLine(result4.Error.FancyPrint()); // CS8602: Dereference of a possible null reference.

Console.WriteLine(result4.Value);


////////////////////////////////////////////////////////////////////////////////
//    Synchronous example with boxed value types.                             //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis attributes.
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

if (!ValueFooBoxed(out Box<int>? value5, out MyError? error5)) {

	// The compiler knows that value5 may be null here.
	Console.WriteLine(value5); // CS8604: Possible null reference argument...

	return error5.ExampleFunction();
}

// The compiler knows value5 is not null outside the if block.
int notNullValue5 = value5; // Implicit cast from Box<int> to int.
Console.WriteLine(value5); // This uses the Console.WriteLine(int value) overload.



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

AsyncTryValueResult<int, MyError> result5 = await ValueFooAsync();
if (result5.IsFailure) {

	// The compiler knows that result5.Value may be null here.
	Console.WriteLine(result5.Value); // CS8604: Possible null reference argument...

	return result5.Error.ExampleFunction();
}

// The compiler knows result5.Error may be null outside the if block.
Console.WriteLine(result5.Error.FancyPrint()); // CS8602: Dereference of a possible null reference.

// The compiler knows that result5.Value will not be null here.
Console.WriteLine(result5.Value); // Box<int> is implicitly cast to int.



////////////////////////////////////////////////////////////////////////////////
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

return 0;