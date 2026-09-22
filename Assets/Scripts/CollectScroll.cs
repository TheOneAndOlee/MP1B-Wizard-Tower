using UnityEngine;
using TMPro;

public class CollectScroll : MonoBehaviour
{
    public string scrollId = "Scroll_Yunfan"; //Every one should have a different scrollId
    public TMP_Text scoreBoard;

    public void OnGrabbed()
    {
        CollectionManager.Instance.CollectScroll(scrollId);
        if (scoreBoard != null)
            scoreBoard.text = "Scroll collected: 1/1";
        Destroy(gameObject);
    }
}
