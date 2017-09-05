// Decompiled with JetBrains decompiler
// Type: SAM.API.Interfaces.ISteamUtils005
// Assembly: SAM.API, Version=6.3.0.804, Culture=neutral, PublicKeyToken=null
// MVID: 7DF108F6-41E2-4750-9029-3DA2C808D0A1
// Assembly location: F:\Windows\Desktop\SAM\SAM.API.dll

using System;
using System.Runtime.InteropServices;

namespace SAM.API.Interfaces
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct ISteamUtils005
  {
    public IntPtr GetSecondsSinceAppActive;
    public IntPtr GetSecondsSinceComputerActive;
    public IntPtr GetConnectedUniverse;
    public IntPtr GetServerRealTime;
    public IntPtr GetIPCountry;
    public IntPtr GetImageSize;
    public IntPtr GetImageRGBA;
    public IntPtr GetCSERIPPort;
    public IntPtr GetCurrentBatteryPower;
    public IntPtr GetAppID;
    public IntPtr SetOverlayNotificationPosition;
    public IntPtr IsAPICallCompleted;
    public IntPtr GetAPICallFailureReason;
    public IntPtr GetAPICallResult;
    public IntPtr RunFrame;
    public IntPtr GetIPCCallCount;
    public IntPtr SetWarningMessageHook;
    public IntPtr IsOverlayEnabled;
    public IntPtr OverlayNeedsPresent;
  }
}
