// Type: <>f__AnonymousType0`2
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

[DebuggerDisplay("\\{ ErrorHeading = {ErrorHeading}, ErrorText = {ErrorText} }", Type = "<Anonymous Type>")]
[CompilerGenerated]
internal sealed class \u003C\u003Ef__AnonymousType0<\u003CErrorHeading\u003Ej__TPar, \u003CErrorText\u003Ej__TPar>
{
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private readonly \u003CErrorHeading\u003Ej__TPar \u003CErrorHeading\u003Ei__Field;
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private readonly \u003CErrorText\u003Ej__TPar \u003CErrorText\u003Ei__Field;

  public \u003CErrorHeading\u003Ej__TPar ErrorHeading
  {
    get
    {
      return this.\u003CErrorHeading\u003Ei__Field;
    }
  }

  public \u003CErrorText\u003Ej__TPar ErrorText
  {
    get
    {
      return this.\u003CErrorText\u003Ei__Field;
    }
  }

  [DebuggerHidden]
  public \u003C\u003Ef__AnonymousType0(\u003CErrorHeading\u003Ej__TPar ErrorHeading, \u003CErrorText\u003Ej__TPar ErrorText)
  {
    // ISSUE: reference to a compiler-generated field
    this.\u003CErrorHeading\u003Ei__Field = ErrorHeading;
    // ISSUE: reference to a compiler-generated field
    this.\u003CErrorText\u003Ei__Field = ErrorText;
  }

  [DebuggerHidden]
  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("{ ErrorHeading = ");
    // ISSUE: reference to a compiler-generated field
    stringBuilder.Append((object) this.\u003CErrorHeading\u003Ei__Field);
    stringBuilder.Append(", ErrorText = ");
    // ISSUE: reference to a compiler-generated field
    stringBuilder.Append((object) this.\u003CErrorText\u003Ei__Field);
    stringBuilder.Append(" }");
    return ((object) stringBuilder).ToString();
  }

  [DebuggerHidden]
  public override bool Equals(object value)
  {
    var fAnonymousType0 = value as \u003C\u003Ef__AnonymousType0<\u003CErrorHeading\u003Ej__TPar, \u003CErrorText\u003Ej__TPar>;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return fAnonymousType0 != null && EqualityComparer<\u003CErrorHeading\u003Ej__TPar>.Default.Equals(this.\u003CErrorHeading\u003Ei__Field, fAnonymousType0.\u003CErrorHeading\u003Ei__Field) && EqualityComparer<\u003CErrorText\u003Ej__TPar>.Default.Equals(this.\u003CErrorText\u003Ei__Field, fAnonymousType0.\u003CErrorText\u003Ei__Field);
  }

  [DebuggerHidden]
  public override int GetHashCode()
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return -1521134295 * (-1521134295 * 701055026 + EqualityComparer<\u003CErrorHeading\u003Ej__TPar>.Default.GetHashCode(this.\u003CErrorHeading\u003Ei__Field)) + EqualityComparer<\u003CErrorText\u003Ej__TPar>.Default.GetHashCode(this.\u003CErrorText\u003Ei__Field);
  }
}
