# Generated code grows exponentially with the depth of shared transient dependencies

**Pure.DI 2.5.6**, .NET SDK 10.0.402, macOS. Repro project attached (`dotnet build`, then look at `obj/Debug/net10.0/generated/Pure.DI/Pure.DI.SourceGenerator/Composition.g.cs`).

Every transient dependency is expanded in place at every use site. When transients depend on other transients that are used more than once (a diamond-shaped graph, which is the normal shape of a service layer), the expansion repeats the whole sub-graph for each use, and the generated code size doubles with every level.

## Minimal code

17 classes: `L0` has no dependencies, every `Lk` depends on two `L(k-1)`, all with the default (transient) lifetime, one root.

```csharp
using Pure.DI;

var composition = new Composition();
Console.WriteLine(composition.Root.GetType().Name);

public sealed class L0;
public sealed class L1(L0 first, L0 second);
public sealed class L2(L1 first, L1 second);
// ... same pattern ...
public sealed class L16(L15 first, L15 second);

partial class Composition
{
    private static void Setup() => DI.Setup(nameof(Composition))
        .Root<L16>("Root");
}
```

## Expected

A composition whose size is in the order of the number of bindings and use sites in the source (here 17 classes, 33 constructor parameters).

## Actual

The root is emitted as one expression that contains every construction of the tree:

| levels | classes | `new L0()` in the generated file | generated file | build |
|---|---|---|---|---|
| 8 | 9 | 256 | 20 KB | 2 s |
| 12 | 13 | 4,096 | 151 KB | 2 s |
| 16 (attached) | 17 | 65,536 | 2.2 MB, on a single 2,228,344-character line | 5 s |
| 18 | 19 | 262,144 | 8.9 MB | 8 s |
| 20 | 21 | 1,048,576 | 35.7 MB | 20 s |

Each extra level doubles the output. On a real application (about 2,500 bindings, view models created through `Func<...>` factories, transient adapters shared by most components) the generated composition exceeded 30 million lines and the build did not complete.
