// Decompiled with JetBrains decompiler
// Type: SAM.API.Interfaces.ISteamGameServer009
// Assembly: SAM.API, Version=6.3.0.804, Culture=neutral, PublicKeyToken=null
// MVID: 7DF108F6-41E2-4750-9029-3DA2C808D0A1
// Assembly location: F:\Windows\Desktop\SAM\SAM.API.dll

using System;
using System.Runtime.InteropServices;

namespace SAM.API.Interfaces
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct ISteamGameServer009
  {
    public IntPtr LogOn;
    public IntPtr LogOff;
    public IntPtr LoggedOn;
    public IntPtr Secure;
    public IntPtr GetSteamID;
    public IntPtr SendUserConnectAndAuthenticate;
    public IntPtr CreateUnauthenticatedUserConnection;
    public IntPtr SendUserDisconnect;
    public IntPtr UpdateUserData;
    public IntPtr SetServerType;
    public IntPtr UpdateServerStatus;
    public IntPtr UpdateSpectatorPort;
    public IntPtr SetGameType;
    public IntPtr GetUserAchievementStatus;
    public IntPtr GetGameplayStats;
    public IntPtr RequestUserGroupStatus;
    public IntPtr GetPublicIP;
    public IntPtr SetGameData;
    public IntPtr GetAuthSessionTicket;
    public IntPtr BeginAuthSession;
    public IntPtr EndAuthSession;
    public IntPtr CancelAuthTicket;
  }
}
