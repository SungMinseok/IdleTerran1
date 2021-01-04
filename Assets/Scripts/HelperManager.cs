using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelperManager : MonoBehaviour
{
    public static HelperManager instance;
    public SoundManager theSound;
    public bool flag;
    public GameObject bundle;
    public GameObject[] helpers; //미네랄, 센터, 상단UI
    public GameObject[] arrows; //미네랄, 센터, 상단UI
    public GameObject finalDes;

    public GameObject alertPop;
    public Text alertText;

    [Header("New UI")]
    public GameObject helpPanel;
    public GameObject[] uis;
    public string[] helpStrings;
    public Text helpText;
    public bool callNext;
    void Awake(){
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.Play("ready");
        UIManager.instance.StartTimer();
        if(PlayerManager.instance.helperDone){
            bundle.SetActive(false);
        }
        else{
            bundle.SetActive(true);
        }
        //Invoke("PlayerManager.instance.GameStart",0.01f) ;
    }
    public void HelperOn(){
        StartCoroutine(HelperCoroutine());
    }
    public void HelperOk(){
        flag = false;
    }
    IEnumerator HelperCoroutine(){
        bundle.SetActive(true);
        SoundManager.instance.Play("transmission");
        helpers[0].SetActive(true);
        //arrows[0].SetActive(true);
        flag = true;
        yield return new WaitUntil(()=>!flag);
        helpers[0].SetActive(false);
        //arrows[0].SetActive(false);

        for(int i=1;i<=4;i++){
            
        SoundManager.instance.Play("transmission");
            helpers[i].SetActive(true);//하단 0번
            arrows[i].SetActive(true);
            flag = true;
            yield return new WaitUntil(()=>!flag);
            helpers[i].SetActive(false);
            arrows[i].SetActive(false);

        }

        
        SoundManager.instance.Play("transmission");

        finalDes.SetActive(true);
        flag = true;
        yield return new WaitUntil(()=>!flag);
        finalDes.SetActive(false);

        PlayerManager.instance.GameStart();
        // helpers[1].SetActive(true);//하단 0번
        // arrows[1].SetActive(true);
        // flag = true;
        // yield return new WaitUntil(()=>!flag);
        // helpers[1].SetActive(false);
        // arrows[1].SetActive(false);

        // helpers[2].SetActive(true);
        // arrows[2].SetActive(true);
        // flag = true;
        // yield return new WaitUntil(()=>!flag);
        // helpers[2].SetActive(false);
        // arrows[2].SetActive(true);
        
        // helpers[3].SetActive(true);
        // arrows[3].SetActive(true);
        // flag = true;
        // yield return new WaitUntil(()=>!flag);
        // helpers[3].SetActive(false);
        // arrows[3].SetActive(true);

        // helpers[4].SetActive(true);
        // arrows[4].SetActive(true);
        // flag = true;
        // yield return new WaitUntil(()=>!flag);
        // helpers[4].SetActive(false);
        // arrows[4].SetActive(true);



        bundle.SetActive(false);
    }

    public void SetPopUp(string _text){
        alertText.text = "미네랄 부족";
        alertPop.SetActive(false);
        alertPop.SetActive(true);
    }
    public void FirstStart(){
        
        PlayerManager.instance.helperDone = true;
        QuestManager.instance.SetQuest(0);
    }

    public void ShowHelp(){
        StartCoroutine(ShowHelpCoroutine());
        //helpText = helpStrings[
    }
    IEnumerator ShowHelpCoroutine(){
        helpPanel.SetActive(true);
        for(int i=0; i<uis.Length; i++){
            callNext = true;
            helpText.text = helpStrings[i];
            for(int j=0;j<uis.Length;j++){
                uis[j].SetActive(false);
            }    
            uis[i].SetActive(true);


            yield return new WaitUntil(()=>!callNext);
        }
        helpPanel.SetActive(false);
    }
    public void NextHelpBtn() => callNext = false;
    
}
