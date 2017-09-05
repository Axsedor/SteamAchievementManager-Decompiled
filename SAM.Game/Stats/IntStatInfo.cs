// Decompiled with JetBrains decompiler
// Type: SAM.Game.Stats.IntStatInfo
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

namespace SAM.Game.Stats
{
  public class IntStatInfo : StatInfo
  {
    public int OriginalValue;
    public int IntValue;

    public override object Value
    {
      get
      {
        return (object) this.IntValue;
      }
      set
      {
        int num = int.Parse((string) value);
        if ((this.Permission & 2) != 0 && this.IntValue != num)
          throw new StatIsProtectedException();
        this.IntValue = num;
      }
    }

    public override bool Modified
    {
      get
      {
        return this.IntValue != this.OriginalValue;
      }
    }
  }
}
