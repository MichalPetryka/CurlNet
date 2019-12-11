using System;
using System.Text;
// ReSharper disable PossibleNullReferenceException

namespace CurlNet
{
	internal static class BomUtil
	{
		private static readonly UTF8Encoding Utf8BomEncoding = new UTF8Encoding(true);
		private static readonly UTF7Encoding Utf7BomEncoding = new UTF7Encoding();
		private static readonly UnicodeEncoding Utf16LeBomEncoding = new UnicodeEncoding(false, true);
		private static readonly UnicodeEncoding Utf16BeBomEncoding = new UnicodeEncoding(true, true);
		private static readonly UTF32Encoding Utf32LeBomEncoding = new UTF32Encoding(false, true);
		private static readonly UTF32Encoding Utf32BeBomEncoding = new UTF32Encoding(true, true);

		internal static Encoding GetEncoding(ArraySegment<byte> bytes, Encoding encoding, out int offset)
		{
			if (bytes.Array[bytes.Offset] == 0x2b && bytes.Array[bytes.Offset + 1] == 0x2f && bytes.Array[bytes.Offset + 2] == 0x76)
			{
				offset = 3;
				return Utf7BomEncoding;
			}

			if (bytes.Array[bytes.Offset] == 0xef && bytes.Array[bytes.Offset + 1] == 0xbb && bytes.Array[bytes.Offset + 2] == 0xbf)
			{
				offset = 3;
				return Utf8BomEncoding;
			}

			if (bytes.Array[bytes.Offset] == 0xff && bytes.Array[bytes.Offset + 1] == 0xfe && bytes.Array[bytes.Offset + 2] == 0 && bytes.Array[bytes.Offset + 3] == 0)
			{
				offset = 4;
				return Utf32LeBomEncoding;
			}

			if (bytes.Array[bytes.Offset] == 0 && bytes.Array[bytes.Offset + 1] == 0 && bytes.Array[bytes.Offset + 2] == 0xfe && bytes.Array[bytes.Offset + 3] == 0xff)
			{
				offset = 4;
				return Utf32BeBomEncoding;
			}

			if (bytes.Array[bytes.Offset] == 0xff && bytes.Array[bytes.Offset + 1] == 0xfe)
			{
				offset = 2;
				return Utf16LeBomEncoding;
			}

			if (bytes.Array[bytes.Offset] == 0xfe && bytes.Array[bytes.Offset + 1] == 0xff)
			{
				offset = 2;
				return Utf16BeBomEncoding;
			}

			byte[] bom = encoding.GetPreamble();
			bool bomPresent = true;
			for (int i = 0; i < bom.Length; i++)
			{
				if (bytes.Array[bytes.Offset + i] != bom[i])
				{
					bomPresent = false;
					break;
				}
			}

			offset = bomPresent ? bom.Length : 0;
			return encoding;
		}
	}
}
