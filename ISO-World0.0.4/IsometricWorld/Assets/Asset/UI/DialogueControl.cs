using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CharacterDialogue
{
    public string id;
    public Sprite image;
    public string name;
    public string[] dialogues;
    public CharacterDialogue() { }
}

public class DialogueControl : MonoBehaviour
{

    public Image characterSprite;
    public Text characterName;
    public Text message;
    public Text buttonText;
    public GameObject NPC;
    public GameObject Player;
    public float talkDistance;

    public List<CharacterDialogue> characters = new List<CharacterDialogue>();

    public CharacterDialogue currentCharacter;
    private int currentLine = 0;



    public void OpenPanel(string id)
    {
        


            currentCharacter = FindDialogue(id);

            if (currentCharacter != null)
            {
                Debug.Log("char found");
                characterSprite.sprite = currentCharacter.image;
                characterName.text = currentCharacter.name;
                message.text = currentCharacter.dialogues[currentLine];

                if (currentCharacter.dialogues.Length > 1)
                {
                    buttonText.text = "Continue";
                }
                else
                {
                    buttonText.text = "Close";
                }
            }
        }
    

    public void NextMessage()
    {
        //Debug.Log(currentCharacter);

        if (currentCharacter == null)
            return;

        //Debug.Log("currentLine: " + currentLine);

        currentLine++;
        if(currentLine >= currentCharacter.dialogues.Length)
        {
            currentLine = 0;
            this.gameObject.SetActive(false);
        }
        else
        {
            message.text = currentCharacter.dialogues[currentLine];
        }
        

        if (currentLine >= currentCharacter.dialogues.Length-1)
        {
            buttonText.text = "Close";
        }
        else
        {
            buttonText.text = "Continue";
        }
      
    }

    private CharacterDialogue FindDialogue(string id)
    {
        for(int i = 0; i < characters.Count; i++)
        {
            if(characters[i].id == id)
            {
                return characters[i];
            }
        }
        return null;
    }

    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
    }
}
