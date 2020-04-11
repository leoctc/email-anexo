using System;
using System.IO;
using System.Linq;
using System.Web;

namespace EmailComAnexo.Utils
{
    /// <summary>
    /// File Operations.
    /// </summary>
    public class FileOperations
    {
        /// <summary>
        /// Create the file.
        /// </summary>
        /// <param name="arq">The file.</param>
        /// <param name="dir">The dir.</param>
        /// <param name="nome">The nome.</param>
        /// <returns></returns>
        public static string CreateFile(HttpPostedFileBase arq, string dir, string nome = null)
        {
            if (arq != null && arq.ContentLength > 0)
            {
                CheckDir(dir);

                if (nome == null) nome = Guid.NewGuid().ToString("N");

                nome += Path.GetExtension(arq.FileName);

                string path = Path.Combine(dir, nome); //HostingEnvironment.MapPath(dir)

                arq.SaveAs(path);

                return nome;
            }
            else { return null; }
        }

        /// <summary>
        /// Checks if exists and create the directory.
        /// </summary>
        /// <param name="dir">The dir.</param>
        public static void CheckDir(string dir)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            if (!di.Exists) di.Create();
        }

        /// <summary>
        /// Determines whether [is directory empty] [the specified path].
        /// </summary>
        /// <param name="dir">The path.</param>
        /// <returns></returns>
        public static bool IsDirectoryEmpty(string dir) { return (GetFiles(dir).Count() > 0) ? false : true; }

        /// <summary>
        /// Delete the specified file.
        /// </summary>
        /// <param name="arq">The file.</param>
        /// <param name="dir">The dir.</param>
        /// <returns></returns>
        public static bool DeleteFile(string arq, string dir)
        {
            bool result = false;

            if (new DirectoryInfo(dir).Exists)
            {
                arq = Path.Combine(dir, arq); //HostingEnvironment.MapPath(dir)

                if (File.Exists(arq))
                {
                    File.Delete(arq);
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="arqDir">Diretório com o arquivo.</param>
        /// <returns></returns>
        public static bool DeleteFile(string arqDir)
        {
            bool result = false;

            if (File.Exists(arqDir))
            {
                File.Delete(arqDir);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Move a file.
        /// </summary>
        /// <param name="de">From.</param>
        /// <param name="para">To.</param>
        /// <param name="arq">The arq.</param>
        /// <returns></returns>
        public static bool MoveFile(string de, string para, string arq)
        {
            bool result = false;

            if (File.Exists(de + arq))
            {
                DirectoryInfo di = new DirectoryInfo(para);
                if (!di.Exists) di.Create();

                File.Move(de + arq, para + arq);

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Renames the file.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="nmOriginal">The original name.</param>
        /// <param name="nmNovo">The new name.</param>
        /// <returns></returns>
        public static bool RenameFile(string dir, string nmOriginal, string nmNovo)
        {
            bool result = false;

            if (File.Exists(dir + nmOriginal))
            {
                DirectoryInfo di = new DirectoryInfo(dir);

                if (di.Exists)
                {
                    string dirOrig = dir + nmOriginal;
                    string dirNova = dir + nmNovo;
                    File.Move(dirOrig, dirNova);

                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the file ext.
        /// </summary>
        /// <param name="arq">The arq.</param>
        /// <returns></returns>
        public static string GetFileExt(string arq) { return Path.GetExtension(arq); }

        /// <summary>
        /// Gets the name of the file without extension.
        /// </summary>
        /// <param name="arq">The arq.</param>
        /// <returns></returns>
        public static string GetFileName(string arq) { return Path.GetFileNameWithoutExtension(arq); }

        /// <summary>
        /// Gets the name of the file with extension.
        /// </summary>
        /// <param name="arq">The arq.</param>
        /// <returns></returns>
        public static string GetFullFileName(string arq) { return Path.GetFileName(arq); }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <returns></returns>
        public static string[] GetFiles(string dir)
        {
            CheckDir(dir);
            return Directory.GetFiles(Path.GetFullPath(dir));
        }
    }
}