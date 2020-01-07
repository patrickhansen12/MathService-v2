// Getting started with thee Moq.FSharp.Extensions:
// Moq uses .Net LINQ Expressions for setting up properties and methods.
// F# 3 inside Visual Stud io 2012 & Xamarin Studio 4 supports .Net LINQ Expressions.
// This means you can use Moq directly from F#.
// The Moq.FSharp.Extensions add a few extension methods & functions to make things a little smoother.
// Use .SetupFunc & .SetupAction over .Setup to avoid specifying additional type information.
// Use .End at the end of a Setup sequence or "|> ignore".
// Leverage F#'s type inference and use any() over It.IsAny<_>() for readability.
// That's it!

module Moqtests
open NUnit.Framework

open Moq
open Moq.FSharp.Extensions

/// Example interface
type IFoo =
    abstract DoSomething : string -> bool
    abstract Value : int with get, set

/// Mock DoSomething method using vanilla Moq
let [<Test>] ``mock a method that returns a value`` () =
    let mock = Mock<IFoo>()
    mock.Setup<bool>(fun foo -> foo.DoSomething("Simple String Moq")).Returns(true) |> ignore
    Assert.That(mock.Object.DoSomething("Simple String Moq"))
    

/// Mock DoSomething method using Moq.FSharp.Extensions
let [<Test>] ``mock a method that returns a value without using a type annotation`` () =
    let mock = Mock<IFoo>()
    mock.SetupFunc(fun foo -> foo.DoSomething("Simple String Moq extension")).Returns(true).End
    Assert.That(mock.Object.DoSomething("Simple String Moq extension"))  
    
/// Mock Value property getter using Moq.FSharp.Extensions
let [<Test>] ``mock a property getter`` () =
    let mock = Mock<IFoo>()
    mock.SetupGet(fun foo -> foo.Value).Returns(1).End
    Assert.That(mock.Object.Value = 1)

/// Mock Value property setter using vanilla Moq
let [<Test>] ``mock a property setter`` () =
    let mock = Mock<IFoo>()
    let value = ref None
    mock.SetupSet<int>(fun foo -> foo.Value <- It.IsAny<_>())
        .Callback(fun x -> value := Some x) |> ignore
    mock.Object.Value <- 1
    Assert.That(!value |> Option.exists((=) 1))

/// Mock Value property setting using Moq.FSharp.Extensions
let [<Test>] ``mock a property setter without using a type annotation`` () =
    let mock = Mock<IFoo>()
    let calledWith = ref None
    mock.SetupSetAction(fun foo -> foo.Value <- any())
        .Callback(fun x -> calledWith := Some x).End
    mock.Object.Value <- 1
    Assert.That(calledWith.Value |> Option.exists((=) 1))

open System.ComponentModel


