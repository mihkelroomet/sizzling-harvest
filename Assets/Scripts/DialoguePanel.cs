using System.Collections;
using UnityEngine;
using TMPro;

public class DialoguePanel : MonoBehaviour
{
    public TMP_Text DialogueText;
    public string[] DialogueLines;
    public float DelayBetweenChars;
    public AudioClipGroup TextAudio;

    private void OnEnable()
    {
        StartCoroutine(GradualTextAppear());
    }

    IEnumerator GradualTextAppear()
    {
        for (int i = 0; i < DialogueLines.Length; i++)
        {
            bool skipped = false;
            string currentDialogueLine = DialogueLines[i];

            for (int j = 0; j < currentDialogueLine.Length; j++)
            {
                DialogueText.text = currentDialogueLine.Insert(j + 1, "</color>").Insert(0, "<color=white>");
                TextAudio.Play();
                yield return new WaitForSeconds(DelayBetweenChars);
                if (Input.GetButton("Submit") && j > 5)
                {
                    skipped = true;
                    break;
                }
            }
            
            if (!skipped)
            {
                yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
            }
        }

        Events.StartGame();
        this.gameObject.SetActive(false);
    }
}
