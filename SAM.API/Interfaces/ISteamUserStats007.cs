// Decompiled with JetBrains decompiler
// Type: SAM.API.Interfaces.ISteamUserStats007
// Assembly: SAM.API, Version=6.3.0.804, Culture=neutral, PublicKeyToken=null
// MVID: 7DF108F6-41E2-4750-9029-3DA2C808D0A1
// Assembly location: F:\Windows\Desktop\SAM\SAM.API.dll

using System;
using System.Runtime.InteropServices;

namespace SAM.API.Interfaces
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public class ISteamUserStats007
  {
    public IntPtr RequestCurrentStats;
    public IntPtr GetStatFloat;
    public IntPtr GetStatInteger;
    public IntPtr SetStatFloat;
    public IntPtr SetStatInteger;
    public IntPtr UpdateAvgRateStat;
    public IntPtr GetAchievement;
    public IntPtr SetAchievement;
    public IntPtr ClearAchievement;
    public IntPtr GetAchievementAndUnlockTime;
    public IntPtr StoreStats;
    public IntPtr GetAchievementIcon;
    public IntPtr GetAchievementDisplayAttribute;
    public IntPtr IndicateAchievementProgress;
    public IntPtr RequestUserStats;
    public IntPtr GetUserStatFloat;
    public IntPtr GetUserStatInt;
    public IntPtr GetUserAchievement;
    public IntPtr GetUserAchievementAndUnlockTime;
    public IntPtr ResetAllStats;
    public IntPtr FindOrCreateLeaderboard;
    public IntPtr FindLeaderboard;
    public IntPtr GetLeaderboardName;
    public IntPtr GetLeaderboardEntryCount;
    public IntPtr GetLeaderboardSortMethod;
    public IntPtr GetLeaderboardDisplayType;
    public IntPtr DownloadLeaderboardEntries;
    public IntPtr GetDownloadedLeaderboardEntry;
    public IntPtr UploadLeaderboardScore;
    public IntPtr GetNumberOfCurrentPlayers;
  }
}
