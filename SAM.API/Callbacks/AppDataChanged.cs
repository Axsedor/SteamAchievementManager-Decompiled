// Decompiled with JetBrains decompiler
// Type: SAM.API.Callbacks.AppDataChanged
// Assembly: SAM.API, Version=6.3.0.804, Culture=neutral, PublicKeyToken=null
// MVID: 7DF108F6-41E2-4750-9029-3DA2C808D0A1
// Assembly location: F:\Windows\Desktop\SAM\SAM.API.dll

namespace SAM.API.Callbacks
{
  public class AppDataChanged : Callback<SAM.API.Types.AppDataChanged>
  {
    public override int Id
    {
      get
      {
        return 1001;
      }
    }

    public override bool Server
    {
      get
      {
        return false;
      }
    }
  }
}
