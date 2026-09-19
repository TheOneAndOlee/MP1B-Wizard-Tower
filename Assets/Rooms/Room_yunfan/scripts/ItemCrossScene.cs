using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ItemCrossScene : MonoBehaviour
{
    public static ItemCrossScene Instance;
    private string itemPrefabId;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RecordItem(NearFarInteractor hand)
    {
        if (hand.interactablesSelected.Count == 0) return;
        var interactable = hand.interactablesSelected[0] as MonoBehaviour;
        var carryable = interactable.GetComponent<CarryableItem>();
        if (carryable != null)
        {
            itemPrefabId = carryable.prefabId;
        }
    }

    public void LoadSceneWithItem(string sceneName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (string.IsNullOrEmpty(itemPrefabId)) return;

        GameObject prefab = Resources.Load<GameObject>(itemPrefabId);
        var point = GameObject.FindWithTag("ItemSpawnPoint");
        if (prefab != null && point != null)
            Instantiate(prefab, point.transform.position, point.transform.rotation);
        itemPrefabId = null;
    }
}
