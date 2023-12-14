using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public ShopManager shopManager;
    public void StartGame()
    {
        if (shopManager == null)
        {
            Debug.LogError("ShopManager referansı ayarlanmamış!");
            return;
        }
        Character selectedCharacter = shopManager.characters[shopManager.currentCharacterIndex];
        if (!selectedCharacter.isUnlocked)
        {
            Debug.Log("Henuz karakteri satin almadiniz");
            return; 
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenShop()
    {
        SceneManager.LoadScene("Shop");
    }
}
