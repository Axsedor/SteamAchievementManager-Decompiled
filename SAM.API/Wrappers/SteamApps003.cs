// Decompiled with JetBrains decompiler
// Type: SAM.API.Wrappers.SteamApps003
// Assembly: SAM.API, Version=6.3.0.804, Culture=neutral, PublicKeyToken=null
// MVID: 7DF108F6-41E2-4750-9029-3DA2C808D0A1
// Assembly location: F:\Windows\Desktop\SAM\SAM.API.dll

using SAM.API.Interfaces;
using System;
using System.Runtime.InteropServices;

namespace SAM.API.Wrappers
{
  public class SteamApps003 : NativeWrapper<ISteamApps003>
  {
    public bool IsSubscribedApp(long nGameID)
    {
      return this.Call<bool, SteamApps003.NativeIsSubscribedApp>(this.Functions.IsSubscribedApp, (object) this.ObjectAddress, (object) nGameID);
    }

    public string GetCurrentGameLanguage()
    {
      return Marshal.PtrToStringAnsi(this.Call<IntPtr, SteamApps003.NativeGetCurrentGameLanguage>(this.Functions.GetCurrentGameLanguage, new object[1]
      {
        (object) this.ObjectAddress
      }));
    }

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool NativeIsSubscribedApp(IntPtr thisObject, long nGameID);

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    private delegate IntPtr NativeGetCurrentGameLanguage(IntPtr thisObject);
  }
}
