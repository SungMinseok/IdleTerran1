using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


public class DBManager : MonoBehaviour
{    
    public static DBManager instance;
    public int loadCount;
    [Header ("닉네임")]
    public string nickName;
    public int debugVersion;
    public bool ActivateLoad;
    public bool loadComplete;
    public Text debugTimer;
    private void Awake()
    {
        instance = this;
        if(SettingManager.instance!=null){

            //debugVersion = SettingManager.instance.saveNum;
            nickName = SettingManager.instance.saveName;
        }
    }
    [System.Serializable]   //SL에 필수적 속성 : 직렬화. 컴퓨터가 읽고쓰기 쉽게.
    public class Data{
        public int loadCount;
        //현재 위치, 현재 시간, 현재 미네랄, 현재 연료, 현재 개스, 현재 장비 레벨, 튜토리얼 유무
        public float playerX,playerY,playerZ;
        public float timer; // UI
        // public float mineral;
        // public float fuel;
        // public float gas;
        
        
    [Header("기타 값 ( Save & Load )")]
        public bool helperDone;
        public long curMineral;
        public long curRP;
        public float curFuel;
        public long curCoin;


    [Header("장비 단계 ( Save & Load )")]
        public int weldingLevel;
        public int engineLevel;
        public int fuelLevel;
        public int bodyLevel;
        public int weightLevel;

        //건설
        public float[] buildTimeCounter;
        public bool[] unlockedNextBuilding;//건물 갯수
        
        //채취로봇
        public List<int> botSaved;

        //업그레이드
        public bool[] unlockedNextUpgrade;//업그레이드 패널 수 만큼, 0,1,2,3/4,5,6,7/8,9,10,11/12,13,14,15

        //팩토리
        public bool[] unlockedNextProduce;//scv종류만큼
        
        //랜덤상자
        public int boxCount;
        //보급
        public int maxPopulation = 5;
        //퀘스트
        //public int[] testArr;
        public int nowPhase;
        public List<int> questOverList;
        //버프
        public float[] remainingCoolTime;
        public float[] remainingDuration;
        public int[] count;
        //연구 단계
        public int fastCall;
        public int moreSupply;
        public int nowAccumulatedRP;
        public int investRP;


        //자동모드
        public bool autoChargeFuel;
        public bool autoGatherRP;
        //public bool autoStimpack;
        
        //캐쉬 구매 목록
        public int[] packageCheck;
        public int[] normalCheck;

        //설정
        public bool autoStimpack;
        public bool bgmState;
        public bool sfxState;
        public bool set_floating;
        public bool set_helpUI;

        //업적
        public int[] achvPhases;
        public long totalMineral;
        public long totalRP;
        public uint totalResearch; 
        public long totalClick; 
    }
    UIManager theUI;
    PlayerManager thePlayer;
    ShopManager theShop;
    AchvManager theAchv;
    public Data data;

    bool isPaused = false; 


    public void CallSave(int num){

        theUI=FindObjectOfType<UIManager>();
        thePlayer=FindObjectOfType<PlayerManager>();

        // data.playerX = thePlayer.transform.position.x;
        // data.playerY = thePlayer.transform.position.y;
        // data.playerZ = thePlayer.transform.position.z;
        data.loadCount = DBManager.instance.loadCount;

        data.timer = theUI.playTime;
        
    //[Header("기타 값 ( Save & Load )")]
        data.helperDone = thePlayer.helperDone;
        data.curMineral = thePlayer.curMineral;
        data.curFuel = thePlayer.curFuel;
        data.curRP = thePlayer.curRP;
        data.curCoin = thePlayer.curCoin;

    //[Header("장비 단계 ( Save & Load )")]
        data.weldingLevel = thePlayer.weldingLevel;
        data.engineLevel = thePlayer.engineLevel;
        data.fuelLevel = thePlayer.fuelLevel;
        data.bodyLevel = thePlayer.bodyLevel;

    //건설
        data.buildTimeCounter = BuildingManager.instance.buildTimeCounter;
        //Debug.Log(data.buildTimeCounter.Length);
        data.unlockedNextBuilding = BuildingManager.instance.unlockedNextBuilding;
 
    //채취로봇
        data.botSaved = BotManager.instance.botSaved;

    //업글
        data.unlockedNextUpgrade = UpgradeManager.instance.unlockedNextUpgrade;
    //팩토리
        data.unlockedNextProduce = FactoryManager.instance.unlockedNextProduce;

    //랜덤상자
        data.boxCount =BuffManager.instance.boxCount;
        

    //보급
        data.maxPopulation = BotManager.instance.maxPopulation;
        
    //퀘스트
        data.nowPhase = QuestManager.instance.nowPhase;
        data.questOverList = QuestManager.instance.questOverList;

    //버프
        data.remainingCoolTime = new float[BuffManager.instance.buffs.Count];
        data.remainingDuration = new float[BuffManager.instance.buffs.Count];
        data.count = new int[BuffManager.instance.buffs.Count];

        for(int i=0;i<BuffManager.instance.buffs.Count;i++){
            data.remainingCoolTime[i] = BuffManager.instance.buffs[i].remainingCoolTime;
            data.remainingDuration[i] = BuffManager.instance.buffs[i].remainingDuration;
            data.count[i] = BuffManager.instance.buffs[i].count;
        }

    //연구 단계
        data.fastCall = PlayerManager.instance.fastCall;
        data.moreSupply = PlayerManager.instance.moreSupply;
        data.nowAccumulatedRP = PlayerManager.instance.nowAccumulatedRP;
        data.investRP = PlayerManager.instance.investRP;
        
        //자동모드
        data.autoChargeFuel = BuffManager.instance.autoChargeFuel;
        data.autoGatherRP= BuffManager.instance.autoGatherRP;
        
        //캐쉬 구매 목록
        data.packageCheck = ShopManager.instance.packageCheck;
        data.normalCheck= ShopManager.instance.normalCheck;

        //설정
        data.autoStimpack= UIManager.instance.autoStimpack;
        data.bgmState= UIManager.instance.bgmState;
        data.sfxState= UIManager.instance.sfxState;
        data.set_floating= UIManager.instance.set_floating;
        data.set_helpUI= UIManager.instance.set_helpUI;

        //업적
        data.achvPhases = new int[AchvManager.instance.achvs.Length];
        for(int i=0;i<AchvManager.instance.achvs.Length;i++){
            data.achvPhases[i] = AchvManager.instance.achvs[i].phase;
        }

        data.totalMineral = AchvManager.instance.totalMineral;
        data.totalRP = AchvManager.instance.totalRP;
        data.totalResearch = AchvManager.instance.totalResearch;
        data.totalClick = AchvManager.instance.totalClick;

        BinaryFormatter bf = new BinaryFormatter();

        //원래 숫자로 확인_210413
        //FileStream file = debugVersion!=-1 ? File.Create(Application.persistentDataPath + "/SaveFile_"+debugVersion+".dat")
        FileStream file = nickName!="" ? File.Create(Application.persistentDataPath + "/SaveFile_"+nickName+".dat")
        : File.Create(Application.persistentDataPath + "/SaveFile.dat");
        bf.Serialize(file, data);
        file.Close();
    }

    public void CallLoad(int num){
        BinaryFormatter bf = new BinaryFormatter();
        //FileInfo fileCheck = new FileInfo(Application.persistentDataPath + "/SaveFile.dat");
        FileInfo fileCheck = nickName!="" ?  new FileInfo(Application.persistentDataPath + "/SaveFile_"+nickName+".dat")
        : new FileInfo(Application.persistentDataPath + "/SaveFile.dat");

//초기 설정
        theUI=FindObjectOfType<UIManager>();
        theUI.autoStimpack = false;

        theUI.bgmState = true;
        theUI.sfxState = true;
        theUI.set_floating = true;
        theUI.set_helpUI = true;


        if(fileCheck.Exists){
        //FileStream file = File.Open(Application.persistentDataPath + "/SaveFile.dat", FileMode.Open);
        FileStream file = nickName!="" ?  File.Open(Application.persistentDataPath + "/SaveFile_"+nickName+".dat", FileMode.Open)
        : File.Open(Application.persistentDataPath + "/SaveFile.dat", FileMode.Open);
        
            if(file != null && file.Length >0){

                data =(Data)bf.Deserialize(file);

                theUI=FindObjectOfType<UIManager>();
                thePlayer=FindObjectOfType<PlayerManager>();
                theShop=FindObjectOfType<ShopManager>();
                theAchv=FindObjectOfType<AchvManager>();

                //Debug.Log(Application.);
                // Vector3 vector =new Vector3(data.playerX, data.playerY, data.playerZ);
                // thePlayer.transform.position = vector;

                DBManager.instance.loadCount = data.loadCount + 1 ;

                theUI.playTime = data.timer;
                
            //[Header("기타 값 ( Save & Load )")]
                thePlayer.helperDone =data.helperDone;
                thePlayer.curMineral =data.curMineral;
                thePlayer.curFuel =data.curFuel;
                thePlayer.curRP =data.curRP;
                thePlayer.curCoin =data.curCoin;

            //[Header("장비 단계 ( Save & Load )")]
                thePlayer.weldingLevel =data.weldingLevel;
                thePlayer.engineLevel =data.engineLevel;
                thePlayer.fuelLevel =data.fuelLevel;
                thePlayer.bodyLevel =data.bodyLevel;
                thePlayer.weightLevel =data.weightLevel;

                //건설 // 배열은 널체크
            //     if(data.buildTimeCounter==null){//아예처음생성시 게임 내 데이터 배열 길이 맞춰줌.
            //    // Debug.Log("아예처음생성시 게임 내 데이터 배열 길이 맞춰줌.");
            //         data.buildTimeCounter = new float[BuildingManager.instance.buildTimeCounter.Length];
            //     }
            //     else if(data.buildTimeCounter.Length!=BuildingManager.instance.buildTimeCounter.Length){//게임내 배열 길이 증가한 경우 저장된값들까지만 로드
            //     //Debug.Log("게임내 배열 길이 증가한 경우 저장된값들까지만 로드");
                    
            //         for(int i=0;i<data.buildTimeCounter.Length;i++){
            //             BuildingManager.instance.buildTimeCounter[i] = data.buildTimeCounter[i];
            //         }
            //         data.buildTimeCounter = new float[BuildingManager.instance.buildTimeCounter.Length];

            //     }
            //     else{
            //     //Debug.Log("그대로 로드");
            //         BuildingManager.instance.buildTimeCounter = data.buildTimeCounter;
            //     }

                //BuildingManager.instance.unlockedNextBuilding = data.unlockedNextBuilding ;
                // if(data.unlockedNextBuilding==null){//아예처음생성시 게임 내 데이터 배열 길이 맞춰줌.
                //     data.unlockedNextBuilding = new bool[BuildingManager.instance.unlockedNextBuilding.Length];
                // }
                // else if(data.unlockedNextBuilding.Length!=BuildingManager.instance.unlockedNextBuilding.Length){//게임내 배열 길이 증가한 경우 저장된값들까지만 로드
                //     for(int i=0;i<data.unlockedNextBuilding.Length;i++){
                //         BuildingManager.instance.unlockedNextBuilding[i] = data.unlockedNextBuilding[i];
                //     }
                //     data.unlockedNextBuilding = new bool[BuildingManager.instance.unlockedNextBuilding.Length];

                // }
                // else{                        
                //     BuildingManager.instance.unlockedNextBuilding= data.unlockedNextBuilding;

                // }
                if(data.buildTimeCounter!=null) BuildingManager.instance.buildTimeCounter = data.buildTimeCounter;

                if(data.unlockedNextBuilding!=null) BuildingManager.instance.unlockedNextBuilding = data.unlockedNextBuilding;

                //채취로봇
                BotManager.instance.botSaved = data.botSaved;

                
                //업글
                //Debug.Log("a "+data.unlockedNextUpgrade.Length);
                if(data.unlockedNextUpgrade!=null) UpgradeManager.instance.unlockedNextUpgrade = data.unlockedNextUpgrade;

                //팩토리
                if(data.unlockedNextProduce!=null) FactoryManager.instance.unlockedNextProduce = data.unlockedNextProduce;

                //랜덤상자
                BuffManager.instance.boxCount = data.boxCount;

                //보급
                BotManager.instance.maxPopulation = data.maxPopulation;

                //퀘스트 //배열 : data는 초기화 하지말고 여기서 널체크 후 배열 길이 만들어주기.
                QuestManager.instance.nowPhase = data.nowPhase;
                if(data.questOverList!=null) QuestManager.instance.questOverList = data.questOverList;

                //버프

                // if(data.remainingCoolTime==null){//아예처음생성시 게임 내 데이터 배열 길이 맞춰줌.
                //     Debug.Log("0");
                //     data.remainingCoolTime = new float[BuffManager.instance.buffs.Count];
                //     data.remainingDuration = new float[BuffManager.instance.buffs.Count];
                //     data.count = new int[BuffManager.instance.buffs.Count];
                // }
                // else if(data.remainingCoolTime.Length!=BuffManager.instance.buffs.Count){//게임내 배열 길이 증가한 경우 저장된값들까지만 로드
                //     Debug.Log("1");
                //     for(int i=0;i<data.remainingCoolTime.Length;i++){
                //         BuffManager.instance.buffs[i].remainingCoolTime = data.remainingCoolTime[i];
                //         BuffManager.instance.buffs[i].remainingDuration = data.remainingDuration[i];
                //         BuffManager.instance.buffs[i].count = data.count[i];
                //     }
                //     data.remainingCoolTime = new float[BuffManager.instance.buffs.Count];
                //     data.remainingDuration = new float[BuffManager.instance.buffs.Count];
                //     data.count = new int[BuffManager.instance.buffs.Count];
                // }
                // else{                    
                //     Debug.Log("2");
                //     for(int i=0;i<BuffManager.instance.buffs.Count;i++){
                //         BuffManager.instance.buffs[i].remainingCoolTime = data.remainingCoolTime[i];
                //         BuffManager.instance.buffs[i].remainingDuration = data.remainingDuration[i];
                //         BuffManager.instance.buffs[i].count = data.count[i];
                //     }
                // }
                if(data.remainingCoolTime != null){
                    for(int i=0;i<data.remainingCoolTime.Length;i++){
                        
                        BuffManager.instance.buffs[i].remainingCoolTime = data.remainingCoolTime[i];
                        BuffManager.instance.buffs[i].remainingDuration = data.remainingDuration[i];
                        BuffManager.instance.buffs[i].count = data.count[i];
                    }
                }
                
            //연구 단계
                PlayerManager.instance.fastCall = data.fastCall;
                PlayerManager.instance.moreSupply = data.moreSupply;
                PlayerManager.instance.nowAccumulatedRP = data.nowAccumulatedRP;
                PlayerManager.instance.investRP = data.investRP;


                
        //자동모드
                BuffManager.instance.autoChargeFuel = data.autoChargeFuel;
                BuffManager.instance.autoGatherRP = data.autoGatherRP;

                
        //캐쉬 구매 목록
                theShop.packageCheck = data.packageCheck;
                theShop.normalCheck = data.normalCheck;
            
            
        //설정
        //Debug.Log("로드카운트 "+data.loadCount);
                //if(data.loadCount == 0 ){

                    // UIManager.instance.autoStimpack = false;
            
                    // UIManager.instance.bgmState = true;
                    // UIManager.instance.sfxState = true;
                    // UIManager.instance.set_floating = true;
                    // UIManager.instance.set_helpUI = true;
                //}
                //else{

                    theUI.autoStimpack = data.autoStimpack;
                    theUI.bgmState = data.bgmState;
                    theUI.sfxState = data.sfxState;
                    theUI.set_floating = data.set_floating;
                    theUI.set_helpUI = data.set_helpUI;
                //}
            //업적 <<<<<<<<<<<<<<<<<<<<<배열 로드 정석>>>>>>>>>>>>>>>>>>
            //Debug.Log(data.achvPhases.Length);
                if(data.achvPhases !=null){
                    for(int i=0;i<data.achvPhases.Length;i++){
                        theAchv.achvs[i].phase = data.achvPhases[i];                        
                    }
                }
                theAchv.totalMineral =data.totalMineral ;
                theAchv.totalRP =data.totalRP ;
                theAchv.totalResearch =data.totalResearch ;
                theAchv.totalClick =data.totalClick ;
            
            }




            file.Close();
        }
    }

    public void ResetDB(){
        //SettingManager.instance.testMode = true;
        
        PlayerManager.instance.curMineral = 10000000;
        PlayerManager.instance.curRP = 10000000;
        PlayerManager.instance.curFuel = PlayerManager.instance.defaultFuel;
        PlayerManager.instance.curSpeed = PlayerManager.instance. defaultSpeed;
        PlayerManager.instance.weldingLevel = 1;
        PlayerManager.instance.engineLevel = 1;
        PlayerManager.instance.fuelLevel = 1;
        PlayerManager.instance.bodyLevel = 1;

        //업글관련
        //UpgradeManager.instance.ApplyEquipsLevel();//업글패널 갱신
        UpgradeManager.instance.ResetUpgradePanelUI();//업글패널 초기화
        UpgradeManager.instance.ApplyEquipsLevel();
        PlayerManager.instance.RefreshEquip();//현재 장비 적용
        UpgradeManager.instance.ResetUnlocked();
        
        //건물 관련
        for(int i=0;i<BuildingManager.instance.buildings.Length;i++){
            
            BuildingManager.instance.buildTimeCounter[i] = 0;
            BuildingManager.instance.buildingsInMap[i].transform.GetChild(0).gameObject.SetActive(false);
            BuildingManager.instance.buildingsInMap[i].transform.GetChild(1).gameObject.SetActive(false);
            BuildingManager.instance.isConstructing[i]=false;
        }

        BuildingManager.instance.BuildingStateCheck();
        

        //봇 관련
        BotManager.instance.DestroyAllBot();

        //팩토리
        FactoryManager.instance.ResetData();
        
        //버프
        BuffManager.instance.boxCount = 100;

        //보급
        BotManager.instance.maxPopulation = 5;

        BuffManager.instance. RefreshUICount();
    }


    // public void DeleteSaveFile(){
        
    //     FileInfo fileCheck = new FileInfo(Application.persistentDataPath + "/SaveFile.dat");

    //     if(fileCheck.Exists){
    //         FileStream file = File.Delete(Application.persistentDataPath + "/SaveFile.dat");
    //     }
    // }
    // public Camera theCamera;       //보여지는 카메라.
 
    // private int resWidth;
    // private int resHeight;
    // string path;
    // // Use this for initialization
    // void Start () {
    //     resWidth = Screen.width;
    //     resHeight = Screen.height;
    //     path = Application.dataPath+"/ScreenShot/";
    //     Debug.Log(path);
    // }
 
    // public void ClickScreenShot()
    // {
    //     DirectoryInfo dir = new DirectoryInfo(path);
    //     if (!dir.Exists)
    //     {
    //         Directory.CreateDirectory(path);
    //     }
    //     string name;
    //     name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
    //     RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
    //     theCamera.targetTexture = rt;
    //     Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //     Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
    //     theCamera.Render();
    //     RenderTexture.active = rt;
    //     screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
    //     screenShot.Apply();
 
    //     byte[] bytes = screenShot.EncodeToPNG();
    //     File.WriteAllBytes(name, bytes);
    // }
#if DEV_MODE
int hour;
int minute;
int second;
    void FixedUpdate(){

        if(Input.GetKeyDown(KeyCode.F12)){
            //ScreenCapture.CaptureScreenshot("SomeLevel");
            StartCoroutine(captureScreenshot());
            Debug.Log("스샷");
            
        Debug.Log(Application.persistentDataPath);
        }

        UIManager.instance.playTime += Time.deltaTime;
        //day = (int)(playTime / 86400f) + 1;
        hour = (int)(UIManager.instance.playTime / 3600f)%24;
        minute = (int)(UIManager.instance.playTime / 60f)%60;
        second = (int)UIManager.instance.playTime % 60;
        debugTimer.text = string.Format("{0:00}:{1:00}:{2:00}", hour, minute,second); // <color=red>Day</color> 1 <color=red>/</color> 24:00
    
    }
    IEnumerator captureScreenshot()
    {
       yield return new WaitForEndOfFrame();

       string path = Application.persistentDataPath + "/Screenshots/";
       
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }

       string name = Application.persistentDataPath + "/Screenshots/"
                + Screen.width + "x" + Screen.height + "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") +".png";

       Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
       //Get Image from screen
       screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
       screenImage.Apply();
       //Convert to png
       byte[] imageBytes = screenImage.EncodeToPNG();

       //Save image to file
       System.IO.File.WriteAllBytes(name, imageBytes);
    }

#endif
}
