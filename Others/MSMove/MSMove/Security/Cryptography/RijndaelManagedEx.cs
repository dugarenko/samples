namespace MSMove.Security.Cryptography
{
    /// <summary>
    /// Klasa dostarcza metody szyfrowania danych algorytmem RijndaelManaged.
    /// </summary>
    internal class RijndaelManagedEx
    {
        #region Public methods.

        /// <summary>
        /// Generuje hash klucza.
        /// </summary>
        /// <param name="password">Hasło.</param>
        /// <param name="keySize">Rozmiar klucza. Maksymalnie 256 bitów, minimalnie 128 bitów, przeskok o 64 bity.</param>
        /// <returns>Hash klucza.</returns>
        public static byte[] GetKey(string password, int keySize)
        {
            if (password == null)
            {
                throw new System.ArgumentNullException("Argument 'password' is null.");
            }

            byte[] pwd = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = System.Text.Encoding.UTF8.GetBytes(password);

            // Na Windows Phone 8 dostępny jest tylko algorytm SHA1.
            //System.Security.Cryptography.PasswordDeriveBytes pdb = 
            //    new System.Security.Cryptography.PasswordDeriveBytes(pwd, salt, "SHA1", 100 + pwd.Length);
            System.Security.Cryptography.PasswordDeriveBytes pdb =
                new System.Security.Cryptography.PasswordDeriveBytes(pwd, salt, "SHA512", 100 + pwd.Length);
            return pdb.GetBytes(keySize);
        }

        /// <summary>Szyfruje przekazany ciąg tekstowy.</summary>
        /// <param name="textToEncrypt">Tekst do zaszyfrowania.</param>
        /// <param name="key">Klucz szyfrujący. Maksymalnie 256 bitów, minimalnie 128 bitów, przeskok o 64 bity.</param>
        /// <param name="iv">Wektor inicjalizacji. Rozmiar 128 bitów.</param>
        /// <param name="encryptedData">Zaszyfrowana tablica bajtów.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static void Encrypt(string textToEncrypt, byte[] key, byte[] iv, out byte[] encryptedData)
        {
            if (textToEncrypt == null)
            {
                throw new System.ArgumentNullException("textToEncrypt");
            }
            if (textToEncrypt.Length < 1)
            {
                throw new System.ArgumentException("Argument 'textToEncrypt' is empty.");
            }

            if (key == null)
            {
                throw new System.ArgumentNullException("key");
            }
            if (key.Length != 16 && key.Length != 22 && key.Length != 32)
            {
                throw new System.ArgumentException("Argument 'key' does not mach the block size for this algorithm.");
            }

            if (iv == null)
            {
                throw new System.ArgumentNullException("iv");
            }
            if (iv.Length != 16)
            {
                throw new System.ArgumentException("Argument 'iv' does not mach the block size for this algorithm.");
            }

            System.IO.MemoryStream dataToEncrypt = null;
            System.IO.StreamWriter sw = null;

            try
            {
                dataToEncrypt = new System.IO.MemoryStream();
                sw = new System.IO.StreamWriter(dataToEncrypt);
                sw.Write(textToEncrypt);
                sw.Flush();

                using (System.IO.MemoryStream dataEncrypted = new System.IO.MemoryStream())
                {
                    Encrypt(dataToEncrypt, dataEncrypted, key, iv);
                    encryptedData = dataEncrypted.ToArray();
                }
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                }
                else if (dataToEncrypt != null)
                {
                    dataToEncrypt.Dispose();
                }
            }
        }

        /// <summary>Szyfruje przekazaną tablicę bajtów.</summary>
        /// <param name="dataToEncrypt">Tablica bajtów do zaszyfrowania.</param>
        /// <param name="key">Klucz szyfrujący. Maksymalnie 256 bitów, minimalnie 128 bitów, przeskok o 64 bity.</param>
        /// <param name="iv">Wektor inicjalizacji. Rozmiar 128 bitów.</param>
        /// <param name="encryptedData">Zaszyfrowana tablica bajtów.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static void Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv, out byte[] encryptedData)
        {
            if (dataToEncrypt == null)
            {
                throw new System.ArgumentNullException("dataToEncrypt");
            }
            if (dataToEncrypt.Length < 1)
            {
                throw new System.ArgumentException("Argument 'dataToEncrypt' is empty.");
            }

            if (key == null)
            {
                throw new System.ArgumentNullException("key");
            }
            if (key.Length != 16 && key.Length != 22 && key.Length != 32)
            {
                throw new System.ArgumentException("Argument 'key' does not mach the block size for this algorithm.");
            }

            if (iv == null)
            {
                throw new System.ArgumentNullException("iv");
            }
            if (iv.Length != 16)
            {
                throw new System.ArgumentException("Argument 'iv' does not mach the block size for this algorithm.");
            }

            using (System.IO.MemoryStream toEncrypt = new System.IO.MemoryStream(dataToEncrypt))
            {
                using (System.IO.MemoryStream dataEncrypted = new System.IO.MemoryStream())
                {
                    Encrypt(toEncrypt, dataEncrypted, key, iv);
                    encryptedData = dataEncrypted.ToArray();
                }
            }
        }

        /// <summary>Szyfruje przekazany strumień danych.</summary>
        /// <param name="streamToEncrypt">Strumień danych do zaszyfrowania.</param>
        /// <param name="streamEncrypted">Zaszyfrowany strumień danych. Uwaga strumień zostanie zamknięty,
        /// ale w razie konieczności można będzie użyć metody 'ToArray()'.</param>
        /// <param name="key">Klucz szyfrujący. Maksymalnie 256 bitów, minimalnie 128 bitów, przeskok o 64 bity.</param>
        /// <param name="iv">Wektor inicjalizacji. Rozmiar 128 bitów.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static void Encrypt(System.IO.Stream streamToEncrypt, System.IO.Stream streamEncrypted, byte[] key, byte[] iv)
        {
            if (streamToEncrypt == null)
            {
                throw new System.ArgumentNullException("streamToEncrypt");
            }
            if (streamToEncrypt.CanRead == false)
            {
                throw new System.ArgumentException("Argument 'streamToEncrypt' is not readable.");
            }

            if (streamEncrypted == null)
            {
                throw new System.ArgumentNullException("streamEncrypted");
            }
            if (streamEncrypted.CanWrite == false)
            {
                throw new System.ArgumentException("Argument 'streamEncrypted' is not writable.");
            }

            if (key == null)
            {
                throw new System.ArgumentNullException("key");
            }
            if (key.Length != 16 && key.Length != 22 && key.Length != 32)
            {
                throw new System.ArgumentException("Argument 'key' does not mach the block size for this algorithm.");
            }

            if (iv == null)
            {
                throw new System.ArgumentNullException("iv");
            }
            if (iv.Length != 16)
            {
                throw new System.ArgumentException("Argument 'iv' does not mach the block size for this algorithm.");
            }

            streamToEncrypt.Position = 0;
            streamEncrypted.Position = 0;

            using (System.Security.Cryptography.RijndaelManaged algoritmCrypt = new System.Security.Cryptography.RijndaelManaged())
            {
                using (System.Security.Cryptography.ICryptoTransform encryptor = algoritmCrypt.CreateEncryptor(key, iv))
                {
                    using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(
                        streamEncrypted, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        streamToEncrypt.CopyTo(cs);
                        cs.FlushFinalBlock();
                    }
                }
            }
        }

        /// <summary>Szyfruje dane algorytmem RijndaelManaged z użyciem klucza 256 bitowego.</summary>
        /// <param name="textToEncrypt">Tekst do zaszyfrowania.</param>
        /// <param name="password">Hasło. Hash hasła wyliczny jest 512 bitowym algorytmem hash-ującym SHA512.</param>
        /// <param name="iv">Wektor inicjalizacji. Rozmiar 128 bitów.</param>
        /// <param name="encryptedData">Zaszyfrowana tablica bajtów.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static void Encrypt(string textToEncrypt, string password, byte[] iv, out byte[] encryptedData)
        {
            byte[] key = GetKey(password, 32);
            Encrypt(textToEncrypt, key, iv, out encryptedData);
        }

        /// <summary>Szyfruje dane algorytmem RijndaelManaged z użyciem klucza 256 bitowego.</summary>
        /// <param name="dataToEncrypt">Tablica bajtów do zaszyfrowania.</param>
        /// <param name="password">Hasło. Hash hasła wyliczny jest 512 bitowym algorytmem hash-ującym SHA512.</param>
        /// <param name="iv">Wektor inicjalizacji. Rozmiar 128 bitów.</param>
        /// <param name="encryptedData">Zaszyfrowana tablica bajtów.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static void Encrypt(byte[] dataToEncrypt, string password, byte[] iv, out byte[] encryptedData)
        {
            byte[] key = GetKey(password, 32);
            Encrypt(dataToEncrypt, key, iv, out encryptedData);
        }

        /// <summary>Deszyfruje przekazaną tablicę bajtów.</summary>
        /// <param name="cipherData">Zaszyfrowana tablica bajtów.</param>
        /// <param name="key">Klucz deszyfrujący. Maksymalnie 256 bitów, minimalnie 128 bitów, przeskok o 64 bity.</param>
        /// <param name="iv">Wektor inicjalizacji. Rozmiar 128 bitów.</param>
        /// <param name="decryptedText">Odszyfrowany tekst.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static void Decrypt(byte[] cipherData, byte[] key, byte[] iv, out string decryptedText)
        {
            if (cipherData == null)
            {
                throw new System.ArgumentNullException("cipherData");
            }
            if (cipherData.Length < 1)
            {
                throw new System.ArgumentException("Argument 'cipherData' is empty.");
            }

            if (key == null)
            {
                throw new System.ArgumentNullException("key");
            }
            if (key.Length != 16 && key.Length != 22 && key.Length != 32)
            {
                throw new System.ArgumentException("Argument 'key' does not mach the block size for this algorithm.");
            }

            if (iv == null)
            {
                throw new System.ArgumentNullException("iv");
            }
            if (iv.Length != 16)
            {
                throw new System.ArgumentException("Argument 'iv' does not mach the block size for this algorithm.");
            }

            using (System.IO.MemoryStream streamToDecrypt = new System.IO.MemoryStream(cipherData))
            {
                using (System.IO.MemoryStream streamDecrypted = new System.IO.MemoryStream())
                {
                    Decrypt(streamToDecrypt, streamDecrypted, key, iv);
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(streamDecrypted))
                    {
                        decryptedText = sr.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>Deszyfruje przekazaną tablicę bajtów.</summary>
        /// <param name="cipherData">Zaszyfrowana tablica bajtów.</param>
        /// <param name="key">Klucz deszyfrujący. Maksymalnie 256 bitów, minimalnie 128 bitów, przeskok o 64 bity.</param>
        /// <param name="iv">Wektor inicjalizacji. Rozmiar 128 bitów.</param>
        /// <param name="decryptedData">Odszyfrowana tablica bajtów.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static void Decrypt(byte[] cipherData, byte[] key, byte[] iv, out byte[] decryptedData)
        {
            if (cipherData == null)
            {
                throw new System.ArgumentNullException("cipherData");
            }
            if (cipherData.Length < 1)
            {
                throw new System.ArgumentException("Argument 'cipherData' is empty.");
            }

            if (key == null)
            {
                throw new System.ArgumentNullException("key");
            }
            if (key.Length != 16 && key.Length != 22 && key.Length != 32)
            {
                throw new System.ArgumentException("Argument 'key' does not mach the block size for this algorithm.");
            }

            if (iv == null)
            {
                throw new System.ArgumentNullException("iv");
            }
            if (iv.Length != 16)
            {
                throw new System.ArgumentException("Argument 'iv' does not mach the block size for this algorithm.");
            }

            using (System.IO.MemoryStream streamToDecrypt = new System.IO.MemoryStream(cipherData))
            {
                using (System.IO.MemoryStream streamDecrypted = new System.IO.MemoryStream())
                {
                    Decrypt(streamToDecrypt, streamDecrypted, key, iv);
                    decryptedData = streamDecrypted.ToArray();
                }
            }
        }

        /// <summary>Deszyfruje przekazany strumień danych.</summary>
        /// <param name="cipherStream">Zaszyfrowany strumień danych.</param>
        /// <param name="decryptedStream">Odszyfrowany strumień bajtów.</param>
        /// <param name="key">Klucz deszyfrujący. Maksymalnie 256 bitów, minimalnie 128 bitów, przeskok o 64 bity.</param>
        /// <param name="iv">Wektor inicjalizacji. Rozmiar 128 bitów.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static void Decrypt(System.IO.Stream cipherStream, System.IO.Stream decryptedStream, byte[] key, byte[] iv)
        {
            if (cipherStream == null)
            {
                throw new System.ArgumentNullException("cipherStream");
            }
            if (cipherStream.CanRead == false)
            {
                throw new System.ArgumentException("Argument 'cipherStream' is not readable.");
            }

            if (decryptedStream == null)
            {
                throw new System.ArgumentNullException("decryptedStream");
            }
            if (decryptedStream.CanWrite == false)
            {
                throw new System.ArgumentException("Argument 'decryptedStream' is not writable.");
            }

            if (key == null)
            {
                throw new System.ArgumentNullException("key");
            }
            if (key.Length != 16 && key.Length != 22 && key.Length != 32)
            {
                throw new System.ArgumentException("Argument 'key' does not mach the block size for this algorithm.");
            }

            if (iv == null)
            {
                throw new System.ArgumentNullException("iv");
            }
            if (iv.Length != 16)
            {
                throw new System.ArgumentException("Argument 'iv' does not mach the block size for this algorithm.");
            }

            cipherStream.Position = 0;
            decryptedStream.Position = 0;

            System.Security.Cryptography.ICryptoTransform decryptor = null;

            try
            {
                using (System.Security.Cryptography.RijndaelManaged algoritmCrypt = new System.Security.Cryptography.RijndaelManaged())
                {
                    decryptor = algoritmCrypt.CreateDecryptor(key, iv);
                    using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(
                        cipherStream, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                    {
                        cs.CopyTo(decryptedStream);
                        decryptedStream.Position = 0;
                    }
                }
            }
            finally
            {
                if (decryptor != null)
                {
                    decryptor.Dispose();
                }
            }
        }

        /// <summary>Deszyfruje dane zaszyfrowane algorytmem RijndaelManaged z użyciem klucza 256 bitowego.</summary>
        /// <param name="cipherData">Zaszyfrowana tablica bajtów.</param>
        /// <param name="password">Hasło. Hash hasła wyliczny jest 512 bitowym algorytmem hash-ującym SHA512.</param>
        /// <param name="iv">Wektor inicjalizacji. Rozmiar 128 bitów.</param>
        /// <param name="decryptedData">Odszyfrowana tablica bajtów.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static void Decrypt(byte[] cipherData, string password, byte[] iv, out byte[] decryptedData)
        {
            byte[] key = GetKey(password, 32);
            Decrypt(cipherData, key, iv, out decryptedData);
        }

        /// <summary>Deszyfruje dane zaszyfrowane algorytmem RijndaelManaged z użyciem klucza 256 bitowego.</summary>
        /// <param name="cipherData">Zaszyfrowana tablica bajtów.</param>
        /// <param name="password">Hasło. Hash hasła wyliczny jest 512 bitowym algorytmem hash-ującym SHA512.</param>
        /// <param name="iv">Wektor inicjalizacji. Rozmiar 128 bitów.</param>
        /// <param name="decryptedText">Odszyfrowany tekst.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Security.Cryptography.CryptographicException"></exception>
        public static void Decrypt(byte[] cipherData, string password, byte[] iv, out string decryptedText)
        {
            byte[] key = GetKey(password, 32);
            Decrypt(cipherData, key, iv, out decryptedText);
        }

        #endregion
    }
}
