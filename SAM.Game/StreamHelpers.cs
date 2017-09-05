// Decompiled with JetBrains decompiler
// Type: SAM.Game.StreamHelpers
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

using System;
using System.IO;
using System.Text;

namespace SAM.Game
{
  internal static class StreamHelpers
  {
    public static byte ReadValueU8(this Stream stream)
    {
      return (byte) stream.ReadByte();
    }

    public static int ReadValueS32(this Stream stream)
    {
      byte[] buffer = new byte[4];
      stream.Read(buffer, 0, 4);
      return BitConverter.ToInt32(buffer, 0);
    }

    public static uint ReadValueU32(this Stream stream)
    {
      byte[] buffer = new byte[4];
      stream.Read(buffer, 0, 4);
      return BitConverter.ToUInt32(buffer, 0);
    }

    public static ulong ReadValueU64(this Stream stream)
    {
      byte[] buffer = new byte[8];
      stream.Read(buffer, 0, 8);
      return BitConverter.ToUInt64(buffer, 0);
    }

    public static float ReadValueF32(this Stream stream)
    {
      byte[] buffer = new byte[4];
      stream.Read(buffer, 0, 4);
      return BitConverter.ToSingle(buffer, 0);
    }

    internal static string ReadStringInternalDynamic(this Stream stream, Encoding encoding, char end)
    {
      int byteCount = encoding.GetByteCount("e");
      string str = end.ToString();
      int num = 0;
      byte[] array = new byte[128 * byteCount];
      while (true)
      {
        if (num + byteCount > array.Length)
          Array.Resize<byte>(ref array, array.Length + 128 * byteCount);
        stream.Read(array, num, byteCount);
        if (!(encoding.GetString(array, num, byteCount) == str))
          num += byteCount;
        else
          break;
      }
      if (num == 0)
        return "";
      return encoding.GetString(array, 0, num);
    }

    public static string ReadStringASCIIZ(this Stream stream)
    {
      return stream.ReadStringInternalDynamic(Encoding.ASCII, char.MinValue);
    }

    public static string ReadStringUTF8Z(this Stream stream)
    {
      return stream.ReadStringInternalDynamic(Encoding.UTF8, char.MinValue);
    }
  }
}
