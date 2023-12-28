using Microsoft.SqlServer.Server;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace Bcfier.Data.Utils
{
  /// <summary>
  /// Conversion methods between an IFC 
  /// encoded GUID string and a .NET GUID.
  /// This is a translation of the C code 
  /// found here: 
  /// http://www.iai-tech.org/ifc/IFC2x3/TC1/html/index.htm
  /// </summary>
  public static class IfcGuid
  {
    #region Private Members
    /// <summary>
    /// The replacement table
    /// </summary>
    private static readonly char[] base64Chars = new char[]
        { '0','1','2','3','4','5','6','7','8','9'
        , 'A','B','C','D','E','F','G','H','I','J'
        , 'K','L','M','N','O','P','Q','R','S','T'
        , 'U','V','W','X','Y','Z','a','b','c','d'
        , 'e','f','g','h','i','j','k','l','m','n'
        , 'o','p','q','r','s','t','u','v','w','x'
        , 'y','z','_','$' };

    /// <summary>
    /// Conversion of an integer into characters 
    /// with base 64 using the table base64Chars
    /// </summary>
    /// <param name="number">The number to convert</param>
    /// <param name="result">The result char array to write to</param>
    /// <param name="start">The position in the char array to start writing</param>
    /// <param name="len">The length to write</param>
    /// <returns></returns>
    static void cv_to_64( uint number, ref char[] result, int start, int len )
    {
      uint act;
      int iDigit, nDigits;

      Debug.Assert( len <= 4 );
      act = number;
      nDigits = len;

      for( iDigit = 0; iDigit < nDigits; iDigit++ )
      {
        result[start + len - iDigit - 1] = base64Chars[( int ) ( act % 64 )];
        act /= 64;
      }
      Debug.Assert( act == 0, "Logic failed, act was not null: " + act.ToString() );
      return;
    }

    /// <summary>
    /// The reverse function to calculate 
    /// the number from the characters
    /// </summary>
    /// <param name="str">The char array to convert from</param>
    /// <param name="start">Position in array to start read</param>
    /// <param name="len">The length to read</param>
    /// <returns>The calculated nuber</returns>
    static uint cv_from_64( char[] str, int start, int len )
    {
      int i, j, index;
      uint res = 0;
      Debug.Assert( len <= 4 );

      for( i = 0; i < len; i++ )
      {
        index = -1;
        for( j = 0; j < 64; j++ )
        {
          if( base64Chars[j] == str[start + i] )
          {
            index = j;
            break;
          }
        }
        Debug.Assert( index >= 0 );
        res = res * 64 + ( ( uint ) index );
      }
      return res;
    }
    #endregion // Private Members

    #region Conversion Methods
    /// <summary>
    /// Reconstruction of the GUID 
    /// from an IFC GUID string (base64)
    /// </summary>
    /// <param name="guid">The GUID string to convert. Must be 22 characters long</param>
    /// <returns>GUID correspondig to the string</returns>
    /// 

    private static string cConversionTable = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_$";
    private static uint cv_from_64(string str)
    {
      int length = str.Length;
      if (length > 4)
      {
        throw new ArgumentException("Invalid Global ID Format");
      }
      uint num = 0u;
      for (int i = 0; i < length; i++)
      {
        int num2 = -1;
        for (int j = 0; j < 64; j++)
        {
          if (cConversionTable[j] == str[i])
          {
            num2 = j;
            break;
          }
        }
        if (num2 == -1)
        {
          throw new ArgumentException("Invalid Global ID Format");
        }
        num = (uint)(num * 64 + num2);
      }
      return num;
    }

    public static Guid FromIfcGUID( string ifcGuid )
    {
      uint[] array = new uint[6];
      if (ifcGuid == null)
      {
        throw new ArgumentNullException("format64");
      }
      ifcGuid = ifcGuid.Trim('\'');
      if (ifcGuid.Length != 22)
      {
        throw new ArgumentException("Invalid Global ID Length: " + ifcGuid);
      }
      int num = 0;
      int num2 = 2;
      for (int i = 0; i < 6; i++)
      {
        string str = ifcGuid.Substring(num, num2);
        num += num2;
        num2 = 4;
        array[i] = cv_from_64(str);
      }
      uint a = array[0] * 16777216 + array[1];
      ushort b = (ushort)(array[2] / 256);
      ushort c = (ushort)(array[2] % 256 * 256 + array[3] / 65536);
      byte d = (byte)(array[3] / 256 % 256);
      byte e = (byte)(array[3] % 256);
      byte f = (byte)(array[4] / 65536);
      byte g = (byte)(array[4] / 256 % 256);
      byte h = (byte)(array[4] % 256);
      byte i2 = (byte)(array[5] / 65536);
      byte j = (byte)(array[5] / 256 % 256);
      byte k = (byte)(array[5] % 256);
      return new Guid(a, b, c, d, e, f, g, h, i2, j, k);

      //Debug.Assert( guid.Length == 22, "Input string must not be longer that 22 chars" );
      //uint[] num = new uint[6];
      //char[] str = guid.ToCharArray();
      //int n = 2, pos = 0, i;
      //for( i = 0; i < 6; i++ )
      //{
      //  num[i] = cv_from_64( str, pos, n );
      //  pos += n; n = 4;
      //}
      //
      //int a = ( int ) ( ( num[0] * 16777216 + num[1] ) );
      //short b = ( short ) ( num[2] / 256 );
      //short c = ( short ) ( ( num[2] % 256 ) * 256 + num[3] / 65536 );
      //byte[] d = new byte[8];
      //d[0] = Convert.ToByte( ( num[3] / 256 ) % 256 );
      //d[1] = Convert.ToByte( num[3] % 256 );
      //d[2] = Convert.ToByte( num[4] / 65536 );
      //d[3] = Convert.ToByte( ( num[4] / 256 ) % 256 );
      //d[4] = Convert.ToByte( num[4] % 256 );
      //d[5] = Convert.ToByte( num[5] / 65536 );
      //d[6] = Convert.ToByte( ( num[5] / 256 ) % 256 );
      //d[7] = Convert.ToByte( num[5] % 256 );
      //
      //return new Guid( a, b, c, d );
    }

    /// <summary>
    /// Conversion of a GUID to a string 
    /// representing the GUID 
    /// </summary>
    /// <param name="guid">The GUID to convert</param>
    /// <returns>IFC (base64) encoded GUID string</returns>
    /// 
    private static string cv_to_64(uint number, int nDigits)
    {
      char[] array = new char[nDigits];
      uint num = number;
      for (int i = 0; i < nDigits; i++)
      {
        array[nDigits - i - 1] = cConversionTable[(int)(num % 64)];
        num /= 64;
      }
      if (num != 0)
      {
        throw new ArgumentException("Number out of range");
      }
      return new string(array);
    }

    public static string ToIfcGuid( Guid guid )
    {
      uint[] array = new uint[6];
      byte[] array2 = guid.ToByteArray();
      uint num = BitConverter.ToUInt32(array2, 0);
      ushort num2 = BitConverter.ToUInt16(array2, 4);
      ushort num3 = BitConverter.ToUInt16(array2, 6);
      byte b = array2[8];
      byte b2 = array2[9];
      byte b3 = array2[10];
      byte b4 = array2[11];
      byte b5 = array2[12];
      byte b6 = array2[13];
      byte b7 = array2[14];
      byte b8 = array2[15];
      array[0] = num / 16777216;
      array[1] = num % 16777216;
      array[2] = (uint)(num2 * 256 + num3 / 256);
      array[3] = (uint)(num3 % 256 * 65536 + b * 256 + b2);
      array[4] = (uint)(b3 * 65536 + b4 * 256 + b5);
      array[5] = (uint)(b6 * 65536 + b7 * 256 + b8);
      StringBuilder stringBuilder = new StringBuilder();
      int nDigits = 2;
      for (int i = 0; i < 6; i++)
      {
        string value = cv_to_64(array[i], nDigits);
        stringBuilder.Append(value);
        nDigits = 4;
      }
      return stringBuilder.ToString();

      //uint[] num = new uint[6];
      //char[] str = new char[22];
      //int i, n;
      //byte[] b = guid.ToByteArray();
      //
      //// Creation of six 32 Bit integers from the components of the GUID structure
      //num[0] = ( uint ) ( BitConverter.ToUInt32( b, 0 ) / 16777216 );
      //num[1] = ( uint ) ( BitConverter.ToUInt32( b, 0 ) % 16777216 );
      //num[2] = ( uint ) ( BitConverter.ToUInt16( b, 4 ) * 256 + BitConverter.ToInt16( b, 6 ) / 256 );
      //num[3] = ( uint ) ( ( BitConverter.ToUInt16( b, 6 ) % 256 ) * 65536 + b[8] * 256 + b[9] );
      //num[4] = ( uint ) ( b[10] * 65536 + b[11] * 256 + b[12] );
      //num[5] = ( uint ) ( b[13] * 65536 + b[14] * 256 + b[15] );
      //
      //// Conversion of the numbers into a system using a base of 64
      //n = 2;
      //int pos = 0;
      //for( i = 0; i < 6; i++ )
      //{
      //  cv_to_64( num[i], ref str, pos, n );
      //  pos += n; n = 4;
      //}
      //return new String( str );
    }
    #endregion // Conversion Methods

    
    #region Extension Methods

     //<summary>
     //Get the Unique ID in encoded on IFC Format (base 64)
     //</summary>
     //<param name="element"></param>
     //<returns></returns>
    public static string IfcGUID(string UniqueId)
    {
      Guid episodeId = new Guid(UniqueId.Substring(0, 36));
      int elementId = int.Parse(UniqueId.Substring(37), NumberStyles.AllowHexSpecifier);
      int last_32_bits = int.Parse(UniqueId.Substring(28, 8), NumberStyles.AllowHexSpecifier);
      int xor = last_32_bits ^ elementId;
      UniqueId = UniqueId.Substring(0, 28) + xor.ToString("x8");
      Guid guid = new Guid(UniqueId);
      return ToIfcGuid(guid);
    }
    #endregion

  }
}
