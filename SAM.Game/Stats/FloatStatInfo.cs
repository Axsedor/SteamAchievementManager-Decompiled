// Decompiled with JetBrains decompiler
// Type: SAM.Game.Stats.FloatStatInfo
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

namespace SAM.Game.Stats
{
  public class FloatStatInfo : StatInfo
  {
    public float OriginalValue;
    public float FloatValue;

    public override object Value
    {
      get
      {
        return (object) this.FloatValue;
      }
      set
      {
        float num = float.Parse((string) value);
        if ((this.Permission & 2) != 0 && (double) this.FloatValue != (double) num)
          throw new StatIsProtectedException();
        this.FloatValue = num;
      }
    }

    public override bool Modified
    {
      get
      {
        return (double) this.FloatValue != (double) this.OriginalValue;
      }
    }
  }
}
