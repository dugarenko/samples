using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace XmlSign
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();

            //RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider();
            //// Wygenerowanie klucza publicznego i prywatnego.
            //// Wywołanie metody z parametren 'false' wygeneruje tylko klucz publiczny.
            //string keys = rsaKey.ToXmlString(true);

            Sing();
            Verify();
        }

        private void Sing()
        {
            try
            {
                // Create a new RSA signing key and save it in the container.
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider();
                rsaKey.FromXmlString("<RSAKeyValue><Modulus>poFCZmV8QG/cabvdxeyBW9j678gPxzjRnzd0WdjHfndHoUN6Aa2nMgdKEl5HMVwipvvAY7WuNkodfxeh3UrkDnggBR7uFHJJ3KMA9HEd/VKyMcoMz+M4MR5L9soTShtgBKjjDNuJcl4uIg7SegMjQaO/Y+NUoux2V3/vUsv5MRk=</Modulus><Exponent>AQAB</Exponent><P>w30qCs2OzqrmJ7UQwVhk4XSWJqS5YRiddGY9SU8D+9nnOXwtc+mSYYUuzpxWqiNvQr8oz/7VaklQJpo/szoAZw==</P><Q>2gtb9MAnUaSMYlIJ+qZ4DJ4sVuX/j3saeislOsRlO35miTrVq/OfvRyO1/hUjSC++y0ljVzHGiGs0Ijobh5Sfw==</Q><DP>rjlWxtn8dGQLS0gr7qUBA44MY9RbAxYU/jBAXp11R3gkgy8Qs0VvmEpCNRFQi8GY3zvO+9B6E4fTTxQZwXnn8Q==</DP><DQ>sCuTXZnPauiPQHVWeLz9q/w0iPWF2ZC2INUxXF0ICdyjzebKcwcBHlOvmhGbhvdZNyoP+Dpo59Ujgs3LNgWr0w==</DQ><InverseQ>mmtU6LAUQIzsurqsgiH9GH61fd3l+4KjWl1EZFQ4n4RUX+cktqysHUm5U338f+uyXOqskg1xsFO5BBmlLVNugA==</InverseQ><D>mK5u9PaauXvZ4hsjghsdg9u0P6x0y3qOvjFbwAfI5275gCcf+eoDJx0ID0/keJ7EJ9sy0DwJRD8yTRielQ3XkPHhefCLrfH+bsv4UGqF4G5kxOqjX+egaoAZ111ZBiAT8uOd/k7Z5RBg7LGp+7d3F5KHvXTcpcNCpgJLNdTBLwE=</D></RSAKeyValue>");
                // Create a new XML document.
                XmlDocument xmlDoc = new XmlDocument();

                // Load an XML file into the XmlDocument object.
                //xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load("test.xml");

                // Sign the XML document.
                SignXml(xmlDoc, rsaKey);

                Console.WriteLine("XML file signed.");

                // Save the document.
                xmlDoc.Save("test_signed.xml");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void Verify()
        {
            try
            {
                // Create a new RSA signing key and save it in the container.
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider();
                // Klucz publiczny i prywatny.
                //rsaKey.FromXmlString("<RSAKeyValue><Modulus>poFCZmV8QG/cabvdxeyBW9j678gPxzjRnzd0WdjHfndHoUN6Aa2nMgdKEl5HMVwipvvAY7WuNkodfxeh3UrkDnggBR7uFHJJ3KMA9HEd/VKyMcoMz+M4MR5L9soTShtgBKjjDNuJcl4uIg7SegMjQaO/Y+NUoux2V3/vUsv5MRk=</Modulus><Exponent>AQAB</Exponent><P>w30qCs2OzqrmJ7UQwVhk4XSWJqS5YRiddGY9SU8D+9nnOXwtc+mSYYUuzpxWqiNvQr8oz/7VaklQJpo/szoAZw==</P><Q>2gtb9MAnUaSMYlIJ+qZ4DJ4sVuX/j3saeislOsRlO35miTrVq/OfvRyO1/hUjSC++y0ljVzHGiGs0Ijobh5Sfw==</Q><DP>rjlWxtn8dGQLS0gr7qUBA44MY9RbAxYU/jBAXp11R3gkgy8Qs0VvmEpCNRFQi8GY3zvO+9B6E4fTTxQZwXnn8Q==</DP><DQ>sCuTXZnPauiPQHVWeLz9q/w0iPWF2ZC2INUxXF0ICdyjzebKcwcBHlOvmhGbhvdZNyoP+Dpo59Ujgs3LNgWr0w==</DQ><InverseQ>mmtU6LAUQIzsurqsgiH9GH61fd3l+4KjWl1EZFQ4n4RUX+cktqysHUm5U338f+uyXOqskg1xsFO5BBmlLVNugA==</InverseQ><D>mK5u9PaauXvZ4hsjghsdg9u0P6x0y3qOvjFbwAfI5275gCcf+eoDJx0ID0/keJ7EJ9sy0DwJRD8yTRielQ3XkPHhefCLrfH+bsv4UGqF4G5kxOqjX+egaoAZ111ZBiAT8uOd/k7Z5RBg7LGp+7d3F5KHvXTcpcNCpgJLNdTBLwE=</D></RSAKeyValue>");
                // Klucz publiczny.
                rsaKey.FromXmlString("<RSAKeyValue><Modulus>poFCZmV8QG/cabvdxeyBW9j678gPxzjRnzd0WdjHfndHoUN6Aa2nMgdKEl5HMVwipvvAY7WuNkodfxeh3UrkDnggBR7uFHJJ3KMA9HEd/VKyMcoMz+M4MR5L9soTShtgBKjjDNuJcl4uIg7SegMjQaO/Y+NUoux2V3/vUsv5MRk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");

                // Create a new XML document.
                XmlDocument xmlDoc = new XmlDocument();

                // Load an XML file into the XmlDocument object.
                //xmlDoc.PreserveWhitespace = true;
                //xmlDoc.Load(@"D:\Projekty\GWW\documents\LicencjaINSIGHT.xml");
                xmlDoc.Load("test_signed.xml");

                // Verify the signature of the signed XML.
                Console.WriteLine("Verifying signature...");
                bool result = VerifyXml(xmlDoc, rsaKey);

                // Display the results of the signature verification to
                // the console.
                if (result)
                {
                    Console.WriteLine("The XML signature is valid.");
                }
                else
                {
                    Console.WriteLine("The XML signature is not valid.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Sign an XML file.
        /// This document cannot be verified unless the verifying code has the key with which it was signed.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="rsaKey"></param>
        private static void SignXml(XmlDocument xmlDoc, RSA rsaKey)
        {
            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException(nameof(xmlDoc));
            if (rsaKey == null)
                throw new ArgumentException(nameof(rsaKey));

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(xmlDoc);

            // Add the key to the SignedXml document.
            signedXml.SigningKey = rsaKey;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            //XmlDocumentFragment fragment = xmlDoc.CreateDocumentFragment();
            //fragment.InnerXml = xmlDoc.ImportNode(xmlDigitalSignature, true).InnerXml;
            //xmlDoc.DocumentElement.AppendChild(fragment);

            // Append the element to the XML document.
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
        }

        /// <summary>
        /// Verify the signature of an XML file against an asymmetric algorithm and return the result.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool VerifyXml(XmlDocument xmlDoc, RSA key)
        {
            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");
            if (key == null)
                throw new ArgumentException("key");

            // Create a new SignedXml object and pass it
            // the XML document class.
            SignedXml signedXml = new SignedXml(xmlDoc);

            // Find the "Signature" node and create a new
            // XmlNodeList object.
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("Signature");

            // Throw an exception if no signature was found.
            if (nodeList.Count <= 0)
            {
                throw new CryptographicException("Verification failed: No Signature was found in the document.");
            }

            // This example only supports one signature for
            // the entire XML document.  Throw an exception
            // if more than one signature was found.
            if (nodeList.Count >= 2)
            {
                throw new CryptographicException("Verification failed: More that one signature was found for the document.");
            }

            // Load the first <signature> node.
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.
            return signedXml.CheckSignature(key);
        }
    }
}
