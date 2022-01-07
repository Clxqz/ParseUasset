using System;
using System.IO;
using System.Text;
class Program 
{
  public static string parsedString = "";
  public static void Main (string[] args) 
  {
    string filePath = "CP_Body_Commando_F_RedKnightWinter.uasset";
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"Parsed Uasset: {filePath.ToString()}");
    Parse(filePath);
    RemoveAllEncoding(parsedString);
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(parsedString);

  }
  public static void Parse(string path)
  {
    try
    {
      byte[] data = File.ReadAllBytes(path);
      byte[] pattern = {47,71,97,109,101,47};
      long offset = ByteArrayExtensions.IndexOf(data, pattern, 0);
      int convertToInt = Convert.ToInt32(offset);
      long offset1 = ByteArrayExtensions.LastIndexOf(data, pattern);
      int convertToIntOffset1 = Convert.ToInt32(offset1);
      int count = 0;
      while(true)
      {
        long index = ByteArrayExtensions.IndexOf(data, pattern, count);
        int convertInt = Convert.ToInt32(index);
        int variable = convertInt - 1;
        count += 1;
        if(count == variable)
        {
          byte b = data[variable];
          data[variable] = 0;
        }
        if(count == convertToIntOffset1)
        {
          break;
        }
      }
    
      while(true)
      {
        
        char c = (char)data[convertToInt];
        parsedString = parsedString += c;
        convertToInt += 1;
        if(convertToInt == convertToIntOffset1)
        {
          SortString(parsedString);
          break;
        }
      }
    }
    catch
    {
      Console.WriteLine("Failed To Parse");
    }
  }
  public static void RemoveAllEncoding(string str)
  {
          string asAscii = Encoding.ASCII.GetString(
        Encoding.Convert(
            Encoding.UTF8,
            Encoding.GetEncoding(
                Encoding.ASCII.EncodingName,
                new EncoderReplacementFallback(string.Empty),
                new DecoderExceptionFallback()
                ),
            Encoding.UTF8.GetBytes(str)
        )
    );
    parsedString = asAscii;
  }
  public static void SortString(string parse)
  {
    parse = parsedString.Replace("/Game/", "\n\n/Game/");
    parsedString = parse;
  }
}

static class ByteArrayExtensions
{
    public static long LastIndexOf(this byte[] data, byte[] pattern)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        if (pattern == null) throw new ArgumentNullException(nameof(pattern));
        if (pattern.LongLength > data.LongLength) return -1;

        var cycles = data.LongLength - pattern.LongLength + 1;
        long patternIndex;
        for (var dataIndex = cycles; dataIndex > 0; dataIndex--)
        {
            if (data[dataIndex] != pattern[0]) continue;
            for (patternIndex = pattern.Length - 1; patternIndex >= 1; patternIndex--) if (data[dataIndex + patternIndex] != pattern[patternIndex]) break;
            if (patternIndex == 0) return dataIndex;
        }
        return -1;
    }
    public static long IndexOf(this byte[] data, byte[] pattern, long startIndex)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        if (pattern == null) throw new ArgumentNullException(nameof(pattern));
        if (pattern.LongLength > data.LongLength) return -1;

        var cycles = data.LongLength - pattern.LongLength + 1;
        long patternIndex;
        for (var dataIndex = startIndex; dataIndex < cycles; dataIndex++)
        {
            if (data[dataIndex] != pattern[0]) continue;
            for (patternIndex = pattern.Length - 1; patternIndex >= 1; patternIndex--) if (data[dataIndex + patternIndex] != pattern[patternIndex]) break;
            if (patternIndex == 0) return dataIndex;
        }
        return -1;
    }
}
