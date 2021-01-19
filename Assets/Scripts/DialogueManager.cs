
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dialogue{
    public Text[] vessels;
    public string[] sentences;
}
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;


    private List<string> listSentences;

    int count;

    private WaitForSeconds waitTime = new WaitForSeconds(0.02f);

    void Awake(){
        instance = this;
    }
    void Start(){
        listSentences = new List<string>();
    }
    public void ShowDialogue(Dialogue dialogue){
        
        for(int i=0;i<dialogue.sentences.Length;i++){
            listSentences.Add(dialogue.sentences[i]);
        }

        StartCoroutine(StartDialogueCoroutine());
        
    
    }
    IEnumerator StartDialogueCoroutine(bool mute = false, int mode = 0){
        yield return new WaitForFixedUpdate();
        
        //for(int i=0;i<listSentences[count])

    }


}
