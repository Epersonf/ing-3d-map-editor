using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.FileSystem
{
    public enum FileSystemItemType
    {
        Directory,
        File,
        Model
    }

    public class FileSystemItem
    {
        public string Name { get; }
        public string Path { get; }
        public string RelativePath { get; }
        public FileSystemItemType Type { get; }
        public string Extension { get; }
        public bool IsExpanded { get; set; }
        public FileSystemItem Parent { get; set; }
        
        private static readonly string[] ModelExtensions = 
        {
            ".fbx", ".obj", ".blend", ".dae", ".3ds", 
            ".dxf", ".max", ".ma", ".mb", ".ply", ".stl"
        };

        public FileSystemItem(string fullPath, string relativePath = null)
        {
            Path = fullPath;
            Name = System.IO.Path.GetFileName(fullPath);
            RelativePath = relativePath ?? fullPath;
            
            if (Directory.Exists(fullPath))
            {
                Type = FileSystemItemType.Directory;
                Extension = string.Empty;
                IsExpanded = false;
            }
            else
            {
                Extension = System.IO.Path.GetExtension(fullPath).ToLower();
                Type = IsModelFile() ? FileSystemItemType.Model : FileSystemItemType.File;
            }
        }

        public bool IsModelFile()
        {
            if (Type == FileSystemItemType.Directory) return false;
            
            foreach (var ext in ModelExtensions)
            {
                if (Extension == ext) return true;
            }
            return false;
        }

        public FileSystemItem[] GetChildren()
        {
            if (Type != FileSystemItemType.Directory)
                return new FileSystemItem[0];

            try
            {
                var children = new List<FileSystemItem>();
                
                // Subdiretórios
                var dirs = Directory.GetDirectories(Path);
                foreach (var dir in dirs)
                {
                    var dirName = System.IO.Path.GetFileName(dir);
                    if (!dirName.StartsWith("."))
                    {
                        var relativePath = System.IO.Path.Combine(RelativePath, dirName);
                        var item = new FileSystemItem(dir, relativePath)
                        {
                            Parent = this
                        };
                        children.Add(item);
                    }
                }
                
                // Arquivos
                var files = Directory.GetFiles(Path);
                foreach (var file in files)
                {
                    var ext = System.IO.Path.GetExtension(file).ToLower();
                    if (ext != ".meta" && ext != ".cs" && ext != ".js" && ext != ".shader")
                    {
                        var fileName = System.IO.Path.GetFileName(file);
                        var relativePath = System.IO.Path.Combine(RelativePath, fileName);
                        var item = new FileSystemItem(file, relativePath)
                        {
                            Parent = this
                        };
                        children.Add(item);
                    }
                }
                
                return children.ToArray();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error reading directory {Path}: {e.Message}");
                return new FileSystemItem[0];
            }
        }

        public string GetIconClass()
        {
            return Type switch
            {
                FileSystemItemType.Directory => IsExpanded ? "folder-open" : "folder",
                FileSystemItemType.Model => "model-icon",
                _ => "file-icon"
            };
        }

        public string GetDisplayName()
        {
            if (Type == FileSystemItemType.Directory)
                return Name;
            
            return System.IO.Path.GetFileNameWithoutExtension(Name);
        }
    }
}
