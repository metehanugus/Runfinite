using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public int price;
    public bool isUnlocked;
}

public class ShopManager : MonoBehaviour
{
    public int currentCharacterIndex;
    public GameObject[] characterModels;
    public Character[] characters;

    void Start()
    {
        currentCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        foreach(GameObject character in characterModels)
            character.SetActive(false);

        characterModels[currentCharacterIndex].SetActive(true);
        foreach (Character character in characters)
        {
            character.isUnlocked = PlayerPrefs.GetInt("CharacterUnlocked_" + character.id, character.isUnlocked ? 1 : 0) == 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeNext()
    {
        characterModels[currentCharacterIndex].SetActive(false);

        currentCharacterIndex++;
        if(currentCharacterIndex == characterModels.Length)
            currentCharacterIndex = 0;

        characterModels[currentCharacterIndex].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", currentCharacterIndex);
    }

    public void ChangePrevious()
    {
        characterModels[currentCharacterIndex].SetActive(false);

        currentCharacterIndex--;
        if (currentCharacterIndex == -1)
            currentCharacterIndex = characterModels.Length -1;

        characterModels[currentCharacterIndex].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", currentCharacterIndex);
    }

    public void PurchaseCharacter(int characterIndex)
    {
        // index kontrolu
        if (characterIndex < 0 || characterIndex >= characters.Length)
        {
            Debug.LogError("Character index is out of range: " + characterIndex);
            return;
        }
        Character characterToPurchase = characters[characterIndex];
        if (!characterToPurchase.isUnlocked && GameManager.inst.playerMoney >= characterToPurchase.price)
        {
            GameManager.inst.AddMoney(-characterToPurchase.price);
            characterToPurchase.isUnlocked = true;

            // kilit acildi kaydet
            PlayerPrefs.SetInt("CharacterUnlocked_" + characterToPurchase.id, characterToPurchase.isUnlocked ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

}
