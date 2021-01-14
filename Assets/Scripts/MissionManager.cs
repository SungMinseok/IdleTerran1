using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public GameObject[] missionPanels;
    public GameObject[] missionHelpPanels;
    public string nowMission;
    [Header("Sync")]
    public Color[] colors;
    public int[] nums;
    public Transform imageParent;
    public Image[] images;
    public Text timerText;
    
    void Start(){
        images = new Image[imageParent.childCount];
        nums = new int[images.Length];
        //colors = new Color[imageParent.childCount];
        for(int i=0; i<imageParent.childCount; i++){
            images[i] = imageParent.GetChild(i).GetComponent<Image>();
        }
    }
    public void StartMission(string name){
        nowMission = name;
        switch(name){
            case "Sync" :
                missionPanels[0].SetActive(true);
                missionHelpPanels[0].SetActive(true);
                //SyncMission();
                break;
        }
    }
    public void SuccessMission(){

        switch(nowMission){
            case "Sync" :
                missionPanels[0].SetActive(false);
                int ranType = Random.Range(0,2);
                int randomAmount = Random.Range(1,4);
                if(ranType==0){
                    UIManager.instance.SetRewardPop("자동 연료 충전 "+randomAmount+"개 획득!", "AutoFuel", randomAmount);

                }
                else if(ranType==1){
                    UIManager.instance.SetRewardPop("자동 연구 점수 습득 "+randomAmount+"개 획득!", "AutoRP", randomAmount);

                }
                break;
        }
    }
    public void FailMission(){
        switch(nowMission){
            case "Sync" :
                missionPanels[0].SetActive(false);
                break;
        }
    }
    public void SyncMission(){
        for(int i=0; i<images.Length;i++){
            nums[i] = Random.Range(0,5);
            SetColor(i);
        }
        StartCoroutine(TimerCoroutine(10f));
    }
    IEnumerator TimerCoroutine(float time){
        float timer = time;
        while(timer>0){
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("N2");
            yield return new WaitForFixedUpdate();
        }
        timerText.text = "0.00";
        Debug.Log("시간 종료");
        FailMission();
    }
    public void SetColor(int j){
        images[j].color = colors[nums[j]];
    }
    public void ClickImage(int k){
        if(nums[k]<colors.Length-1)
            nums[k]++;
        else
            nums[k] = 0;

        SetColor(k);
        CheckAnswer();
        
    }
    public void CheckAnswer(){
        for(int i=1;i<images.Length;){
            if(nums[i-1]==nums[i]){
                i++;
            }
            else{
                return;
            }
        }
        Debug.Log("성공");
        SuccessMission();

    }
}
