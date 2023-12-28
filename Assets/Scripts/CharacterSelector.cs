using UnityEngine;


public class CharacterSelector : MonoBehaviour
{
    public int currentCharacterIndex;
    public GameObject[] characters;

    void Start()
    {
        currentCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        UpdateCharacterSelection(currentCharacterIndex);
    }

    public void UpdateCharacterSelection(int index)
    {
        // Önceki tüm karakterleri devre dışı bırak
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }

        // Yeni seçilen karakteri aktif et
        characters[index].SetActive(true);
        currentCharacterIndex = index;

    }
}
