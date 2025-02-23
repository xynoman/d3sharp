using System;
using System.Text;

namespace ProtoHelpers.Utils
{
    public static class Extensions
    {
        public static string DumpHex(this byte[] array, bool spaced) {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in array) {
                sb.Append(b.ToString("X2"));
                if (spaced)
                    sb.Append(' ');
            }
            return sb.ToString();
        }
    }

    public static class Conversion {
        // Behold, the horrific beauty -- courtesy of the dastardly fellas from protobuf-csharp
        public static byte[] Unescape(string input)
        {
            byte[] result = new byte[input.Length];
            int pos = 0;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c > 127 || c < 32)
                {
                    throw new FormatException("Escaped string must only contain ASCII");
                }
                if (c != '\\')
                {
                    result[pos++] = (byte)c;
                    continue;
                }
                if (i + 1 >= input.Length)
                {
                    throw new FormatException("Invalid escape sequence: '\\' at end of string.");
                }
                
                i++;
                c = input[i];
                if (c >= '0' && c <= '7')
                {
                    // Octal escape. 
                    int code = ParseDigit(c);
                    if (i + 1 < input.Length && IsOctal(input[i + 1]))
                    {
                        i++;
                        code = code * 8 + ParseDigit(input[i]);
                    }
                    if (i + 1 < input.Length && IsOctal(input[i + 1]))
                    {
                        i++;
                        code = code * 8 + ParseDigit(input[i]);
                    }
                    result[pos++] = (byte)code;
                }
                else
                {
                    switch (c)
                    {
                    case 'a':
                        result[pos++] = 0x07;
                        break;
                    case 'b':
                        result[pos++] = (byte)'\b';
                        break;
                    case 'f':
                        result[pos++] = (byte)'\f';
                        break;
                    case 'n':
                        result[pos++] = (byte)'\n';
                        break;
                    case 'r':
                        result[pos++] = (byte)'\r';
                        break;
                    case 't':
                        result[pos++] = (byte)'\t';
                        break;
                    case 'v':
                        result[pos++] = 0x0b;
                        break;
                    case '\\':
                        result[pos++] = (byte)'\\';
                        break;
                    case '\'':
                        result[pos++] = (byte)'\'';
                        break;
                    case '"':
                        result[pos++] = (byte)'\"';
                        break;
                    case 'x':
                        // hex escape
                        int code;
                        if (i + 1 < input.Length && IsHex(input[i + 1]))
                        {
                            i++;
                            code = ParseDigit(input[i]);
                        }
                        else
                        {
                            throw new FormatException("Invalid escape sequence: '\\x' with no digits");
                        }
                        if (i + 1 < input.Length && IsHex(input[i + 1]))
                        {
                            ++i;
                            code = code * 16 + ParseDigit(input[i]);
                        }
                        result[pos++] = (byte)code;
                        break;
                    default:
                        throw new FormatException("Invalid escape sequence: '\\" + c + "'");
                    }
                }
            }
            byte[] bout = new byte[pos];
            Array.Copy(result, bout, pos);
            return bout;
        }

        public static bool IsOctal(char c)
        {
            return '0' <= c && c <= '7';
        }

        public static bool IsHex(char c)
        {
            return ('0' <= c && c <= '9')
                || ('a' <= c && c <= 'f')
                || ('A' <= c && c <= 'F');
        }

        public static int ParseDigit(char c)
        {
            if ('0' <= c && c <= '9')
                return c - '0';
            else if ('a' <= c && c <= 'z')
                return c - 'a' + 10;
            else
                return c - 'A' + 10;
        }
    }
}
