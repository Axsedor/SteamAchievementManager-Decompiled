// Decompiled with JetBrains decompiler
// Type: SAM.Picker.GameInfo
// Assembly: SAM.Picker, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: EFF56F92-DCC2-4727-8A90-101E83408D77
// Assembly location: F:\Windows\Desktop\SAM\SAM.Picker.exe

using System.Windows.Forms;

namespace SAM.Picker
{
  public class GameInfo
  {
    public long Id;
    public string Type;
    public ListViewItem Item;
    public string Logo;

    public string Name
    {
      get
      {
        return this.Item.Text;
      }
      set
      {
        this.Item.Text = value == null ? "App " + this.Id.ToString() : value;
      }
    }

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

    public GameInfo(long id, string type)
    {
      this.Id = id;
      this.Type = type;
      this.Item = new ListViewItem();
      this.Item.Tag = (object) this;
      this.Name = (string) null;
      this.ImageIndex = 0;
      this.Logo = (string) null;
    }
  }
}
