using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileEncrypterW
{
    public partial class MainForm : Form
    {
        static byte[] password_bytes;
        private static readonly byte[] Salt = new byte[] { 10, 20, 30, 40, 50, 60, 70, 80 };


        static List<string[]> encrypted_files = new List<string[]>();
        static List<string> decrypted_file_paths = new List<string>();
        static List<byte[]> decrypted_file_contents = new List<byte[]>();
        static List<string> paths = new List<string>();
        static int deepness = -1;
        static string enc_path = @"";
        static Random random = new Random();
        static string temp_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\temp\";
        static string dec_path = temp_path + random.Next() + "\\";
        static MainForm this_;
        public MainForm()
        {
            InitializeComponent();
            this_ = this;

            Log("\t\tFileEncryptor by Unyxe\n\n");
            if (Directory.Exists(temp_path))
            {
                DeleteFolder(temp_path);
            }
        }

        private void encrypt_btn_Click(object sender, EventArgs e)
        {
            DeleteFolder(temp_path);
            encrypt_btn.Enabled = false;
            decrypt_btn.Enabled = false;
            ClearVars();
            dec_path = temp_path + random.Next() + "\\";
            if (Directory.Exists(dec_path))
            {
                Directory.Delete(dec_path, true);
            }
            Directory.CreateDirectory(dec_path);
            string path = path_txtbox.Text;
            if (path[path.Length - 1] != '\\')
            {
                path += "\\";
            }
            SetDeepness(path);
            password_bytes = CreateKey(password_txtbox.Text);
            Log("\nScanning your folder...");
            try
            {
                ScanFolder(path);
            }
            catch
            {
                Log("\nScanning failed! Re-check the path and the password.");
                encrypt_btn.Enabled = true;
                decrypt_btn.Enabled = true;
                return;
            }
            Log($"\nDone! {paths.Count} files were found.");
            Log("\nEncrypting your files...");
            try
            {
                foreach (string p in paths)
                {
                    //Console.WriteLine(p);
                    encrypted_files.Add(new string[] { EncryptPath(p), EncryptFile(p) });
                }

            }
            catch
            {
                Log("\nEncryption failed! Re-check the path and the password.");
                encrypt_btn.Enabled = true;
                decrypt_btn.Enabled = true;
                return;
            }
            Log($"\nDone! {encrypted_files.Count} files were encrypted.");
            enc_path = path;
            Log("\nOverwriting your folder with encrypted one...");
            WriteNewEncryptedFolder();
            Log($"\nDone! Your encrypted files are located on: {enc_path}");
            Process.Start(enc_path);
            encrypt_btn.Enabled = true;
            decrypt_btn.Enabled = true;
            DeleteFolder(temp_path);
        }

        private void decrypt_btn_Click(object sender, EventArgs e)
        {
            DeleteFolder(temp_path);
            encrypt_btn.Enabled = false;
            decrypt_btn.Enabled = false;
            ClearVars();
            dec_path = temp_path + random.Next() + "\\";
            if (Directory.Exists(dec_path))
            {
                Directory.Delete(dec_path, true);
            }
            Directory.CreateDirectory(dec_path);
            string path = path_txtbox.Text;
            if (path[path.Length - 1] != '\\')
            {
                path += "\\";
            }
            enc_path = path;
            SetDeepness(path);
            password_bytes = CreateKey(password_txtbox.Text);
            Log("\nScanning your folder...");
            try
            {
                ScanFolder(path);
            }
            catch
            {
                Log("\nScanning failed! Re-check the path and the password.");
                encrypt_btn.Enabled = true;
                decrypt_btn.Enabled = true;
                return;
            }
            Log($"\nDone! {paths.Count} files were found.");
            Log("\nDecrypting your files...");
            try
            {
                foreach (string p in paths)
                {
                    //Log(p);
                    decrypted_file_paths.Add(DecryptPath(p));
                    decrypted_file_contents.Add(DecryptFile(p));
                }
            }
            catch(DivideByZeroException ex)
            {
                Log("\nDecryption failed! Re-check the path and the password. " + ex.Message);
                encrypt_btn.Enabled = true;
                decrypt_btn.Enabled = true;
                return;
            }
            Log($"\nDone! {decrypted_file_paths.Count} files were decrypted.");
            Log("\nCopying decrypted files to the temporary location...");
            WriteNewDecryptedFolder();
            Log($"\nDone! Your decrypted files are located on: {dec_path}");
            Process.Start(dec_path);
            encrypt_btn.Enabled = true;
            decrypt_btn.Enabled = true;
        }

        static void Log(string message)
        {
            this_.log_txtbox.AppendText(message + Environment.NewLine + Environment.NewLine);
        }




        static void DeleteFolder(string path)
        {
            //Console.WriteLine(path);
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);

                try
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
                catch { return; }
                try
                {
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        try
                        {
                            DeleteFolder(dir.FullName);
                        }
                        catch { continue; }
                    }
                }
                catch { return; }
                try
                {
                    di.Delete(true);
                }
                catch { return; }
                DeleteFolder(path);
            }
            catch { return; }
        }
        static void ScanFolder(string path)
        {

            DirectoryInfo d = new DirectoryInfo(path);

            FileInfo[] Files = d.GetFiles("*.*");
            string str = "";

            foreach (FileInfo file in Files)
            {
                paths.Add(file.FullName);
            }
            string[] directories = Directory.GetDirectories(path);
            foreach (string dir in directories)
            {
                ScanFolder(dir);
            }
        }
        static void ClearVars()
        {
            paths.Clear();
            encrypted_files.Clear();
            decrypted_file_contents.Clear();
            decrypted_file_paths.Clear();
        }
        static void SetDeepness(string path)
        {
            deepness = path.Split('\\').Length - 1;
        }
        static int GetDeepness(string path)
        {
            return path.Split('\\').Length - 1;
        }
        static string EncryptPath(string path)
        {
            string parent_folder = enc_path;
            bool is_file = false;
            if (path[path.Length - 1] != '\\')
            {
                is_file = true;
                path += "\\";
            }
            //Console.WriteLine(is_file);
            string[] splitted = path.Split('\\');
            for (int i = GetDeepness(enc_path); i < splitted.Length - 1; i++)
            {
                //Console.WriteLine(splitted[i]);
                if (i == splitted.Length - 2)
                {
                    parent_folder += EncryptSymmetric(Encoding.ASCII.GetBytes(splitted[i]), password_bytes);
                    break;
                }
                parent_folder += EncryptSymmetric(Encoding.ASCII.GetBytes(splitted[i]), password_bytes) + "\\";

            }
            if (is_file)
            {
                return parent_folder;
            }
            return parent_folder + "\\";
        }
        static string DecryptPath(string path)
        {
            string parent_folder = enc_path;
            bool is_file = false;

            //Console.WriteLine(is_file);
            string[] splitted = path.Split('\\');
            //Console.WriteLine(enc_path);
            for (int i = GetDeepness(enc_path); i < splitted.Length; i++)
            {
                //Console.WriteLine(splitted[0]);
                if (i == splitted.Length - 1)
                {
                    parent_folder += Encoding.Default.GetString(DecryptSymmetric(splitted[i], password_bytes));
                    break;
                }
                parent_folder += Encoding.Default.GetString(DecryptSymmetric(splitted[i], password_bytes)) + "\\";

            }
            if (parent_folder[parent_folder.Length - 1] != '\\')
            {
                is_file = true;
            }
            return parent_folder;
        }
        static void WriteNewEncryptedFolder()
        {
            if (Directory.Exists(enc_path))
            {
                DeleteFolder(enc_path);
            }
            Directory.CreateDirectory(enc_path);
            foreach (string[] file in encrypted_files)
            {
                int attempt = 1;

                string dir_path = enc_path + GetDirectoryPath(file[0]);
                //Console.WriteLine(file[0] +"\t" + dir_path);
                string file_path = enc_path + GetFilePath(file[0]);
                if (!Directory.Exists(dir_path))
                {
                    Directory.CreateDirectory(dir_path);
                }
                while (true)
                {
                    try
                    {
                        File.WriteAllText(file_path, file[1]);
                        break;
                    }
                    catch (Exception e)
                    {
                        Log($"\nAttempt {attempt} failed! " + e.Message);
                        Log("Retrying...");

                    }
                    attempt++;
                }
            }
        }
        static void WriteNewDecryptedFolder()
        {
            if (Directory.Exists(dec_path))
            {
                DeleteFolder(dec_path);
            }
            Directory.CreateDirectory(dec_path);
            for (int i = 0; i < decrypted_file_paths.Count; i++)
            {
                string dir_path = dec_path + GetDirectoryPath(decrypted_file_paths[i]);
                string file_path = dec_path + GetFilePath(decrypted_file_paths[i]);
                if (!Directory.Exists(dir_path))
                {
                    Directory.CreateDirectory(dir_path);
                }
                File.WriteAllBytes(file_path, decrypted_file_contents[i]);
            }
        }

        static string GetDirectoryPath(string path)
        {
            string[] splitted = path.Split('\\');
            string directory_path = "";
            for (int i = deepness; i < splitted.Length - 1; i++)
            {
                directory_path += splitted[i] + @"\";
            }
            return directory_path;
        }
        static string GetDirectoryName(string path)
        {
            string[] splitted = path.Split('\\');
            return splitted[splitted.Length - 2];
        }
        static string GetDirectoryOrFileName(string path)
        {
            string[] splitted = path.Split('\\');
            return splitted[splitted.Length - 2];
        }
        static string GetFilePath(string path)
        {
            string[] splitted = path.Split('\\');
            string file_path = "";
            for (int i = deepness; i < splitted.Length; i++)
            {
                if (i == splitted.Length - 1)
                {
                    file_path += splitted[i];
                    break;
                }
                file_path += splitted[i] + @"\";
            }
            return file_path;
        }
        static string EncryptFile(string path)
        {
            return EncryptSymmetric(GetFileContent(path), password_bytes);
        }
        static byte[] DecryptFile(string path)
        {
            return DecryptSymmetric(Encoding.Default.GetString(GetFileContent(path)), password_bytes);
        }
        static byte[] GetFileContent(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            var bytes = default(byte[]);
            using (var memstream = new MemoryStream())
            {
                reader.BaseStream.CopyTo(memstream);
                bytes = memstream.ToArray();
            }
            reader.Close();
            return bytes;
        }


        public static string ToBase64(byte[] input)
        {
            return Convert.ToBase64String(input).Replace('/', '_').Replace('\\', '^');
        }
        public static byte[] FromBase64(string input)
        {
            return Convert.FromBase64String(input.Replace('_', '/').Replace('^', '\\'));
        }

        public static string EncryptSymmetric(byte[] data, byte[] key)
        {
            byte[] initializationVector = Encoding.ASCII.GetBytes("abcede0123456789");
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = initializationVector;
                var symmetricEncryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream as Stream, symmetricEncryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream as Stream))
                        {
                            streamWriter.BaseStream.Write(data, 0, data.Length);
                        }
                        return Convert.ToBase64String(memoryStream.ToArray()).Replace('/', '_').Replace('\\', '^');
                    }
                }
            }
        }
        public static byte[] DecryptSymmetric(string cipherText, byte[] key)
        {
            byte[] initializationVector = Encoding.ASCII.GetBytes("abcede0123456789");
            byte[] buffer = Convert.FromBase64String(cipherText.Replace('_', '/').Replace('^', '\\'));
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = initializationVector;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream as Stream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream as Stream))
                        {
                            var bytes = default(byte[]);
                            using (var memstream = new MemoryStream())
                            {
                                streamReader.BaseStream.CopyTo(memstream);
                                bytes = memstream.ToArray();
                            }
                            return bytes;
                        }
                    }
                }
            }
        }
        public static byte[] CreateKey(string password, int keyBytes = 32)
        {
            const int Iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, Salt, Iterations);
            return keyGenerator.GetBytes(keyBytes);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeleteFolder(temp_path);
        }
    }
}


/*
        static byte[] password_bytes;
        private static readonly byte[] Salt = new byte[] { 10, 20, 30, 40, 50, 60, 70, 80 };


        static List<string[]> encrypted_files = new List<string[]>();
        static List<string> decrypted_file_paths = new List<string>();
        static List<byte[]> decrypted_file_contents = new List<byte[]>();
        static List<string> paths = new List<string>();
        static int deepness = -1;
        static string enc_path = @"";
        static Random random = new Random();
        static string temp_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\temp\";
        static string dec_path = temp_path + random.Next() + "\\";
        static void Main(string[] args)
        {
            Console.WriteLine("\t\tFileEncryptor by Unyxe\n\n");

            if (Directory.Exists(temp_path))
            {
                DeleteFolder(temp_path);
            }
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            while (true)
            {
                ClearVars();
                dec_path = temp_path + random.Next() + "\\";
                if (Directory.Exists(dec_path))
                {
                    Directory.Delete(dec_path, true);
                }
                Directory.CreateDirectory(dec_path);
                Console.WriteLine("\nSelect mode (0 - encrypt, 1 - decrypt): ");
                int mode = Int32.Parse(Console.ReadLine());
                if (mode == 0)
                {
                    Console.WriteLine("\nEnter folder path you need to encrypt: ");
                    string path = Console.ReadLine();
                    if (path[path.Length - 1] != '\\')
                    {
                        path += "\\";
                    }
                    SetDeepness(path);
                    Console.WriteLine("\nEnter your password: ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    password_bytes = CreateKey(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nScanning your folder...");
                    try
                    {
                        ScanFolder(path);
                    }
                    catch 
                    {
                        Console.WriteLine("\nScanning failed! Re-check the path and the password.");
                        continue;
                    }
                    Console.WriteLine($"\nDone! {paths.Count} files were found.");
                    Console.WriteLine("\nEncrypting your files...");
                    try
                    {
                        foreach (string p in paths)
                        {
                            //Console.WriteLine(p);
                            encrypted_files.Add(new string[] { EncryptPath(p), EncryptFile(p) });
                        }
                        
                    } catch
                    {
                        Console.WriteLine("\nEncryption failed! Re-check the path and the password.");
                        continue;
                    }
                    Console.WriteLine($"\nDone! {encrypted_files.Count} files were encrypted.");
                    enc_path = path;
                    Console.WriteLine("\nOverwriting your folder with encrypted one...");
                    WriteNewEncryptedFolder();
                    Console.WriteLine($"\nDone! Your encrypted files are located on: {enc_path}");
                    Process.Start(enc_path);

                }
                else
                {
                    Console.WriteLine("\nEnter folder path you need to decrypt: ");
                    string path = Console.ReadLine();
                    if (path[path.Length - 1] != '\\')
                    {
                        path += "\\";
                    }
                    enc_path = path;
                    SetDeepness(path);
                    dec_path += GetDirectoryName(path) + '\\';
                    Console.WriteLine("\nEnter your password: ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    password_bytes = CreateKey(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nScanning your folder...");
                    try
                    {
                        ScanFolder(path);
                    }
                    catch
                    {
                        Console.WriteLine("\nScanning failed! Re-check the path and the password.");
                        continue;
                    }
                    Console.WriteLine($"\nDone! {paths.Count} files were found.");
                    Console.WriteLine("\nDecrypting your files...");
                    try
                    {
                        foreach (string p in paths)
                        {
                            //Console.WriteLine(p);
                            decrypted_file_paths.Add(DecryptPath(p));
                            decrypted_file_contents.Add(DecryptFile(p));
                        } 
                    }
                    catch
                    {
                        Console.WriteLine("\nDecryption failed! Re-check the path and the password.");
                        continue;
                    }
                    Console.WriteLine($"\nDone! {decrypted_file_paths.Count} files were decrypted.");
                    Console.WriteLine("\nCopying decrypted files to the temporary location...");
                    WriteNewDecryptedFolder();
                    Console.WriteLine($"\nDone! Your decrypted files are located on: {dec_path}");
                    Process.Start(dec_path);

                    while (true)
                    {
                        ClearVars();
                        Console.WriteLine("\nPress Enter to save changes (type - to exit to the main menu)");
                        string s = Console.ReadLine();
                        if (s == "")
                        {
                            Console.WriteLine("\nApplying changes...");
                            SetDeepness(dec_path);
                            ScanFolder(dec_path);
                            foreach (string p in paths)
                            {
                                //Console.WriteLine(p);
                                encrypted_files.Add(new string[] { EncryptPath(p), EncryptFile(p) });
                            }
                            WriteNewEncryptedFolder();
                            Console.WriteLine($"\nChanges successfully applied! {encrypted_files.Count} files were encrypted.");
                            continue;
                        }
                        else if (s == "-")
                        {
                            Console.WriteLine("\nExitting...");
                            DeleteFolder(temp_path);
                            break;
                        }
                    }
                }


            }
        }


        static void DeleteFolder(string path)
        {
            //Console.WriteLine(path);
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);

                try
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
                catch { return; }
                try
                {
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        try
                        {
                            DeleteFolder(dir.FullName);
                        }
                        catch { continue; }
                    }
                }
                catch { return; }
                try
                {
                    di.Delete(true);
                }
                catch { return; }
                DeleteFolder(path);
            }
            catch { return; }
        }
        static void ScanFolder(string path)
        {

            DirectoryInfo d = new DirectoryInfo(path);

            FileInfo[] Files = d.GetFiles("*.*");
            string str = "";

            foreach (FileInfo file in Files)
            {
                paths.Add(file.FullName);
            }
            string[] directories = Directory.GetDirectories(path);
            foreach (string dir in directories)
            {
                ScanFolder(dir);
            }
        }
        static void ClearVars()
        {
            paths.Clear();
            encrypted_files.Clear();
            decrypted_file_contents.Clear();
            decrypted_file_paths.Clear();
        }
        static void SetDeepness(string path)
        {
            deepness = path.Split('\\').Length - 1;
        }
        static int GetDeepness(string path)
        {
            return path.Split('\\').Length - 1;
        }
        static string EncryptPath(string path)
        {
            string parent_folder = enc_path;
            bool is_file = false;
            if (path[path.Length - 1] != '\\')
            {
                is_file = true;
                path += "\\";
            }
            //Console.WriteLine(is_file);
            string[] splitted = path.Split('\\');
            for (int i = GetDeepness(enc_path); i < splitted.Length - 1; i++)
            {
                //Console.WriteLine(splitted[i]);
                if (i == splitted.Length - 2)
                {
                    parent_folder += EncryptSymmetric(Encoding.ASCII.GetBytes(splitted[i]), password_bytes);
                    break;
                }
                parent_folder += EncryptSymmetric(Encoding.ASCII.GetBytes(splitted[i]), password_bytes) + "\\";

            }
            if (is_file)
            {
                return parent_folder;
            }
            return parent_folder + "\\";
        }
        static string DecryptPath(string path)
        {
            string parent_folder = enc_path;
            bool is_file = false;

            //Console.WriteLine(is_file);
            string[] splitted = path.Split('\\');
            for (int i = GetDeepness(enc_path); i < splitted.Length; i++)
            {
                //Console.WriteLine(splitted[i]);
                if (i == splitted.Length - 1)
                {
                    parent_folder += Encoding.Default.GetString(DecryptSymmetric(splitted[i], password_bytes));
                    break;
                }
                parent_folder += Encoding.Default.GetString(DecryptSymmetric(splitted[i], password_bytes)) + "\\";

            }
            if (parent_folder[parent_folder.Length - 1] != '\\')
            {
                is_file = true;
            }
            return parent_folder;
        }
        static void WriteNewEncryptedFolder()
        {
            if (Directory.Exists(enc_path))
            {
                DeleteFolder(enc_path);
            }
            Directory.CreateDirectory(enc_path);
            foreach (string[] file in encrypted_files)
            {
                int attempt = 1;

                string dir_path = enc_path + GetDirectoryPath(file[0]);
                //Console.WriteLine(file[0] +"\t" + dir_path);
                string file_path = enc_path + GetFilePath(file[0]);
                if (!Directory.Exists(dir_path))
                {
                    Directory.CreateDirectory(dir_path);
                }
                while (true)
                {
                    try
                    {
                        File.WriteAllText(file_path, file[1]);
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"\nAttempt {attempt} failed! " + e.Message);
                        Console.WriteLine("Retrying...");

                    }
                    attempt++;
                }
            }
        }
        static void WriteNewDecryptedFolder()
        {
            if (Directory.Exists(dec_path))
            {
                DeleteFolder(dec_path);
            }
            Directory.CreateDirectory(dec_path);
            for (int i = 0; i < decrypted_file_paths.Count; i++)
            {
                string dir_path = dec_path + GetDirectoryPath(decrypted_file_paths[i]);
                string file_path = dec_path + GetFilePath(decrypted_file_paths[i]);
                if (!Directory.Exists(dir_path))
                {
                    Directory.CreateDirectory(dir_path);
                }
                File.WriteAllBytes(file_path, decrypted_file_contents[i]);
            }
        }

        static string GetDirectoryPath(string path)
        {
            string[] splitted = path.Split('\\');
            string directory_path = "";
            for (int i = deepness; i < splitted.Length - 1; i++)
            {
                directory_path += splitted[i] + @"\";
            }
            return directory_path;
        }
        static string GetDirectoryName(string path)
        {
            string[] splitted = path.Split('\\');
            return splitted[splitted.Length - 2];
        }
        static string GetDirectoryOrFileName(string path)
        {
            string[] splitted = path.Split('\\');
            return splitted[splitted.Length - 2];
        }
        static string GetFilePath(string path)
        {
            string[] splitted = path.Split('\\');
            string file_path = "";
            for (int i = deepness; i < splitted.Length; i++)
            {
                if (i == splitted.Length - 1)
                {
                    file_path += splitted[i];
                    break;
                }
                file_path += splitted[i] + @"\";
            }
            return file_path;
        }
        static string EncryptFile(string path)
        {
            return EncryptSymmetric(GetFileContent(path), password_bytes);
        }
        static byte[] DecryptFile(string path)
        {
            return DecryptSymmetric(Encoding.Default.GetString(GetFileContent(path)), password_bytes);
        }
        static byte[] GetFileContent(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);
            var bytes = default(byte[]);
            using (var memstream = new MemoryStream())
            {
                reader.BaseStream.CopyTo(memstream);
                bytes = memstream.ToArray();
            }
            reader.Close();
            return bytes;
        }


        public static string ToBase64(byte[] input)
        {
            return Convert.ToBase64String(input).Replace('/', '_').Replace('\\', '^');
        }
        public static byte[] FromBase64(string input)
        {
            return Convert.FromBase64String(input.Replace('_', '/').Replace('^', '\\'));
        }

        public static string EncryptSymmetric(byte[] data, byte[] key)
        {
            byte[] initializationVector = Encoding.ASCII.GetBytes("abcede0123456789");
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = initializationVector;
                var symmetricEncryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream as Stream, symmetricEncryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream as Stream))
                        {
                            streamWriter.BaseStream.Write(data, 0, data.Length);
                        }
                        return Convert.ToBase64String(memoryStream.ToArray()).Replace('/', '_').Replace('\\', '^');
                    }
                }
            }
        }
        public static byte[] DecryptSymmetric(string cipherText, byte[] key)
        {
            byte[] initializationVector = Encoding.ASCII.GetBytes("abcede0123456789");
            byte[] buffer = Convert.FromBase64String(cipherText.Replace('_', '/').Replace('^', '\\'));
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = initializationVector;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream as Stream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream as Stream))
                        {
                            var bytes = default(byte[]);
                            using (var memstream = new MemoryStream())
                            {
                                streamReader.BaseStream.CopyTo(memstream);
                                bytes = memstream.ToArray();
                            }
                            return bytes;
                        }
                    }
                }
            }
        }
        public static byte[] CreateKey(string password, int keyBytes = 32)
        {
            const int Iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, Salt, Iterations);
            return keyGenerator.GetBytes(keyBytes);
        }


        static bool ConsoleEventCallback(int eventType)
        {
            DeleteFolder(temp_path);

            return false;
        }
        static ConsoleEventDelegate handler;
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
*/
