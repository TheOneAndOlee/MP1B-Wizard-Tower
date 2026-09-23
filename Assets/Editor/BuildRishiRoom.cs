using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// The repaired scene is supplied as an actual .unity file. Do not regenerate it
// using the old builder, which would undo the verified scene references/layout.
public static class BuildRishiRoom
{
    private const string Path = "Assets/Rooms/Room_rishi/RishiRoom.unity";
    [MenuItem("Tools/Wizard Tower/Open repaired Rishi room")]
    public static void OpenRoom()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(Path);
    }
    [MenuItem("Tools/Wizard Tower/Check repaired Rishi room")]
    public static void CheckRoom()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != Path) { Debug.LogError("Open RishiRoom before checking it."); return; }
        var errors = new List<string>();
        RishiRoomProgress progress = null;
        DesktopRoomController desktop = null;
        var seals = new List<RishiSeal>();
        foreach (var root in scene.GetRootGameObjects())
        {
            foreach (var p in root.GetComponentsInChildren<RishiRoomProgress>(true)) progress = p;
            foreach (var p in root.GetComponentsInChildren<DesktopRoomController>(true)) desktop = p;
            seals.AddRange(root.GetComponentsInChildren<RishiSeal>(true));
        }
        if (progress == null) errors.Add("Missing RishiRoomProgress.");
        if (desktop == null || desktop.playerCamera == null || desktop.xrRig == null) errors.Add("Desktop camera or XR rig reference missing.");
        if (desktop != null && desktop.room != progress) errors.Add("Desktop room reference is incorrect.");
        if (progress != null)
        {
            if (progress.status == null) errors.Add("Missing progress board.");
            if (progress.hiddenScroll == null || progress.hiddenScroll.GetComponent<CollectScroll>() == null) errors.Add("Missing scroll or CollectScroll script.");
            if (progress.runeKey == null || progress.strengthKey == null || progress.hiddenKey == null) errors.Add("Missing key reference.");
            foreach (var seal in seals)
                if (seal.room != progress || seal.GetComponent<Collider>() == null) errors.Add("Missing reference/collider: " + seal.name);
        }
        if (errors.Count == 0) Debug.Log("RishiRoom reference checks passed. Play mode / headset behavior still needs testing.");
        else foreach (var error in errors) Debug.LogError(error);
    }
}
