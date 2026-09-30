using UnityEditor;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features.Interactions;

public static class ECXRConfig
{
    [MenuItem("EC/3 Configurar OpenXR")]
    public static void Configure()
    {
        if (!AssetDatabase.IsValidFolder("Assets/XR")) AssetDatabase.CreateFolder("Assets", "XR");

        EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out XRGeneralSettingsPerBuildTarget perBT);
        if (perBT == null)
        {
            perBT = ScriptableObject.CreateInstance<XRGeneralSettingsPerBuildTarget>();
            AssetDatabase.CreateAsset(perBT, "Assets/XR/XRGeneralSettingsPerBuildTarget.asset");
            EditorBuildSettings.AddConfigObject(XRGeneralSettings.k_SettingsKey, perBT, true);
        }

        foreach (var group in new[] { BuildTargetGroup.Standalone, BuildTargetGroup.Android })
        {
            if (!perBT.HasManagerSettingsForBuildTarget(group))
                perBT.CreateDefaultManagerSettingsForBuildTarget(group);

            var mgr = perBT.ManagerSettingsForBuildTarget(group);
            bool ok = XRPackageMetadataStore.AssignLoader(mgr, "UnityEngine.XR.OpenXR.OpenXRLoader", group);
            Debug.Log("[EC] OpenXR loader asignado a " + group + ": " + ok);

            var openxr = OpenXRSettings.GetSettingsForBuildTargetGroup(group);
            if (openxr != null)
            {
                var touch = openxr.GetFeature<OculusTouchControllerProfile>();
                if (touch != null) touch.enabled = true;
                var khr = openxr.GetFeature<KHRSimpleControllerProfile>();
                if (khr != null) khr.enabled = true;
                EditorUtility.SetDirty(openxr);
            }
        }

        EditorUtility.SetDirty(perBT);
        AssetDatabase.SaveAssets();
        Debug.Log("[EC] Configuracion XR completada.");
    }
}
