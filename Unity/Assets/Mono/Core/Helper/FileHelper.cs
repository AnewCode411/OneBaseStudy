using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{
	public static class FileHelper
	{
		public static void GetAllFiles(List<string> files, string dir)
		{
			string[] fls = Directory.GetFiles(dir);
			foreach (string fl in fls)
			{
				files.Add(fl);
			}

			string[] subDirs = Directory.GetDirectories(dir);
			foreach (string subDir in subDirs)
			{
				GetAllFiles(files, subDir);
			}
		}
		
		public static void CleanDirectory(string dir)
		{
			foreach (string subdir in Directory.GetDirectories(dir))
			{
				Directory.Delete(subdir, true);		
			}

			foreach (string subFile in Directory.GetFiles(dir))
			{
				File.Delete(subFile);
			}
		}

		public static void CopyDirectory(string srcDir, string tgtDir)
		{
			DirectoryInfo source = new DirectoryInfo(srcDir);
			DirectoryInfo target = new DirectoryInfo(tgtDir);
	
			if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
			{
				throw new Exception("父目录不能拷贝到子目录！");
			}
	
			if (!source.Exists)
			{
				return;
			}
	
			if (!target.Exists)
			{
				target.Create();
			}
	
			FileInfo[] files = source.GetFiles();
	
			for (int i = 0; i < files.Length; i++)
			{
				File.Copy(files[i].FullName, Path.Combine(target.FullName, files[i].Name), true);
			}
	
			DirectoryInfo[] dirs = source.GetDirectories();
	
			for (int j = 0; j < dirs.Length; j++)
			{
				CopyDirectory(dirs[j].FullName, Path.Combine(target.FullName, dirs[j].Name));
			}
		}
		
		public static void ReplaceExtensionName(string srcDir, string extensionName, string newExtensionName)
		{
			if (Directory.Exists(srcDir))
			{
				string[] fls = Directory.GetFiles(srcDir);

				foreach (string fl in fls)
				{
					if (fl.EndsWith(extensionName))
					{
						File.Move(fl, fl.Substring(0, fl.IndexOf(extensionName)) + newExtensionName);
						File.Delete(fl);
					}
				}

				string[] subDirs = Directory.GetDirectories(srcDir);

				foreach (string subDir in subDirs)
				{
					ReplaceExtensionName(subDir, extensionName, newExtensionName);
				}
			}
		}
		
		public static bool CopyFile(string sourcePath, string targetPath, bool overwrite)
		{
			string sourceText = null;
			string targetText = null;

			if (File.Exists(sourcePath))
			{
				sourceText = File.ReadAllText(sourcePath);
			}

			if (File.Exists(targetPath))
			{
				targetText = File.ReadAllText(targetPath);
			}

			if (sourceText != targetText && File.Exists(sourcePath))
			{
				File.Copy(sourcePath, targetPath, overwrite);
				return true;
			}

			return false;
		}

		public static string GetFileName(string filePath)
        {
            var tpath = filePath.Replace('\\', '/');
            var index = tpath.LastIndexOf('/');
            if (index > 0) return tpath.Substring(index + 1);
            return filePath;
        }

		public static string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

		public static string GetDirName(string filePath)
        {
            var tpath = filePath.Replace('\\', '/');
            if (tpath.EndsWith("/")) tpath = tpath.Remove(tpath.Length - 1);
            var index = tpath.LastIndexOf('/');
            if (index > 0) return tpath.Substring(index + 1);
            return filePath;
        }

		public static void CheckAndCreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

		public static void DeleteDir(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                try
                {
                    DirectoryInfo[] childs = dir.GetDirectories();
                    foreach (DirectoryInfo child in childs)
                    {
                        child.Delete(true);
                    }
                    dir.Delete(true);
                }
                catch (Exception e)
                {
                    Log.Error("DeleteDir ERROR:" + e.Message);
                }
            }
        }

		public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception e)
                {
                    Log.Error("DeleteFile ERROR:" + e.Message);
                }
            }
        }

		public static void MoveFile(string sourPath, string destPath)
        {
            if (File.Exists(sourPath))
            {
                try
                {
                    File.Move(sourPath, destPath);
                }
                catch (Exception e)
                {
                    Log.Error("MoveFile ERROR:" + e.Message);
                }
            }
        }

		public static bool IsExistFile(string path)
        {
            return File.Exists(path);
        }

		public static bool RenameDir(string sourPath, string destPath)
        {
            if (System.IO.Directory.Exists(sourPath))
            {
                try
                {
                    if (Directory.Exists(destPath))
                    {
                        Directory.Delete(destPath, true);
                    }
                    System.IO.DirectoryInfo folder = new System.IO.DirectoryInfo(sourPath);
                    folder.MoveTo(destPath);
                    return true;
                }
                catch (Exception e)
                {
                    Log.Error("RenameDir ERROR:" + e.Message);
                    return false;
                }
            }
            return false;
        }

		public static string GetFileText(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string SaveByteToFile(string path, byte[] data, out int fileSize)
        {
            try
            {
                fileSize = data.Length;
                File.WriteAllBytes(path, data);
                return null;
            }
            catch (Exception e)
            {
                fileSize = 0;
                return e.Message;
            }
        }

        public static string SaveToFile(string path, string text)
        {
            try
            {
                File.WriteAllText(path, text);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string SaveToFile(string path, List<string> txtList)
        {
            try
            {
                File.WriteAllLines(path, txtList);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
	}
}
