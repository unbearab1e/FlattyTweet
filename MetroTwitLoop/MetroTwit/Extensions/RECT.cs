﻿// Type: MetroTwit.Extensions.RECT
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System;
using System.Windows;

namespace MetroTwit.Extensions
{
  public struct RECT
  {
    public static readonly RECT Empty = new RECT();
    public int left;
    public int top;
    public int right;
    public int bottom;

    public int Width
    {
      get
      {
        return Math.Abs(this.right - this.left);
      }
    }

    public int Height
    {
      get
      {
        return this.bottom - this.top;
      }
    }

    public bool IsEmpty
    {
      get
      {
        return this.left >= this.right || this.top >= this.bottom;
      }
    }

    static RECT()
    {
    }

    public RECT(int left, int top, int right, int bottom)
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
    }

    public RECT(RECT rcSrc)
    {
      this.left = rcSrc.left;
      this.top = rcSrc.top;
      this.right = rcSrc.right;
      this.bottom = rcSrc.bottom;
    }

    public static bool operator ==(RECT rect1, RECT rect2)
    {
      return rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom;
    }

    public static bool operator !=(RECT rect1, RECT rect2)
    {
      return !(rect1 == rect2);
    }

    public override string ToString()
    {
      if (this == RECT.Empty)
        return "RECT {Empty}";
      return "RECT { left : " + (object) this.left + " / top : " + (string) (object) this.top + " / right : " + (string) (object) this.right + " / bottom : " + (string) (object) this.bottom + " }";
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Rect))
        return false;
      else
        return this == (RECT) obj;
    }

    public override int GetHashCode()
    {
      return this.left.GetHashCode() + this.top.GetHashCode() + this.right.GetHashCode() + this.bottom.GetHashCode();
    }
  }
}
