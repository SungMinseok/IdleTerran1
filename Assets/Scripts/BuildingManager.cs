using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Building{
    [Header("번호")]
    public int index;
    [Header("이름")]
    public string name;
    [Header("설명")]
    public string des;
    [Header("건설시간")]
    public float buildTime;//업그레이드 내용 증가량 : 적재량(8)
    [Header("가격")]
    public int price;//업그레이드 비용 증가량 : 100
    // [Header("최대 레벨")]
    // public int maxLevel;//업그레이드 비용 증가량 : 100
    public Building(int e,string a,string b, float c, int d ){
        index = e;
        name = a;
        des = b;
        buildTime = c;
        price = d;
    }
}
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;
    public bool[] unlockedNextBuilding = new bool[4];

    [Header("요구 연구점수/확률(0<=x<1)")]
    public int[] rpRequirement;
    public float[] ptgRequirement;
    [Header("작성해야할 것")]
    public Building[] buildings;
    public int[] order = new int[]{0,1,2};
    [Header("채워야할 것")]
    public GameObject[] buildingsInMap;
    public float[] buildTimeCounter = new float[3];    //건물 상태 체크(건설전/건설중/건설완료). 건물 별 건설시간 받아서 그것보다 높으면 완성상태 , 아니면 건설중. 0 이면 없음.
    public bool[] isConstructing = new bool[3];
    [Header("Build Panel UI")]
    public Transform buildScrollPanel;
    public Transform[] childPanels;
    public GameObject buildingPanel;
    public GameObject buildPanel;
    public Image buildingImage;
    public Text nameText;
    public Text desText;
    public Text buildTimeText;
    public Text priceText;


    [Header("요구사항 패널")]
    public GameObject lockedPanel;
    public Text priceNextUpgradeText;
    public Text ptgNextUpgradeText;
    public GameObject unlockCover;
    void Awake(){

        instance =this;
        // buildTimeCounter = new float[buildings.Length];
        // buildTimeCounter.Initialize();
        // isConstructing = new bool[buildings.Length];
        // isConstructing.Initialize();
    }
    void Start(){
        //buildingList = buildScrollPanel.GetComponentsInChildren<Transform>();// 0 // 1,2,3,4,5 // 6,7,8,9,10 // 버튼,이름,이미지,잠금,건설완료
        //Debug.Log(buildScrollPanel.transform.childCount); 건물 갯수.
        childPanels = new Transform[buildScrollPanel.childCount];
        for(int i=0;i<buildScrollPanel.transform.childCount;i++){
            childPanels[i] = buildScrollPanel.GetChild(i);
            //Debug.Log(i);
            int temp = i;
            buildScrollPanel.GetChild(temp).GetComponent<Button>().onClick.AddListener(()=>BuildPanelBtn(temp));
            //buildingList[temp].GetComponent<Button>().onClick.AddListener(()=>BuildPanelBtn(temp));
            childPanels[temp].GetChild(2).GetComponent<Button>().onClick.AddListener(()=>OpenLockedBtn(temp));
        //     buildingList.Add(buildScrollPanel.get);
        }        
        // for(int i=1;i<buildScrollPanel.transform.childCount;i+=5){
        //     Debug.Log(i);
        //     int temp = i;
        //     buildingList[temp].GetComponent<Button>().onClick.AddListener(()=>BuildPanelBtn(temp/5));
        // //     buildingList.Add(buildScrollPanel.get);
        // }

        //처음 두 건물 오픈.
        // buildingList[4].gameObject.SetActive(false);
        // buildingList[9].gameObject.SetActive(false);
        BuildingManager.instance.BuildingStateCheck();
        ApplyUnlocked();
    }
    void LateUpdate(){
        //if(DBManager.instance.loadComplete){

            if(ConstructingCheck()) BuildTimeCount();
        //}
    }
    
    public int nowBuilding;
    //건설하기 누름
    public void BuildBtn(){        
        if(PlayerManager.instance.curMineral >= buildings[nowBuilding].price){
            buildingPanel.SetActive(false);
            buildPanel.SetActive(false);
            StartCoroutine(BuildingCoroutine(nowBuilding));//0번건물 정보 넘김. 건설시간 받아오기
            buildingPanel.SetActive(false);
            buildPanel.SetActive(false);
            //SoundManager.instance.Play("up");
        }
        else{
            //SoundManager.instance.Play("notenoughmin");
            UIManager.instance.SetPopUp("미네랄이 부족합니다.","error1");
        }

    }

    // void Update(){
    //     for(int i=0;i<)
    // }
    //건물 버튼 누름.
    public void BuildPanelBtn(int num){
        nowBuilding = num;//인덱스

        nameText.text = buildings[nowBuilding].name;
        desText.text = buildings[nowBuilding].des;
        buildTimeText.text = buildings[nowBuilding].buildTime.ToString()+"초";
        priceText.text = string.Format("{0:#,###0}", buildings[nowBuilding].price);//buildings[num].price.ToString();
        buildingImage.sprite = childPanels[num].GetChild(1).GetComponent<Image>().sprite;

        buildPanel.SetActive(true);
    }

    IEnumerator BuildingCoroutine(int buildingNum){
        Debug.Log(buildingNum +"번 건설이동");
        PlayerManager.instance.canMove = false;
        PlayerManager.instance.Order(buildingsInMap[buildingNum].transform,OrderType.Build);
        yield return new WaitUntil(()=>PlayerManager.instance.buildStart);
        PlayerManager.instance.buildStart = false;
            PlayerManager.instance.HandleMineral(-buildings[nowBuilding].price);

        Debug.Log(buildingNum +"번 건설시작");
        isConstructing[buildingNum] = true;
        buildingsInMap[buildingNum].transform.GetChild(0).gameObject.SetActive(true);
        //var tempBuilding = gameObject;
        //for(int i=0;i<buildingsInMap.Length;i++){
            //if(buildingsInMap[i].name == building.name){
                //tempBuilding = buildingsInMap[i];
        buildingsInMap[buildingNum].transform.GetChild(0).GetComponent<Animator>().SetFloat("Speed",6/buildings[buildingNum].buildTime);
        buildingsInMap[buildingNum].SetActive(true);

        buildTimeCounter[buildingNum]+=0.001f;
                //break;
            //}
        //}
        // yield return new WaitForSeconds(buildings[buildingNum].buildTime);
        // Debug.Log("건설완료");
        // buildingsInMap[buildingNum].transform.GetChild(0).gameObject.SetActive(false);
        // buildingsInMap[buildingNum].transform.GetChild(1).gameObject.SetActive(true);

    }

    public void BuildTimeCount(){
        for(int i=0;i< buildingsInMap.Length; i++){
            //if(buildingsInMap[i].transform.GetChild(0).gameObject.activeSelf){
            if(isConstructing[i]){
                if(buildTimeCounter[i]>0&&buildTimeCounter[i]<buildings[i].buildTime){
                    buildTimeCounter[i] += Time.deltaTime;
                    //if(!isConstructing[i]) isConstructing[i] = true;
                    if(!buildingsInMap[i].transform.GetChild(0).gameObject.activeSelf){
                        buildingsInMap[i].transform.GetChild(0).gameObject.SetActive(true); 
                        buildingsInMap[i].transform.GetChild(0).GetComponent<Animator>().Play("Constructing", 0, buildTimeCounter[i]/buildings[i].buildTime); 
                        buildingsInMap[i].transform.GetChild(0).GetComponent<Animator>().SetFloat("Speed",6/buildings[i].buildTime);
                    }
                }
                else{
                    isConstructing[i] = false;
                    BuildingStateCheck(i);
                    buildingsInMap[i].transform.GetChild(0).gameObject.SetActive(false); 
                    buildingsInMap[i].transform.GetChild(1).gameObject.SetActive(true); 
                    
                    UIManager.instance.SetPopUp("건설이 완료되었습니다 : "+buildings[i].name);

                    if(i==1) UIManager.instance.ActivateLowerUIPanel(2);
                }
            }
        }
    }

    public bool ConstructingCheck(){
        for(int i=0;i< buildingsInMap.Length; i++){
            if(buildTimeCounter[i]>0&&buildTimeCounter[i]<buildings[i].buildTime){ //건설중 건물 체크
                if(!isConstructing[i]) isConstructing[i] = true;
            }
            // else if(buildTimeCounter[i]>=buildings[i].buildTime){   //지어진 건물 체크
                
            //             buildingsInMap[i].transform.GetChild(1).gameObject.SetActive(true); 
            //             // buildingList[i].transform.GetChild(2).gameObject.SetActive(false); //잠금해제
            //             // buildingList[i].transform.GetChild(3).gameObject.SetActive(true); //건설 완료
            // }
        }




        for(int i=0;i<isConstructing.Length;i++){
            if(isConstructing[i]){
                return true;
            }
        }
        return false;
    }
    public void BuildingStateCheck(int flag = -1){//건설완료시 체크, 맨처음 체크
        //건설 완료 :  텍스트 활성화
        if(flag == -1){//전체 체크

            for(int i=0;i<buildings.Length;i++){
                if(buildTimeCounter[i] >= buildings[i].buildTime){  //건설 되있으면.
                    childPanels[i].GetChild(3).gameObject.SetActive(true);//텍스트 활성화
                    buildingsInMap[i].transform.GetChild(1).gameObject.SetActive(true);
                    
                    if(i==1) UIManager.instance.ActivateLowerUIPanel(2);
                    if(i==1) UIManager.instance.depotLock.SetActive(false);
                    if(i==3) UIManager.instance.ActivateLowerUIPanel(7);
                    if(i==4) UIManager.instance.RPCollect();
                }
                else{
                    childPanels[i].GetChild(3).gameObject.SetActive(false);//텍스트 활성화
                    buildingsInMap[i].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }     
        else{
            childPanels[flag].GetChild(3).gameObject.SetActive(true);//텍스트 활성화
            buildingsInMap[flag].transform.GetChild(1).gameObject.SetActive(true);
            
                    if(flag==1) UIManager.instance.ActivateLowerUIPanel(2);
                    if(flag==1) UIManager.instance.depotLock.SetActive(false);
                    if(flag==3) UIManager.instance.ActivateLowerUIPanel(7);
                    if(flag==4) UIManager.instance.RPCollect();
        }   
    }
    // public void SetBuilt(int num){

    // }

    //건설중 : 버튼 비활성화
    // public void ResetBuilding(){

    //     for(int i=1;i<buildScrollPanel.transform.childCount;i+=5){
    //         int temp = i;
    //         buildingList[temp].GetComponent<Button>().onClick.AddListener(()=>BuildPanelBtn(temp/5));
    //     //     buildingList.Add(buildScrollPanel.get);
    //     }

    //     for(int i=0;i< buildingsInMap.Length; i++){
    //         buildingList[i].transform.GetChild(2).gameObject.SetActive(true); //잠금해제
    //         buildingList[i].transform.GetChild(3).gameObject.SetActive(false); //건설 완료
            
    //     }




    //     for(int i=0;i< buildingsInMap.Length; i++){
    //         if(buildTimeCounter[i]>0&&buildTimeCounter[i]<buildings[i].buildTime){ //건설중 건물 체크
    //             if(!isConstructing[i]) isConstructing[i] = true;
    //         }
    //         else if(buildTimeCounter[i]>=buildings[i].buildTime){   //지어진 건물 체크
                
    //             buildingsInMap[i].transform.GetChild(1).gameObject.SetActive(true); 
    //             buildingList[i].transform.GetChild(2).gameObject.SetActive(false); //잠금해제
    //             buildingList[i].transform.GetChild(3).gameObject.SetActive(true); //건설 완료
    //         }
    //     }
    // }

    public void ApplyUnlocked(){
        
        for(int i=0;i<unlockedNextBuilding.Length;i++){
            if(unlockedNextBuilding[i]){
                childPanels[i].transform.GetChild(2).gameObject.SetActive(false);
            }
            else{
                childPanels[i].transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        childPanels[0].transform.GetChild(2).gameObject.SetActive(false);
        unlockedNextBuilding[0] = true;
    }
    public int nowNum;
    public void OpenLockedBtn(int num){//잠겨있는 버튼 누름.
        nowNum = num;
        
        priceNextUpgradeText.text = string.Format("{0:#,###0}", rpRequirement[nowNum]);//(num * (num-1) * pricePerUpgrade).ToString();
                
        if(UIManager.instance.ptgBonus==0){
            ptgNextUpgradeText.text = (100*ptgRequirement[nowNum]).ToString() + "%";//(num * (num-1) * pricePerUpgrade).ToString();

        }
        else{
            if(100*ptgRequirement[nowNum]*(1+UIManager.instance.ptgBonus)>100){

                ptgNextUpgradeText.text = "100%"
                +"(<color=#C0F678>+"+(100*UIManager.instance.ptgBonus)+"%</color>)";
            }
            else{

                ptgNextUpgradeText.text = string.Format("{0:0.##}", 100*ptgRequirement[nowNum]*(1+UIManager.instance.ptgBonus)) + "%"
                +"(<color=#C0F678>+"+(100*UIManager.instance.ptgBonus)+"%</color>)";

            }
        }

        // if(childPanels[nowNum-1].transform.GetChild(3).gameObject.activeSelf  && PlayerManager.instance.curRP >= rpRequirement[nowNum]){
        //     unlockCover.SetActive(false);
        // }
        // else{
        //     unlockCover.SetActive(true);
        // }
        
    }
    float tempPtg;
    public void Unlock(){//잠금 해제 버튼 누름.
        if(childPanels[nowNum-1].transform.GetChild(3).gameObject.activeSelf){
            if(PlayerManager.instance.curRP >= rpRequirement[nowNum]){

                PlayerManager.instance.curRP -= rpRequirement[nowNum];

            //     UIManager.instance.successImage.SetActive(false);
            //     UIManager.instance.failImage.SetActive(false);
            //     UIManager.instance.okBtn.SetActive(false);
            //     UIManager.instance.researchPanel.SetActive(true);
            //     UIManager.instance.recallImage.SetActive(true);
            // SoundManager.instance.Play("recall");
                
            //     int ranPtg = Random.Range(0,10000);
            //     tempPtg = ranPtg * 0.0001f;
            
                tempPtg = UIManager.instance.OpenResearchPanel();

                Debug.Log("확률 : "+ptgRequirement[nowNum]*(1+UIManager.instance.ptgBonus)+"/ 뽑힌 수 : "+ tempPtg);

                Invoke("DelayResult", 0.7f);
            }
            else{
                UIManager.instance.SetPopUp("연구점수가 부족합니다.","error1");
            }
        }
        else{
            UIManager.instance.SetPopUp("이전 단계 건물을 완성해야합니다.","error1");

        }
        

    }
    void DelayResult(){
//성공
        if(ptgRequirement[nowNum]*(1+UIManager.instance.ptgBonus)     >=tempPtg){
            UIManager.instance.successImage.SetActive(true);
            UIManager.instance.okBtn.SetActive(true);    
            SoundManager.instance.Play("rescue");
                lockedPanel.SetActive(false);

            unlockedNextBuilding[nowNum] = true;
            childPanels[nowNum].transform.GetChild(2).gameObject.SetActive(false);                
            //PlayerManager.instance.curRP -= rpRequirement[nowNum];
        }
//실패
        else{
            UIManager.instance.failImage.SetActive(true);
            UIManager.instance.okBtn.SetActive(true);
            SoundManager.instance.Play("error");

        }
    }
    
}
