using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public static partial class ECSceneBuilder
{
    const string ScenePath = "Assets/Scenes/EC_XR_MasiasJhon.unity";
    const string MatFolder = "Assets/Materials";
    const string SamplesRoot = "Assets/Samples/XR Interaction Toolkit";

    static Material Mat(string name, Color c)
    {
        string path = MatFolder + "/" + name + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m == null)
        {
            m = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            AssetDatabase.CreateAsset(m, path);
        }
        m.SetColor("_BaseColor", c);
        EditorUtility.SetDirty(m);
        return m;
    }

    static GameObject Prim(PrimitiveType t, string name, Transform parent, Vector3 pos, Vector3 scale, Material mat)
    {
        var go = GameObject.CreatePrimitive(t);
        go.name = name;
        if (parent != null) go.transform.SetParent(parent, true);
        go.transform.position = pos;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }

    static GameObject Grabbable(PrimitiveType t, string name, Transform parent, Vector3 pos, Vector3 scale, Material mat)
    {
        var go = Prim(t, name, parent, pos, scale, mat);
        var rb = go.AddComponent<Rigidbody>();
        rb.mass = 0.5f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        var g = go.AddComponent<XRGrabInteractable>();
        g.throwOnDetach = true;
        g.useDynamicAttach = true;
        return go;
    }

    static GameObject FindPrefab(string name)
    {
        var guids = AssetDatabase.FindAssets("t:Prefab", new[] { SamplesRoot });
        foreach (var guid in guids)
        {
            var p = AssetDatabase.GUIDToAssetPath(guid);
            if (System.IO.Path.GetFileNameWithoutExtension(p) == name)
                return AssetDatabase.LoadAssetAtPath<GameObject>(p);
        }
        return null;
    }

    static GameObject UIElem(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go;
    }

    static void Stretch(RectTransform r)
    {
        r.anchorMin = Vector2.zero; r.anchorMax = Vector2.one; r.offsetMin = Vector2.zero; r.offsetMax = Vector2.zero;
    }

    static Text Label(Transform parent, string name, string text, int size, Vector2 pos, Vector2 dim, Font font, FontStyle style)
    {
        var t = UIElem(name, parent).AddComponent<Text>();
        t.text = text; t.fontSize = size; t.font = font; t.fontStyle = style;
        t.alignment = TextAnchor.MiddleCenter; t.color = Color.white;
        t.rectTransform.anchoredPosition = pos; t.rectTransform.sizeDelta = dim;
        return t;
    }

    static Button MakeButton(Transform parent, string name, string text, Vector2 pos, Font font)
    {
        var go = UIElem(name, parent);
        var img = go.AddComponent<Image>();
        img.color = new Color(0.15f, 0.55f, 0.85f);
        var btn = go.AddComponent<Button>();
        var colors = btn.colors;
        colors.highlightedColor = new Color(0.6f, 0.85f, 1f);
        colors.pressedColor = new Color(0.1f, 0.3f, 0.5f);
        btn.colors = colors;
        var r = go.GetComponent<RectTransform>();
        r.anchoredPosition = pos; r.sizeDelta = new Vector2(480, 70);
        var lbl = Label(go.transform, "Texto", text, 28, Vector2.zero, new Vector2(480, 70), font, FontStyle.Bold);
        Stretch(lbl.rectTransform);
        return btn;
    }
}
