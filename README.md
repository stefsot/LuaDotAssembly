# LuaDotAssembly
## Lua 5.2 assembler and dissambler in a compact and single class file written in C#

LuaDotAssembly provides a simple and compact object-oriented API for serializing, manipulating and deserializing Lua 5.2 bytecode.
It targets the official bytecode as specified by https://www.lua.org/ and therefore doesn't work for LuaJIT bytecode.

The code can be modified to support both Lua 5.1 and Lua 5.3 as the differences are very minor but a generic assembler for Lua 5.X is outside the scope of this project.

# API usage examples
### _disassembling/loading bytecode_
```csharp
var bytecode = File.ReadAllBytes(...);
// deserialize bytecode
var asm = new LuaAssembly(bytecode);
...
```

### _print instructions and children functions of the main fuction (entry point)_
```csharp
...
foreach (var op in asm.EntryPoint.Opcodes)
    Console.WriteLine(op);
    
foreach (var proto in asm.EntryPoint.FunctionPrototypes)
{
    var lineStart = proto.SourceLines.First().Value;
    var lineEnd = proto.SourceLines.Last().Value;
    Console.WriteLine($"child function defined at {lineStart}, {lineEnd}");
}
```


### _print all the constants defined in the main function_
```csharp
foreach (var constant in asm.EntryPoint.Constants)
    Console.WriteLine($"constant of type {constant.CType} with value {constant.StringValue}");
```


### _patch the main function to exit imediatelly and serialize_
```csharp
// ToLuaOpcode is defined inside the Extensions.cs class
asm.EntryPoint.Opcodes[0] = "RETURN 0 1".ToLuaOpcode();
var buffer = asm.Serialize();
```


### _replace the value of a constant_
```csharp
asm.EntryPoint.Constants[0] = "String my new value".ToLuaConstant();
asm.EntryPoint.Constants[1] = "Number 9.12".ToLuaConstant();
// OR
asm.EntryPoint.Constants[0].StringValue = "my new value";

// make sure to use valid lua types (byte[], null, double)
// these will throw an exception
asm.EntryPoint.Constants[0].Value = "hi"; // in lua string constants as represented as byte arrays
                                         // either convert the string to byte array
                                         // asm.EntryPoint.Constants[0].Value = Encoding.UTF8.GetBytes("hi");
                                         // or modify the value using the StringValue property
                                         // asm.EntryPoint.Constants[0].StringValue = "hi";
// invalid type
asm.EntryPoint.Constants[0].Value = new object();
```

It is advised to replace constants using the _ToLuaConstant_ extension method to avoid any mistakes. If you need to manually change the _Value_ property of a constant make sure to use the correct .Net type.
