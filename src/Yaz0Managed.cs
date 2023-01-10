using System.Runtime.InteropServices;

namespace Yaz0Library
{
    /// <summary>
    /// <para>Managed C# implementation of Yaz0 de/compression.</para>
    /// 
    /// <a href="https://github.com/KillzXGaming/BfresPlatformConverter/blob/master/LICENSE">MIT License</a><br/>
    /// Copyright © 2020 <a href="https://github.com/KillzXGaming/">KillzXGaming</a><br/>
    /// </summary>
    public class Yaz0Managed
    {
        [Obsolete("consider using Yaz0.Compress instead")]
        public static unsafe byte[] Compress(string FileName, int level = 7, uint res1 = 0, uint res2 = 0) => Compress(File.ReadAllBytes(FileName), level, res1, res2);

        [Obsolete("consider using Yaz0.Compress instead")]
        public static unsafe byte[] Compress(byte[] data, int level = 7, uint reserved1 = 0, uint reserved2 = 0)
        {
            int maxBackLevel = (int)(0x10e0 * (level / 9.0) - 0x0e0);

            byte* dataptr = (byte*)Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

            byte[] result = new byte[data.Length + data.Length / 8 + 0x10];
            byte* resultptr = (byte*)Marshal.UnsafeAddrOfPinnedArrayElement(result, 0);
            *resultptr++ = (byte)'Y';
            *resultptr++ = (byte)'a';
            *resultptr++ = (byte)'z';
            *resultptr++ = (byte)'0';
            *resultptr++ = (byte)((data.Length >> 24) & 0xFF);
            *resultptr++ = (byte)((data.Length >> 16) & 0xFF);
            *resultptr++ = (byte)((data.Length >> 8) & 0xFF);
            *resultptr++ = (byte)((data.Length >> 0) & 0xFF);
            {
                var res1 = BitConverter.GetBytes(reserved1);
                var res2 = BitConverter.GetBytes(reserved2);
                if (BitConverter.IsLittleEndian) {
                    Array.Reverse(res1);
                    Array.Reverse(res2);
                }
                *resultptr++ = res1[0];
                *resultptr++ = res1[1];
                *resultptr++ = res1[2];
                *resultptr++ = res1[3];
                *resultptr++ = res2[0];
                *resultptr++ = res2[1];
                *resultptr++ = res2[2];
                *resultptr++ = res2[3];
            }
            int length = data.Length;
            int dstoffs = 16;
            int Offs = 0;
            while (true) {
                int headeroffs = dstoffs++;
                resultptr++;
                byte header = 0;
                for (int i = 0; i < 8; i++) {
                    int comp = 0;
                    int back = 1;
                    int nr = 2;
                    {
                        byte* ptr = dataptr - 1;
                        int maxnum = 0x111;
                        if (length - Offs < maxnum) maxnum = length - Offs;
                        //Use a smaller amount of bytes back to decrease time
                        int maxback = maxBackLevel;//0x1000;
                        if (Offs < maxback) maxback = Offs;
                        maxback = (int)dataptr - maxback;
                        int tmpnr;
                        while (maxback <= (int)ptr) {
                            if (*(ushort*)ptr == *(ushort*)dataptr && ptr[2] == dataptr[2]) {
                                tmpnr = 3;
                                while (tmpnr < maxnum && ptr[tmpnr] == dataptr[tmpnr]) tmpnr++;
                                if (tmpnr > nr) {
                                    if (Offs + tmpnr > length) {
                                        nr = length - Offs;
                                        back = (int)(dataptr - ptr);
                                        break;
                                    }
                                    nr = tmpnr;
                                    back = (int)(dataptr - ptr);
                                    if (nr == maxnum) break;
                                }
                            }
                            --ptr;
                        }
                    }
                    if (nr > 2) {
                        Offs += nr;
                        dataptr += nr;
                        if (nr >= 0x12) {
                            *resultptr++ = (byte)(((back - 1) >> 8) & 0xF);
                            *resultptr++ = (byte)((back - 1) & 0xFF);
                            *resultptr++ = (byte)((nr - 0x12) & 0xFF);
                            dstoffs += 3;
                        }
                        else {
                            *resultptr++ = (byte)((((back - 1) >> 8) & 0xF) | (((nr - 2) & 0xF) << 4));
                            *resultptr++ = (byte)((back - 1) & 0xFF);
                            dstoffs += 2;
                        }
                        comp = 1;
                    }
                    else {
                        *resultptr++ = *dataptr++;
                        dstoffs++;
                        Offs++;
                    }
                    header = (byte)((header << 1) | ((comp == 1) ? 0 : 1));
                    if (Offs >= length) {
                        header = (byte)(header << (7 - i));
                        break;
                    }
                }
                result[headeroffs] = header;
                if (Offs >= length) break;
            }
            while ((dstoffs % 4) != 0) dstoffs++;
            byte[] realresult = new byte[dstoffs];
            Array.Copy(result, realresult, dstoffs);
            return realresult;
        }

        public static byte[] Decompress(string file) => Decompress(File.ReadAllBytes(file));
        public static byte[] Decompress(ReadOnlySpan<byte> data)
        {
            uint leng = (uint)(data[4] << 24 | data[5] << 16 | data[6] << 8 | data[7]);
            byte[] result = new byte[leng];
            int Offs = 16;
            int dstoffs = 0;
            while (true) {
                byte header = data[Offs++];
                for (int i = 0; i < 8; i++) {
                    if ((header & 0x80) != 0) {
                        result[dstoffs++] = data[Offs++];
                    }
                    else {
                        byte b = data[Offs++];
                        int offs = ((b & 0xF) << 8 | data[Offs++]) + 1;
                        int length = (b >> 4) + 2;
                        if (length == 2) {
                            length = data[Offs++] + 0x12;
                        }

                        for (int j = 0; j < length; j++) {
                            result[dstoffs] = result[dstoffs - offs];
                            dstoffs++;
                        }
                    }
                    if (dstoffs >= leng) {
                        return result;
                    }

                    header <<= 1;
                }
            }
        }
    }
}
