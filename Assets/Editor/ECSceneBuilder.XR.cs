using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using UnityEngine.XR.Interaction.Toolkit.UI;

public static partial class ECSceneBuilder
{
    static void BuildXRAndUI()
    {
        new GameObject("XR Interaction Manager").AddComponent<XRInteractionManager>();
        var rigPrefab = FindPrefab("XR Origin (XR Rig)");
        GameObject rig = null;
        if (rigPrefab != null)
        {
            rig = (GameObject)PrefabUtility.InstantiatePrefab(rigPrefab);
            rig.transform.position = new Vector3(0, 0, -1.5f);
        }
        else Debug.LogError("[EC] No se encontro el prefab XR Origin (XR Rig).");

        var simPrefab = FindPrefab("XR Interaction Simulator");
        if (simPrefab == null) simPrefab = FindPrefab("XR Device Simulator");
        if (simPrefab != null) PrefabUtility.InstantiatePrefab(simPrefab);

        var tp = floor.AddComponent<TeleportationArea>();
        tp.interactionLayers = 1 << 31;

        var es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        es.AddComponent<XRUIInputModule>();

        var cgo = new GameObject("Panel_UI_Espacial");
        var canvas = cgo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        cgo.AddComponent<CanvasScaler>();
        cgo.AddComponent<TrackedDeviceGraphicRaycaster>();
        var rt = cgo.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(600, 460);
        rt.position = new Vector3(1.6f, 1.5f, 2.2f);
        rt.rotation = Quaternion.Euler(0, 25, 0);
        rt.localScale = Vector3.one * 0.002f;
        if (rig != null)
        {
            var cam = rig.GetComponentInChildren<Camera>();
            if (cam != null) canvas.worldCamera = cam;
        }

        var bg = UIElem("Fondo", cgo.transform).AddComponent<Image>();
        bg.color = new Color(0.08f, 0.1f, 0.15f, 0.9f);
        Stretch(bg.rectTransform);

        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        string title = "XR Interaction Challenge" + System.Environment.NewLine + "Masias Jhon";
        Label(cgo.transform, "Titulo", title, 34, new Vector2(0, 170), new Vector2(560, 100), font, FontStyle.Bold);
        score.scoreText = Label(cgo.transform, "Texto_Puntos", "Puntos: 0", 32, new Vector2(0, 85), new Vector2(560, 50), font, FontStyle.Normal);

        var b1 = MakeButton(cgo.transform, "Btn_CambiarColor", "Cambiar color del cubo", new Vector2(0, 10), font);
        UnityEventTools.AddPersistentListener(b1.onClick, changer.NextColor);
        var b2 = MakeButton(cgo.transform, "Btn_AparecerPelota", "Aparecer pelota", new Vector2(0, -80), font);
        UnityEventTools.AddPersistentListener(b2.onClick, spawner.Spawn);
        var b3 = MakeButton(cgo.transform, "Btn_Reiniciar", "Reiniciar puntos", new Vector2(0, -170), font);
        UnityEventTools.AddPersistentListener(b3.onClick, score.ResetScore);
    }

    static void SaveScene(Scene scene)
    {
        EditorSceneManager.SaveScene(scene, ScenePath);
        var list = EditorBuildSettings.scenes.Where(s => s.path != ScenePath).ToList();
        list.Insert(0, new EditorBuildSettingsScene(ScenePath, true));
        EditorBuildSettings.scenes = list.ToArray();
        AssetDatabase.SaveAssets();
        Debug.Log("[EC] Escena creada y guardada en " + ScenePath);
    }
}
