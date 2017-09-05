// Decompiled with JetBrains decompiler
// Type: SAM.Game.Stats.StatInfo
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

namespace SAM.Game.Stats
{
  public abstract class StatInfo
  {
    public abstract bool Modified { get; }

    public string Id { get; set; }

    public string DisplayName { get; set; }

    public abstract object Value { get; set; }

    public bool IncrementOnly { get; set; }

    public int Permission { get; set; }

    public string Extra
    {
      get
      {
        return ((StatFlags) (0 | (!this.IncrementOnly ? 0 : 1) | ((this.Permission & 2) == 0 ? 0 : 2) | ((this.Permission & -3) == 0 ? 0 : 4))).ToString();
      }
    }
  }
}
