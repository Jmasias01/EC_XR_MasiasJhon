using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public static class ECFixValidation
{
    [MenuItem("EC/6 Corregir Project Validation")]
    public static void Fix()
    {
        PlayerSettings.runInBackground = true;
        Debug.Log("[EC] Run In Background activado");

        foreach (var t in new[] { NamedBuildTarget.Standalone, NamedBuildTarget.Android })
        {
            var defs = PlayerSettings.GetScriptingDefineSymbols(t);
            foreach (var d in new[] { "USE_INPUT_SYSTEM_POSE_CONTROL", "USE_STICK_CONTROL_THUMBSTICKS" })
                if (!defs.Contains(d)) defs = string.IsNullOrEmpty(defs) ? d : defs + ";" + d;
            PlayerSettings.SetScriptingDefineSymbols(t, defs);
        }
        Debug.Log("[EC] Defines OpenXR agregados");

        var type = System.Type.GetType("UnityEngine.XR.Interaction.Toolkit.InteractionLayerSettings, Unity.XR.Interaction.Toolkit");
        if (type != null)
        {
            var flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            var inst = type.GetProperty("Instance", flags)?.GetValue(null) as Object;
            var m = type.GetMethod("SetLayerNameAt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (inst != null && m != null)
            {
                m.Invoke(inst, new object[] { 31, "Teleport" });
                EditorUtility.SetDirty(inst);
                Debug.Log("[EC] Interaction Layer 31 = Teleport");
            }
            else Debug.LogWarning("[EC] No se pudo asignar la capa Teleport");
        }

        string[] candidates = {
            "Packages/com.unity.ugui/Package Resources/TMP Essential Resources.unitypackage",
            "Packages/com.unity.textmeshpro/Package Resources/TMP Essential Resources.unitypackage"
        };
        foreach (var c in candidates)
        {
            var full = Path.GetFullPath(c);
            if (File.Exists(full))
            {
                AssetDatabase.ImportPackage(full, false);
                Debug.Log("[EC] Importando TMP Essentials desde " + c);
                break;
            }
        }
        AssetDatabase.SaveAssets();
    }
}
