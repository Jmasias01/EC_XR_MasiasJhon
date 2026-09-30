using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public static partial class ECSceneBuilder
{
    static GameObject floor;
    static ColorChanger changer;
    static ObjectSpawner spawner;
    static ScoreZone score;

    [MenuItem("EC/4 Construir escena EC_XR_MasiasJhon")]
    public static void Build()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        if (!AssetDatabase.IsValidFolder(MatFolder)) AssetDatabase.CreateFolder("Assets", "Materials");
        var matFloor = Mat("M_Piso", new Color(0.35f, 0.38f, 0.42f));
        var matWall = Mat("M_Limite", new Color(0.15f, 0.55f, 0.85f));
        var matWood = Mat("M_Madera", new Color(0.55f, 0.36f, 0.2f));
        var matRed = Mat("M_Rojo", new Color(0.9f, 0.2f, 0.2f));
        var matGreen = Mat("M_Verde", new Color(0.2f, 0.8f, 0.3f));
        var matYellow = Mat("M_Amarillo", new Color(0.95f, 0.8f, 0.1f));
        var matGrey = Mat("M_Gris", new Color(0.6f, 0.6f, 0.6f));
        var matBulb = Mat("M_Bombilla", new Color(1f, 0.9f, 0.4f));
        var matDisplay = Mat("M_CuboColor", Color.white);

        var sun = new GameObject("Directional Light");
        var sunL = sun.AddComponent<Light>();
        sunL.type = LightType.Directional; sunL.intensity = 1f; sunL.shadows = LightShadows.Soft;
        sun.transform.rotation = Quaternion.Euler(50, -30, 0);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.55f, 0.6f, 0.7f);
        RenderSettings.ambientEquatorColor = new Color(0.35f, 0.35f, 0.4f);
        RenderSettings.ambientGroundColor = new Color(0.2f, 0.2f, 0.2f);

        var env = new GameObject("Escenario");
        floor = Prim(PrimitiveType.Plane, "Piso", env.transform, Vector3.zero, Vector3.one, matFloor);
        var lim = new GameObject("Limites").transform; lim.SetParent(env.transform);
        Prim(PrimitiveType.Cube, "Limite_Norte", lim, new Vector3(0, 0.6f, 5), new Vector3(10, 1.2f, 0.2f), matWall);
        Prim(PrimitiveType.Cube, "Limite_Sur", lim, new Vector3(0, 0.6f, -5), new Vector3(10, 1.2f, 0.2f), matWall);
        Prim(PrimitiveType.Cube, "Limite_Este", lim, new Vector3(5, 0.6f, 0), new Vector3(0.2f, 1.2f, 10), matWall);
        Prim(PrimitiveType.Cube, "Limite_Oeste", lim, new Vector3(-5, 0.6f, 0), new Vector3(0.2f, 1.2f, 10), matWall);

        var props = new GameObject("Objetos").transform; props.SetParent(env.transform);
        Prim(PrimitiveType.Cube, "Mesa", props, new Vector3(0, 0.4f, 0.5f), new Vector3(2f, 0.8f, 0.8f), matWood);
        Prim(PrimitiveType.Cylinder, "Pedestal_Lampara", props, new Vector3(-2.5f, 0.5f, 1.5f), new Vector3(0.3f, 0.5f, 0.3f), matGrey);
        Prim(PrimitiveType.Cylinder, "Pedestal_Color", props, new Vector3(2.5f, 0.25f, 1.5f), new Vector3(0.6f, 0.25f, 0.6f), matGrey);
        Prim(PrimitiveType.Cube, "Estante", props, new Vector3(-4.3f, 0.75f, -2f), new Vector3(0.6f, 1.5f, 2f), matWood);

        var grab = new GameObject("Interactuables").transform;
        Grabbable(PrimitiveType.Cube, "Cubo_Agarrable", grab, new Vector3(-0.6f, 0.95f, 0.5f), Vector3.one * 0.15f, matRed);
        Grabbable(PrimitiveType.Sphere, "Esfera_Agarrable", grab, new Vector3(0f, 0.95f, 0.5f), Vector3.one * 0.15f, matGreen);
        var tool = Grabbable(PrimitiveType.Capsule, "Herramienta_Agarrable", grab, new Vector3(0.6f, 0.9f, 0.5f), new Vector3(0.06f, 0.1f, 0.06f), matYellow);
        tool.transform.rotation = Quaternion.Euler(0, 0, 90);

        var bulb = Prim(PrimitiveType.Sphere, "Bombilla_Rayo", grab, new Vector3(-2.5f, 1.2f, 1.5f), Vector3.one * 0.35f, matBulb);
        var lg = new GameObject("Luz_Lampara"); lg.transform.SetParent(bulb.transform, false);
        var lamp = lg.AddComponent<Light>();
        lamp.type = LightType.Point; lamp.range = 6f; lamp.intensity = 4f; lamp.color = new Color(1f, 0.85f, 0.5f);
        var toggle = bulb.AddComponent<LightToggle>();
        toggle.targetLight = lamp; toggle.bulbRenderer = bulb.GetComponent<Renderer>();
        var s1 = bulb.AddComponent<XRSimpleInteractable>();
        UnityEventTools.AddVoidPersistentListener(s1.selectEntered, toggle.Toggle);

        var display = Prim(PrimitiveType.Cube, "Cubo_Cambia_Color", grab, new Vector3(2.5f, 0.75f, 1.5f), Vector3.one * 0.5f, matDisplay);
        changer = display.AddComponent<ColorChanger>();
        changer.target = display.GetComponent<Renderer>();
        var s2 = display.AddComponent<XRSimpleInteractable>();
        UnityEventTools.AddVoidPersistentListener(s2.selectEntered, changer.NextColor);

        var basket = new GameObject("Canasta").transform; basket.SetParent(grab);
        var bp = new Vector3(0, 0, 2.8f); basket.position = bp;
        Prim(PrimitiveType.Cube, "Base", basket, bp + new Vector3(0, 0.05f, 0), new Vector3(0.8f, 0.1f, 0.8f), matGrey);
        Prim(PrimitiveType.Cube, "Pared_N", basket, bp + new Vector3(0, 0.3f, 0.4f), new Vector3(0.8f, 0.5f, 0.05f), matWall);
        Prim(PrimitiveType.Cube, "Pared_S", basket, bp + new Vector3(0, 0.3f, -0.4f), new Vector3(0.8f, 0.5f, 0.05f), matWall);
        Prim(PrimitiveType.Cube, "Pared_E", basket, bp + new Vector3(0.4f, 0.3f, 0), new Vector3(0.05f, 0.5f, 0.8f), matWall);
        Prim(PrimitiveType.Cube, "Pared_O", basket, bp + new Vector3(-0.4f, 0.3f, 0), new Vector3(0.05f, 0.5f, 0.8f), matWall);
        var zone = new GameObject("Zona_Puntos");
        zone.transform.SetParent(basket); zone.transform.position = bp + new Vector3(0, 0.3f, 0);
        var zc = zone.AddComponent<BoxCollider>(); zc.isTrigger = true; zc.size = new Vector3(0.7f, 0.4f, 0.7f);
        score = zone.AddComponent<ScoreZone>();

        var template = Grabbable(PrimitiveType.Sphere, "Pelota_Plantilla", grab, new Vector3(0, -5, 0), Vector3.one * 0.12f, matGreen);
        template.SetActive(false);
        var sp = new GameObject("Punto_Aparicion").transform;
        sp.SetParent(grab); sp.position = new Vector3(0.3f, 1.2f, 0.5f);
        spawner = new GameObject("Generador_Objetos").AddComponent<ObjectSpawner>();
        spawner.transform.SetParent(grab);
        spawner.template = template; spawner.spawnPoint = sp;

        BuildXRAndUI();
        SaveScene(scene);
    }
}
