module FoqTests

open NUnit.Framework
open Foq
open System
open System.Collections
open System.Reflection
open System.ComponentModel
open System.Collections.Generic


type IInterface =
    abstract MethodReturnsSomething : unit -> int
    abstract MethodReturnsNothing : unit -> unit
    abstract MethodReturnsOption: unit -> int option
    abstract Arity1Method : int -> bool
    abstract MethodReturnString : string -> bool
    abstract Arity1MethodReturnsNothing : int -> unit

    abstract StringProperty : string    
     abstract DoSomething : string -> bool    
[<Test>]
let ``an interface method that is not implemented should return the default value`` () =
    let stub = Mock<IInterface>().Create()
    Assert.AreEqual(Unchecked.defaultof<int>, stub.MethodReturnsSomething())

[<Test>]
let ``an interface method that is not implemented and returns nothing should not throw`` () =
    let stub = Mock<IInterface>().Create()
    Assert.DoesNotThrow( fun () -> stub.MethodReturnsNothing() )

[<Test>]
let ``test return strategy is used on mock create`` () =
    let mock = Mock<IInterface>(returnStrategy=fun t -> box "String").Create()
    Assert.AreEqual("String", mock.StringProperty)

[<TestFixture>]
type ``testcase with bool`` () =
    let n = "Simple String Foq"
    [<Test>]
    member __.``test field argument`` () =
        let mock =
            Mock<IInterface>()
                .Setup(fun x -> <@ x.MethodReturnString(n) @>).Returns(true)
                .Create()
        Assert.IsTrue(mock.MethodReturnString("Simple String Foq"))

[<Test>]
let ``an implemented interface method should return the specified value`` () =
    let value = 2
    let stub = 
        Mock<IInterface>()
            .Setup(fun x -> <@ x.MethodReturnsSomething() @>).Returns(value*3)
            .Create()
    let returnValue = stub.MethodReturnsSomething()
    Assert.AreEqual(returnValue,value*3)



[<Test; Combinatorial>]
let ``an implemented interface property getter should return the specified value`` () =
    let stub = 
        Mock<IInterface>()
            .Setup(fun x -> <@ x.StringProperty @>).Returns("Simple String Foq")
            .Create()
    let returnValue = stub.StringProperty
    Assert.AreEqual(returnValue,"Simple String Foq")

