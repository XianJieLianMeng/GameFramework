using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileTool 
{
    #region 文件与路径的增加删除创建

    #region 不忽视出错

    /// <summary>
    /// 判断有没有这个文件路径，如果没有则创建它（路径会去掉文件名）
    /// </summary>
    /// <param name="filepath">文件路径</param>
    public static void CreateFilePath(string filepath)
    {
        string newPathDir = Path.GetDirectoryName(filepath);

        CreatePath(newPathDir);
    }

    /// <summary>
    /// 判断有没有这个路径，如果没有则创建它
    /// </summary>
    /// <param name="path"></param>
    public static void CreatePath(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    /// <summary>
    /// 删除某个目录下的所有子目录和子文件，但是保留在这个目录
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteDirectory(string path)
    {
        string[] directories = Directory.GetDirectories(path);

        //删掉所有的子目录
        for (int i = 0; i < directories.Length; i++)
        {
            string pathTmp = directories[i];

            if(Directory.Exists(pathTmp))
            {
                Directory.Delete(pathTmp,true);
            }
        }

        //删掉所有子文件
        string[] files = Directory.GetFiles(path);

        for (int i = 0; i < files.Length; i++)
        {
            string pathTmp = files[i];

            if(Directory.Exists(pathTmp))
            {
                Directory.Delete(pathTmp);
            }
        }
    }

    /// <summary>
    /// 复制文件夹（及文件夹下所有子文件夹和文件）
    /// </summary>
    /// <param name="sourcePath">待复制的文件夹路径</param>
    /// <param name="destinationPath">目标路径</param>
    public static void CopyDiretory(string sourcePath,string destinationPath)
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        Directory.CreateDirectory(destinationPath);

        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);

            if (fsi is FileInfo)             //如果是文件，复制文件
                File.Copy(fsi.Name, destName);
            else                             //如果是文件夹，则新建文件夹，递归
            {
                Directory.CreateDirectory(destName);
                CopyDiretory(fsi.FullName, destName);
            }
        }
    }

    #endregion

    #region 忽视出错(会跳过所有出错的操作，一般用来无视权限)

    /// <summary>
    /// 删除所有可以删除的文件
    /// </summary>
    /// <param name="path"></param>
    public static void SafeDeleteDiretories(string path)
    {
        string[] diretories = Directory.GetDirectories(path);

        //删掉所有子目录
        for (int i = 0; i < diretories.Length; i++)
        {
            string pathTmp = diretories[i];

            if(Directory.Exists(pathTmp))
            {
                SafeDeleteDiretories(pathTmp);

                try
                {
                    Directory.Delete(pathTmp,false);
                }
                catch { }
            }
        }

        //删掉所有子文件
        string[] files = Directory.GetFiles(path);

        for (int i = 0; i < files.Length; i++)
        {
            string pathTmp = files[i];

            if(File.Exists(pathTmp))
            {
                try
                {
                    File.Delete(pathTmp);
                }
                catch { }
            }
        }
    }

    /// <summary>
    /// 复制所有可以复制的文件夹
    /// </summary>
    /// <param name="sourcePaht">待复制的文件夹路径</param>
    /// <param name="destinationPath">目标路径</param>
    public static void SafeCopyDirectories(string sourcePath,string destinationPath)
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        Directory.CreateDirectory(destinationPath);

        foreach (FileSystemInfo fsi in info.GetDirectories())
        {
            string destName = Path.Combine(destinationPath, fsi.Name);

            if(fsi is FileInfo)     //如果是文件，复制文件
            {
                try
                {
                    File.Copy(fsi.FullName, destName); 
                }
                catch { }
            }
            else                   //如果是文件夹，新建文件夹，递归
            {
                Directory.CreateDirectory(destName);
                SafeCopyDirectories(fsi.FullName, destName);
            }
        }
    }

    #endregion

    #region 文件名

    /// <summary>
    /// 移除拓展名
    /// </summary>
    /// <param name="name">e.g. account.txt</param>
    /// <returns></returns>
    public static string RemoveExpandName(string name)
    {
        if (Path.HasExtension(name))
            name = Path.ChangeExtension(name, null);
        return name;
    }

    /// <summary>
    /// 获取拓展名
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetExpandName(string name)
    {
        return Path.GetExtension(name);
    }

    /// <summary>
    /// 取出一个路径下的文件名
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFileNameByPath(string path)
    {
        FileInfo fi = new FileInfo(path);
        return fi.Name; //text.txt
    }

    /// <summary>
    /// 取出一个相对路径下的文件名
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFileNameBySring(string path)
    {
        string[] paths = path.Split('/');
        return paths[path.Length - 1];
    }

    /// <summary>
    /// 修改文件名
    /// </summary>
    /// <param name="path"></param>
    /// <param name="newName"></param>
    public static void ChangeFileName(string path,string newName)
    {
        if(System.IO.File.Exists(path))
        {
            System.IO.File.Move(path, newName);
        }
    }
    #endregion

    #region 文件编码

    /// <summary>
    /// 文件编码转换
    /// </summary>
    /// <param name="sourceFile">源文件</param>
    /// <param name="destFile">目标文件，如果为空，则覆盖源文件</param>
    /// <param name="targetEncoding">目标编码</param>
    public static void ConvertFileEncoding(string sourceFile,string destFile,System.Text.Encoding targetEncoding)
    {
        destFile = string.IsNullOrEmpty(destFile) ? sourceFile : destFile;
        //Encoding sourceEncoding = 
    }

    /// <summary>
    /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
    /// </summary>
    /// <param name="FILE_NAME">文件路径</param>
    /// <returns>文件的编码类型</returns>
    //public static Encoding GetEncodingType(string FILE_NAME)
    //{
    //    FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
    //    Encoding r = GetEncodingType()
    //}

    //public static Encoding GetEncodingType(FileStream fs)
    //{
    //    Encoding reVal = Encoding.Default;

    //    BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
    //    int i;
    //    int.TryParse(fs.Length.ToString(), out i);
    //    byte[] ss = r.ReadBytes(i);
    //    if()
    //}

    #endregion

    #endregion

}
