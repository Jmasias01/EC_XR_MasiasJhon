using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public static class ECSetupSamples
{
    [MenuItem("EC/2 Importar samples XRI")]
    public static void ImportSamples()
    {
        var samples = Sample.FindByPackage("com.unity.xr.interaction.toolkit", null).ToList();
        foreach (var s in samples)
            Debug.Log("[EC] Sample disponible: " + s.displayName + " imported=" + s.isImported);

        foreach (var s in samples)
        {
            if (s.displayName == "Starter Assets" || s.displayName.Contains("Simulator"))
            {
                if (!s.isImported)
                {
                    bool ok = s.Import(Sample.ImportOptions.OverridePreviousImports);
                    Debug.Log("[EC] Importado " + s.displayName + ": " + ok);
                }
            }
        }
    }
}
