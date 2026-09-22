using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GateController : MonoBehaviour
{
    public string nextSceneName = "Win_room";
    public NearFarInteractor leftHand;
    public NearFarInteractor rightHand;

    public void OnGateActivated()
    {
        ItemCrossScene.Instance.RecordItem(leftHand);
        ItemCrossScene.Instance.RecordItem(rightHand);
        ItemCrossScene.Instance.LoadSceneWithItem(nextSceneName);
    }
}
