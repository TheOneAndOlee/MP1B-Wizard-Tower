using UnityEngine;
using System.Collections.Generic;

public class CollectionManager : MonoBehaviour
{
    public const int RequiredNumber = 3;

    private static CollectionManager _instance;
    public static CollectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject objectCM = new GameObject("CollectionManager");
                _instance = objectCM.AddComponent<CollectionManager>();
                DontDestroyOnLoad(objectCM);
            }
            return _instance;
        }
    }

    private readonly HashSet<string> collectedIds = new HashSet<string>();
    public int TotalCollected
    {
        get { return collectedIds.Count; }
    }

    public void CollectScroll(string scrollId)
    {
        collectedIds.Add(scrollId);
    }
    
    public bool IsCollected(string scrollId)
    {
        return collectedIds.Contains(scrollId);
    }
}
