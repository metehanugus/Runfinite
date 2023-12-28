using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public int price;
    public bool isUnlocked;
    public AudioClip voiceClip; // Her karakter için ses kaydı
}

public class ShopManager : MonoBehaviour
{
    public int currentCharacterIndex;
    public GameObject[] characterModels;
    public Character[] characters;
    public Button buyButton;
    public AudioSource voiceSource; // Ses kayıtlarını oynatmak için AudioSource

    void Start()
    {
        currentCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        foreach (GameObject character in characterModels)
            character.SetActive(false);

        characterModels[currentCharacterIndex].SetActive(true);
        foreach (Character character in characters)
        {
            character.isUnlocked = PlayerPrefs.GetInt("CharacterUnlocked_" + character.id, character.isUnlocked ? 1 : 0) == 1;
        }
        UpdateBuyButtonVisibility();
        UpdateBuyButtonEvent();
        PlayVoiceClip(); // Başlangıçta karakter sesini oynat
    }

    public void ChangeNext()
    {
        characterModels[currentCharacterIndex].SetActive(false);

        currentCharacterIndex++;
        if (currentCharacterIndex == characterModels.Length)
            currentCharacterIndex = 0;

        characterModels[currentCharacterIndex].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", currentCharacterIndex);
        UpdateBuyButtonVisibility();
        UpdateBuyButtonEvent();
        PlayVoiceClip(); // Karakter değişikliği sırasında ses oynat
    }

    public void ChangePrevious()
    {
        characterModels[currentCharacterIndex].SetActive(false);

        currentCharacterIndex--;
        if (currentCharacterIndex == -1)
            currentCharacterIndex = characterModels.Length - 1;

        characterModels[currentCharacterIndex].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", currentCharacterIndex);
        UpdateBuyButtonVisibility();
        UpdateBuyButtonEvent();
        PlayVoiceClip(); // Karakter değişikliği sırasında ses oynat
    }

    // Karakter sesini oynatmak için yeni bir metod
    private void PlayVoiceClip()
    {
        if (currentCharacterIndex < 0 || currentCharacterIndex >= characters.Length)
            return;

        Character currentCharacter = characters[currentCharacterIndex];
        if (currentCharacter.voiceClip != null)
        {
            voiceSource.clip = currentCharacter.voiceClip;
            voiceSource.Play();
        }
    }

    public void PurchaseCharacter(int characterIndex)
    {
        // Karakter indeksinin doğru olduğundan emin ol
        if (characterIndex < 0 || characterIndex >= characters.Length)
        {
            Debug.LogError("Character index is out of range: " + characterIndex);
            return;
        }

        // Seçilen karakteri al
        Character characterToPurchase = characters[characterIndex];

        // Eğer karakter zaten açıksa veya yeterli para yoksa işlemi durdur
        if (characterToPurchase.isUnlocked)
        {
            Debug.Log("Bu karakter zaten açık.");
            return;
        }
        if (GameManager.inst.playerMoney < characterToPurchase.price)
        {
            Debug.Log("Yeterli paranız yok.");
            return;
        }

        // Paranın yeterli olduğunu kontrol et ve karakter için öde
        GameManager.inst.AddMoney(-characterToPurchase.price);
        characterToPurchase.isUnlocked = true;

        // Yalnızca satın alınan karakter için PlayerPrefs ayarını güncelle
        PlayerPrefs.SetInt("CharacterUnlocked_" + characterToPurchase.id, 1);

        // Diğer karakterlerin ayarlarını değiştirme
        PlayerPrefs.Save();
        UpdateBuyButtonVisibility();
        UpdateBuyButtonEvent();
    }


    public void UpdateBuyButtonVisibility()
    {
        if (currentCharacterIndex < 0 || currentCharacterIndex >= characters.Length)
        {
            Debug.LogError("Geçersiz karakter indeksi: " + currentCharacterIndex);
            return;
        }
        buyButton.gameObject.SetActive(!characters[currentCharacterIndex].isUnlocked);
    }

    public void UpdateBuyButtonEvent()
    {
        
        buyButton.onClick.RemoveAllListeners(); 
        buyButton.onClick.AddListener(() => PurchaseCharacter(currentCharacterIndex)); // Yeni event listener ekle
    }

}
