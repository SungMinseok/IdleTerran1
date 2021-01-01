﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Tech{
    public string name;
    public GameObject btn;
    public Text mainText;
    public Text desText;
    public Text priceText;
    public float priceDefault;  //기본가격
    public float delta;
    public float priceDelta;
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    // void Awake(){
        
    //     if (instance != null)
    //     {
    //         Destroy(this.gameObject);
    //     }
    //     else
    //     {
    //         DontDestroyOnLoad(this.gameObject);
    //         instance = this;
    //     }
    // }
    public string btnClickSound ;
    public float totalTime = 86341f; //2 minutes
    
    [Header("상단 UI")]
    [SerializeField] public Slider fuelBar;
    public Text fuelPercentText;
    [SerializeField] public Text minText;
    [SerializeField] public Text rpText;
    [SerializeField] public Text timerText;
    [Header("하단 UI")]
    public GameObject buildLock;
    public Text statusInfoText;
    bool timerBtn;
    int second, minute, hour, day;
    public GameObject alertPopup;
    public Text alertPopupText;
    public GameObject[] lowerUI;
    [Header("센터")]
    public GameObject centerUI;
    [Header("Bay")]
    public GameObject bayUI;
    public Text[] upgradeTextArr;
    public Text totalLevel;
    [Header("센터")]
    public List<BoxCollider2D> boxes;

    public Texture2D characterTexture2D;

    [Header("빌딩 입장")]
    public GameObject[] buildings;
    public GameObject selectPanel;
    
    [Header("UI Bundle ( Fix Camera )")]
    public GameObject[] uis;

    [Header("공항")]
    public Tech[] teches_Starport;
    [Header("과학실")]
    public Tech[] teches_Science;
    // public Text fastCallMainText;
    // public Text fastCallDesText;
    // public Text moreSupplyMainText;
    // public Text moreSupplyDesText;
    
    void Awake(){
        instance = this;
        //StartTimer();
    //UpdateCharacterTexture();
    }
    void Start(){
        RefreshSciencePanel();
        RefreshStarportPanel();
    }

    // void Update(){
    //     if(timerBtn){
    //         // second = (int)(Time.time - curTime);
                        
    //         // if( second > 59)
    //         // {
    //         //     curTime = Time.time;
    //         //     second = 0;
    //         //     minute++;
               
    //         //     if( minute > 59)
    //         //     {
    //         //         minute = 0;
    //         //         hour++;
    //         //     }
    //         // }
           
    //         // timerText.text = string.Format("{0:00} : {1:00} : {2:00}", hour, minute, second);
    //     }
    // }
    public void ToggleAuto(){ 
        SoundManager.instance.Play("btn1");

        PlayerManager.instance.goTo = false;
        //if(PlayerManager.instance.isAuto || PlayerManager.instance.goToCenter) PlayerManager.instance.StopAuto();
        if(!PlayerManager.instance.isAuto) {
            
            PlayerManager.instance.YesSound();
        }
        else{
            
                PlayerManager.instance.StopAuto();
        }
        PlayerManager.instance.isAuto = !PlayerManager.instance.isAuto;
    }    
    public void TogglegoTo(string where){ 
        SoundManager.instance.Play("btn1");
        PlayerManager.instance.isAuto = false;
        //if(PlayerManager.instance.goToCenter||PlayerManager.instance.isAuto) PlayerManager.instance.StopAuto();
//목적지로 향할 때 버튼 클릭. 1. 같은 목적지 2. 다른 목적지
        if(PlayerManager.instance.goTo){
            if(PlayerManager.instance.destination.name == where){//같은 목적지 버튼일경우 멈춤

                PlayerManager.instance.StopAuto();

                //PlayerManager.instance.goTo = !PlayerManager.instance.goTo;    
            }
            else{
                PlayerManager.instance.orderType = OrderType.Enter;
                PlayerManager.instance.YesSound();
                PlayerManager.instance.destination = GameObject.Find(where).transform;
            }
        }
//목적지 없을 때
        else{
                PlayerManager.instance.StopAuto();
            
                PlayerManager.instance.orderType = OrderType.Enter;
            PlayerManager.instance.YesSound();

            PlayerManager.instance.destination = GameObject.Find(where).transform;
            
        PlayerManager.instance.goTo = !PlayerManager.instance.goTo;    
        }

        
        // PlayerManager.instance.StopAuto();

        // PlayerManager.instance.isAuto = false;
        // //if(PlayerManager.instance.goToCenter||PlayerManager.instance.isAuto) PlayerManager.instance.StopAuto();

        // // if(PlayerManager.instance.destination !=null){
        // //     if(PlayerManager.instance.destination.name == where){//같은 목적지 버튼일경우 멈춤

        // //     }
        // // }

        // if(!PlayerManager.instance.goTo) {
            
        //     PlayerManager.instance.YesSound();

        //     PlayerManager.instance.destination = GameObject.Find(where).transform;

        // }
        // PlayerManager.instance.goTo = !PlayerManager.instance.goTo;
    }

    public void ChargeFuel(){
        PlayerManager.instance.HandleFuel(1000f, false);
    }
    public void ChargeFullFuel(){

        PlayerManager.instance.HandleFuel(PlayerManager.instance.maxFuel, false);
        //PlayerManager.instance.HandleFuel(PlayerManager.instance.maxFuel);
        //PlayerManager.instance.curFuel = PlayerManager.instance.maxFuel;
    }

    public void StartTimer(){
        timerBtn = !timerBtn;
    }
 
    private void Update()
    {
        if(timerBtn){
            UpdateTimer(totalTime );
        }

        //if(QuestManager.instance.questOverList.Contains(3) || SettingManager.instance.testMode){

            if(PlayerManager.instance.goTo || BuildingManager.instance.ConstructingCheck()){
                buildLock.SetActive(true);
            }
            else{
                buildLock.SetActive(false);
            }
        //}
    }


    // public void UpdateTimer(float totalSeconds)
    // {     
    //     totalTime += Time.deltaTime*20;
    //     day = (int)(totalSeconds / 86400f) + 1;
    //     hour = (int)(totalSeconds / 3600f)%24;
    //     minute = (int)(totalSeconds / 60f)%60;
    //     //second = (int)totalSeconds % 60;
    //     timerText.text = string.Format("<color=red>Day</color> {0} <color=red>/</color> {1:00}:{2:00}", day, hour, minute); // <color=red>Day</color> 1 <color=red>/</color> 24:00
    // }
    public void UpdateTimer(float totalMinutes)
    {     
        totalTime += Time.deltaTime*2;
        day = (int)(totalMinutes / 1440f) + 1;
        hour = (int)(totalMinutes / 60f)%24;
        minute = (int)(totalMinutes)%60;
        //second = (int)totalSeconds % 60;
        timerText.text = string.Format("<color=red>Day</color> {0} <color=red>/</color> {1:00}:{2:00}", day, hour, minute); // <color=red>Day</color> 1 <color=red>/</color> 24:00
    }

/////////////////////////////////////////////센터
    public void ExitCenterBtn(){
        PlayerManager.instance.ExitCenter();
    }

    /////////////////콜라이더 설정
    
    public void EnableColliders(){
        foreach(BoxCollider2D col in boxes){
            if(col.tag!="RP"){

                col.isTrigger = false;
            }
        }
        boxes.Clear();
    }

    public void DisableColliders(){
        BoxCollider2D[] collider2Ds=FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
        foreach(BoxCollider2D col in collider2Ds){
            if(!col.isTrigger){
                boxes.Add(col);
                col.isTrigger = true;
            }
        }
    }
    
    public void ExitGameBtn(){
        Application.Quit();
    }

    public void RefreshEquipUI(){

        // for(int i=0; i<UpgradeManager.instance.upgradeList.Count; i++){
        //     upgradeTextArr[i].text = UpgradeManager.instance.upgradeList[i].te
            
        // }
    }
    public void EnterBuilding(){
        PlayerManager.instance.StopAuto();
        //GameObject temp = GameObject.Find(PlayerManager.instance.enterableBuilding + "Panel");
        for(int i=0; i<buildings.Length; i++){
            if(buildings[i].name == PlayerManager.instance.enterableBuilding + "Panel"){
                buildings[i].SetActive(true);
                //selectPanel.SetActive(false);
                return;        
            }
            
        }
        Debug.Log("없음");
        
    }
    public void ExitBuilding(){
        for(int i=0; i<buildings.Length; i++){
            if(buildings[i].activeSelf){
                buildings[i].SetActive(false);
                //ActivateSelectPanel();
            }
            // if(buildings[i].name == PlayerManager.instance.enterableBuilding + "Panel"){
            //     buildings[i].SetActive(true);
            //     selectPanel.SetActive(false);
                
            // }
        }
    }
    // public void ActivateSelectPanel(Collider2D collision =null){
    //     if(collision!=null){

    //         if((collision.tag == "Building" || collision.tag == "Center") && !selectPanel.activeSelf){
    //             selectPanel.SetActive(true);
    //             PlayerManager.instance.enterableBuilding = collision.name;
    //         }
    //     }
    //     else{
    //         if(!selectPanel.activeSelf){

    //                         selectPanel.SetActive(true);
    //         }
    //     }

    // }
    public bool OnUI(){  //UI켜져있으면 true
        for(int i=0;i<uis.Length;i++){
            if(uis[i].activeSelf){
                return true;
            }
        }
        return false;
    }
    public void SetPopUp(string _text, string _track = "transmission", float extraLength = 0){
        StartCoroutine(PopupCoroutine(_text, _track, extraLength));
    }
    IEnumerator PopupCoroutine(string _text, string _track, float extraLength){
        //alertPopup.GetComponent<Animator>().speed = 1f;
        SoundManager.instance.Play(_track);
        alertPopupText.text = _text;
        alertPopup.SetActive(false);
        alertPopup.SetActive(true);
        // if(extraLength!=0){
        //     alertPopup.GetComponent<Animator>().speed = 0f;
        //     yield return new WaitForSeconds(extraLength);
        //     alertPopup.GetComponent<Animator>().speed = 1f;

        // }
        // else{
        //     yield return null;
        // }
        yield return null;
    }
    public void ActivateLowerUIPanel(int num){
        lowerUI[num].transform.GetChild(2).gameObject.SetActive(false);
    }

    public void RefreshStarportPanel(){
        //초기 설정
        BuffManager.instance.buffs[2].coolTime = (3600-PlayerManager.instance.fastCall*60*teches_Starport[0].delta);

        teches_Starport[0].mainText.text = "신속 호출 " + PlayerManager.instance.fastCall + "단계";
        if(PlayerManager.instance.fastCall<20){
            teches_Starport[0].btn.SetActive(true);
            teches_Starport[0].desText.text = "보급선 호출 쿨타임 : "+(60-PlayerManager.instance.fastCall*teches_Starport[0].delta)+"분 > "+(60-(PlayerManager.instance.fastCall+1)*teches_Starport[0].delta)+"분";
            teches_Starport[0].priceText.text = string.Format("{0:#,###0}", teches_Starport[0].priceDefault + teches_Starport[0].priceDelta * PlayerManager.instance.fastCall);//(tempLevel*nowUpgradePanel.priceDelta));
        }
        else{

            teches_Starport[0].btn.SetActive(false);
            teches_Starport[0].desText.text = "보급선 호출 쿨타임 : "+(60-PlayerManager.instance.fastCall*teches_Starport[0].delta)+"분";
            teches_Starport[0].priceText.text = "N/A";
        }

        teches_Starport[1].mainText.text = "알찬 구성품 " + PlayerManager.instance.moreSupply + "단계";
        if(PlayerManager.instance.moreSupply<20){

            teches_Starport[1].btn.SetActive(true);
            teches_Starport[1].desText.text = "랜덤 보급품 획득시 보너스 추가 지급\n미네랄/연구점수 : +"+(PlayerManager.instance.moreSupply*5)+"% > +"+((PlayerManager.instance.moreSupply+1)*5)+"%\n스팀팩 : +"+(PlayerManager.instance.moreSupply)+"개 > +"+((PlayerManager.instance.moreSupply+1))+"개";
            teches_Starport[1].priceText.text = string.Format("{0:#,###0}", teches_Starport[1].priceDefault + teches_Starport[1].priceDelta * PlayerManager.instance.moreSupply);//(tempLevel*nowUpgradePanel.priceDelta));
        }
        else{

            teches_Starport[1].btn.SetActive(false);
            teches_Starport[1].desText.text = "랜덤 보급품 획득시 보너스 추가 지급\n미네랄/연구점수 : +"+(PlayerManager.instance.moreSupply*5)+"%\n스팀팩 : +"+(PlayerManager.instance.moreSupply*5)+"개";
            teches_Starport[1].priceText.text = "N/A";
        }
    }
    public float Pibo(float a, float b){
        return a*b;
    }
    public void UpgradeFastCall(){
        if(PlayerManager.instance.curMineral>float.Parse(teches_Starport[0].priceText.text)){
            UIManager.instance.SetPopUp("업그레이드 완료.","up");
            
            PlayerManager.instance.HandleMineral(-float.Parse(teches_Starport[0].priceText.text));
            PlayerManager.instance.fastCall ++;
            RefreshStarportPanel();
        }
        else{
            UIManager.instance.SetPopUp("미네랄이 부족합니다.","notenoughmin");

        }

    }
    public void UpgradeMoreSupply(){
        
        if(PlayerManager.instance.curMineral>float.Parse(teches_Starport[1].priceText.text)){
            UIManager.instance.SetPopUp("업그레이드 완료.","up");
            
            PlayerManager.instance.HandleMineral(-float.Parse(teches_Starport[1].priceText.text));
            PlayerManager.instance.moreSupply ++;
            RefreshStarportPanel();
        }
        else{
            UIManager.instance.SetPopUp("미네랄이 부족합니다.","notenoughmin");

        }
    }    


    public void RefreshSciencePanel(){
       //초기 설정
        //BuffManager.instance.buffs[2].coolTime = (3600-PlayerManager.instance.fastCall*60*teches_Starport[0].delta);

        teches_Science[0].mainText.text = "연구점수 획득";
        //if(PlayerManager.instance.fastCall<20){
            teches_Science[0].btn.SetActive(true);
            teches_Science[0].desText.text = "자동으로 축적되는 연구점수를 획득합니다.\n"+"(최대 5000, 분당 "+((PlayerManager.instance.investRP+1)*teches_Science[1].delta)+")";
            teches_Science[0].priceText.text = string.Format("{0:#,###0}",PlayerManager.instance.nowAccumulatedRP);
        //}
        //else{

            // teches_Science[0].btn.SetActive(false);
            // teches_Science[0].desText.text = "보급선 호출 쿨타임 : "+(60-PlayerManager.instance.fastCall*teches_Science[0].delta)+"분";
            // teches_Science[0].priceText.text = "N/A";
        //}

        teches_Science[1].mainText.text = "연구비 투자 " + PlayerManager.instance.investRP + "단계";
        if(PlayerManager.instance.investRP<20){

            teches_Science[1].btn.SetActive(true);
            teches_Science[1].desText.text = "연구점수를 더 많이 획득합니다.\n맵 : "+((PlayerManager.instance.investRP+1)*100)+" > "+((PlayerManager.instance.investRP+2)*100)+"\n과학시설 : "+((PlayerManager.instance.investRP+1)*100)+"/분 > "+((PlayerManager.instance.investRP+2)*100)+"/분";
            teches_Science[1].priceText.text = string.Format("{0:#,###0}", teches_Science[1].priceDefault + teches_Science[1].priceDelta * PlayerManager.instance.moreSupply);//(tempLevel*nowUpgradePanel.priceDelta));
        }
        else{

            teches_Science[1].btn.SetActive(false);
            teches_Science[1].desText.text = "연구점수를 더 많이 획득합니다.\n맵 : "+((PlayerManager.instance.investRP+1)*100)+"\n과학시설 : "+((PlayerManager.instance.investRP+1)*100)+"/분";
            teches_Science[1].priceText.text = "N/A";
        }
    }
    public void RPCollect(){
        StartCoroutine(RPCollectCoroutine());
    }
    IEnumerator RPCollectCoroutine(){
        if(PlayerManager.instance.maxAccumulatedRP>PlayerManager.instance.nowAccumulatedRP){
            PlayerManager.instance.nowAccumulatedRP += (PlayerManager.instance.investRP+1)*100;
            if(PlayerManager.instance.maxAccumulatedRP<PlayerManager.instance.nowAccumulatedRP){
                PlayerManager.instance.nowAccumulatedRP = PlayerManager.instance.maxAccumulatedRP;
            }
            teches_Science[0].priceText.text = string.Format("{0:#,###0}",PlayerManager.instance.nowAccumulatedRP);

        }
        yield return new WaitForSeconds(60f);
        StartCoroutine(RPCollectCoroutine());

    }
    public void GetRP(){
        PlayerManager.instance.HandleRP(PlayerManager.instance.nowAccumulatedRP);
        PlayerManager.instance.nowAccumulatedRP = 0;
    }
    public void UpgradeInvestRP(){
        
        if(PlayerManager.instance.curMineral>float.Parse(teches_Science[1].priceText.text)){
            UIManager.instance.SetPopUp("업그레이드 완료.","up");
            
            PlayerManager.instance.HandleMineral(-float.Parse(teches_Science[1].priceText.text));
            PlayerManager.instance.investRP ++;
            RefreshSciencePanel();
        }
        else{
            UIManager.instance.SetPopUp("미네랄이 부족합니다.","notenoughmin");

        }
    }
}