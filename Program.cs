using Pure.DI;

var composition = new Composition();
Console.WriteLine(composition.Root.GetType().Name);

public sealed class L0;
public sealed class L1(L0 first, L0 second);
public sealed class L2(L1 first, L1 second);
public sealed class L3(L2 first, L2 second);
public sealed class L4(L3 first, L3 second);
public sealed class L5(L4 first, L4 second);
public sealed class L6(L5 first, L5 second);
public sealed class L7(L6 first, L6 second);
public sealed class L8(L7 first, L7 second);
public sealed class L9(L8 first, L8 second);
public sealed class L10(L9 first, L9 second);
public sealed class L11(L10 first, L10 second);
public sealed class L12(L11 first, L11 second);
public sealed class L13(L12 first, L12 second);
public sealed class L14(L13 first, L13 second);
public sealed class L15(L14 first, L14 second);
public sealed class L16(L15 first, L15 second);

partial class Composition
{
    private static void Setup() => DI.Setup(nameof(Composition))
        .Root<L16>("Root");
}
