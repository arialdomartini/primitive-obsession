Primitive
=========
A simple library to deal with [Primitive Obsession](http://wiki.c2.com/?PrimitiveObsession) in C#.

## Usage
To replace a primitive of type `T` with a custom class, just define the class and let it extend `Primitive<T>`.

For example, in a class `Foo` defined as:

```csharp
public class Foo
{
    public string ConnectionString { get; set; }
    public int MaxUsers { get; set; }
}
```

the 2 properties could be made non-primitive and replaced with:

```csharp
public class Foo
{
    public ConnectionString ConnectionString { get; set; }
    public MaxUsers MaxUsers { get; set; }
}
```

by defining the classes:

```csharp
public class ConnectionString : Primitive<string> { }

public class MaxUsers : Primitive<int> { }
```

### Assigning the instance a value
Use the default constructor and assign the property `Value` a value. For example:

```csharp
var connectionString = new ConnectionString{ Value = "User ID=root;Host=localhost;Port=5432" };
```
It is of course possible to extend the class and define an additional constructor:

```csharp
public class ConnectionString : Primitive<string>
{
    public ConnectionString(string value)
    {
        Value = value;
    }
}
```

which makes it possible to write:

```csharp
var connectionString = new ConnectionString{ Value = "User ID=root;Host=localhost;Port=5432" };
```
#### Assigning the instance a value with implicit conversion
The most convenient way to assign a value is to use an implicit conversion, which make it possible to use the non-primitive property as it was a primitive:

```csharp
public class Foo
{
    public ConnectionString ConnectionString { get; set; } = "some value";
    public MaxUsers MaxUsers { get; set; }
}

var foo = new Foo();
foo.MaxUsers = 100;
```

This unfortunately requires that the implicit operator to be defined in each non-primitive class, like the following:


```csharp
public class ConnectionString : Primitive<string>
{
    public static implicit operator ConnectionString(string value) =>
        new ConnectionString {Value = value};
}
```

This implicit operator cannot be defined in the base class, as it would not be inherited, and would result in the conversion to `Primitive<string>` instead of to `ConnectionString`, causing an exception at runtime (see the discussion on StackOverflow at the questions [implicit operator on generic type](https://stackoverflow.com/questions/3823145/implicit-operator-on-generic-types) and [Are implicity/explicit conversion methods inherited in C#?](https://stackoverflow.com/questions/967630/are-implicity-explicit-conversion-methods-inherited-in-c)).


### Conversion to primitive
A class extending `Primitive<T>` can be converted back to the primitive `T` both implicitly and explicitily:

```csharp
var sut = new MaxUsers {Value = 42};

int result1 = sut;
result1.Should().Be(42);
            
var result = sut + 100;
result.Should().Be(142);
```

Basically, this is what the library does. It just saves the developer to write the implicit operator to convert from non-primitive type to primitive type over and over. Yes, it's a 10-line library.


## Building from source code
### Build
Run:

```
dotnet build
```

### Run tests
Run:

```bash
dotnet test PrimitiveTest/PrimitiveTest.csproj
```

It should be possible to run tests with a simpler `dotnet test`, but I run in the issue ['dotnet test' in solution folder fails when non-test projects are in the solution #1129](http://wiki.c2.com/?PrimitiveObsession)

### NuGet package
Create the NuGet package with:

```bash
dotnet pack -c release -o .
```

Publish it with:

```bash
dotnet nuget push PrimitiveObsession/[nupkg_file] -k [apikey] -s https://api.nuget.org/v3/index.json
```

replacing `apikey` with the API Key managed by the [NuGet account page](https://www.nuget.org/account/apikeys).
