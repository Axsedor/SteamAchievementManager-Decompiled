﻿// Decompiled with JetBrains decompiler
// Type: SAM.API.Types.AppDataChanged
// Assembly: SAM.API, Version=6.3.0.804, Culture=neutral, PublicKeyToken=null
// MVID: 7DF108F6-41E2-4750-9029-3DA2C808D0A1
// Assembly location: F:\Windows\Desktop\SAM\SAM.API.dll

using System.Runtime.InteropServices;

namespace SAM.API.Types
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct AppDataChanged
  {
    public uint m_nAppID;
    public bool m_eResult;
  }
}
