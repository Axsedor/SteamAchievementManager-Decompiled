// Decompiled with JetBrains decompiler
// Type: SAM.Game.DoubleBufferedListView
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

using System.Windows.Forms;

namespace SAM.Game
{
  public class DoubleBufferedListView : ListView
  {
    public DoubleBufferedListView()
    {
      this.DoubleBuffered = true;
    }
  }
}
