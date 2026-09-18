using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PotionDrink : MonoBehaviour
{
    [HideInInspector] public GameObject hiddenText;
    [HideInInspector] public GameObject brick;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(OnActivated);
    }

    private void OnActivated(ActivateEventArgs args)
    {
        if (hiddenText != null)
        {
            hiddenText.SetActive(true);
        }
        if (brick != null)
        {
            BrickHighlight highlight = brick.GetComponent<BrickHighlight>();
            highlight.Show();
        }

        Destroy(gameObject);
    }
}
