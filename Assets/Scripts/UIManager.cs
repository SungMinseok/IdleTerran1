﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public bool uiBlocked;
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
    [Header ("세팅")]
    public Transform settingGrid;
    GameObject[] toggleImage;
    public bool autoStimpack;
    public bool bgmState;
    public bool sfxState;
    public bool set_floating;
    public bool set_helpUI;
    [Header ("플로팅 텍스트")]
    public Transform ready_FT;
    public Transform  activated_FT;
    public Color mineralColor;
    public Color rpColor;
    [Header ("페이더")]
    public Animator fader;
    [Space]
    public string btnClickSound ;
    //public float totalTime = 86341f; //2 minutes
    public float playTime;
    public Color activatedColor;
    public Color normalColor;
    public Transform[] mineralsInMap;
    public GameObject debugPanel;
    
    [Header("상단 UI")]
    [SerializeField] public Slider fuelBar;
    public Text fuelPercentText;
    [SerializeField] public Text minText;
    [SerializeField] public Text rpText;
    [SerializeField] public Text timerText;
    public Text coinText;
    public Text minPerSecText;
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
    [Header("디팟")]
    public GameObject depotLock;
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
    [Header("사일로")]
    public GameObject nukePanel;
    public GameObject nukeInMap;
    
    [Header("에러창")]
    public GameObject errorPop;
    public Text errorPopText;
    // public Text errorPopOkText;
    
    [Header("선택창")]
    public GameObject selectPop;
    public Text selectPopText;
    
    [Header("리워드창")]
    public GameObject rewardPop;
    public Text rewardPopText;
    public Image rewardImage;
    public GameObject autoText;
    public Sprite[] sprites;
    // public Text selectPopOkText;
    // public Text selectPopCancelText;
    
    
    [Header("연구결과창")]
    public GameObject researchPanel;
    public GameObject recallImage;
    public GameObject successImage;
    public GameObject failImage;
    public GameObject okBtn;
    [Header("연구효율(0<=x<1)")]
    public float ptgBonus;
    void Awake(){
        instance = this;
        //StartTimer(); 
    //UpdateCharacterTexture();
#if DEV_MODE
        debugPanel.SetActive(true);
#else
        debugPanel.SetActive(false);
#endif
    }
    void Start(){
        toggleImage = new GameObject[settingGrid.childCount];

        for(int i=0;i<settingGrid.childCount;i++){
            int temp = i;
            toggleImage[i] = settingGrid.GetChild(i).GetChild(2).gameObject;
            settingGrid.GetChild(temp).GetComponent<Button>().onClick.AddListener(()=>ToggleSettings(temp));

        }





        RefreshSciencePanel();
        RefreshStarportPanel();

        SetSettings();
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
        AchvManager.instance.totalClick ++;
        AchvManager.instance.RefreshAchv(3);
    }
    public void ChargeFullFuel(){

        PlayerManager.instance.HandleFuel(PlayerManager.instance.maxFuel, false);
        //PlayerManager.instance.HandleFuel(PlayerManager.instance.maxFuel);
        //PlayerManager.instance.curFuel = PlayerManager.instance.maxFuel;
    }

    public void StartTimer(){
        timerBtn = !timerBtn;
    }
    IEnumerator CalculateMineralPerSecond(){
        long temp  = PlayerManager.instance.curMineral;
        yield return new WaitForSeconds(5f);
        minPerSecText.text = "+"+string.Format("{0:#,###0}",(PlayerManager.instance.curMineral- temp))+"/s";
        
        //Debug.Log(PlayerManager.instance.curMineral- temp);
    }
    private void Update()
    {
        //StartCoroutine(CalculateMineralPerSecond());

        // if(timerBtn){
        //     UpdateTimer(totalTime );
        // }

        //if(QuestManager.instance.questOverList.Contains(3) || SettingManager.instance.testMode){

            if(PlayerManager.instance.goTo || BuildingManager.instance.ConstructingCheck()){
                buildLock.SetActive(true);
            }
            else{
                buildLock.SetActive(false);
            }
        //}
    }
    // void FixedUpdate(){
    //     UpdateTimer();
    // }


    public void UpdateTimer()
    {     
        playTime += Time.deltaTime;
        //day = (int)(playTime / 86400f) + 1;
        hour = (int)(playTime / 3600f)%24;
        minute = (int)(playTime / 60f)%60;
        second = (int)playTime % 60;
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hour, minute,second); // <color=red>Day</color> 1 <color=red>/</color> 24:00
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
    // public void UpdateTimer(float totalMinutes)
    // {     
    //     totalTime += Time.deltaTime*2;
    //     day = (int)(totalMinutes / 1440f) + 1;
    //     hour = (int)(totalMinutes / 60f)%24;
    //     minute = (int)(totalMinutes)%60;
    //     //second = (int)totalSeconds % 60;
    //     timerText.text = string.Format("<color=red>Day</color> {0} <color=red>/</color> {1:00}:{2:00}", day, hour, minute); // <color=red>Day</color> 1 <color=red>/</color> 24:00
    // }

/////////////////////////////////////////////센터
    public void ExitCenterBtn(){
        PlayerManager.instance.ExitCenter();
    }

    /////////////////콜라이더 설정
    
    public void EnableColliders(){
        foreach(BoxCollider2D col in boxes){
            if(!col.CompareTag("RP")){

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
        //드랍쉽
        if(num==7) num = 6;
        

        lowerUI[num].transform.GetChild(2).gameObject.SetActive(false);
    }
    public void DeactivateLowerUIPanel(int num){
        lowerUI[num].transform.GetChild(2).gameObject.SetActive(true);
    }

    public void RefreshStarportPanel(){
        //초기 설정
        BuffManager.instance.buffs[2].coolTime = (3600-PlayerManager.instance.fastCall*60*teches_Starport[0].delta);
        
        //BuffManager.instance.buffs[2].remainingCoolTime = (3600-PlayerManager.instance.fastCall*60*teches_Starport[0].delta);

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

        teches_Starport[1].mainText.text = "알찬 보급품 " + PlayerManager.instance.moreSupply + "단계";
        BuffManager.instance.bonusLevelText_Main.text = "알찬 보급품 " + PlayerManager.instance.moreSupply + "단계";
        BuffManager.instance.bonusLevelText.text = "알찬 보급품 " + PlayerManager.instance.moreSupply + "단계";
        if(PlayerManager.instance.moreSupply<20){

            teches_Starport[1].btn.SetActive(true);
            teches_Starport[1].desText.text = "랜덤 보급품 획득시 보너스 추가 지급\n미네랄/연구점수 : +"+(PlayerManager.instance.moreSupply*5)+"% > +"+((PlayerManager.instance.moreSupply+1)*5)+"%\n스팀팩 : +"+(PlayerManager.instance.moreSupply)+"개 > +"+((PlayerManager.instance.moreSupply+1))+"개";
            teches_Starport[1].priceText.text = string.Format("{0:#,###0}", teches_Starport[1].priceDefault + teches_Starport[1].priceDelta * PlayerManager.instance.moreSupply);//(tempLevel*nowUpgradePanel.priceDelta));
        }
        else{

            teches_Starport[1].btn.SetActive(false);
            teches_Starport[1].desText.text = "랜덤 보급품 획득시 보너스 추가 지급\n미네랄/연구점수 : +"+(PlayerManager.instance.moreSupply*5)+"%\n스팀팩 : +"+(PlayerManager.instance.moreSupply)+"개";
            teches_Starport[1].priceText.text = "N/A";
        }

        BuffManager.instance.buffs[2].coolTime = 3600 - PlayerManager.instance.fastCall*teches_Starport[0].delta*60;

    }
    public float Pibo(float a, float b){
        return a*b;
    }
    public void UpgradeFastCall(){
        if(PlayerManager.instance.fastCall<20){
                
            if(PlayerManager.instance.curMineral>float.Parse(teches_Starport[0].priceText.text)){
                UIManager.instance.SetPopUp("업그레이드 완료.","up");
                
                PlayerManager.instance.HandleMineral(-(long)float.Parse(teches_Starport[0].priceText.text));
                PlayerManager.instance.fastCall ++;
                RefreshStarportPanel();
            }
            else{
                UIManager.instance.SetPopUp("미네랄이 부족합니다.","notenoughmin");

            }
        }

    }
    public void UpgradeMoreSupply(){
        if(PlayerManager.instance.moreSupply<20){
        
            if(PlayerManager.instance.curMineral>float.Parse(teches_Starport[1].priceText.text)){
                UIManager.instance.SetPopUp("업그레이드 완료.","up");
                //string.Format("{0:#,###0}",PlayerManager.instance.nowAccumulatedRP);
                //PlayerManager.instance.HandleMineral(-long.Parse(teches_Starport[1].priceText.text));
                PlayerManager.instance.HandleMineral(-(long)float.Parse(teches_Starport[1].priceText.text));
                PlayerManager.instance.moreSupply ++;
                RefreshStarportPanel();
            }
            else{
                UIManager.instance.SetPopUp("미네랄이 부족합니다.","notenoughmin");

            }
        }
    }    


    public void RefreshSciencePanel(){
       //초기 설정
        //BuffManager.instance.buffs[2].coolTime = (3600-PlayerManager.instance.fastCall*60*teches_Starport[0].delta);

        teches_Science[0].mainText.text = "연구점수 획득";
        //if(PlayerManager.instance.fastCall<20){
            teches_Science[0].btn.SetActive(true);
            teches_Science[0].desText.text = "자동으로 축적되는 연구점수를 획득합니다.\n"+"(최대 10500, 분당 "+((PlayerManager.instance.investRP+1)*teches_Science[1].delta)+")";
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
            teches_Science[1].priceText.text = string.Format("{0:#,###0}", teches_Science[1].priceDefault + teches_Science[1].priceDelta * PlayerManager.instance.investRP);//(tempLevel*nowUpgradePanel.priceDelta));
        }
        else{

            teches_Science[1].btn.SetActive(false);
            teches_Science[1].desText.text = "연구점수를 더 많이 획득합니다.\n맵 : "+((PlayerManager.instance.investRP+1)*100)+"\n과학시설 : "+((PlayerManager.instance.investRP+1)*100)+"/분";
            teches_Science[1].priceText.text = "N/A";
        }
    }
    public void RPCollect(){
        StartCoroutine(RPCollectCoroutine());
        Debug.Log("연구점수 자동수집");
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
        
        teches_Science[0].priceText.text = string.Format("{0:#,###0}",PlayerManager.instance.nowAccumulatedRP);
    }
    public void UpgradeInvestRP(){
        if(PlayerManager.instance.investRP<20){
            
            if(PlayerManager.instance.curMineral>float.Parse(teches_Science[1].priceText.text)){
                UIManager.instance.SetPopUp("업그레이드 완료.","up");
                
                PlayerManager.instance.HandleMineral(-(long)float.Parse(teches_Science[1].priceText.text));
                PlayerManager.instance.investRP ++;
                RefreshSciencePanel();
            }
            else{
                UIManager.instance.SetPopUp("미네랄이 부족합니다.","notenoughmin");

            }
            }
    }
    // public void SetColorUI(GameObject ui,string type){
    //     switch(type){
    //         case "Activate" :
    //             ui.GetComponent<Image>().color = 
    //             break;
    //     }
    // }
    public string popDes;
    public void ExitErrorPop(){
        switch(popDes){
            default :
                break;
        }
    }
    public void SetErrorPop(string des){
        errorPopText.text = des;
        popDes = des;
        
        errorPop.SetActive(true);

    }
    public void SetRewardPop(string des, string what, int amount = 1){
        rewardPopText.text = des;
        SoundManager.instance.Play("rescue");
        autoText.SetActive(false);
        switch(what){
            case "Box":
                rewardImage.sprite = sprites[0];
                BuffManager.instance.boxCount += amount;
                break;
            case "AutoFuel":
                autoText.SetActive(true);
                rewardImage.sprite = sprites[1];
                BuffManager.instance.buffs[4].count += amount;
                break;
            case "AutoRP":
                autoText.SetActive(true);
                rewardImage.sprite = sprites[2];
                BuffManager.instance.buffs[5].count += amount;
                BuffManager.instance.SetAutoRemainRP();
                break;
            case "Coin":
                rewardImage.sprite = sprites[3];
                PlayerManager.instance.curCoin += amount;
                //BuffManager.instance.SetAutoRemainRP();
                break;
            
        }

        
        BuffManager.instance.RefreshUICount();
        popDes = des;
        
        rewardPop.SetActive(true);

    }
    public void ArmNuke(Transform btn){
        if(PlayerManager.instance.curMineral>=99 && PlayerManager.instance.curRP>=99){
            btn.GetChild(btn.childCount-1).gameObject.SetActive(true);
            StartCoroutine(ArmNukeCoroutine());
        }
    }
    IEnumerator ArmNukeCoroutine(){
        UIManager.instance.SetPopUp("핵 미사일이 준비되었습니다. 잠시 후 핵 조준이 완료되면 핵을 발사할 수 있습니다.");
        yield return new WaitForSeconds(5f);
        UIManager.instance.SetPopUp("핵 발사 지점이 확보되었습니다. '핵 발사' 버튼을 눌러 핵을 발사하세요.");
        SoundManager.instance.Play("ghostaim");
        nukePanel.SetActive(true);
    }
    public void LaunchNuke(){


        StartCoroutine(LaunchNukeCoroutine());
    }
    IEnumerator LaunchNukeCoroutine(){
        nukeInMap.SetActive(true);
        SoundManager.instance.Play("nukelaunched");

        yield return new WaitForSeconds(1f);

        fader.gameObject.SetActive(true);
        fader.SetTrigger("out");

        yield return new WaitForSeconds(2f);
        SoundManager.instance.Play("nukedetected");
        
        yield return new WaitForSeconds(3f);

        DBManager.instance.CallSave(0);
        SceneManager.LoadScene("LaunchNuke");
        //nukePanel.SetActive(true);
    }
    public float OpenResearchPanel(){
        AchvManager.instance.totalResearch ++;
        AchvManager.instance.RefreshAchv(2);
        UIManager.instance.successImage.SetActive(false);
        UIManager.instance.failImage.SetActive(false);
        UIManager.instance.okBtn.SetActive(false);
        UIManager.instance.researchPanel.SetActive(true);
        UIManager.instance.recallImage.SetActive(true);
        SoundManager.instance.Play("recall");
        int ranPtg = Random.Range(0,10000);
        return ranPtg * 0.0001f;
    }
    public void ToggleAutoStim(){
        autoStimpack = !autoStimpack;
    }
    public void SetSettings(){
        //Default : off
        toggleImage[0].SetActive(autoStimpack);

        
        //Default : on(true)
        toggleImage[1].SetActive(bgmState);
        if(!bgmState){
            ToggleSettings(1);
            ToggleSettings(1);
        }
        //
        toggleImage[2].SetActive(sfxState);
        if(!sfxState){
            ToggleSettings(2);
            ToggleSettings(2);
        }
        //
        toggleImage[3].SetActive(set_floating);
        // if(!set_floating){
        //     ToggleSettings(3);
        // }
        toggleImage[4].SetActive(set_helpUI);
        if(!set_helpUI){
            for(int i=0;i<lowerUI.Length;i++){
                lowerUI[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(set_helpUI);
            }
        }

    }
    public void ToggleSettings(int num){
        //bool goState = !toggleImage[num].activeSelf;
        
        switch(num){
            case 0:
                autoStimpack = !autoStimpack;
                break;
            case 1:
                bgmState = !bgmState;
                SoundManager.instance.ToggleBGM();
                break;
            case 2:
                sfxState = !sfxState;
                SoundManager.instance.ToggleSound();
                break;
            case 3:
                set_floating = !set_floating;
                break;
            case 4:
                set_helpUI = !set_helpUI;
                for(int i=0;i<lowerUI.Length;i++){
                    //bool temp = lowerUI[0].transform.GetChild(0).GetChild(0).gameObject.activeSelf;
                    lowerUI[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(set_helpUI);
                }
                break;
        }
        
        toggleImage[num].SetActive(!toggleImage[num].activeSelf);
    }
    public void PrintFloating(string text, Transform _transform, Sprite sprite = null, int type = 0)//0: 미네랄, 1 : RP
    {

        if (text != "")
        {
            var clone = ready_FT.GetChild(0);
            clone.transform.position = new Vector2(_transform.position.x,_transform.position.y+0.3f);
            //var clone = Instantiate(floatingText, floatingCanvas.transform.position, Quaternion.identity);
            //var clone = Instantiate(floatingText, new Vector2(transform.position.x,transform.position.y+0.3f), Quaternion.identity);
            clone.transform.GetChild(0).GetComponent<Text>().text = text;
            if(type==0)
                clone.transform.GetChild(0).GetComponent<Text>().color = mineralColor;
            else
                clone.transform.GetChild(0).GetComponent<Text>().color = rpColor;

            
            if(activated_FT.transform.childCount>=1){

                clone.GetComponent<Canvas>().sortingOrder = activated_FT.transform.GetChild(activated_FT.transform.childCount-1).transform.GetComponent<Canvas>().sortingOrder+1;
            }
            clone.transform.SetParent(activated_FT.transform);

            clone.gameObject.SetActive(true);
        }
        // else
        // {
        //     var clone = Instantiate(floatingImage, floatingCanvas.transform.position, Quaternion.identity);
        //     clone.GetComponent<FloatingText>().image.sprite = sprite;
        //     clone.transform.SetParent(floatingCanvas.transform);
        // }
    }
















#if DEV_MODE
    public void SetGameSpeed(float speed){
        Time.timeScale = speed;
    }
    public void DebugBtn(int num){
        switch(num){
            case 0 : 
                //ShopManager.instance.Buy("100");
                break;
            case 1 : 
                ShopManager.instance.BuyCoin(3);
                break;
        }
    }
#endif
}
