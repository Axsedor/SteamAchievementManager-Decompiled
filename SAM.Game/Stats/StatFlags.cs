// Decompiled with JetBrains decompiler
// Type: SAM.Game.Stats.StatFlags
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

using System;

namespace SAM.Game.Stats
{
  [Flags]
  public enum StatFlags
  {
    None = 0,
    IncrementOnly = 1,
    Protected = 2,
    UnknownPermissionFlags = 4,
  }
}
