// Decompiled with JetBrains decompiler
// Type: SAM.API.Callback`1
// Assembly: SAM.API, Version=6.3.0.804, Culture=neutral, PublicKeyToken=null
// MVID: 7DF108F6-41E2-4750-9029-3DA2C808D0A1
// Assembly location: F:\Windows\Desktop\SAM\SAM.API.dll

using System;
using System.Runtime.InteropServices;

namespace SAM.API
{
  public abstract class Callback<TParameter> : ICallback where TParameter : struct
  {
    public event Callback<TParameter>.CallbackFunction OnRun;

    public abstract int Id { get; }

    public abstract bool Server { get; }

    public void Run(IntPtr pvParam)
    {
      this.OnRun((TParameter) Marshal.PtrToStructure(pvParam, typeof (TParameter)));
    }

    public delegate void CallbackFunction(TParameter arg) where TParameter : struct;
  }
}
