using UnityEngine;
using UnityEngine.Events;

public class SequenceValidator : MonoBehaviour
{
    public string[] correctSequence;
    public UnityEvent Pass;
    private int progress = 0;

    public void RegisterStep(string stepId)
    {
        if (correctSequence[progress] == stepId)
        {
            progress++;
            if (progress >= correctSequence.Length)
            {
                Pass.Invoke();
                progress = 0;
            }
        }
        else
        {
            progress = 0;
        }
    }
}
