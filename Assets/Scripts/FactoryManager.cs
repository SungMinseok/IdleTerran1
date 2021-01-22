using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryManager : MonoBehaviour
{   
    public static FactoryManager instance;

    [Header("요구 연구점수/확률(0<=x<1)")]
    public int[] rpRequirement;
    public float[] ptgRequirement;

    [Header("재판매 할인율")]
    public float discount;
    public Transform botManager;
    public Transform factoryPos;
    public Transform parentPanel;
    public Transform[] childPanels;
    public GameObject[] robots;
    public int nowNum;
    public int pricePerUpgrade = 500;

    [Header("요구사항 패널")]
    public GameObject lockedPanel;
    public Text priceNextUpgradeText;
    public Text ptgNextUpgradeText;

    [Header("생산 패널")]
    public Text nameText;
    public Image botImage;
    public Text efficiencyText;
    public Text priceText;
    public Text populationText;
    public Text populationText_Status;
    //public int population;

    public bool[] unlockedNextProduce = new bool[8];
    public GameObject unlockCover;

    [Header("장비 리스트")]
    public GameObject botStatusPanel;
    public Transform botStatusScroll;
    public Transform[] botStatusScrollChildren;
    public Text nameText_Status;
    public Text priceText_Status;
    public GameObject sellLock;
    public Sprite nullSprite;
    public Text[] botStatusCount;
    void Awake(){
        
        instance = this;
        //unlockedNextProduce.Initialize();
    }
    void Start()
    {
        //if(unlockedNextProduce.Length ==0) unlockedNextProduce = new bool[8];
        
        nameText_Status.text = "";
        priceText_Status.text = "";
        sellLock.SetActive(true);
        //unlockedNextProduce = new bool[8];
        childPanels = new Transform[parentPanel.childCount];
        botStatusScrollChildren = new Transform[botStatusScroll.childCount];
        botStatusCount = new Text[botStatusScroll.childCount];
        //childPanels = parentPanel.GetComponentsInChildren<Transform>();
        for(int i=0;i<parentPanel.transform.childCount;i++){
            childPanels[i]=parentPanel.GetChild(i);
            int temp = i;
            int temp1 = i;
            parentPanel.GetChild(temp).GetComponent<Button>().onClick.AddListener(()=>OpenProducePanel(temp));
            childPanels[temp1].GetChild(2).GetComponent<Button>().onClick.AddListener(()=>OpenLockedBtn(temp1));
        }   
        for(int i=0;i<botStatusScroll.childCount;i++){
            int temp = i;
            botStatusScrollChildren[temp] = botStatusScroll.GetChild(temp);
            botStatusScrollChildren[temp].GetComponent<Button>().onClick.AddListener(()=>ShowBotStatusEach(temp));
            botStatusCount[temp] = botStatusScrollChildren[temp].GetChild(2).GetComponent<Text>();
        }
        populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
        populationText_Status.text = BotManager.instance.botSaved.Count.ToString()+" / "+BotManager.instance.maxPopulation;
    }
    public void OpenProducePanel(int num){
        nowNum = num;

        nameText.text = BotManager.instance.botInfoList[num].name;
        botImage.color = childPanels[num].GetChild(1).GetComponent<Image>().color;
        botImage.transform.localScale = childPanels[num].GetChild(1).transform.localScale*6;
        efficiencyText.text = "본체의 "+(BotManager.instance.botInfoList[num].efficiency*100).ToString()+"%"+"(<color=#C0F678>"+Mathf.RoundToInt(BotManager.instance.botInfoList[num].efficiency*PlayerManager.instance.capacity)+"</color>)";
        priceText.text = string.Format("{0:#,###0}", BotManager.instance.botInfoList[num].price);//BotManager.instance.botInfoList[num].price.ToString();

    }
    public void ProduceInTuto(){
        var clone = Instantiate(robots[0],BuildingManager.instance.buildingsInMap[0].transform.position,Quaternion.identity);
                clone.transform.localScale = childPanels[nowNum].GetChild(1).transform.localScale *6;
        clone.GetComponent<SpriteRenderer>().color = childPanels[0].GetChild(1).GetComponent<Image>().color;
        clone.GetComponent<BotScript>().botState = BotState.Mine;
        clone.GetComponent<BotScript>().efficiency = BotManager.instance.botInfoList[0].efficiency;
        clone.transform.parent = botManager;
        BotManager.instance.RefreshBotEquip(BotManager.instance.transform.childCount-1);


        BotManager.instance.botSaved.Add(nowNum);
        populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
        
        populationText_Status.text = BotManager.instance.botSaved.Count.ToString()+" / "+BotManager.instance.maxPopulation;
        SoundManager.instance.Play("ready");
    }

    public void ProduceBtn(){
        if(BotManager.instance.botSaved.Count<BotManager.instance.maxPopulation){ 

            if(PlayerManager.instance.curMineral>=BotManager.instance.botInfoList[nowNum].price){
                
                PlayerManager.instance.HandleMineral(-BotManager.instance.botInfoList[nowNum].price, false);
                    
                var clone = Instantiate(robots[0],factoryPos.position,Quaternion.identity);
                clone.transform.localScale = childPanels[nowNum].GetChild(1).transform.localScale *6;
                clone.GetComponent<SpriteRenderer>().color = childPanels[nowNum].GetChild(1).GetComponent<Image>().color;
                clone.GetComponent<BotScript>().botState = BotState.Mine;
                clone.GetComponent<BotScript>().botType = nowNum;
                clone.GetComponent<BotScript>().efficiency = BotManager.instance.botInfoList[nowNum].efficiency;
                clone.transform.parent = botManager;
                BotManager.instance.RefreshBotEquip(BotManager.instance.transform.childCount-1);


                BotManager.instance.botSaved.Add(nowNum);
                populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
                
                populationText_Status.text = BotManager.instance.botSaved.Count.ToString()+" / "+BotManager.instance.maxPopulation;
                SoundManager.instance.Play("ready");
            } 
            else{
                //SoundManager.instance.Play("notenoughmin");
                UIManager.instance.SetPopUp("미네랄이 부족합니다.","error1");
            }

        }
        else{
            
            //Debug.Log("인구수 부족");
            //SoundManager.instance.Play("error");
            if(BotManager.instance.maxPopulation<BotManager.instance.populationLimit){
                UIManager.instance.SetPopUp("보급품이 더 필요합니다. 보급고를 강화하세요.","error1");

            }
            else{
                
                UIManager.instance.SetPopUp("보급품 최대치입니다.","error");
            }
        }

    }    
    public void ProduceByLoad(){
        var clone = Instantiate(robots[0],factoryPos.position,Quaternion.identity);
                clone.transform.localScale = childPanels[nowNum].GetChild(1).transform.localScale *6;
        clone.GetComponent<SpriteRenderer>().color = childPanels[nowNum].GetChild(1).GetComponent<Image>().color;
        clone.GetComponent<BotScript>().botState = BotState.Mine;
        clone.GetComponent<BotScript>().efficiency = BotManager.instance.botInfoList[nowNum].efficiency;
        clone.transform.parent = botManager;
        // if(botManager.childCount!=0){
        //     clone.GetComponent<SpriteRenderer>().sortingOrder = botManager.GetChild(botManager.childCount-1).GetComponent<SpriteRenderer>().sortingOrder +1;

        // }
        BotManager.instance.RefreshBotEquip(BotManager.instance.transform.childCount-1);


        //BotManager.instance.botSaved.Add(nowNum);
        populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
        
        populationText_Status.text = BotManager.instance.botSaved.Count.ToString()+" / "+BotManager.instance.maxPopulation;
    }

    public void ApplyUnlocked(){
        for(int i=0;i<unlockedNextProduce.Length;i++){
            if(unlockedNextProduce[i]){
                childPanels[i].transform.GetChild(2).gameObject.SetActive(false);
            }
            else{
                childPanels[i].transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        childPanels[0].transform.GetChild(2).gameObject.SetActive(false);
    }
    
    public void OpenLockedBtn(int num){//잠겨있는 버튼 누름.
        nowNum = num;
        
        priceNextUpgradeText.text = string.Format("{0:#,###0}", rpRequirement[nowNum-1]);//(num * (num-1) * pricePerUpgrade).ToString();
        
        if(UIManager.instance.ptgBonus==0){
            ptgNextUpgradeText.text = (100*ptgRequirement[nowNum-1]).ToString() + "%";//(num * (num-1) * pricePerUpgrade).ToString();

        }
        else{
            if(100*ptgRequirement[nowNum-1]*(1+UIManager.instance.ptgBonus)>100){

                ptgNextUpgradeText.text = "100%"
                +"(<color=#C0F678>+"+(100*UIManager.instance.ptgBonus)+"%</color>)";
            }
            else{

                // ptgNextUpgradeText.text = (100*ptgRequirement[nowNum-1]*(1+UIManager.instance.ptgBonus)).ToString("N2") + "%"
                // +"(<color=#C0F678>+"+(100*UIManager.instance.ptgBonus)+"%</color>)";

                ptgNextUpgradeText.text = string.Format("{0:0.##}", 100*ptgRequirement[nowNum-1]*(1+UIManager.instance.ptgBonus)) + "%"
                +"(<color=#C0F678>+"+(100*UIManager.instance.ptgBonus)+"%</color>)";

            }
        }
        //+"(<color=#C0F678>"+Mathf.RoundToInt(BotManager.instance.botInfoList[num].efficiency*PlayerManager.instance.capacity)+"</color>)"
        
        
        
        // if(PlayerManager.instance.curRP >= rpRequirement[nowNum-1]){
        //     unlockCover.SetActive(false);
        // }
        // else{
        //     unlockCover.SetActive(true);
        // }
        
    }
    float tempPtg;
    public void Unlock(){//잠금 해제 버튼 누름.
        
        if(PlayerManager.instance.curRP >= rpRequirement[nowNum-1]){        
            PlayerManager.instance.curRP -= rpRequirement[nowNum-1];

            UIManager.instance.successImage.SetActive(false);
            UIManager.instance.failImage.SetActive(false);
            UIManager.instance.okBtn.SetActive(false);
            UIManager.instance.researchPanel.SetActive(true);
            UIManager.instance.recallImage.SetActive(true);
            SoundManager.instance.Play("recall");
            
            int ranPtg = Random.Range(0,10000);
            tempPtg = ranPtg * 0.0001f;

            Debug.Log("확률 : "+ptgRequirement[nowNum-1]*(1+UIManager.instance.ptgBonus)+"/ 뽑힌 수 : "+ tempPtg);

            Invoke("DelayResult", 0.7f);
            //DelayResult();

            //StartCoroutine(UnlockCoroutine());
        }
        else{
            UIManager.instance.SetPopUp("연구점수가 부족합니다.","error1");
        }

    }
    void DelayResult(){
//성공
        if(ptgRequirement[nowNum-1]*(1+UIManager.instance.ptgBonus)     >=tempPtg){
            UIManager.instance.successImage.SetActive(true);
            UIManager.instance.okBtn.SetActive(true);
            SoundManager.instance.Play("rescue");
            lockedPanel.SetActive(false);
            unlockedNextProduce[nowNum] = true;
            childPanels[nowNum].transform.GetChild(2).gameObject.SetActive(false);      
        }
//실패
        else{
            UIManager.instance.failImage.SetActive(true);
            UIManager.instance.okBtn.SetActive(true);
            SoundManager.instance.Play("error");

        }
    }
    // IEnumerator UnlockCoroutine(){
    //     float tempPtg = Random.Range(0f,100f);


    //     unlockedNextProduce[nowNum] = true;
    //     childPanels[nowNum].transform.GetChild(2).gameObject.SetActive(false);        
    // }

    public void ResetData(){
        unlockedNextProduce = new bool[8];
        for(int i=0;i<unlockedNextProduce.Length;i++){
            unlockedNextProduce[i] = false;
            childPanels[i].transform.GetChild(2).gameObject.SetActive(true);
        }
        childPanels[0].transform.GetChild(2).gameObject.SetActive(false);
    }
    public void EnrollBot(){
        
    }
    // public void OpenBotStatusPanel(){
    //     //Debug.Log(BotManager.instance.botSaved.Count + "개의 로봇");
    //     //Debug.Log("~"+BotManager.instance.botSaved.Count);
    //     for(int i=0;i<BotManager.instance.botSaved.Count;i++){    
    //         botStatusScrollChildren[i].GetChild(2).GetComponent<Text>().text =     
    //         //botStatusScrollChildren[i].GetChild(0).GetComponent<RectTransform>().localScale = childPanels[BotManager.instance.botSaved[i]].GetChild(1).GetComponent<RectTransform>().localScale*2f;
    //         //botStatusScrollChildren[i].GetChild(0).GetComponent<Image>().sprite = childPanels[BotManager.instance.botSaved[i]].GetChild(1).GetComponent<Image>().sprite;
    //         //botStatusScrollChildren[i].GetChild(0).GetComponent<Image>().color = childPanels[BotManager.instance.botSaved[i]].GetChild(1).GetComponent<Image>().color;
    //     }
    //     //Debug.Log(BotManager.instance.botSaved.Count+"~"+tempPre);
    //     for(int i=BotManager.instance.botSaved.Count;i<tempPre;i++){

    //     botStatusScrollChildren[i].GetChild(0).GetComponent<Image>().sprite = nullSprite;
    //     }

        
    //     populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
        
    //     populationText_Status.text = BotManager.instance.botSaved.Count.ToString()+" / "+BotManager.instance.maxPopulation;
    // }   
     public void OpenBotStatusPanel(){

        for(int i=0;i<botStatusCount.Length;i++){    
            botStatusCount[i].text = "0";     
        }

        for(int i=0;i<BotManager.instance.botSaved.Count;i++){   
            //int temp = int.Parse(botStatusCount[BotManager.instance.botSaved[i]].text);
            botStatusCount[BotManager.instance.botSaved[i]].text = (int.Parse(botStatusCount[BotManager.instance.botSaved[i]].text)+1).ToString();
        }

        for(int i=0;i<botStatusCount.Length;i++){    
            if(botStatusCount[i].text != "0"){
                botStatusScrollChildren[i].GetChild(3).gameObject.SetActive(false);
            }  
            else{
                botStatusScrollChildren[i].GetChild(3).gameObject.SetActive(true);

            }   
        }
        
        populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
        
        populationText_Status.text = BotManager.instance.botSaved.Count.ToString()+" / "+BotManager.instance.maxPopulation;
    }
    int selectedNum;
    long selectedPrice;
    // public void ShowBotStatusEach(int num){ // BotManager.instance.botSaved[num] : scv번호
    // selectedNum = num;
    // //Debug.Log(num + "/ "+BotManager.instance.botSaved.Count);
    //     if(BotManager.instance.botSaved.Count>num){

    //         nameText_Status.text = childPanels[BotManager.instance.botSaved[num]].GetChild(0).GetComponent<Text>().text;
    //         priceText_Status.text = string.Format("{0:#,###0}", Mathf.RoundToInt((float)BotManager.instance.botInfoList[BotManager.instance.botSaved[num]].price*discount));//Mathf.RoundToInt((float)BotManager.instance.botInfoList[BotManager.instance.botSaved[num]].price*discount).ToString();
            
    //         selectedPrice = Mathf.RoundToInt((float)BotManager.instance.botInfoList[BotManager.instance.botSaved[num]].price*discount);
    //         sellLock.SetActive(false);
    //     }
    //     else{
            
    //         nameText_Status.text = "";
    //         priceText_Status.text = "";
            
    //         sellLock.SetActive(true);
    //     }
    // }    
    public void ShowBotStatusEach(int num){ // BotManager.instance.botSaved[num] : scv번호
        selectedNum = num;
    //Debug.Log(num + "/ "+BotManager.instance.botSaved.Count);

        nameText_Status.text = childPanels[num].GetChild(0).GetComponent<Text>().text;
        priceText_Status.text = string.Format("{0:#,###0}", Mathf.RoundToInt((float)BotManager.instance.botInfoList[num].price*discount));

        //selectedPrice = Mathf.RoundToInt((float)BotManager.instance.botInfoList[num].price*discount);
        sellLock.SetActive(false);
        // if(BotManager.instance.botSaved.Count>num){

        //     nameText_Status.text = childPanels[BotManager.instance.botSaved[num]].GetChild(0).GetComponent<Text>().text;
        //     priceText_Status.text = string.Format("{0:#,###0}", Mathf.RoundToInt((float)BotManager.instance.botInfoList[BotManager.instance.botSaved[num]].price*discount));//Mathf.RoundToInt((float)BotManager.instance.botInfoList[BotManager.instance.botSaved[num]].price*discount).ToString();
            
        //     selectedPrice = Mathf.RoundToInt((float)BotManager.instance.botInfoList[BotManager.instance.botSaved[num]].price*discount);
        //     sellLock.SetActive(false);
        // }
        // else{
            
        //     nameText_Status.text = "";
        //     priceText_Status.text = "";
            
        //     sellLock.SetActive(true);
        // }
    }
    // public void SellBot(){
    //     tempPre = BotManager.instance.botSaved.Count;
    //     BotManager.instance.botSaved.RemoveAt(selectedNum);
    //     botManager.GetChild(selectedNum).GetComponent<BotScript>().DestroyBot();
    //     PlayerManager.instance.HandleMineral(selectedPrice);

    //     nameText_Status.text = "";
    //     priceText_Status.text = "";
    //     sellLock.SetActive(true);

    //     OpenBotStatusPanel();
    // }
    public void SellBot(){
        //tempPre = BotManager.instance.botSaved.Count;
        //BotManager.instance.botSaved.Remove(selectedNum);
        int tempIndex = BotManager.instance.botSaved.IndexOf(selectedNum);
        Debug.Log(tempIndex+"번째 제거");
        BotManager.instance.botSaved.RemoveAt(tempIndex);
        botManager.GetChild(tempIndex).GetComponent<BotScript>().DestroyBot();
        //PlayerManager.instance.HandleMineral(selectedPrice);
        PlayerManager.instance.HandleMineral((long)float.Parse(priceText_Status.text));
        // nameText_Status.text = "";
        // priceText_Status.text = "";
        // sellLock.SetActive(true);
        if(botStatusCount[selectedNum].text == "1"){
            sellLock.SetActive(true);
            nameText_Status.text = "";
            priceText_Status.text = "";
        }  
        OpenBotStatusPanel();
    }
    //int tempPre;
    // public void SelltheSameTypeBot(){
    //     //List<int> temp = new List<int>();
    //     for(int i=0;i<BotManager.instance.botSaved.Count;i++){
    //         if(BotManager.instance.botSaved[i]==BotManager.instance.botSaved[selectedNum]){
                
    //             //BotManager.instance.botSaved.RemoveAt(i);
    //             botManager.GetChild(i).GetComponent<BotScript>().DestroyBot();
    //             PlayerManager.instance.HandleMineral(selectedPrice);
    //             //temp.Add(i);
    //         }
    //         //Debug.Log("제거한 로봇 개수"+));
    //     }
    //     tempPre = BotManager.instance.botSaved.Count;
    //     //Debug.Log("선택한 로봇 번호"+BotManager.instance.botSaved[selectedNum]);
    //     int tempSelected = BotManager.instance.botSaved[selectedNum];
    //     while(BotManager.instance.botSaved.Contains(tempSelected)){
    //         //Debug.Log("리스트 리무브");
    //         BotManager.instance.botSaved.Remove(tempSelected);
    //         //BotManager.instance.botSaved.RemoveAll(delegate (int x){return x==BotManager.instance.botSaved[selectedNum];});
    //         //Debug.Log("선택한 로봇 개수(리스트)"+BotManager.instance.botSaved.RemoveAll(delegate (int x){return x==BotManager.instance.botSaved[selectedNum];}));
    //     }
        
    //     nameText_Status.text = "";
    //     priceText_Status.text = "";
    //     sellLock.SetActive(true);

    //     OpenBotStatusPanel();
    // }
    public void SelltheSameTypeBot(){
        //List<int> temp = new List<int>();
        while(BotManager.instance.botSaved.Contains(selectedNum)){

            int tempIndex = BotManager.instance.botSaved.IndexOf(selectedNum);
            Debug.Log(tempIndex+"번째 제거");
            BotManager.instance.botSaved.RemoveAt(tempIndex);
            botManager.GetChild(tempIndex).GetComponent<BotScript>().DestroyBot();
            PlayerManager.instance.HandleMineral((long)float.Parse(priceText_Status.text));
        }
        // for(int i=0;i<BotManager.instance.botSaved.Count;i++){
        //     if(BotManager.instance.botSaved[i]==BotManager.instance.botSaved[selectedNum]){
                
        //         //BotManager.instance.botSaved.RemoveAt(i);
        //         botManager.GetChild(i).GetComponent<BotScript>().DestroyBot();
        //         PlayerManager.instance.HandleMineral(selectedPrice);
        //         //temp.Add(i);
        //     }
        //     //Debug.Log("제거한 로봇 개수"+));
        // }
        //tempPre = BotManager.instance.botSaved.Count;
        //Debug.Log("선택한 로봇 번호"+BotManager.instance.botSaved[selectedNum]);
        // int tempSelected = BotManager.instance.botSaved[selectedNum];
        // while(BotManager.instance.botSaved.Contains(tempSelected)){
        //     //Debug.Log("리스트 리무브");
        //     BotManager.instance.botSaved.Remove(tempSelected);
        //     //BotManager.instance.botSaved.RemoveAll(delegate (int x){return x==BotManager.instance.botSaved[selectedNum];});
        //     //Debug.Log("선택한 로봇 개수(리스트)"+BotManager.instance.botSaved.RemoveAll(delegate (int x){return x==BotManager.instance.botSaved[selectedNum];}));
        // }
        
        nameText_Status.text = "";
        priceText_Status.text = "";
        sellLock.SetActive(true);

        OpenBotStatusPanel();
    }

}
