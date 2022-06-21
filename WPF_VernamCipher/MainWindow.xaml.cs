using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace WPF_ColourNerve
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Encrypt_Click(object sender, RoutedEventArgs e)
        {
            string plainText = txt_PlainBox.Text;                  // get the plaintext message from the big TextBox.
            Console.WriteLine("PlainText = {0}", plainText);        // output plainText to Console.

            string key = GenerateKey(plainText);                    // use this subroutine to generate the KEY.
            WriteKey(key);

            string cipherText = EncryptMessage(plainText, key);     // get the cipherText from the Encryption operation.
            WriteFile(cipherText, "CipherText");
            txt_CipherBox.Text = cipherText;
        }

        private string GenerateKey(string plainText)
        {
            Random rnd = new Random();              // Declare a new 'Random' class that we can use.
            string key = "";                        // Declare a blank 'key' variable to use.

            // Generate the Random Key - as long as the plaintext message.
            for (int i = 0; i < plainText.Length; i++)
            {
                int RandNum = rnd.Next(33, 126);    // generate a random number between 33-126 (visible ASCII character.
                char RandChar = (char)RandNum;      // cast the random number to a character.

                key += RandChar;                    // add the random character to the 'key'
            }
            txt_Key.Text = key;
            Console.WriteLine("Key = {0}", key);    // output the key value

            return key;                             // return the 'key' value
        }

        private string EncryptMessage(string plainText, string key)
        {
            string cipherText = "";
            int count = 0;              // 'count' comparisons made (amount of characters encrypted)

            // Now ENCRYPT each character in the plainText string.
            for (int i = 0; i < plainText.Length; i++)
            {
                int vernamNum = plainText[i] ^ key[i];      // XOR the plainText characetr and 'key' value pair at index [i]
                char vernamChar = (char)vernamNum;          // Cast the XOR'd number to a character.

                cipherText += vernamChar;       // add in the newly encryptedChar to the cipherText string.
                count++;                        // add 1 to count of charcaters encrypted.
            }
            Console.WriteLine("Comparisons = {0}\nCipherText = {1}", count, cipherText);

            // MessageBox to output the cipherText to screen
            MessageBox.Show(cipherText, "Cipher Text:", MessageBoxButton.OK, MessageBoxImage.Information);

            return cipherText;
        }


        // ------------------------ File Handling -----------------------------------------
        private void WriteFile(string message, string name)
        {
            
            string filepath = String.Concat(@"Vernam - ", name, ".txt");       // declare filename of TEXT File.

            StreamWriter wFile;             // start up a 'StreamWriter' called wFile to output the message
            bool FileOK = true;

            try         // Exception Handling - in case the file path or name breaks.
            {
                wFile = new StreamWriter(File.Open(filepath, FileMode.Create));     // Create and Open new text file
                wFile.Write(message);
                wFile.Close();
            }
            catch
            {
                FileOK = false;
            }

            if (!FileOK)
                Console.WriteLine("File could not be opened.");     // Error message if file failed.
        }
        private void WriteKey(string key)
        {
            string filepath = @"Vernam - Key.txt";              // declare filename of KEY File.

            StreamWriter wFile;             // start up a 'StreamWriter' called wFile to output the message
            bool FileOK = true;

            try             // Exception Handling - in case the file path or name breaks.
            {
                wFile = new StreamWriter(File.Open(filepath, FileMode.Create));     // Create and Open new text file
                wFile.Write(key);
                wFile.Close();
            }
            catch
            {
                FileOK = false;
            }

            if (!FileOK)
                Console.WriteLine("File could not be opened.");     // Error message if file failed.
        }

        

        // ----------------------- DECRYPTING ------------------------------------------------------

        private void btn_Decrypt_Click(object sender, RoutedEventArgs e)
        {
            string cipherText = Read_CipherText_File();                 // get the plaintext message from the big TextBox.
            Console.WriteLine("CipherText = {0}", cipherText);          // output plainText to Console.

            string key = ReadKey();                     // use this subroutine to generate the KEY.
            Console.WriteLine("Key = {0}", key);        // output plainText to Console.

            string plainText = DecryptMessage(cipherText, key);     // get the cipherText from the Encryption operation.
            txt_PlainBox.Text = plainText;
            WriteFile(plainText, "Decrypted");
        }

        private string Read_CipherText_File()
        {
            string cipherText = "";

            try
            {
                OpenFileDialog openFile = new OpenFileDialog();                         // Create a new OpenFileDialog picker box
                openFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";       // Set to view Text files
                openFile.InitialDirectory = Directory.GetCurrentDirectory();            // default application folder
                openFile.Title = "Select CIPHERTEXT file...";

                if (openFile.ShowDialog() == true)
                    txt_CipherBox.Text = File.ReadAllText(openFile.FileName);           // Assign the values based on text file read

                cipherText = txt_CipherBox.Text;
            }
            catch
            {
                Console.WriteLine("File could not be opened.");
            }

            return cipherText;                             // return the 'cipherText' value
        }

        private string ReadKey()
        {
            string key = "";

            try
            {
                OpenFileDialog openFile = new OpenFileDialog();                         // Create a new OpenFileDialog picker box
                openFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";       // Set to view Text files
                openFile.InitialDirectory = Directory.GetCurrentDirectory();            // default application folder
                openFile.Title = "Select KEY file...";

                if (openFile.ShowDialog() == true)
                    txt_Key.Text = File.ReadAllText(openFile.FileName);                 // Assign the values based on text file read

                key = txt_Key.Text;
            }
            catch
            {
                Console.WriteLine("File could not be opened.");
            }

            return key;                             // return the 'key' value
        }
        
        private string DecryptMessage(string cipherText, string key)
        {
            string plainText = "";
            int count = 0;              // 'count' comparisons made (amount of characters encrypted)

            // Now DECRYPT each character in the plainText string.
            for (int i = 0; i < cipherText.Length; i++)
            {
                int vernamNum = cipherText[i] ^ key[i];      // XOR the cipherText character and 'key' value pair at index [i]
                char vernamChar = (char)vernamNum;          // Cast the XOR'd number to a character.

                plainText += vernamChar;       // add in the newly decryptedChar to the plainText string.
                count++;                       // add 1 to count of characters encrypted.
            }
            Console.WriteLine("Comparisons = {0}\nPlainText = {1}", count, plainText);

            // MessageBox to output the cipherText to screen
            MessageBox.Show(plainText, "Decrypted Text:", MessageBoxButton.OK, MessageBoxImage.Information);

            return plainText;
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)      // CLEAR ALL textboxes
        {
            txt_PlainBox.Text = "";
            txt_CipherBox.Text = "";
            txt_Key.Text = "";
        }
    }
}
