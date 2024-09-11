using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    //public Image characterAvatar; // Opcional: para exibir avatar do personagem
    public GameObject dialogueBox;

    private Queue<Dialogue.DialogueLine> dialogueLinesQueue;
    private bool isDialogueActive = false;

    private CharacterController characterController;

    private Coroutine typingCoroutine;
    private bool isTextFullyDisplayed = false;
    private string currentSentence;

    [SerializeField]
    private float typingSpeed = 0.05f;

    void Start()
    {
        dialogueLinesQueue = new Queue<Dialogue.DialogueLine>();
        characterController = FindFirstObjectByType<CharacterController>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);
        isDialogueActive = true;
        dialogueLinesQueue.Clear();
        characterController.DisablePlayerControls();
        foreach (var line in dialogue.dialogueLines)
        {
            dialogueLinesQueue.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogueLinesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialogue.DialogueLine line = dialogueLinesQueue.Dequeue();
        characterNameText.text = line.characterName;
        currentSentence = line.dialogueText;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        isDialogueActive = false;
        characterController.EnablePlayerControls();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            if (isTextFullyDisplayed)
            {
                DisplayNextLine();
            }
            else
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                dialogueText.text = currentSentence;
                isTextFullyDisplayed = true;
            }
        }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTextFullyDisplayed = false;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTextFullyDisplayed = true;
    }
}
