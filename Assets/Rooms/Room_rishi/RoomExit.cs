using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Put on Osmond's exit interactable once its puzzle already controls access to that exit.
[RequireComponent(typeof(XRSimpleInteractable))]
public class RoomExit : MonoBehaviour
{
    public string nextScene = "RishiRoom";
    private void OnEnable() { GetComponent<XRSimpleInteractable>().selectEntered.AddListener(Go); }
    private void OnDisable() { GetComponent<XRSimpleInteractable>().selectEntered.RemoveListener(Go); }
    private void Go(SelectEnterEventArgs args) { SceneManager.LoadScene(nextScene); }
}
