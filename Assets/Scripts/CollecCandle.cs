using UnityEngine;
using TMPro;

public class CollecCandle : MonoBehaviour
{
    public TMP_Text scoreBoard;
    
    public void OnCandleGrabbed()
    {
        if (scoreBoard != null)
            scoreBoard.text = "Candle collected: 1/1";
        Destroy(gameObject);
    }
}
