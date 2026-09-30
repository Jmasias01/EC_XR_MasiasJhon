using System.IO;
using UnityEditor;
using UnityEngine;

public static class ECWriteDocs
{
    [MenuItem("EC/5 Generar README y gitignore")]
    public static void Write()
    {
        string root = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        Directory.CreateDirectory(Path.Combine(root, "Docs"));
        File.WriteAllText(Path.Combine(root, "README.md"), Readme);
        File.WriteAllText(Path.Combine(root, ".gitignore"), GitIgnore);
        Debug.Log("[EC] README.md y .gitignore escritos en " + root);
    }

    const string GitIgnore = @"/[Ll]ibrary/
/[Tt]emp/
/[Oo]bj/
/[Bb]uild/
/[Bb]uilds/
/[Ll]ogs/
/[Uu]ser[Ss]ettings/
/[Mm]emoryCaptures/
/[Rr]ecordings/
.vs/
.vscode/
.idea/
*.csproj
*.sln
*.suo
*.user
*.pidb
*.apk
*.aab
*.unitypackage
sysinfo.txt
";

    const string Readme = @"# XR Interaction Challenge - EC_XR_MasiasJhon

| | |
|---|---|
| **Estudiante** | Masias Jhon |
| **Codigo** | 2221897304 |
| **Curso** | Laboratorio de Realidad Extendida (XR) para Videojuegos |
| **Docente** | Victor Alejandro Arroyo Castro |

## Descripcion
Sala de entrenamiento XR construida en Unity con URP y XR Interaction Toolkit. El usuario puede agarrar y lanzar objetos, encender una lampara y cambiar colores con el rayo, usar un panel de UI espacial, teletransportarse y sumar puntos encestando objetos en una canasta.

Escena principal: `Assets/Scenes/EC_XR_MasiasJhon.unity`

## Funcionalidades implementadas
1. **Configuracion XR**: URP, XR Plug-in Management con OpenXR (PC y Android), XR Interaction Toolkit 3.6.1, XR Origin (XR Rig) de Starter Assets y XR Interaction Simulator para probar sin visor.
2. **Escenario**: piso, luz direccional + luz puntual, cuatro limites visuales (paredes azules), mesa, estante, pedestales, canasta y objetos interactivos.
3. **Objetos manipulables** (Rigidbody + XR Grab Interactable, con lanzamiento): `Cubo_Agarrable`, `Esfera_Agarrable`, `Herramienta_Agarrable`.
4. **Interaccion a distancia (rayo)**:
   - `Bombilla_Rayo`: XR Simple Interactable que enciende/apaga la luz de la lampara (script `LightToggle`).
   - `Cubo_Cambia_Color`: XR Simple Interactable que cambia de color (script `ColorChanger`).
5. **Reto libre**:
   - **UI espacial** (Canvas World Space + Tracked Device Graphic Raycaster) con botones: cambiar color, aparecer pelota, reiniciar puntos.
   - **Aparicion de objetos** (`ObjectSpawner`): crea pelotas agarrables.
   - **Contador** (`ScoreZone`): suma un punto por cada objeto que cae en la canasta.
   - **Teletransporte**: Teleportation Area sobre el piso.

## Controles
**Con visor (Meta Quest / OpenXR)**
- Grip: agarrar objetos (cerca o a distancia). Soltar el grip en movimiento: lanzar.
- Trigger apuntando con el rayo: activar lampara, cubo de color y botones de la UI.
- Joystick izquierdo: moverse. Joystick derecho: girar / teletransportarse.

**Sin visor (XR Interaction Simulator en el Editor)**
- Al presionar Play aparece el panel del simulador con todos los atajos de teclado.
- WASD: desplazarse. Mouse: mirar / mover el control seleccionado.
- Shift: cambiar la accion (trigger, grip...). Espacio: ejecutar la accion.

## Capturas
1. Vista general del escenario
![Escenario](Docs/captura1_escenario.png)
2. Configuracion XR / componentes en el Inspector
![Inspector](Docs/captura2_inspector.png)
3. Interaccion funcionando
![Interaccion](Docs/captura3_interaccion.png)

## Tecnologias y paquetes
- Unity 6 + Universal Render Pipeline (URP)
- XR Interaction Toolkit 3.6.1 (Starter Assets, XR Interaction Simulator)
- XR Plug-in Management + OpenXR (Oculus Touch y KHR Simple Controller profiles)
- Input System, TextMesh Pro
- C# (scripts en `Assets/Scripts`)

## Como abrir
1. Clonar el repositorio y abrirlo con Unity Hub (Unity 6).
2. Abrir `Assets/Scenes/EC_XR_MasiasJhon.unity`.
3. Presionar Play (usa el simulador si no hay visor conectado).
";
}
