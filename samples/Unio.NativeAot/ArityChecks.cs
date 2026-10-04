// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Unio;

namespace Unio.NativeAot;

// Reference branches allow native generic sharing at high arities.
// Program also exercises value-type branches, including formatting, mapping and async operations.
internal static class ArityChecks
{
    internal static void Run()
    {
        Branch0 value = new();
        Unio<Branch0, Branch1> union2 = value;
        Program.Check(union2.Match(static _ => 0, static _ => 1) == 0, "Arity 2 first branch");
        union2 = new Branch1();
        Program.Check(union2.Match(static _ => 0, static _ => 1) == 1, "Arity 2 last branch");
        Named2 named2 = value;
        Program.Check(named2.AsT0 == value && named2 == (Named2)value, "Arity 2 named equality");
        Unio<Branch0, Branch1, Branch2> union3 = value;
        Program.Check(union3.Match(static _ => 0, static _ => 1, static _ => 2) == 0, "Arity 3 first branch");
        union3 = new Branch2();
        Program.Check(union3.Match(static _ => 0, static _ => 1, static _ => 2) == 2, "Arity 3 last branch");
        Named3 named3 = value;
        Program.Check(named3.AsT0 == value && named3 == (Named3)value, "Arity 3 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3> union4 = value;
        Program.Check(union4.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3) == 0, "Arity 4 first branch");
        union4 = new Branch3();
        Program.Check(union4.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3) == 3, "Arity 4 last branch");
        Named4 named4 = value;
        Program.Check(named4.AsT0 == value && named4 == (Named4)value, "Arity 4 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4> union5 = value;
        Program.Check(union5.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4) == 0, "Arity 5 first branch");
        union5 = new Branch4();
        Program.Check(union5.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4) == 4, "Arity 5 last branch");
        Named5 named5 = value;
        Program.Check(named5.AsT0 == value && named5 == (Named5)value, "Arity 5 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5> union6 = value;
        Program.Check(union6.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5) == 0, "Arity 6 first branch");
        union6 = new Branch5();
        Program.Check(union6.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5) == 5, "Arity 6 last branch");
        Named6 named6 = value;
        Program.Check(named6.AsT0 == value && named6 == (Named6)value, "Arity 6 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6> union7 = value;
        Program.Check(union7.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6) == 0, "Arity 7 first branch");
        union7 = new Branch6();
        Program.Check(union7.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6) == 6, "Arity 7 last branch");
        Named7 named7 = value;
        Program.Check(named7.AsT0 == value && named7 == (Named7)value, "Arity 7 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7> union8 = value;
        Program.Check(union8.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7) == 0, "Arity 8 first branch");
        union8 = new Branch7();
        Program.Check(union8.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7) == 7, "Arity 8 last branch");
        Named8 named8 = value;
        Program.Check(named8.AsT0 == value && named8 == (Named8)value, "Arity 8 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8> union9 = value;
        Program.Check(union9.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8) == 0, "Arity 9 first branch");
        union9 = new Branch8();
        Program.Check(union9.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8) == 8, "Arity 9 last branch");
        Named9 named9 = value;
        Program.Check(named9.AsT0 == value && named9 == (Named9)value, "Arity 9 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9> union10 = value;
        Program.Check(union10.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9) == 0, "Arity 10 first branch");
        union10 = new Branch9();
        Program.Check(union10.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9) == 9, "Arity 10 last branch");
        Named10 named10 = value;
        Program.Check(named10.AsT0 == value && named10 == (Named10)value, "Arity 10 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10> union11 = value;
        Program.Check(union11.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10) == 0, "Arity 11 first branch");
        union11 = new Branch10();
        Program.Check(union11.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10) == 10, "Arity 11 last branch");
        Named11 named11 = value;
        Program.Check(named11.AsT0 == value && named11 == (Named11)value, "Arity 11 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11> union12 = value;
        Program.Check(union12.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11) == 0, "Arity 12 first branch");
        union12 = new Branch11();
        Program.Check(union12.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11) == 11, "Arity 12 last branch");
        Named12 named12 = value;
        Program.Check(named12.AsT0 == value && named12 == (Named12)value, "Arity 12 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12> union13 = value;
        Program.Check(union13.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12) == 0, "Arity 13 first branch");
        union13 = new Branch12();
        Program.Check(union13.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12) == 12, "Arity 13 last branch");
        Named13 named13 = value;
        Program.Check(named13.AsT0 == value && named13 == (Named13)value, "Arity 13 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13> union14 = value;
        Program.Check(union14.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13) == 0, "Arity 14 first branch");
        union14 = new Branch13();
        Program.Check(union14.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13) == 13, "Arity 14 last branch");
        Named14 named14 = value;
        Program.Check(named14.AsT0 == value && named14 == (Named14)value, "Arity 14 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14> union15 = value;
        Program.Check(union15.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14) == 0, "Arity 15 first branch");
        union15 = new Branch14();
        Program.Check(union15.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14) == 14, "Arity 15 last branch");
        Named15 named15 = value;
        Program.Check(named15.AsT0 == value && named15 == (Named15)value, "Arity 15 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14, Branch15> union16 = value;
        Program.Check(union16.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14, static _ => 15) == 0, "Arity 16 first branch");
        union16 = new Branch15();
        Program.Check(union16.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14, static _ => 15) == 15, "Arity 16 last branch");
        Named16 named16 = value;
        Program.Check(named16.AsT0 == value && named16 == (Named16)value, "Arity 16 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14, Branch15, Branch16> union17 = value;
        Program.Check(union17.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14, static _ => 15, static _ => 16) == 0, "Arity 17 first branch");
        union17 = new Branch16();
        Program.Check(union17.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14, static _ => 15, static _ => 16) == 16, "Arity 17 last branch");
        Named17 named17 = value;
        Program.Check(named17.AsT0 == value && named17 == (Named17)value, "Arity 17 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14, Branch15, Branch16, Branch17> union18 = value;
        Program.Check(union18.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14, static _ => 15, static _ => 16, static _ => 17) == 0, "Arity 18 first branch");
        union18 = new Branch17();
        Program.Check(union18.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14, static _ => 15, static _ => 16, static _ => 17) == 17, "Arity 18 last branch");
        Named18 named18 = value;
        Program.Check(named18.AsT0 == value && named18 == (Named18)value, "Arity 18 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14, Branch15, Branch16, Branch17, Branch18> union19 = value;
        Program.Check(union19.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14, static _ => 15, static _ => 16, static _ => 17, static _ => 18) == 0, "Arity 19 first branch");
        union19 = new Branch18();
        Program.Check(union19.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14, static _ => 15, static _ => 16, static _ => 17, static _ => 18) == 18, "Arity 19 last branch");
        Named19 named19 = value;
        Program.Check(named19.AsT0 == value && named19 == (Named19)value, "Arity 19 named equality");
        Unio<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14, Branch15, Branch16, Branch17, Branch18, Branch19> union20 = value;
        Program.Check(union20.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14, static _ => 15, static _ => 16, static _ => 17, static _ => 18, static _ => 19) == 0, "Arity 20 first branch");
        union20 = new Branch19();
        Program.Check(union20.Match(static _ => 0, static _ => 1, static _ => 2, static _ => 3, static _ => 4, static _ => 5, static _ => 6, static _ => 7, static _ => 8, static _ => 9, static _ => 10, static _ => 11, static _ => 12, static _ => 13, static _ => 14, static _ => 15, static _ => 16, static _ => 17, static _ => 18, static _ => 19) == 19, "Arity 20 last branch");
        Named20 named20 = value;
        Program.Check(named20.AsT0 == value && named20 == (Named20)value, "Arity 20 named equality");
    }
}

[GenerateUnio]
internal partial class Named2 : UnioBase<Branch0, Branch1>;

[GenerateUnio]
internal partial class Named3 : UnioBase<Branch0, Branch1, Branch2>;

[GenerateUnio]
internal partial class Named4 : UnioBase<Branch0, Branch1, Branch2, Branch3>;

[GenerateUnio]
internal partial class Named5 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4>;

[GenerateUnio]
internal partial class Named6 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5>;

[GenerateUnio]
internal partial class Named7 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6>;

[GenerateUnio]
internal partial class Named8 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7>;

[GenerateUnio]
internal partial class Named9 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8>;

[GenerateUnio]
internal partial class Named10 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9>;

[GenerateUnio]
internal partial class Named11 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10>;

[GenerateUnio]
internal partial class Named12 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11>;

[GenerateUnio]
internal partial class Named13 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12>;

[GenerateUnio]
internal partial class Named14 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13>;

[GenerateUnio]
internal partial class Named15 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14>;

[GenerateUnio]
internal partial class Named16 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14, Branch15>;

[GenerateUnio]
internal partial class Named17 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14, Branch15, Branch16>;

[GenerateUnio]
internal partial class Named18 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14, Branch15, Branch16, Branch17>;

[GenerateUnio]
internal partial class Named19 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14, Branch15, Branch16, Branch17, Branch18>;

[GenerateUnio]
internal partial class Named20 : UnioBase<Branch0, Branch1, Branch2, Branch3, Branch4, Branch5, Branch6, Branch7, Branch8, Branch9, Branch10, Branch11, Branch12, Branch13, Branch14, Branch15, Branch16, Branch17, Branch18, Branch19>;

internal sealed class Branch0;
internal sealed class Branch1;
internal sealed class Branch2;
internal sealed class Branch3;
internal sealed class Branch4;
internal sealed class Branch5;
internal sealed class Branch6;
internal sealed class Branch7;
internal sealed class Branch8;
internal sealed class Branch9;
internal sealed class Branch10;
internal sealed class Branch11;
internal sealed class Branch12;
internal sealed class Branch13;
internal sealed class Branch14;
internal sealed class Branch15;
internal sealed class Branch16;
internal sealed class Branch17;
internal sealed class Branch18;
internal sealed class Branch19;
