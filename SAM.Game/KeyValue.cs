// Decompiled with JetBrains decompiler
// Type: SAM.Game.KeyValue
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAM.Game
{
  public class KeyValue
  {
    private static KeyValue Invalid = new KeyValue();
    public string Name = "<root>";
    public KeyValueType Type;
    public object Value;
    public bool Valid;
    public List<KeyValue> Children;

    public KeyValue this[string key]
    {
      get
      {
        if (this.Children == null)
          return KeyValue.Invalid;
        return this.Children.SingleOrDefault<KeyValue>((Func<KeyValue, bool>) (c => c.Name.ToLowerInvariant() == key.ToLowerInvariant())) ?? KeyValue.Invalid;
      }
    }

    public string AsString(string defaultValue)
    {
      if (!this.Valid || this.Value == null)
        return defaultValue;
      return this.Value.ToString();
    }

    public int AsInteger(int defaultValue)
    {
      if (!this.Valid)
        return defaultValue;
      switch (this.Type)
      {
        case KeyValueType.String:
        case KeyValueType.WideString:
          int result;
          if (!int.TryParse((string) this.Value, out result))
            return defaultValue;
          return result;
        case KeyValueType.Int32:
          return (int) this.Value;
        case KeyValueType.Float32:
          return (int) (float) this.Value;
        case KeyValueType.UInt64:
          return (int) ((long) (ulong) this.Value & (long) uint.MaxValue);
        default:
          return defaultValue;
      }
    }

    public float AsFloat(float defaultValue)
    {
      if (!this.Valid)
        return defaultValue;
      switch (this.Type)
      {
        case KeyValueType.String:
        case KeyValueType.WideString:
          float result;
          if (!float.TryParse((string) this.Value, out result))
            return defaultValue;
          return result;
        case KeyValueType.Int32:
          return (float) (int) this.Value;
        case KeyValueType.Float32:
          return (float) this.Value;
        case KeyValueType.UInt64:
          return (float) ((ulong) this.Value & (ulong) uint.MaxValue);
        default:
          return defaultValue;
      }
    }

    public bool AsBoolean(bool defaultValue)
    {
      if (!this.Valid)
        return defaultValue;
      switch (this.Type)
      {
        case KeyValueType.String:
        case KeyValueType.WideString:
          int result;
          if (!int.TryParse((string) this.Value, out result))
            return defaultValue;
          return result != 0;
        case KeyValueType.Int32:
          return (int) this.Value != 0;
        case KeyValueType.Float32:
          return (double) (int) (float) this.Value != 0.0;
        case KeyValueType.UInt64:
          return (long) (ulong) this.Value != 0L;
        default:
          return defaultValue;
      }
    }

    public override string ToString()
    {
      if (!this.Valid)
        return "<invalid>";
      if (this.Type == KeyValueType.None)
        return this.Name;
      return string.Format("{0} = {1}", (object) this.Name, this.Value);
    }

    public static KeyValue LoadAsBinary(string path)
    {
      if (!File.Exists(path))
        return (KeyValue) null;
      try
      {
        FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        KeyValue keyValue = new KeyValue();
        if (!keyValue.ReadAsBinary((Stream) fileStream))
          return (KeyValue) null;
        fileStream.Close();
        return keyValue;
      }
      catch (Exception ex)
      {
        return (KeyValue) null;
      }
    }

    public bool ReadAsBinary(Stream input)
    {
      this.Children = new List<KeyValue>();
      try
      {
        while (true)
        {
          KeyValueType keyValueType = (KeyValueType) input.ReadValueU8();
          if (keyValueType != KeyValueType.End)
          {
            KeyValue keyValue = new KeyValue();
            keyValue.Type = keyValueType;
            keyValue.Name = input.ReadStringUTF8Z();
            switch (keyValueType)
            {
              case KeyValueType.None:
                keyValue.ReadAsBinary(input);
                break;
              case KeyValueType.String:
                keyValue.Valid = true;
                keyValue.Value = (object) input.ReadStringUTF8Z();
                break;
              case KeyValueType.Int32:
                keyValue.Valid = true;
                keyValue.Value = (object) input.ReadValueS32();
                break;
              case KeyValueType.Float32:
                keyValue.Valid = true;
                keyValue.Value = (object) input.ReadValueF32();
                break;
              case KeyValueType.Pointer:
                keyValue.Valid = true;
                keyValue.Value = (object) input.ReadValueU32();
                break;
              case KeyValueType.WideString:
                goto label_5;
              case KeyValueType.Color:
                keyValue.Valid = true;
                keyValue.Value = (object) input.ReadValueU32();
                break;
              case KeyValueType.UInt64:
                keyValue.Valid = true;
                keyValue.Value = (object) input.ReadValueU64();
                break;
              default:
                goto label_11;
            }
            if (input.Position < input.Length)
              this.Children.Add(keyValue);
            else
              goto label_13;
          }
          else
            goto label_15;
        }
label_5:
        throw new FormatException("wstring is unsupported");
label_11:
        throw new FormatException();
label_13:
        throw new FormatException();
label_15:
        this.Valid = true;
        return input.Position == input.Length;
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  }
}
