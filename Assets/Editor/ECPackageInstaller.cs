using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public static class ECPackageInstaller
{
    static AddAndRemoveRequest request;

    [MenuItem("EC/1 Instalar paquetes XR")]
    public static void Install()
    {
        request = Client.AddAndRemove(new[]
        {
            "com.unity.xr.interaction.toolkit",
            "com.unity.xr.management",
            "com.unity.xr.openxr"
        }, null);
        EditorApplication.update += Progress;
        Debug.Log("[EC] Instalando XR Interaction Toolkit, XR Plugin Management y OpenXR...");
    }

    static void Progress()
    {
        if (!request.IsCompleted) return;
        EditorApplication.update -= Progress;
        if (request.Status == StatusCode.Success)
            Debug.Log("[EC] Paquetes XR instalados correctamente.");
        else
            Debug.LogError("[EC] Error instalando paquetes: " + request.Error.message);
    }
}
