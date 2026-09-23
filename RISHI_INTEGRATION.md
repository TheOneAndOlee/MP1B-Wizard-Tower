# Rishi room integration

Copy the `Assets` folder from this overlay into the root of your team project, preserving paths. Do this in a fresh branch and keep your teammates' scene changes. Open the project in **Unity 6000.5.6f1**. Let Unity import all assets and resolve any Console errors before using the builder.

The GitHub ZIP provided for this task contains Git LFS pointer text in place of many `.fbx`, `.png`, and other binary files, including the scroll model. Start from a proper Git checkout with Git LFS installed, then run `git lfs pull` before importing the project in Unity. Do not use the extracted GitHub ZIP as your working Unity project.

## Generate the room and connect the start scene

1. In Unity, choose **Tools > Wizard Tower > Build Rishi room and scene order**. Confirm replacement if you previously generated the room. This command creates `Assets/Rooms/Room_rishi/RishiRoom.unity` and `Scroll_Rishi.prefab`.
2. It puts scenes in this order: `Start_room` > `OsmondRoom` > `RishiRoom` > `yunfan_room` > `Win_room`. It also changes the start button's existing `OnlyTeleport.sceneName` from `yunfan_room` to `OsmondRoom`.
3. Open `RishiRoom` and press Play. Select the blue seal to reveal the high key. Grab the three gold keys. Select the green exit seal after the gate opens. Grab the scroll at the far left side of the room. The scroll calls the existing `CollectScroll.OnGrabbed()` and records the unique ID `Scroll_Rishi` in the existing `CollectionManager`.

## Connect Osmond's exit

1. Open `OsmondRoom` and choose **the object players interact with to leave after its puzzle is solved** in the Hierarchy. Check with Osmond if the exit object is unclear. It must be reachable only after the room's puzzle is complete.
2. Choose **Tools > Wizard Tower > Wire selected Osmond exit**. The command adds a collider if needed, `XRSimpleInteractable`, and `RoomExit` to that object, then saves OsmondRoom. Selecting it loads `RishiRoom`.
3. If that exit already has its own interaction/transition callback, coordinate with Osmond and use that callback to call the existing `OnlyTeleport.Teleport()` with `sceneName = RishiRoom` instead. Avoid having both callbacks fire.
4. If Osmond has not placed a scroll, select a shelf or other location in `OsmondRoom` and choose **Tools > Wizard Tower > Place Osmond scroll at selection**. The command saves a separate `Scroll_Osmond.prefab` and places it slightly above the selected object. Move the instance to its final position in the Scene view. Do not place duplicates when rebuilding.

## Integration playthrough and limitations

Start at `Start_room`, solve Osmond's room, use its exit, collect the Rishi scroll and all keys, use Rishi's gate, collect Yunfan's scroll, and finish in `Win_room`. The success ending requires **three distinct scroll IDs**. Confirm the Osmond scroll was placed and can be grabbed. The current archive contains Yunfan's scroll prefab but does not contain an Osmond scroll placement.

The builder creates a playable baseline from cubes and XR interactions. Move the keys, clues, shelves, and materials in the generated scene to match your design. Running the builder again replaces that scene. The two transition scripts use ordinary scene loading; held items do **not** carry across these transitions. If the submission requires held items across scenes, integrate the team's `ItemCrossScene` system after agreeing on the item prefab IDs and per-hand behavior. The final story records scroll IDs without carrying scroll objects.

I could inspect the source files but could not run Unity or a headset in this environment. Test in the Editor and on Osmond's headset before recording. Commit the generated `.unity`, `.prefab`, `.mat`, and their `.meta` files along with these scripts after Unity creates them.
