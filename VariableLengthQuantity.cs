using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public static class VariableLengthQuantity
{
    public static uint[] Encode(uint[] numbers)
    {
        List<uint> bytes = new List<uint>();
        for (int i = 0; i < numbers.Length; i++)
        {
            var resultat = Convert.ToString(numbers[i], 2);
            if (resultat.Length < 8)
            {
                bytes.Add(BinaryStringToUint(resultat));
            }
            else
            {
                List<string> list = new List<string>();
                var left = resultat.Length % 7;
                if (left != 0)
                {
                    StringBuilder leftByte = new StringBuilder();
                    leftByte.Append("1");
                    for (int j = 0; j < 7 - left; j++)
                        leftByte.Append("0");
                    leftByte.Append(resultat.Substring(0, left));
                    bytes.Add(BinaryStringToUint(leftByte.ToString()));
                }
                list.AddRange(GetBinaryStrings(resultat, left));
                bytes.AddRange(list.Select(x => BinaryStringToUint(x)));
            }
        }
        return bytes.ToArray();
    }
    private static IEnumerable<string> GetBinaryStrings(string remainBinaries, int leftBits)
    {
        for (int i = leftBits; i < remainBinaries.Length; i += 7)
            yield return i + 7 >= remainBinaries.Length ? "0" + remainBinaries.Substring(i, 7)
                                                        : "1" + remainBinaries.Substring(i, 7);
    }
    private static uint BinaryStringToUint(string binaryString) => Convert.ToUInt32(binaryString, 2);

    public static uint[] Decode(uint[] bytes)
    {
        List<uint> numbers = new List<uint>();
        StringBuilder initialeBinary = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            var stringByte = Convert.ToString(bytes[i], 2);
            if (stringByte.Length == 8 && stringByte.StartsWith("1"))
            {
                if (i + 1 == bytes.Length)
                    throw new InvalidOperationException("Incorrect sequence !");
                initialeBinary.Append(stringByte.Substring(1));
            }
            else
            {
                for (int j = 0; j < 7 - stringByte.Length; j++)
                    initialeBinary.Append("0");
                initialeBinary.Append(stringByte);
                numbers.Add(BinaryStringToUint(initialeBinary.ToString()));
                initialeBinary.Clear();
            }
        }
        return numbers.ToArray();
    }
}