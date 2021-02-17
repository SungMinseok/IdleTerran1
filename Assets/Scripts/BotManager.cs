using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Bot{
    public int index;
    public string name;
    public float efficiency;
    public int price;
    // public Bot(int a, string b, float c, float d){
    //     index = a;
    //     name = b;
    //     efficiency =c;
    //     price = d;

    // }
}
public class BotManager : MonoBehaviour
{
    public static BotManager instance;
    public Transform ready, activated;
    public List<Bot> botInfoList;
    public List<int> botSaved;
    public int maxPopulation = 5;
    public int populationLimit = 50;
    public int[] addPopulationPrices= new int[9];

    [Header("생산 위치")]
    public Transform factoryExit;
    public Transform centerExit;
    [Header("보급고 UI")]
    public Text populationText;
    public Text priceText;
    public GameObject AddBtn;
    public GameObject depotPanel;
    public GameObject upDone;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start(){
        //botSaved = new List<int>();
        //Debug.Log(botSaved.Count);
        RefreshPopulationState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshBotEquip(int num= -1){
        if(num == -1){
                
            for(int i=0;i<activated.childCount;i++){
                var temp =activated.GetChild(i).GetComponent<BotScript>();
                temp.speed = PlayerManager.instance.speed;
                temp.weldingSec = PlayerManager.instance.weldingSec + Random.Range(PlayerManager.instance.weldingSec * -0.01f, PlayerManager.instance.weldingSec * 0.01f);
                temp.capacity = Mathf.RoundToInt(temp.efficiency * PlayerManager.instance.capacity);
                if(temp.capacity==0) temp.capacity = 1;
                temp.fuelUsagePerWalk = temp.efficiency * PlayerManager.instance.fuelUsagePerWalk;
            }
        }
        else{
            
            var temp =activated.GetChild(num).GetComponent<BotScript>();
            temp.speed = PlayerManager.instance.speed;
            temp.weldingSec = PlayerManager.instance.weldingSec + Random.Range(PlayerManager.instance.weldingSec * -0.01f, PlayerManager.instance.weldingSec * 0.01f);
            temp.capacity = (int)(temp.efficiency * PlayerManager.instance.capacity);
            if(temp.capacity==0) temp.capacity = 1;
            temp.fuelUsagePerWalk = temp.efficiency * PlayerManager.instance.fuelUsagePerWalk;
        }
    }
    public void LoadBot(){
        
//        Debug.Log(botSaved.Count);
        //if(botSaved != null){
            StartCoroutine(LoadBotCoroutine());
        //}
    }
    IEnumerator LoadBotCoroutine(){
        if(botSaved != null){
            for(int i=0; i<botSaved.Count; i++){
                FactoryManager.instance.nowNum = botSaved[i];
                FactoryManager.instance.ProduceByLoad();
                yield return new WaitForSeconds(0.1f);
            }
            
            BuffManager.instance. CheckBuffState();
        }
        else{

            BuffManager.instance. CheckBuffState();
        }
    }

    public void DestroyBot(int index){
        botSaved.RemoveAt(index);
        transform.GetChild(index).GetComponent<BotScript>().DestroyBot();
        //Destroy(transform.GetChild(index));
        
        FactoryManager.instance.populationText.text = "인구수 : "+(BotManager.instance.botSaved.Count-1).ToString()+"/"+BotManager.instance.maxPopulation;
    }
    public void DestroyAllBot(){
        for(int i=0;i<transform.childCount;i++){
            botSaved.Clear();
            transform.GetChild(i).GetComponent<BotScript>().DestroyBot();
            
            //Destroy(transform.GetChild(i));
        }
        
        FactoryManager.instance.populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
    }
    
    public void RefreshPopulationState(){
        if(maxPopulation<populationLimit){
            //Debug.Log(maxPopulation/5 -1);
            //Debug.Log("5/5 " +5/5);
            priceText.text = addPopulationPrices[(maxPopulation/5-1) ].ToString();
            populationText.text = maxPopulation.ToString() + " >> " + (maxPopulation + 5).ToString();
            upDone.SetActive(false);
            AddBtn.SetActive(true);

        }
        else{
            priceText.text = "N/A";
            populationText.text = maxPopulation.ToString();
            upDone.SetActive(true);
            //AddBtn.SetActive(false);
        }

    }
    public void AddPopulation(){
        if(PlayerManager.instance.curMineral >= addPopulationPrices[(maxPopulation/5-1) ]){
            //depotPanel.SetActive(false);
            SoundManager.instance.Play("up");
            PlayerManager.instance.HandleMineral(-addPopulationPrices[(maxPopulation/5-1) ]);
            maxPopulation += 5;
            
            FactoryManager.instance.populationText.text = "인구수 : "+botSaved.Count.ToString()+"/"+maxPopulation;
            RefreshPopulationState();
        }
        else{
            SoundManager.instance.Play("notenoughmin");

        }
    }

    public void ActivateBot(int typeNum, int type = 0){
        var clone = ready.GetChild(0);

                        
                //var clone = Instantiate(robots[0],factoryPos.position,Quaternion.identity);
        clone.transform.localScale = FactoryManager.instance.childPanels[typeNum].GetChild(1).transform.localScale *6;
        clone.GetComponent<SpriteRenderer>().color = FactoryManager.instance.childPanels[typeNum].GetChild(1).GetComponent<Image>().color;
        clone.GetComponent<BotScript>().botState = BotState.Mine;
        clone.GetComponent<BotScript>().botType = typeNum;
        clone.GetComponent<BotScript>().efficiency = botInfoList[typeNum].efficiency;
        clone.parent = activated;
        BotManager.instance.RefreshBotEquip(activated.childCount-1);
                
        //BotManager.instance.botSaved.Add(num);
        if(type == 0){
            clone.position = factoryExit.position;
        }
        else if(type == 1){

            clone.position = centerExit.position;
        }



        clone.gameObject.name = "Bot_Type_"+typeNum;
        clone.gameObject.SetActive(true);

    }
    public void DeactivateBot(int typeNum){
        
        int tempIndex = botSaved.IndexOf(typeNum);
        Debug.Log(tempIndex+"번째 제거");
        BotManager.instance.botSaved.RemoveAt(tempIndex);
        //activated.GetChild(tempIndex).GetComponent<BotScript>().DestroyBot();
        var clone = activated.GetChild(tempIndex);
        clone.gameObject.name = "Bot_Ready";
        clone.parent = ready;
        clone.gameObject.SetActive(false);
    }
}
