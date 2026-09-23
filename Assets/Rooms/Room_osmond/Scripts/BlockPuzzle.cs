using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.Events;

public class BlockPuzzle : MonoBehaviour
{
    public UnityEvent puzzleComplete;
    
    [SerializeField] private Element[] correctOrder;

    [SerializeField] private BlockHolder socket1;
    [SerializeField] private BlockHolder socket2;
    [SerializeField] private BlockHolder socket3;
    [SerializeField] private BlockHolder socket4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckCorrectness()
    {
        if (socket1.element != correctOrder[0] || socket2.element != correctOrder[1] || socket3.element != correctOrder[2] || socket4.element != correctOrder[3])
        {
            return;
        }
        
        puzzleComplete.Invoke();
    }
}
