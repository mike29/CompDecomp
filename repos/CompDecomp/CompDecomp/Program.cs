using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Linq.Expressions;

namespace CompDecomp
{
    public class Program
    {
        private static string fileDirectory = @"c:\ICompress";
        private static Stopwatch sw = new Stopwatch();

        static void Main(string[] args)
        {
            

            DirectoryInfo di = new DirectoryInfo(fileDirectory);

           foreach (FileInfo fi in di.GetFiles())
            {
                // Compress all in file method call
               
                Compress(fi);

            }

            
            foreach (FileInfo fi in di.GetFiles(".gz"))
            {
                // Decompress all file to *.gz.
                Decompress(fi);

            }
        }

        public static void Compress(FileInfo fi)
        {

            sw.Start();
            using (FileStream inFile = fi.OpenRead())
            {
                //Controll compression 
                if ((File.GetAttributes(fi.FullName)
                     & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    // Create the file.
                    using (FileStream outFile =
                        File.Create(fi.FullName + ".gz"))
                    {
                        using (GZipStream Compress =
                            new GZipStream(outFile,
                                CompressionMode.Compress))
                        {
                            // Copy the source file into 
                            // the compression stream.
                            inFile.CopyTo(Compress);

                            if (fi.Name.Length > 0)
                            {
                                sw.Stop();
                                Console.WriteLine("Compressed Time {0} ", sw.ElapsedMilliseconds);
                                sw.Reset();
                            }
                          
                            Console.WriteLine("Compressed {0} from {1} to {2} ",
                                fi.Name, fi.Length.ToString(), outFile.Length.ToString());

                        }
                    }
                }
            }
        }

        public static void Decompress(FileInfo fi)
        {
            sw.Start();

            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Get original file extension, for example
                // "doc" from report.doc.gz.
                string curFile = fi.FullName;
                string origName = curFile.Remove(curFile.Length -
                                                 fi.Extension.Length);

                //Create the decompressed file.
                using (FileStream outFile = File.Create(origName))
                {
                    using (GZipStream Decompress = new GZipStream(inFile,
                        CompressionMode.Decompress))
                    {
                        // Copy the decompression stream 
                        // into the output file.
                        Decompress.CopyTo(outFile);

                        if (fi.Name.Length > 0)
                        {
                            sw.Stop();
                            Console.WriteLine("Decompressed Time {0} ", sw.ElapsedMilliseconds);
                            sw.Reset();

                        }

                        Console.WriteLine("Decompressed: {0}", fi.Name);

                    }
                }
            }
        }

    }


}
    
