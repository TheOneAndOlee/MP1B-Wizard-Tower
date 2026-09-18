using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PotionLock : MonoBehaviour
{
    public GameObject potionPrefab;
    public Transform potionSpawnPoint;
    public GameObject hiddenText;
    public GameObject brick;
    public void OnIngredientInserted(SelectEnterEventArgs args)
    {
        GameObject ingredient = args.interactableObject.transform.gameObject;
        Destroy(ingredient);

        if (potionPrefab != null)
        {
            GameObject potion = Instantiate(potionPrefab, potionSpawnPoint.position, potionSpawnPoint.rotation);
            PotionDrink drink = potion.GetComponent<PotionDrink>();
            if (drink != null)
            {
                drink.hiddenText = hiddenText;
                drink.brick = brick;
            }
        }

    }
}
