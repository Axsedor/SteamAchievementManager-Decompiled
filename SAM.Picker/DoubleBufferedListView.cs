// Decompiled with JetBrains decompiler
// Type: SAM.Picker.DoubleBufferedListView
// Assembly: SAM.Picker, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: EFF56F92-DCC2-4727-8A90-101E83408D77
// Assembly location: F:\Windows\Desktop\SAM\SAM.Picker.exe

using System.Windows.Forms;

namespace SAM.Picker
{
  internal class DoubleBufferedListView : ListView
  {
    public DoubleBufferedListView()
    {
      this.DoubleBuffered = true;
    }
  }
}
