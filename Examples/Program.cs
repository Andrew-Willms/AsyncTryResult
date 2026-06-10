using System;
using AsyncTryResult;
using Examples;



////////////////////////////////////////////////////////////////////////////////
//    Example with return values for success and failure.                     //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis attributes.
if (!Foo.Foo1(out MyData? value1, out MyError? error1)) {
	Console.WriteLine(error1.FancyPrint());
	return;
}

// The compiler knows error1 may be null.
Console.WriteLine(error1.FancyPrint()); // CS8602: Dereference of a possible null reference.

// The compiler knows value1 will not be null.
Console.WriteLine(value1);



// Null awareness possible using AsyncTryResult<int, Error>.
AsyncTryResult<MyData, MyError> result1 = await Foo.Foo1Async();
if (result1.IsFailure) {
	Console.WriteLine(result1.Error.FancyPrint());
	return;
}

// The compiler knows result1.Error may be null.
Console.WriteLine(result1.Error.FancyPrint()); // CS8602: Dereference of a possible null reference.

// The compiler result1.Value will not be null.
Console.WriteLine(result1.Value);



////////////////////////////////////////////////////////////////////////////////
//    Example with return value for success only.                             //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis attributes.
if (!Foo.Foo2(out MyData? value2)) {
	Console.WriteLine("An error occurred.");
	return;
}
Console.WriteLine(value2.Number);

// Null awareness possible using AsyncTryResult<int>.
AsyncTryResult<MyData> result2 = await Foo.Foo2Async();
if (result2.IsFailure) {
	Console.WriteLine("An error occurred.");
	return;
}
Console.WriteLine(result2.Value);



////////////////////////////////////////////////////////////////////////////////
//    Example with return value for failure only.                             //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis attributes.
if (!Foo.Foo3(out MyError? error3)) {
	Console.WriteLine(error3.FancyPrint());
}
Console.WriteLine("Operation completed successfully.");

// Null awareness possible using AsyncTryError<Error>.
AsyncTryError<MyError> result3 = await Foo.Foo3Async();
if (result3.IsError) {
	Console.WriteLine(result3.Error.FancyPrint());
}
Console.WriteLine("Operation completed successfully.");



////////////////////////////////////////////////////////////////////////////////
//    Example with value types.                                               //
////////////////////////////////////////////////////////////////////////////////

// Synchronous method with nullable value types.
if (!Foo.ValueFoo(out int? value4, out MyError? error4)) {
	Console.WriteLine(error4.FancyPrint());
	return;
}
int notNullValue4 = value4; // CS0266 Cannot implicitly convert type 'int?' to 'int'.
Console.WriteLine(value4); // No error but this is using the Console.WriteLine(object? value) overload.



// UnrestrictedAsyncTryResult<TValue, TError> was created just for this example.
// Unlike AsyncTryResult<TValue, TError> It does not constraint TValue to reference types.
UnrestrictedAsyncTryResult<int, MyError> result4 = await Foo.ValueFooAsyncUnrestricted();

// No if statement checking result4.IsSuccess or result4.IsFailure.

// MyError is a reference types and the appropriate warning is raised for this unchecked access.
Console.WriteLine(result4.Error.FancyPrint()); // CS8602: Dereference of a possible null reference.

// result4.Value is declared as TValue? but the compiler thinks it's just int here.
int notNullResultValue4 = result4.Value; // No warning raised. If result4.Value is null this silently evaluates to 0.



// Synchronous method with boxed value types.
if (!Foo.ValueFooBoxed(out Box<int>? value5, out MyError? error5)) {
	Console.WriteLine(error5.FancyPrint());
	return;
}
int notNullValue5 = value5; // Proper type narrowing and no error. Implicit cast from Box<int> to int.
Console.WriteLine(value5); // This uses the Console.WriteLine(int value) overload.



// Null awareness possible using AsyncTryValueResult<int, Error>.
AsyncTryValueResult<int, MyError> result5 = await Foo.ValueFooAsync();
if (result5.IsFailure) {

	// The compiler knows that result5.Value may be null here.
	Console.WriteLine(result5.Value); // CS8604: Possible null reference argument...

	Console.WriteLine(result5.Error.FancyPrint());
	return;
}

// The compiler knows that result5.Value will not be null here.
Console.WriteLine(result5.Value); // Box<int> is implicitly cast to int.