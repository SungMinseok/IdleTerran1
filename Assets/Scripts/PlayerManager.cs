using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;    
    void Awake(){
        instance = this;
        
        if(DBManager.instance.ActivateLoad){
            DBManager.instance.CallLoad(0);
            DBManager.instance.loadComplete = true;
        }


            }
    public GameObject autoPanel;
    public Color mineralColor;
    public Color rpColor;



    public Slider fuelBar;
    public Text mineralBar;
    public Text coinBar;
    Text rpBar;
        long calculated = 0;
        long calculatedRP = 0;
        long calculatedCoin = 0;

    //public float runSpeed = 4f;
    //private float defaultSpeed;


    //public float fuelUsagePerRun = 2f;

    //public float miningSpeed = 5f;
    [Header("기타 값 ( Save & Load )")]
    public bool helperDone;
    public long curMineral;
    public long curRP;//researchPoint
    public float curFuel;
    public long curCoin;
    [Header("기타 값")]
    public float curSpeed;
    public float bonusSpeed = 1;
    public float bonusCapacity = 1;
    
    [Header("장비 초기값 ( Fixed )")]
    public float fuelUsagePerWalk = 1f;
    public float crawlSpeed;
    public float defaultWeldingSec;
    public float defaultSpeed;
    public float defaultFuel;
    public int defaultCapacity;

    [Header("장비 단계 ( Save & Load )")]
    public int weldingLevel;
    public int engineLevel;
    public int fuelLevel;
    public int bodyLevel;
        public int weightLevel;

    [Header("장비 내용")]
    public float weldingSec = 2f;
    public float speed = 2f;
    public float maxFuel;
    public int capacity = 8;//적재량  

    // [HideInInspector] public Animator animator;
    // Animator animatorMother;
    // Animator animatorChild;
    // GameObject booster;
    // RaycastHit2D hitTemp;
    // public SpriteRenderer[] boosters;
    //  [SerializeField]private Vector2 defaultSide;
    //     [HideInInspector]public Vector2 movement;
    //     [HideInInspector]public Vector2 movementDirection;
    //     [HideInInspector]public bool canMove;
    //     [HideInInspector]public bool isAlive;
    //     [HideInInspector]public bool isHolding;//뭔가 들고 있을 때
    //     [HideInInspector]public bool isMining;//채취 중일 때
    // private bool miningFlag;
    // [Header("이동 관련")]//auto 버튼 눌렀을 때
    //     public bool isAuto;
    //     public bool gotMine;//미네랄 발견
    //     public bool gotDestination;//센터 발견
    //     [HideInInspector]public bool placeFlag;
    //     [HideInInspector]public SpriteRenderer mineral;
    //     public GameObject workLight;
        public Transform centerPos;
        // Transform mineralPos;
        // public GameObject miningMineral;
    //public Transform destination;
    //IEnumerator miningCoroutine;
    //bool onX;
    //bool onY;
    //public GameObject effect;
    [Header("지점 이동")]//auto 버튼 눌렀을 때
        public bool goTo;
        public bool buildStart;
        public Transform selectedMineral;
    [Header("UI")]//auto 버튼 눌렀을 때
        public string enterableBuilding;


    [Header("연구 단계")]
    public int fastCall;
    public int moreSupply;
    public int maxAccumulatedRP = 5000;
    public int nowAccumulatedRP;
    public int investRP;

    bool getItemFlag;
    void Start()
    {


        //체력
        fuelBar = UIManager.instance.fuelBar;
        if(fuelBar != null) fuelBar.value = (float) curFuel / (float) maxFuel;

        //미네랄
        mineralBar = UIManager.instance.minText;
        rpBar = UIManager.instance.rpText;
        coinBar = UIManager.instance.coinText;


        // //LoadData();
        //초기설정
        if(!DBManager.instance.loadComplete){

            weldingSec = defaultWeldingSec;
            curSpeed = defaultSpeed;
            maxFuel = defaultFuel;
            capacity = defaultCapacity;

            curMineral = 0;
            
            curFuel = 300;
            curRP = 0;
            
            mineralBar.text = "0";
            rpBar.text = "0";
            coinBar.text = "0";
        }

            fuelBar.value = (float) curFuel / (float) maxFuel;
        UIManager.instance.fuelPercentText.text = (Mathf.RoundToInt(fuelBar.value*100)).ToString() + "%";
        RefreshEquip();

         //채취로봇
        BotManager.instance.LoadBot();

        //팩토리
        FactoryManager.instance. ApplyUnlocked();

        
        
    }

    void Update()
    {
#region PC 이동
#endregion
#region PC 공격
#endregion
#region Fuel
#endregion
#region Auto Mode
#endregion

#region SetMineral
        //if(int.Parse(mineralBar.text)!=curMineral){
        if(calculated!=curMineral){
            long temp = curMineral-calculated;
            //Debug.Log(temp);
            if(temp>=10 || temp <=-10){
                
                //calculated= (int.Parse(mineralBar.text) + temp/10);
                calculated= calculated + (long)(temp/10);
            }
            else{
                
                //calculated= temp>0 ? (int.Parse(mineralBar.text) + 1) : (int.Parse(mineralBar.text) - 1);
                calculated= temp>0 ? calculated + 1 : calculated - 1;

            }
            mineralBar.text = string.Format("{0:#,###0}", calculated);
            AchvManager.instance.RefreshAchv(0);
            
            //mineralBar.text = ((int)Mathf.Lerp(int.Parse(mineralBar.text), curMineral, Time.deltaTime *speed )).ToString();
        }
        else{
            SoundManager.instance.Stop("fill");
        }
#endregion
#region SetRP
        if(calculatedRP!=curRP){
            long temp = curRP-calculatedRP;
            if(temp>=10 || temp <=-10){
                calculatedRP= calculatedRP + (long)(temp/10);
            }
            else{
                calculatedRP= temp>0 ? calculatedRP + 1 : calculatedRP - 1;

            }
            rpBar.text = string.Format("{0:#,###0}", calculatedRP);
            AchvManager.instance.RefreshAchv(1);
        }
#endregion
#region SetCoin
        if(calculatedCoin!=curCoin){
            long temp = curCoin-calculatedCoin;
            if(temp>=10 || temp <=-10){
                calculatedCoin= calculatedCoin + (long)(temp/10);
            }
            else{
                calculatedCoin= temp>0 ? calculatedCoin + 1 : calculatedCoin - 1;

            }
            coinBar.text = string.Format("{0:#,###0}", calculatedCoin);
            //AchvManager.instance.RefreshAchv(1);
        }
#endregion

    }


    void FixedUpdate(){        
        if(curFuel<=0){
            if(BuffManager.instance.autoChargeFuel || BuffManager.instance.buffs[4].count > 0){
                if(BuffManager.instance.buffs[4].count > 0){
                   //BuffManager.instance.buffs[4].count--; 
                   BuffManager.instance.SubtractBuffCount(BuffManager.instance.buffs[4]);
                }
                BuffManager.instance.AutoChargeFuel();
            }
            else{
                if(!UIManager.instance.alertPopup.activeSelf){
                UIManager.instance.SetPopUp("연료가 부족합니다. 연료통을 클릭해 충전하세요.","outofgas");

                }
            }
        }
        
    }


    public void HandleFuel(float amount, bool lerp=true){
        if(curFuel>=0 && curFuel<=maxFuel){
            curFuel += amount;
//Debug.Log("a");
        }
        else if(curFuel<0){
            curFuel = 0f;
            fuelBar.value = 0;
            //canMove = false;
            //animator.SetFloat("Speed", 0f);
//Debug.Log("b");


            //Debug.Log("앵꼬");
        }
        else if(curFuel>maxFuel){
            curFuel = maxFuel;
            //Debug.Log("풀차지");
        }
        if(lerp){
                
            fuelBar.value = Mathf.Lerp(fuelBar.value,(float) curFuel / (float) maxFuel, Time.deltaTime*10 );
        }
        else{
            fuelBar.value = (float) curFuel / (float) maxFuel;
        }
        UIManager.instance.fuelPercentText.text = (Mathf.RoundToInt(fuelBar.value*100)).ToString() + "%";
        
    }

    public void HandleMineral(long amount = 0,bool floating = true){
        //int temp0 = curMineral;
        //int preMineral = curMineral;
        // if(amount==0){
        //     float temp = Mathf.Ceil(capacity * bonusCapacity);
        //     switch(packageType){
        //         case PackageType.normal :
        //             curMineral += (long)temp;
        //             AchvManager.instance.totalMineral +=(long)temp;
        //             break;
        //         default :
        //             break;
        //     }
        //     if(UIManager.instance.set_floating) {

        //         if(floating) UIManager.instance.PrintFloating("+ "+temp.ToString(), transform);
        //     }
        // }
        // else{
            curMineral += amount;
            
            if(UIManager.instance.set_floating) {
                if(floating){
                    if(amount>=0){
                         UIManager.instance.PrintFloating("+ "+amount.ToString(), transform);
                    }
                    else{
                        //PrintFloating("- "+(-amount).ToString());
                    }
                }
            }
        //}

        
    }    
    public void HandleRP(long amount = 0,bool floating = true){
        if(amount==0){
        }
        else{
            AchvManager.instance.totalRP += amount;
            curRP += amount;
        }

    }

    //장비 레벨 현재 장비에 적용
    public void RefreshEquip(int num = -1){
        if(num==-1){

            weldingSec = defaultWeldingSec + (weldingLevel-1) * UpgradeManager.instance.upgradeList[0].upgradeDelta;
            speed = defaultSpeed + (engineLevel-1) * UpgradeManager.instance.upgradeList[1].upgradeDelta;
            maxFuel = defaultFuel + (fuelLevel-1)  * UpgradeManager.instance.upgradeList[2].upgradeDelta;
            capacity = defaultCapacity + (bodyLevel-1) * (int)UpgradeManager.instance.upgradeList[3].upgradeDelta;

            //for(int i=0;i<4;i++){
                // FactoryManager.instance.motherStatusText[0].text = weldingSec.ToString() + " 초";
                // FactoryManager.instance.motherStatusText[1].text = speed.ToString();
                // FactoryManager.instance.motherStatusText[2].text = string.Format("{0:#,###0}",maxFuel);
                // FactoryManager.instance.motherStatusText[3].text = string.Format("{0:#,###0}",capacity);
            //}
            FactoryManager.instance.RefreshEquipStatus();
        }
        else{

        }

        BotManager.instance.RefreshBotEquip();
    }    
    public void RepSound(){
        SoundManager.instance.Play("rep"+Random.Range(0,5));
    }    
    public void YesSound(){
        SoundManager.instance.Play("SCYes"+Random.Range(0,5));
    }
    


    // void SaveData(string key){
    //     switch(key){
    //         case "curMineral" : 
    //             PlayerPrefs.SetInt(key, curMineral);
    //             break;
    //         case "helperDone" : 
    //             PlayerPrefs.SetInt(key, helperDone);
    //             break;

    //     }
        
    //     //Debug.Log("저장성공");
    // }
    // public void PrintFloating(string text, Sprite sprite = null, int type = 0)//0: 미네랄, 1 : RP
    // {

    //     if (text != "")
    //     {
    //         //var clone = Instantiate(floatingText, floatingCanvas.transform.position, Quaternion.identity);
    //         var clone = Instantiate(floatingText, new Vector2(transform.position.x,transform.position.y+0.3f), Quaternion.identity);
    //         clone.transform.GetChild(0).GetComponent<Text>().text = text;
    //         if(type==0)
    //             clone.transform.GetChild(0).GetComponent<Text>().color = mineralColor;
    //         else
    //             clone.transform.GetChild(0).GetComponent<Text>().color = rpColor;

            
    //         if(floatingCanvas.transform.childCount>=1){

    //             clone.GetComponent<Canvas>().sortingOrder = floatingCanvas.transform.GetChild(floatingCanvas.transform.childCount-1).transform.GetComponent<Canvas>().sortingOrder+1;
    //         }
    //         clone.transform.SetParent(floatingCanvas.transform);
    //     }
    //     // else
    //     // {
    //     //     var clone = Instantiate(floatingImage, floatingCanvas.transform.position, Quaternion.identity);
    //     //     clone.GetComponent<FloatingText>().image.sprite = sprite;
    //     //     clone.transform.SetParent(floatingCanvas.transform);
    //     // }
    // }


//         public Texture2D CopyTexture2D(Texture2D copiedTexture)
//     {
//                //Create a new Texture2D, which will be the copy.
//         Texture2D texture = new Texture2D(copiedTexture.width, copiedTexture.height);
//                //Choose your filtermode and wrapmode here.
//         texture.filterMode = FilterMode.Point;
//         texture.wrapMode = TextureWrapMode.Clamp;
 
//         int y = 0;
//         while (y < texture.height)
//         {
//             int x = 0;
//             while (x < texture.width)
//             {
//                                //INSERT YOUR LOGIC HERE
//                 if(copiedTexture.GetPixel(x,y) ==  new Color32(222, 222, 222, 255))
//                 {
//                                        //This line of code and if statement, turn Green pixels into Red pixels.
//                     texture.SetPixel(x, y,Color.red);
//                     Debug.Log("1");
//                 }
//                 else
//                 {
//                                //This line of code is REQUIRED. Do NOT delete it. This is what copies the image as it was, without any change.
//                 texture.SetPixel(x, y, copiedTexture.GetPixel(x,y));
//                 //Debug.Log("2");
//                 }
//                 ++x;
//             }
//             ++y;
//         }
//                 //Name the texture, if you want.
//         //texture.name = (Species+Gender+"_SpriteSheet");
 
//                //This finalizes it. If you want to edit it still, do it before you finish with .Apply(). Do NOT expect to edit the image after you have applied. It did NOT work for me to edit it after this function.
//         texture.Apply();
 
// //Return the variable, so you have it to assign to a permanent variable and so you can use it.
//         return texture;
//     }
 
// public void UpdateCharacterTexture()
//     {
//         Sprite[] loadSprite = Resources.LoadAll<Sprite> (spritePath);
//         characterTexture2D = CopyTexture2D(loadSprite[0].texture);
 
//         int i = 0;
//         while(i != characterSprites.Length)
//         {
//             //SpriteRenderer sr = GetComponent<SpriteRenderer>();
//             //string tempName = sr.sprite.name;
//             //sr.sprite = Sprite.Create (characterTexture2D, sr.sprite.rect, new Vector2(0,1));
//             //sr.sprite.name = tempName;
 
//             //sr.material.mainTexture = characterTexture2D;
//             //sr.material.shader = Shader.Find ("Sprites/Transparent Unlit");
//             string tempName = characterSprites[i].name;
//             characterSprites[i] = Sprite.Create (characterTexture2D, characterSprites[i].rect, new Vector2(0,1));
//             characterSprites[i].name = tempName;
//             names[i] = tempName;
//             ++i;
//         }
 
//         SpriteRenderer sr = GetComponent<SpriteRenderer>();
//         sr.material.mainTexture = characterTexture2D;
//         sr.material.shader = Shader.Find ("Transparent Unlit");
 
//     }


    // void LoadData(){

    //     //helperDone= PlayerPrefs.GetInt("helperDone", helperDone);
    //     curMineral= PlayerPrefs.GetInt("curMineral", curMineral);
        
    //     //Debug.Log("로드성공");
    // }    

    public void GameStart(){
        SoundManager.instance.Play("ready");
        UIManager.instance.StartTimer();
    }    
    
    // public void Order(Transform where, OrderType type){
    //     StopAuto();
    //     autoPanel.SetActive(true);
    //     CMCamera.SetActive(true);
    //     orderType = type;
    //     destination = where;
    //     if(type == OrderType.Build){
                
    //         SoundManager.instance.Play("btn1");
    //         isAuto = false;
    //         goTo = true;    
    //         YesSound();
    //         //destination = GameObject.Find(where).transform;
    //     }        
    //     if(type == OrderType.Enter){
                
    //         SoundManager.instance.Play("btn1");
    //         isAuto = false;
    //         goTo = true;    
    //         YesSound();
    //         //destination = GameObject.Find(where).transform;
    //     }
    //     else if(type == OrderType.Stop){

    //     }
    //     else if(type == OrderType.Get){
            
    //         SoundManager.instance.Play("btn1");
    //         isAuto = false;
    //         goTo = true;    
    //         YesSound();
    //         //destination = GameObject.Find(where).transform;
    //     }
    // }

//     public void GetItem(GameObject item){
//         //box.GetComponent<SpriteButton>().DestroySprite();
//         //Debug.Log("상자 삭제");
//         int tempAmount = 0;
// //Debug.Log("GETITEM");
// //Instantiate(effect, item.transform.position, Quaternion.identity);

//         SoundManager.instance.Play("rescue");
//         switch(item.GetComponent<SpriteButton>().objectName){
//             case "RP" :
//                 tempAmount = 100*(investRP+1);
//                 curRP += tempAmount;
//                 if(UIManager.instance.set_floating) UIManager.instance.PrintFloating("+ "+tempAmount.ToString(), centerPos,null,1);//PrintFloating("+ "+tempAmount.ToString(),null,1);
//                 if(!BuffManager.instance.autoGatherRP) StopAuto();
//                 break;
//         }
//         //boxCountText.text = "x "+(++boxCount).ToString();

                        
//         Destroy(item);
//                 //getItemFlag = false;

//     }

}
