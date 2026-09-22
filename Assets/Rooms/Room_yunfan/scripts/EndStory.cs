using UnityEngine;
using TMPro;

public class EndStory : MonoBehaviour
{
    public TMP_Text resultText;

    [TextArea] public string successEnding = "You gathered the recipe for the elixir and later successfully saved your sister by making it.";
    [TextArea] public string failEnding = "You didn't gather all the formulas for the elixir, so you continued on an adventure to find it.";

    void Start()
    {
        int total = CollectionManager.Instance.TotalCollected;
        int required = CollectionManager.RequiredNumber;

        string countText = $"You have collected a total of {total}/{required} scrolls";
        string endingText = (total >= required) ? successEnding : failEnding;

        if (resultText != null)
            resultText.text = countText + "\n\n" + endingText;
    }
}
