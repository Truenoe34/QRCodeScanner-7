using System;
using System.Collections.Generic;
using System.Text;

public class CodeWars
{
    public static string Scanner(int[][] qrcode)
    {
        int size = qrcode.Length;
        List<int> bits = new List<int>();
        
        // Визначаємо патерн маски та знімаємо стандартну маску формату (XOR 101 -> 5)
        int maskPattern = ((qrcode[8][2] << 2) | (qrcode[8][3] << 1) | qrcode[8][4]) ^ 5;

        bool up = true;
        
        // Зигзагоподібне зчитування матриці (по 2 колонки справа наліво)
        for (int c = size - 1; c > 0; c -= 2)
        {
            if (c == 6) c--; // Пропускаємо вертикальну лінію синхронізації

            for (int i = 0; i < size; i++)
            {
                int r = up ? size - 1 - i : i;

                for (int j = 0; j < 2; j++)
                {
                    int col = c - j;

                    // Пропускаємо службові зони та лінії синхронізації
                    if ((r < 9 && col < 9) ||
                        (r < 9 && col > size - 9) ||
                        (r > size - 9 && col < 9) ||
                        r == 6 || col == 6)
                    {
                        continue;
                    }

                    int bit = qrcode[r][col];
                    
                    // Застосовуємо маску
                    if (GetMask(maskPattern, r, col))
                    {
                        bit ^= 1;
                    }
                    bits.Add(bit);
                }
            }
            up = !up;
        }

        // Читаємо довжину повідомлення (біти з 4 по 11)
        int length = ReadBits(bits, 4, 8);
        
        StringBuilder result = new StringBuilder();
        int idx = 12;
        
        for (int i = 0; i < length; i++)
        {
            if (idx + 8 > bits.Count) break;
            int charCode = ReadBits(bits, idx, 8);
            result.Append((char)charCode);
            idx += 8;
        }

        return result.ToString();
    }

    private static bool GetMask(int pattern, int r, int c)
    {
        switch (pattern)
        {
            case 0: return (r + c) % 2 == 0;
            case 1: return r % 2 == 0;
            case 2: return c % 3 == 0;
            case 3: return (r + c) % 3 == 0;
            case 4: return (r / 2 + c / 3) % 2 == 0;
            case 5: return ((r * c) % 2 + (r * c) % 3) == 0;
            case 6: return (((r * c) % 2) + ((r * c) % 3)) % 2 == 0;
            case 7: return (((r + c) % 2) + ((r * c) % 3)) % 2 == 0;
            default: return false;
        }
    }

    private static int ReadBits(List<int> bits, int start, int length)
    {
        int value = 0;
        for (int i = 0; i < length; i++)
        {
            value = (value << 1) | bits[start + i];
        }
        return value;
    }
}
