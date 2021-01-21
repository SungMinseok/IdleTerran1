using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Buff{
    public string name;
    public Transform btn;
    public float coolTime;
    //public int count;//DB저장
    public float remainingCoolTime;
    //public int remainingCount;
    [Space(5)]
    public Transform buffImage;//상단UI표시
    public float duration;
    public float remainingDuration;
    [Space(5)]
    public int count;//DB저장
}
public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    public int boxCount;
    public Text boxCountText;
    public Text boxCountText2;
    public Transform botManager;
    [SerializeField]
    public List<Buff> buffs = new List<Buff>();
    // Start is called before the first frame update
    //public List<float> coolTimeCounter = new List<float>();
    //public List<float> durationTimeCounter = new List<float>();
    // public float[] coolTimeCounter = new float[1];
    // public float[] durationTimeCounter = new float[1];
    // public int[] countCounter = new int[1];
    public Image centerBuffImg;
    public Text centerBuffText;
    public Button centerBuffBtn;
    [Header("랜덤박스 UI")]
    public GameObject randomBoxPanel;
    public Text randomAmountText;
    public Text bonusAmountText;
    public Text totalAmountText;
    public GameObject randomOk;
    public Image randomImage;
    public Sprite mineralSprite;
    public Sprite rpSprite;
    public Sprite spSprite;
    public GameObject recallEffect;
    [Header("드랍쉽")]
    public GameObject dropship;
    public GameObject dropbox;
    public BoxCollider2D mapBound;
    public float dropshipSpeed = 5f;
    public float dropshipCoolTime;
    public float dropshipRemainingCoolTime;
    [Header("RP")]
    public BoxCollider2D[] createArea;
    public float rpCoolTime;
    public int maxRP;
    public GameObject rpBox;
    public Transform rpParent;
    
    [Header("오토모드")]
    public bool autoChargeFuel;
    private bool autoCharging;
    public bool autoGatherRP;
    public bool autoStimpack;
    [Header("랜덤박스 전체")]
    public GameObject randomBoxPanel_All;
    public Transform randomBoxPanel_All_Grid;
    public GameObject randomBoxPanel_All_Ok;
    public Text randomBoxRemain;
    void Awake(){
        instance = this;
    }
    void Start()
    {
        //CreateDropship();
        boxCountText.text = "x " + boxCount.ToString();
        boxCountText2.text = "x " + boxCount.ToString();

        for(int i=0; i< buffs.Count;i++){
            if(buffs[i].count != -1){
                    
                //var temp = buffs[i].btn.GetChild(buffs[i].btn.childCount-1).GetComponent<Text>();
                //if(temp.gameObject.name == "LeftCount"){//카운트가 있는 경우(스팀팩)
                buffs[i].btn.GetChild(buffs[i].btn.childCount-1).GetComponent<Text>().text = buffs[i].count.ToString();
            }
            else{
                buffs[i].btn.GetChild(buffs[i].btn.childCount-1).gameObject.SetActive(false);
            }

            if(buffs[i].buffImage != null){
                buffs[i].buffImage.gameObject.SetActive(false);
            }
            

                
                    //buffs[i].btn.GetChild(buffs[i].btn.childCount-2).GetComponent<Text>().text
            //}
        }
RefreshUICount();
    }

    public void ActivateBuff(string name){
        for(int i=0;i<buffs.Count;i++){
            if(buffs[i].name == name){
                //var temp = buffs[i].btn.GetChild(buffs[i].btn.childCount-1);
                //if(temp.gameObject.name == "LeftCount"){//카운트가 있는 경우(스팀팩)
                if(buffs[i].count > 0 || buffs[i].count==-1 ){
                    if(buffs[i].count != -1){
                        buffs[i].btn.GetChild(buffs[i].btn.childCount-1).GetComponent<Text>().text = (--buffs[i].count).ToString();

                    }

                        if(buffs[i].name=="AD"){
                            
                            StartCoroutine(ADCoolTimeCoroutine(buffs[i]));
                        }
                        else{
                            
                            StartCoroutine(BuffCoolTimeCoroutine(buffs[i]));
                        }

                    if(buffs[i].duration!= -1){
                        StartCoroutine(BuffCoroutine(buffs[i]));
                    }
                }
                else{

                }
                    
                        //buffs[i].btn.GetChild(buffs[i].btn.childCount-2).GetComponent<Text>().text
                // }
                // else{

                //     StartCoroutine(BuffCoolTimeCoroutine(buffs[i]));
                // }
            }
        }
    }

    IEnumerator BuffCoolTimeCoroutine(Buff buff){
        buff.btn.GetComponent<Button>().interactable = false;
        var coolTimeImage = buff.btn.GetChild(buff.btn.childCount-2).GetComponent<Image>();
        var coolTimeText = buff.btn.GetChild(buff.btn.childCount-2).transform.GetChild(0).GetComponent<Text>();
        //buff.btn.GetComponent<Button>().interactable = false;//centerBuffBtn.interactable = false;
        coolTimeImage.gameObject.SetActive(true);//centerBuffImg.gameObject.SetActive(true);
        coolTimeText.text = buff.coolTime.ToString();//centerBuffText.text = buff.time.ToString();
        
        

        while(buff.remainingCoolTime>0f){  
            
            // buff.time--;

           // Debug.Log("TempTime : "+ tempTime);
            // centerBuffText.text = buff.time.ToString();
            // centerBuffImg.fillAmount = (float)buff.time / (float)full ;
            // Debug.Log(centerBuffImg.fillAmount);
            // yield return new WaitForSeconds(1f);

            buff.remainingCoolTime -= Time.deltaTime;
            
            coolTimeText.text = (Mathf.CeilToInt(buff.remainingCoolTime)).ToString();//centerBuffText.text = (Mathf.CeilToInt(tempTime)).ToString();
            coolTimeImage.fillAmount = buff.remainingCoolTime / buff.coolTime ;
            yield return new WaitForFixedUpdate();
        }

        //centerBuffBtn.interactable = true;
        buff.btn.GetComponent<Button>().interactable = true;
        coolTimeImage.gameObject.SetActive(false);//centerBuffImg.gameObject.SetActive(false);

        buff.remainingCoolTime = buff.coolTime;
    }    
    IEnumerator BuffCoroutine(Buff buff){
        buff.buffImage.gameObject.SetActive(true);
        // buff.btn.GetComponent<Button>().interactable = false;
        var coolTimeImage = buff.buffImage.GetChild(0).GetChild(0).GetComponent<Image>();
        // var coolTimeText = buff.btn.GetChild(buff.btn.childCount-2).transform.GetChild(0).GetComponent<Text>();
        // //buff.btn.GetComponent<Button>().interactable = false;//centerBuffBtn.interactable = false;
        // coolTimeImage.gameObject.SetActive(true);//centerBuffImg.gameObject.SetActive(true);
        // coolTimeText.text = buff.coolTime.ToString();//centerBuffText.text = buff.time.ToString();
        // float tempTime = buff.coolTime;
        Debug.Log("버프코루틴 시작 : "+buff.name);
        switch(buff.name){
            case "Stimpack0" : 
                SetBoosterColor("red");
                SetSCVSpeed(1.5f);
                break;
            case "Stimpack1" : 
                SetMineralSize(1.5f);
                break;
            default :
                break;
        }







        
        while(buff.remainingDuration>0f){  
            
            // buff.time--;

            //Debug.Log("TempTime : "+ tempTime);
            // centerBuffText.text = buff.time.ToString();
            // centerBuffImg.fillAmount = (float)buff.time / (float)full ;
            // Debug.Log(centerBuffImg.fillAmount);
            // yield return new WaitForSeconds(1f);

            buff.remainingDuration -= Time.deltaTime;
            
            //coolTimeText.text = (Mathf.CeilToInt(tempTime)).ToString();//centerBuffText.text = (Mathf.CeilToInt(tempTime)).ToString();
            coolTimeImage.fillAmount = (buff.duration-buff.remainingDuration) / buff.duration ;
            yield return new WaitForFixedUpdate();
        }

        //centerBuffBtn.interactable = true;
        //buff.btn.GetComponent<Button>().interactable = true;
        buff.buffImage.gameObject.SetActive(false);//centerBuffImg.gameObject.SetActive(false);

        buff.remainingDuration = buff.duration;


        switch(buff.name){
            case "Stimpack0" : 
                SetBoosterColor();
                SetSCVSpeed();
                break;
            case "Stimpack1" : 
                SetMineralSize();
                break;

            default :
                break;
        }

        if(autoStimpack){
            ActivateBuff(buff.name);
        }


    }
    // IEnumerator NewBuffCoroutine(Buff buff){ // 버프 중첩용.
    //     buff.buffImage.gameObject.SetActive(true);
    //     var coolTimeImage = buff.buffImage.GetChild(0).GetComponent<Image>();
    //     Debug.Log("버프코루틴 시작");
    //     switch(buff.name){
    //         case "Stimpack" : 
    //             SetBoosterColor("red");
    //             SetSCVSpeed(1.5f);
    //             break;
    //         default :
    //             break;
    //     }
    //     while(buff.remainingDuration>0f){  
    //         buff.remainingDuration -= Time.deltaTime;
    //         coolTimeImage.fillAmount = (buff.duration-buff.remainingDuration) / buff.duration ;
    //         yield return new WaitForFixedUpdate();
    //     }
    //     buff.buffImage.gameObject.SetActive(false);

    //     buff.remainingDuration = buff.duration;


    //     switch(buff.name){
    //         case "Stimpack" : 
    //             SetBoosterColor();
    //             SetSCVSpeed();
    //             break;

    //         default :
    //             break;
    //     }


    // }
    int ranType;
    int ranNum;
    public void SetDailyRandomBox(){    // 일일 랜덤상자 클릭
        ActivateBuff("Center");
        recallEffect.SetActive(true);
        randomBoxPanel.SetActive(true);
        SoundManager.instance.Play("recall");
        ranType = Random.Range(0,2);//0,1
        
        if(ranType == 0){ //미네랄 50000~100000
            
            ranNum = Random.Range(50,100);
            randomImage.sprite = mineralSprite;
            randomAmountText.text = (ranNum * 1000).ToString();
        }
        else if(ranType == 1){//연구점수 200~2000

            ranNum = Random.Range(10,100); 
            randomImage.sprite = rpSprite;
            randomAmountText.text = (ranNum * 20).ToString();
        }
        

        Invoke("DelayOkBtn",0.7f);
    }    
    public void SetRandomBox(bool all = false){ //획득 가능 랜덤 상자 클릭
        //if(boxCount >0){
            // //ActivateBuff("Center");
            // recallEffect.SetActive(true);
            // randomBoxPanel.SetActive(true);
            // //boxCount --;
            // boxCountText.text = "x "+(--boxCount).ToString();
                
            // SoundManager.instance.Play("recall");
            //if(setRandom == -1){

                ranType = Random.Range(0,4);//0,1
            //}
            //else{
                //ranType = setRandom;
            //}
            float tempAmount=0;
            float tempBonusAmount=0;
            
            if(ranType == 0){ //미네랄 (용접기레벨+적재함레벨+엔진레벨) * (100~200) * 2
                ranNum = Random.Range(100,201);
                randomImage.sprite = mineralSprite;
                tempAmount = (PlayerManager.instance.weldingLevel+PlayerManager.instance.bodyLevel+PlayerManager.instance.engineLevel)
                *(ranNum) * 2;
                tempBonusAmount = Mathf.CeilToInt(((float)(PlayerManager.instance.moreSupply * 5) * 0.01f ) * tempAmount );
                // amount.text = (tempAmount+float.Parse(amount.text)).ToString();
                // bonus.text = (Mathf.CeilToInt(((float)(PlayerManager.instance.moreSupply * 5) / 100 ) * tempAmount )).ToString();
            }
            else if(ranType == 1){//연구점수 현재 연구점수 획득량(100) * (3~5)

                ranNum = Random.Range(3,6); 
                randomImage.sprite = rpSprite;
                tempAmount = (((PlayerManager.instance.investRP+1)*100) * (ranNum));
                tempBonusAmount = Mathf.CeilToInt(((float)(PlayerManager.instance.moreSupply * 5) * 0.01f ) * tempAmount );
                // amount.text = tempAmount.ToString();
                // bonus.text = Mathf.CeilToInt(((float)(PlayerManager.instance.moreSupply * 5) / 100 ) * tempAmount ).ToString();

            }
            else if(ranType ==2){//엔진 스팀팩 3~5 + 연료탱크레벨 10당 1 + 보너스레벨

                ranNum = Random.Range(3,6); 
                randomImage.sprite = spSprite;
                
                tempAmount = (PlayerManager.instance.fuelLevel/10 + (ranNum));
                tempBonusAmount = (PlayerManager.instance.moreSupply);
                // amount.text = tempAmount.ToString();
                // bonus.text = (PlayerManager.instance.moreSupply).ToString();

            }
            else if(ranType ==3){//탱크 스팀팩 3~5 + 적재량레벨 10당 1 + 보너스레벨

                ranNum = Random.Range(3,6); 
                randomImage.sprite = spSprite;
                
                tempAmount = (PlayerManager.instance.bodyLevel/10 + (ranNum));
                tempBonusAmount = (PlayerManager.instance.moreSupply);

            }
            if(!all){
                randomAmountText.text = string.Format("{0:#,###0}", tempAmount);//tempAmount.ToString();
                bonusAmountText.text = string.Format("{0:#,###0}", tempBonusAmount);//tempBonusAmount.ToString();
                totalAmountText.text = string.Format("{0:#,###0}", tempAmount+tempBonusAmount);//(tempAmount+tempBonusAmount).ToString();

            Invoke("DelayOkBtn",0.7f);
            }
            else{
                //float tempTotal = tempAmount +tempBonusAmount;
                randomBoxPanel_All_Grid.GetChild(ranType).GetChild(0).gameObject.SetActive(false);
                randomBoxPanel_All_Grid.GetChild(ranType).GetChild(0).gameObject.SetActive(true);
                //Text amount = randomBoxPanel_All_Grid.GetChild(ranType).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();
                //Text bonus = randomBoxPanel_All_Grid.GetChild(ranType).GetChild(2).GetChild(3).GetChild(0).GetComponent<Text>();
                Text total = randomBoxPanel_All_Grid.GetChild(ranType).GetChild(2).GetChild(0).GetComponent<Text>();
                Text count = randomBoxPanel_All_Grid.GetChild(ranType).GetChild(1).GetChild(0).GetComponent<Text>();
                //amount.text = (tempAmount+float.Parse(amount.text)).ToString();
                //bonus.text = (tempBonusAmount+float.Parse(bonus.text)).ToString();
                //total.text = (float.Parse(amount.text)+float.Parse(bonus.text)).ToString();
                total.text = string.Format("{0:#,###0}", (float.Parse(total.text) + tempAmount));//(float.Parse(total.text) + tempAmount).ToString();
                count.text = (float.Parse(count.text)+1).ToString();
            }
            

        //}

    }
    public void DelayOkBtn(){
        SoundManager.instance.Play("rescue");
        randomOk.SetActive(true);
    }
    public void DelayOkBtn_All(){
        SoundManager.instance.Play("rescue");
        randomBoxPanel_All_Ok.SetActive(true);
    }
    public void GetRandomBox(bool all = false){ //확인 클릭
        if(!all){
            switch(ranType){
                case 0:
            PlayerManager.instance.HandleMineral(int.Parse(randomAmountText.text) + int.Parse(bonusAmountText.text) );
                    break;
                case 1:
            PlayerManager.instance.HandleRP(int.Parse(randomAmountText.text) + int.Parse(bonusAmountText.text) );
                    break;
                case 2:
            BuffManager.instance.buffs[0].count+=int.Parse(randomAmountText.text) + int.Parse(bonusAmountText.text);
                    break;
                case 3:
            BuffManager.instance.buffs[1].count+=int.Parse(randomAmountText.text) + int.Parse(bonusAmountText.text);
                    break;

            }
            randomAmountText.text = "";
            bonusAmountText.text = "";
        }
        else{//string.Format("{0:#,###0}", (float.Parse(totalText.text) + tempBonus))  
            //Text tempResultText = randomBoxPanel_All_Grid.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();

            //PlayerManager.instance.HandleMineral(float.Parse(randomBoxPanel_All_Grid.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text) );
            
            
            //PlayerManager.instance.HandleMineral(tempResult[0]);
            PlayerManager.instance.HandleMineral((long)float.Parse(string.Format("{0:#,###0}", randomBoxPanel_All_Grid.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text) ));
            PlayerManager.instance.HandleRP((long)float.Parse(string.Format("{0:#,###0}",randomBoxPanel_All_Grid.GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text) ));
            BuffManager.instance.buffs[0].count+=int.Parse(randomBoxPanel_All_Grid.GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>().text);
            BuffManager.instance.buffs[1].count+=int.Parse(randomBoxPanel_All_Grid.GetChild(3).GetChild(2).GetChild(0).GetComponent<Text>().text);
                
            for(int i=0;i< randomBoxPanel_All_Grid.childCount;i++){

                //Text amount = randomBoxPanel_All_Grid.GetChild(i).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>();
                //Text bonus = randomBoxPanel_All_Grid.GetChild(i).GetChild(2).GetChild(3).GetChild(0).GetComponent<Text>();
                Text total = randomBoxPanel_All_Grid.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>();
                Text count = randomBoxPanel_All_Grid.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>();
                //amount.text = "0";
                //bonus.text = "0";
                total.text = "0";
                count.text = "0";

            }


        }
        
        RefreshUICount();
        // if(ranType ==0 ){
        //     PlayerManager.instance.HandleMineral(int.Parse(randomAmountText.text) + int.Parse(bonusAmountText.text) );
        // }
        // else if(ranType ==1){
        //     PlayerManager.instance.HandleRP(int.Parse(randomAmountText.text) + int.Parse(bonusAmountText.text) );
        // }
        // else if (ranType==2){

        //     BuffManager.instance.buffs[0].count+=int.Parse(randomAmountText.text) + int.Parse(bonusAmountText.text);
        // }
        // else if (ranType==3){

        //     BuffManager.instance.buffs[1].count+=int.Parse(randomAmountText.text) + int.Parse(bonusAmountText.text);
        // }
    }

    public void CreateDropship(){
        StartCoroutine(DropshipMovementCoroutine());
    }

    IEnumerator DropshipMovementCoroutine(){
        SoundManager.instance.Play("dropship");

        dropship.SetActive(true);
        bool itemFlag = false;

        //var pos = SettingManager.instance.screenSize/2;
        Vector2 mainPos = CameraMovement.instance.transform.position;
        
        Vector2 mapBoundPos = new Vector2(mapBound.bounds.size.x/2f + 2f,Random.Range(mapBound.bounds.min.y + 5f,mapBound.bounds.max.y - 5f));
        
       //Debug.Log(SettingManager.instance.screenSize);
        Vector2 tempPos = Vector2.zero;
        //드랍쉽 생성 위치
        //Vector2 dropshipPos = new Vector2(10f,Random.Range(-2f,2f));
        //박스 생성 위치
        float boxPosX =  Random.Range(mapBound.bounds.min.x +2f, mapBound.bounds.max.x -2f); //new Vector2(mainPos.x + Random.Range(-4f,4f), mainPos.y + Random.Range(-1.5f,1.5f));
        //
        Debug.Log(boxPosX);
        int ranNum0 = Random.Range(0,2);
        if(ranNum0==0){ //왼쪽등장
            dropship.GetComponent<SpriteRenderer>().flipX = false;
            dropship.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            //dropship.transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = false;
            //dropship.transform.GetChild(1).transform.local = false;
            dropship.transform.position = -mapBoundPos;//new Vector2(pos.x-10,Random.Range(pos.y-2,pos.y+2));
            tempPos = dropship.transform.position;
            //destination.transform.position = new Vector2(-pos.x-300,Random.Range(-pos.y,+pos.y));
        }
        else{
            dropship.GetComponent<SpriteRenderer>().flipX = true;
            dropship.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            //dropship.transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = true;
            dropship.transform.position = mapBoundPos;
            //dropship.transform.position = new Vector2(pos.x+10,Random.Range(pos.y-2,pos.y+2));
            tempPos = dropship.transform.position;
        }

        Debug.Log("드랍쉽생성성공");
        //Debug.Log("드랍쉽 위치 : "+dropship.transform.position.x+"목적지 위치 : "+ (-tempPos.x));






        while((int)dropship.transform.position.x != (int)(-tempPos.x)){
        Debug.Log("이동중");
            dropship.transform.Translate((ranNum0 == 0 ? Vector3.right : Vector3.left) * Time.deltaTime * dropshipSpeed);

            if(!itemFlag&&(int)dropship.transform.position.x == (int)boxPosX){
                Debug.Log("아이템 드랍");
                
        SoundManager.instance.Play("drop");
                var clone = Instantiate(dropbox, dropship.transform.position , Quaternion.identity);

                itemFlag= true;
            }
            
                        yield return new WaitForFixedUpdate();

        }
        Debug.Log("이동완료후 제거");

        dropship.SetActive(false);
        itemFlag= false;


        //yield return null;
    }

    public void GetDropBox(GameObject box){
        //box.GetComponent<SpriteButton>().DestroySprite();
        Debug.Log("상자 삭제");
        SoundManager.instance.Play("rescue");

        boxCountText.text = "x "+(++boxCount).ToString();
        boxCountText2.text = "x " + boxCount.ToString();

        Destroy(box.transform.parent.gameObject);
    }

    //버프 남은 시간 세이브/로드
    public void CheckBuffState(){
        //스팀팩 잠금 해제
        UIManager.instance.ActivateLowerUIPanel(3);
        UIManager.instance.ActivateLowerUIPanel(4);

        // BuildingManager.instance.BuildingStateCheck(1);
        // BuildingManager.instance.BuildingStateCheck(4);



        for(int i=0;i<buffs.Count;i++){


            // if(buffs[i].buffImage!=null){

            //         buffs[i].buffImage.gameObject.SetActive(true);
            // }



            if(buffs[i].remainingCoolTime!=buffs[i].coolTime){
                if(buffs[i].name=="AD"){
                    
                StartCoroutine(ADCoolTimeCoroutine(buffs[i], true));
                }
                else{

                StartCoroutine(BuffCoolTimeCoroutine(buffs[i]));
                }
            }
            else{
                buffs[i].btn.GetChild(buffs[i].btn.childCount-2).gameObject.SetActive(false);
                //buffs[i].btn.GetChild(buffs[i].btn.childCount-2).GetComponent<Image>().fillAmount = 0;
                //buffs[i].btn.GetChild(buffs[i].btn.childCount-2).transform.GetChild(0).GetComponent<Text>().text ;
            }
            if(buffs[i].duration!=-1  && buffs[i].remainingDuration!=buffs[i].duration){
                
                //buffs[i].buffImage.gameObject.SetActive(true);
                StartCoroutine(BuffCoroutine(buffs[i]));
            }
        }
    }
    public void SetBoosterColor(string _color = "default"){
        //if(buffs[i].remainingDuration!=buffs[i].duration){
            //Debug.Log("부스터 색 변경 > "+_color);
        for(int i=0;i<PlayerManager.instance.boosters.Length;i++){
            switch(_color){
                case "red" : 
            //Debug.Log("f");
                    PlayerManager.instance.boosters[i].color =new Color(1,0,0,0.5f);
                    break;
                default :
                    PlayerManager.instance.boosters[i].color = new Color(1,1,1,0.5f);
                    break;
            }
        }
        
        Color newColor = PlayerManager.instance.boosters[0].color;

        if(BotManager.instance.botSaved.Count!=0){
            for(int i=0;i<BotManager.instance.botSaved.Count;i++){
                //Debug.Log(BotManager.instance.botSaved)
                var temp = botManager.GetChild(i).GetComponent<BotScript>();
                for(int j=0;j<3;j++){
                    //Debug.Log(i+"번 봇 "+j+"번 째 색변경 성공");
                    //botManager.GetChild(i).GetComponent<BotScript>().booster.gameObject.SetActive(true);
                    temp.boosters[j].color = newColor;
                    //botManager.GetChild(i).GetComponent<BotScript>().booster.gameObject.SetActive(false);
                    if(_color != "default"){

                        temp.booster.gameObject.SetActive(true);
                    }
                    else{
                        temp.booster.gameObject.SetActive(false);
                        
                    }
                    
                    // switch(_color){
                    //     case "red" : 
                    //         PlayerManager.instance.boosters[i].color = Color.red;
                    //         break;
                    //     default :
                    //         PlayerManager.instance.boosters[i].color = new Color(1,1,1,1);
                    //         break;
                    // }
                    
                }
                // switch(_color){
                //     case "red" : 
                //         PlayerManager.instance.boosters[i].color = Color.red;
                //         break;
                //     default :
                //         PlayerManager.instance.boosters[i].color = new Color(1,1,1,1);
                //         break;
                // }
            }
        }
    }
    public void SetSCVSpeed(float amount = 1){
        PlayerManager.instance.bonusSpeed = amount;
    }
    public void CreateRandomRP(){
        StartCoroutine(CreateRandomRPCoroutine());
    }
    public IEnumerator CreateRandomRPCoroutine(){
        yield return new WaitForSeconds(rpCoolTime);
        //var temp = mapBound.bounds;
        if(rpParent.childCount < maxRP){

            var temp = createArea[Random.Range(0,createArea.Length)].bounds;
            Vector3 ranPos =  new Vector3( Random.Range(temp.min.x, temp.max.x ),Random.Range(temp.min.y , temp.max.y ),0);
        
            var clone = Instantiate(rpBox, ranPos , Quaternion.identity);
            clone.transform.SetParent(rpParent);
            //Debug.Log("RP랜덤생성");

            if(autoGatherRP || buffs[5].count > 0){
                //StartCoroutine(AutoGatherRPCoroutine(clone.transform));
                if(buffs[5].count > 0){
                   //BuffManager.instance.buffs[4].count--; 
                    SubtractBuffCount(buffs[5]);
                }
                AutoGatherRP(clone.transform);
            }
        }
        

        StartCoroutine(CreateRandomRPCoroutine());

    }    
    public void AutoGatherRP(Transform clone){
        
        StartCoroutine(AutoGatherRPCoroutine(clone));
    }
    public IEnumerator AutoGatherRPCoroutine(Transform clone){
        //Debug.Log("자동 이동");
        while(clone!=null){
            clone.position = Vector2.MoveTowards(clone.position, PlayerManager.instance.transform.position, Time.deltaTime*2);
            yield return new WaitForFixedUpdate();
        }
//        PlayerManager.instance.GetItem(clone.gameObject);


    }
    public void SetMineralSize(float amount = 1){
        PlayerManager.instance.mineral.transform.localScale = new Vector2(amount,amount);
        PlayerManager.instance.bonusCapacity = amount;
        if(BotManager.instance.botSaved.Count!=0){
            for(int i=0;i<BotManager.instance.botSaved.Count;i++){
                var temp = botManager.GetChild(i).GetComponent<BotScript>();
                for(int j=0;j<3;j++){
                    temp.mineral.transform.localScale =  new Vector2(amount,amount);
                }
            }
        }
    }

    public void AutoChargeFuel(){
        if(!autoCharging){
            autoCharging = true;
            StartCoroutine(AutoChargeFuelCoroutine());
        }
    }

    IEnumerator AutoChargeFuelCoroutine(){
        //Debug.Log("오토차징");
        while(PlayerManager.instance.curFuel/PlayerManager.instance.maxFuel < 0.99f){

            PlayerManager.instance.HandleFuel(PlayerManager.instance.maxFuel * 0.075f);

            yield return new WaitForFixedUpdate();
        }
        autoCharging = false;
    }

    //     IEnumerator DropCoolTimeCoroutine(){
    //     buff.btn.GetComponent<Button>().interactable = false;
    //     var coolTimeImage = buff.btn.GetChild(buff.btn.childCount-2).GetComponent<Image>();
    //     var coolTimeText = buff.btn.GetChild(buff.btn.childCount-2).transform.GetChild(0).GetComponent<Text>();
    //     //buff.btn.GetComponent<Button>().interactable = false;//centerBuffBtn.interactable = false;
    //     coolTimeImage.gameObject.SetActive(true);//centerBuffImg.gameObject.SetActive(true);
    //     coolTimeText.text = buff.coolTime.ToString();//centerBuffText.text = buff.time.ToString();
        
        

    //     while(buff.remainingCoolTime>0f){  
            
    //         // buff.time--;

    //        // Debug.Log("TempTime : "+ tempTime);
    //         // centerBuffText.text = buff.time.ToString();
    //         // centerBuffImg.fillAmount = (float)buff.time / (float)full ;
    //         // Debug.Log(centerBuffImg.fillAmount);
    //         // yield return new WaitForSeconds(1f);

    //         buff.remainingCoolTime -= Time.deltaTime;
            
    //         coolTimeText.text = (Mathf.CeilToInt(buff.remainingCoolTime)).ToString();//centerBuffText.text = (Mathf.CeilToInt(tempTime)).ToString();
    //         coolTimeImage.fillAmount = buff.remainingCoolTime / buff.coolTime ;
    //         yield return new WaitForFixedUpdate();
    //     }

    //     //centerBuffBtn.interactable = true;
    //     buff.btn.GetComponent<Button>().interactable = true;
    //     coolTimeImage.gameObject.SetActive(false);//centerBuffImg.gameObject.SetActive(false);

    //     buff.remainingCoolTime = buff.coolTime;
    // }    
    IEnumerator ADCoolTimeCoroutine(Buff buff, bool firstLoad = false){
        //buff.btn.GetComponent<Button>().interactable = false;
        var coolTimeImage = buff.btn.GetChild(buff.btn.childCount-2).GetComponent<Image>();
        var coolTimeText = buff.btn.GetChild(buff.btn.childCount-2).transform.GetChild(0).GetComponent<Text>();
        //buff.btn.GetComponent<Button>().interactable = false;//centerBuffBtn.interactable = false;
        //Debug.Log("남은 카운트"+buff.count);
        if(buff.count<=0){
            //Debug.Log("실행");
            buff.btn.GetComponent<Button>().interactable = false;
            coolTimeImage.gameObject.SetActive(true);//centerBuffImg.gameObject.SetActive(true);
            coolTimeText.text = buff.coolTime.ToString();//centerBuffText.text = buff.time.ToString();
        }

        if(buff.remainingCoolTime==buff.coolTime || firstLoad){
            firstLoad = false;
            //Debug.Log("쿨타임 돌기");
            while(buff.remainingCoolTime>0f){  

                buff.remainingCoolTime -= Time.deltaTime;
                
                if(buff.count<=0){
                    coolTimeText.text = (Mathf.CeilToInt(buff.remainingCoolTime)).ToString();//centerBuffText.text = (Mathf.CeilToInt(tempTime)).ToString();
                    coolTimeImage.fillAmount = buff.remainingCoolTime / buff.coolTime ;
                }
                yield return new WaitForFixedUpdate();
            }
            buff.remainingCoolTime = buff.coolTime;
            buff.btn.GetComponent<Button>().interactable = true;
            coolTimeImage.gameObject.SetActive(false);
            buff.count++;
            buff.btn.GetChild(buff.btn.childCount-1).GetComponent<Text>().text = buff.count.ToString();
            if(buff.count!=5){
                StartCoroutine(ADCoolTimeCoroutine(buff));
            }
        }
        // buff.btn.GetComponent<Button>().interactable = true;
        // coolTimeImage.gameObject.SetActive(false);//centerBuffImg.gameObject.SetActive(false);
            //Debug.Log("코루틴 종료");

    }   
    public void AutoStimOff(){
        autoStimpack = false;
    }
    public void AutoStimOn(){
        autoStimpack = true;

    }
    public void RefreshUICount(){
        
        boxCountText.text = "x " + boxCount.ToString();
        boxCountText2.text = "x " + boxCount.ToString();
        for(int i=0;i<buffs.Count;i++){
            if(buffs[i].count > 0 || buffs[i].count==-1 ){
                if(buffs[i].count != -1){
                    buffs[i].btn.GetChild(buffs[i].btn.childCount-1).GetComponent<Text>().text = (buffs[i].count).ToString();

                }

                //     if(buffs[i].name=="AD"){
                        
                //         StartCoroutine(ADCoolTimeCoroutine(buffs[i]));
                //     }
                //     else{
                        
                //         StartCoroutine(BuffCoolTimeCoroutine(buffs[i]));
                //     }

                // if(buffs[i].duration!= -1){
                //     StartCoroutine(BuffCoroutine(buffs[i]));
                // }
            }
            
        }
        if(autoChargeFuel){
            buffs[4].btn.GetChild(buffs[4].btn.childCount-1).gameObject.SetActive(false);
        }
        if(autoGatherRP){
            buffs[5].btn.GetChild(buffs[5].btn.childCount-1).gameObject.SetActive(false);
        }
        
        
    }
    public void OpenBox_One(){
        if(boxCount >0){
            recallEffect.SetActive(true);
            randomBoxPanel.SetActive(true);
            boxCountText.text = "x "+(--boxCount).ToString();
            boxCountText2.text = "x " + boxCount.ToString();
            SoundManager.instance.Play("recall");
            
            SetRandomBox();

            Invoke("DelayOkBtn",0.7f);
        }
    }
    public void OpenBox_All(){
        StartCoroutine(OpenBox_All_Coroutine());
    }
    IEnumerator OpenBox_All_Coroutine(){
        if(boxCount >0){
        randomBoxPanel_All_Ok.SetActive(false);
            //recallEffect.SetActive(true);
            for(int i=0;i< randomBoxPanel_All_Grid.childCount;i++){
                randomBoxPanel_All_Grid.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                Text totalText = randomBoxPanel_All_Grid.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>();
                Text countText = randomBoxPanel_All_Grid.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>();
                Text bonusPtgText = randomBoxPanel_All_Grid.GetChild(i).GetChild(3).GetComponent<Text>();
                totalText.text = "0";
                countText.text = "0";
                bonusPtgText.text = "";

            }
            randomBoxPanel_All.SetActive(true);
            //boxCount = 0;
            int count = boxCount;
               // Debug.Log(count+"번 실행");
            SoundManager.instance.Play("recall");
            for(int i=0; i<count;i++){
                if(!SoundManager.instance.IsPlaying("recall")){
                    SoundManager.instance.Play("recall");

                }



                boxCountText.text = "x "+(--boxCount).ToString();
                boxCountText2.text = "x " + boxCount.ToString();
                randomBoxRemain.text = "남은 보급품 : "+ boxCount.ToString();
                    
                SetRandomBox(true);

                yield return new WaitForSeconds(0.01f);
                //}
            }

                yield return new WaitForSeconds(1f);
            //보너스 적용.
            for(int i=0;i< randomBoxPanel_All_Grid.childCount;i++){
                randomBoxPanel_All_Grid.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
                randomBoxPanel_All_Grid.GetChild(i).GetChild(0).gameObject.SetActive(false);
                randomBoxPanel_All_Grid.GetChild(i).GetChild(0).gameObject.SetActive(true);
            SoundManager.instance.Play("recall");
                
                Text totalText = randomBoxPanel_All_Grid.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>();
                Text countText = randomBoxPanel_All_Grid.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>();
                Text bonusPtgText = randomBoxPanel_All_Grid.GetChild(i).GetChild(3).GetComponent<Text>();

                float tempBonus = 0;
                switch(i){
                    case 0:
                        tempBonus = float.Parse(totalText.text) * PlayerManager.instance.moreSupply * 0.05f;
                        totalText.text = string.Format("{0:#,###0}", (float.Parse(totalText.text) + tempBonus));//(float.Parse(totalText.text) + tempBonus).ToString();
                        //totalText.text = (long.Parse(totalText.text) + tempBonus).ToString();//(int.Parse(totalText.text) + tempBonus).ToString();
                        bonusPtgText.text = "+"+(PlayerManager.instance.moreSupply * 5).ToString()+"%";
                        break;
                    case 1:
                        tempBonus = float.Parse(totalText.text) * PlayerManager.instance.moreSupply * 0.05f;
                        totalText.text = string.Format("{0:#,###0}", (float.Parse(totalText.text) + tempBonus));//(int.Parse(totalText.text) + tempBonus).ToString();
                        bonusPtgText.text = "+"+(PlayerManager.instance.moreSupply * 5).ToString()+"%";
                        break;
                    case 2:
                        tempBonus = PlayerManager.instance.moreSupply * int.Parse(countText.text);
                        totalText.text = (int.Parse(totalText.text) + tempBonus).ToString();
                        bonusPtgText.text = tempBonus.ToString();
                        break;
                    case 3:
                        tempBonus = PlayerManager.instance.moreSupply * int.Parse(countText.text);
                        totalText.text = (int.Parse(totalText.text) + tempBonus).ToString();
                        bonusPtgText.text = tempBonus.ToString();
                        break;
                }
                //tempResult[i] = long.Parse(totalText.text);
            }
            

            Invoke("DelayOkBtn_All",0.7f);
        }
    }
    //long[] tempResult;
    public void SubtractBuffCount(Buff buff){
        buff.btn.GetChild(buff.btn.childCount-1).GetComponent<Text>().text = (--buff.count).ToString();
    }
//오토알피 증가 시 이거 실행.
    public void SetAutoRemainRP(){
        if(!autoGatherRP &&buffs[5].count>0){
            for(int i=0;i<rpParent.childCount;i++){
                rpParent.GetChild(i).GetComponent<SpriteButton>().buildingType = BuildingType.None;
                AutoGatherRP(BuffManager.instance.rpParent.GetChild(i));
                buffs[5].count--;
            }
        }
    }
}
