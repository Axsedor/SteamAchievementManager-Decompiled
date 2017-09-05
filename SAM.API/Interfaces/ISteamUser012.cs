// Decompiled with JetBrains decompiler
// Type: SAM.API.Interfaces.ISteamUser012
// Assembly: SAM.API, Version=6.3.0.804, Culture=neutral, PublicKeyToken=null
// MVID: 7DF108F6-41E2-4750-9029-3DA2C808D0A1
// Assembly location: F:\Windows\Desktop\SAM\SAM.API.dll

using System;
using System.Runtime.InteropServices;

namespace SAM.API.Interfaces
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct ISteamUser012
  {
    public IntPtr GetHSteamUser;
    public IntPtr LoggedOn;
    public IntPtr GetSteamID;
    public IntPtr InitiateGameConnection;
    public IntPtr TerminateGameConnection;
    public IntPtr TrackAppUsageEvent;
    public IntPtr GetUserDataFolder;
    public IntPtr StartVoiceRecording;
    public IntPtr StopVoiceRecording;
    public IntPtr GetCompressedVoice;
    public IntPtr DecompressVoice;
    public IntPtr GetAuthSessionTicket;
    public IntPtr BeginAuthSession;
    public IntPtr EndAuthSession;
    public IntPtr CancelAuthTicket;
    public IntPtr UserHasLicenseForApp;
  }
}
