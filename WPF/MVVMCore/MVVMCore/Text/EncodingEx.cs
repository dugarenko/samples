using System.IO;
using System.Text;

namespace MVVMCore.Text
{
    /// <summary>
    /// Rozszerzenie klasy Encoding.
    /// </summary>
	public static class EncodingEx
    {
        /// <summary>
        /// Identyfikuje przekazane znaki BOM i zwraca odpowiedni obiekt kodowania znaków.
        /// </summary>
        /// <param name="length">Ilość wczytanych bajtów do bufora 4-bajtowego.</param>
        /// <param name="bomBuffer">Bufor 4-bajtowy BOM.</param>
        /// <param name="encoding">Obiekt kodowania znaków.</param>
        /// <returns>True, jeśli zidentyfikowano obiekt kodowania znaków, w przeciwnym razie false.</returns>
        internal static bool DetectEncoding(int length, byte[] bomBuffer, out Encoding encoding)
        {
            // ===============================================
            // Wykrywane kodowanie.
            // ===============================================
            // UTF-16LE - Little Endian Unicode: (FF FE ?? ??)
            // UTF-16BE - Big Endian Unicode   : (FE FF)
            // UTF-32LE - Little Endian Unicode: (FF FE 00 00)
            // UTF-32BE - Big Endian Unicode   : (00 00 FE FF)
            // UTF-8                           : (EF BB BF)
            // ===============================================
            // Kodowanie bez BOM.
            // ===============================================
            // ASCII - new ASCIIEncoding()
            // UTF-7                           : (2B 2F 76)
            // ===============================================

            encoding = null;

            if (length < 2)
            {
                return false;
            }

            if (bomBuffer[0] == 0xFE && bomBuffer[1] == 0xFF)
            {
                // Big Endian Unicode (UTF-16BE) - (FE FF).
                encoding = new UnicodeEncoding(true, true);
                return true;
            }
            else if (bomBuffer[0] == 0xFF && bomBuffer[1] == 0xFE)
            {
                if (length < 4 || bomBuffer[2] != 0 || bomBuffer[3] != 0)
                {
                    // Little Endian Unicode (UTF-16LE) - (FF FE ?? ??).
                    encoding = new UnicodeEncoding(false, true);
                }
                else
                {
                    // Little Endian Unicode (UTF-32LE) - (FF FE 00 00).
                    encoding = new UTF32Encoding(false, true);
                }
                return true;
            }
            else if (length >= 3 && bomBuffer[0] == 0xEF && bomBuffer[1] == 0xBB && bomBuffer[2] == 0xBF)
            {
                // UTF-8 - (EF BB BF).
                encoding = new UTF8Encoding(true);
                return true;
            }
            else if (length >= 4 && bomBuffer[0] == 0 && bomBuffer[1] == 0 && bomBuffer[2] == 0xFE && bomBuffer[3] == 0xFF)
            {
                // Big Endian Unicode (UTF-32BE) - (00 00 FE FF).
                encoding = new UTF32Encoding(true, true);
                return true;
            }
            else if (length >= 3 && bomBuffer[0] == 0x2B && bomBuffer[1] == 0x2F && bomBuffer[2] == 0x76)
            {
                // UTF-7 - (2B 2F 76).
                encoding = Encoding.UTF7;
            }

            return false;
        }

        /// <summary>
        /// Zwraca obiekt kodowania znaków wskazujący informację jakiej użyto strony kodowej do zapisania danych w podanym pliku.
        /// </summary>
        /// <param name="filePath">Ścieżka do pliku.</param>
        /// <returns>Strona kodowa znaków.</returns>
        public static Encoding GetEncoding(string filePath)
        {
            //using (StreamReader reader = new StreamReader(filePath, Encoding.Default, true, 4))
            //{
            //    reader.Peek();
            //    return reader.CurrentEncoding;
            //}

            Encoding fileEncoding = null;
            byte[] bomBuffer = new byte[4];
            int byteLen = 0;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bomBuffer.Length))
            {
                byteLen = fs.Read(bomBuffer, 0, bomBuffer.Length);
            }

            if (DetectEncoding(byteLen, bomBuffer, out fileEncoding))
            {
                return fileEncoding;
            }
            return null;
        }

        /// <summary>
        /// Zwraca obiekt kodowania znaków wskazujący informację jakiej użyto strony kodowej do zapisania danych w podanym pliku.
        /// </summary>
        /// <param name="filePath">Ścieżka do pliku.</param>
        /// <param name="defaultEncoding">Wskazuje domyślne kodowanie, które zostanie zwrócone w przypadku nie odczytania strony kodowej ze strumienia danych.</param>
        /// <returns>Strona kodowa znaków.</returns>
        public static Encoding GetEncoding(string filePath, Encoding defaultEncoding)
        {
            Encoding fileEncoding = GetEncoding(filePath);
            if (fileEncoding == null)
            {
                return defaultEncoding;
            }
            return fileEncoding;
        }
    }
}
