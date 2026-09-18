using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PotionLock : MonoBehaviour
{
    public GameObject potionPrefab;
    public Transform potionSpawnPoint;
    public void OnIngredientInserted(SelectEnterEventArgs args)
    {
        GameObject ingredient = args.interactableObject.transform.gameObject;
        Destroy(ingredient);

        if (potionPrefab != null)
        {
            Transform spawnAt = potionSpawnPoint != null ? potionSpawnPoint : transform;
            Instantiate(potionPrefab, spawnAt.position, spawnAt.rotation);
        }

    }
}
