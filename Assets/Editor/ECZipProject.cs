using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEngine;

public static class ECZipProject
{
    [MenuItem("EC/7 Empaquetar para GitHub")]
    public static void Zip()
    {
        AssetDatabase.SaveAssets();
        string root = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string zipPath = Path.Combine(root, "Temp", "EC_upload.zip");
        if (File.Exists(zipPath)) File.Delete(zipPath);
        int count = 0;
        using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
        {
            foreach (var dir in new[] { "Assets", "Packages", "ProjectSettings", "Docs" })
            {
                string full = Path.Combine(root, dir);
                if (!Directory.Exists(full)) continue;
                foreach (var f in Directory.GetFiles(full, "*", SearchOption.AllDirectories))
                {
                    string rel = f.Substring(root.Length + 1).Replace('\\', '/');
                    zip.CreateEntryFromFile(f, rel, System.IO.Compression.CompressionLevel.Optimal);
                    count++;
                }
            }
            foreach (var f in new[] { "README.md", ".gitignore", ".vsconfig" })
            {
                string full = Path.Combine(root, f);
                if (File.Exists(full)) { zip.CreateEntryFromFile(full, f); count++; }
            }
        }
        Debug.Log("[EC] ZIP creado: " + zipPath + " archivos=" + count + " bytes=" + new FileInfo(zipPath).Length);
    }
}
