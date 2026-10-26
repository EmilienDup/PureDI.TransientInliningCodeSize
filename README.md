# Pure.DI: generated code size doubles with every level of shared transient dependencies

Minimal repro for Pure.DI 2.5.6: a 17-class graph of transients where each class depends on two instances of the previous one generates a 2.2 MB composition (65,536 inlined constructions on one line).

[`ISSUE.md`](ISSUE.md) is the text to paste into a Pure.DI GitHub issue; it includes the measurements for 9 to 21 classes.

```
dotnet build
ls -l obj/Debug/net10.0/generated/Pure.DI/Pure.DI.SourceGenerator/Composition.g.cs
```

Add or remove `Lk` levels (and update the root) to see the size double or halve.
