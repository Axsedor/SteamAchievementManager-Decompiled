// Decompiled with JetBrains decompiler
// Type: SAM.Game.Stats.AchievementInfo
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

using System.Windows.Forms;

namespace SAM.Game.Stats
{
  public class AchievementInfo
  {
    public string Id;
    public bool Achieved;
    public int Permission;
    public string Icon;
    public string IconGray;
    public string Name;
    public string Description;
    public ListViewItem Item;

    public int ImageIndex
    {
      get
      {
        return this.Item.ImageIndex;
      }
      set
      {
        this.Item.ImageIndex = value;
      }
    }
  }
}
