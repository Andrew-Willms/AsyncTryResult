using AsyncTryResult;
using Examples;



////////////////////////////////////////////////////////////////////////////////
//    Example with return values for success and failure.                     //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis Attributes.
if (!Foo.Foo1(out int? value1, out Error? error1)) {
	Console.WriteLine(error1.FancyPrint());
}
Console.WriteLine(value1 * 2);

// Null awareness possible using AsyncTryResult<int, Error>.
AsyncTryResult<int, Error> result1 = await Foo.Foo1Async();
if (result1.IsError) {
	Console.WriteLine(result1.Error.FancyPrint());
}
Console.WriteLine(result1.Value * 2);



////////////////////////////////////////////////////////////////////////////////
//    Example with return value for success only.                             //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis Attributes.
if (!Foo.Foo2(out int? value2)) {
	Console.WriteLine("An error occurred.");
}
Console.WriteLine(value2 * 2);

// Null awareness possible using AsyncTryResult<int>.
AsyncTryResult<int> result2 = await Foo.Foo2Async();
if (result2.IsError) {
	Console.WriteLine("An error occurred.");
}
Console.WriteLine(result2.Value * 2);



////////////////////////////////////////////////////////////////////////////////
//    Example with return value for failure only.                             //
////////////////////////////////////////////////////////////////////////////////

// Null awareness possible using System.Diagnostics.CodeAnalysis Attributes.
if (!Foo.Foo3(out Error? error3)) {
	Console.WriteLine(error3.FancyPrint());
}
Console.WriteLine("Operation completed successfully.");

// Null awareness possible using AsyncTryError<Error>.
AsyncTryError<Error> result3 = await Foo.Foo3Async();
if (result3.IsError) {
	Console.WriteLine(result3.Error.FancyPrint());
}
Console.WriteLine("Operation completed successfully.");