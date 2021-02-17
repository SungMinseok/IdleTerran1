using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public GameObject shopPanel;
    
    [Header("구매 성공 패널")]
    public GameObject successBuyPanel;
    public Text contentNameText;
    public Text contentsText;
    [Header("패널")]
    public GameObject[] panels;
    public GameObject[] panelBtns;


    [Header("구매여부")]
    public int[] packageCheck;
    public int[] normalCheck;

    [Header("패키지 TEXT")]
    public Transform packageParent;
    Transform[] packageChild;
    public Text[] orgPrice0;
    public Text[] disPercent0;
    public Text[] price0;
    public Text[] des0;
    public string[] orgPrice0Text;
    public string[] disPercent0Text;
    public string[] price0Text;
    [TextArea(1,3)]
    public string[] des0Text;
    [Header("아이템 TEXT")]
    public Transform normalParent;
    Transform[] normalChild;
    Text[] name1;
     Text[] price1;
     Text[] des1;
     Image[] image1;
    public string[] name1Text;
    public string[] price1Text;
    [TextArea(1,3)]
    public string[] des1Text;

    [Header("코인 TEXT")]
    public Transform coinParent;
    Transform[] coinChild;
    public Text[] orgPrice2;
    public Text[] disPercent2;
    public Text[] price2;
    public string[] orgPrice2Text;
    public string[] disPercent2Text;
    public string[] price2Text;
    [Header("아이템/패키지 구매 패널")]
    public GameObject buyCheckPanel;
    public Text nameText_check;
    public Text desText_check;
    public Image itemImage_check;
    public Text amountText_check;
    public Text priceText_check;
    public InputField inputFieldAmount;
    public GameObject autoText;
    public Sprite packageSprite;
    public int amount_check;
    public int price_check;
    public int defaultPrice;
    

    void Awake(){
        instance = this;
    }
    void Start(){
        inputFieldAmount.onEndEdit.AddListener(delegate { SetInputAmount(); });



        // packageCheck = new bool[packageParent.childCount];
        // normalCheck = new bool[normalParent.childCount];
        packageChild = new Transform[packageParent.childCount];
        normalChild = new Transform[normalParent.childCount];
        for(int i=0;i<packageParent.childCount;i++){
            packageChild[i] = packageParent.GetChild(i);
        }
        for(int i=0;i<normalParent.childCount;i++){
            normalChild[i] = normalParent.GetChild(i);
        }

        for(int i=0;i<packageParent.childCount;i++){
            des0[i] = packageChild[i].GetChild(1).GetComponent<Text>();
            orgPrice0[i] = packageChild[i].GetChild(5).GetChild(1).GetComponent<Text>();
            price0[i] = packageChild[i].GetChild(4).GetChild(0).GetComponent<Text>();
            disPercent0[i] = packageChild[i].GetChild(6).GetChild(0).GetComponent<Text>();

            des0[i].text = des0Text[i];
            orgPrice0[i].text = orgPrice0Text[i];
            price0[i].text = price0Text[i];
            disPercent0[i].text = disPercent0Text[i];
            //orgPriceText[i] = grid0.GetChild();
        }

    name1 = new Text[normalChild.Length];
    price1 = new Text[normalChild.Length];
    des1= new Text[normalChild.Length];
    image1 = new Image[normalChild.Length];

        for(int i=0;i<normalParent.childCount;i++){
            name1[i] = normalChild[i].GetChild(1).GetComponent<Text>();
            des1[i] = normalChild[i].GetChild(1).GetComponent<Text>();
            price1[i] = normalChild[i].GetChild(4).GetChild(0).GetComponent<Text>();
            image1[i] = normalChild[i].GetChild(3).GetChild(0).GetComponent<Image>();

            name1[i].text = name1Text[i];
            des1[i].text = des1Text[i];
            price1[i].text = price1Text[i];
        }
        RefreshShopUI();

        OpenPanel(0);
    }
    public void OpenPanel(int num){
        for(int i=0;i<3;i++){
            panels[i].SetActive(false);
            panelBtns[i].SetActive(false);
        }

        panels[num].SetActive(true);
        panelBtns[num].SetActive(true);
    }
    public void RefreshShopUI(){
//패키지 한번만 구매 가능
        // if(packageCheck[0]==1){
            
        //     normalChild[0].GetChild(normalChild[0].childCount-2).gameObject.SetActive(true);
        //     packageChild[0].GetChild(packageChild[0].childCount-1).gameObject.SetActive(true);
        // }

        // if(normalCheck[0]==1){
            
        //     normalChild[0].GetChild(normalChild[0].childCount-1).gameObject.SetActive(true);
        //     packageChild[0].GetChild(packageChild[0].childCount-2).gameObject.SetActive(true);
        // }
    }
    public void PurchaseFailure(){
        Debug.Log("구입 실패");
    }
    public void PurchaseSuccess(){
        Debug.Log("구입 성공");
    }
    //public void 

    public void Buy(string code, int amount = 1){//000 001 002 003 004 , 100 101 102 103 104 105 106
        SoundManager.instance.Play("rescue");

        //shopPanel.SetActive(false);
        //contentNameText.text = name;
        int type = int.Parse(code)/100;
        int num = int.Parse(code)%100;

        if(type == 0){
        //successBuyPanel.SetActive(true);
            
            //packageCheck[num] = true;
            packageCheck[num] +=1;
            UIManager.instance.SetPopUp("구매 성공! : <color=#C0F678>"+ packageChild[num].GetChild(0).GetComponent<Text>().text + "</color>, " 
            + inputFieldAmount.text +"개","rescue");
            //contentNameText.text = packageChild[num].GetChild(0).GetComponent<Text>().text;
            //contentNameText.color = packageChild[num].GetChild(0).GetComponent<Text>().color;
            //contentsText.text = packageChild[num].GetChild(1).GetComponent<Text>().text;
        }
        else{
            normalCheck[num] +=1;
            // contentNameText.text = normalChild[num].GetChild(0).GetComponent<Text>().text;
            // contentNameText.color = normalChild[num].GetChild(0).GetComponent<Text>().color;
            // contentsText.text = normalChild[num].GetChild(1).GetComponent<Text>().text;
            //if(num==0||num==1){
                UIManager.instance.SetPopUp("구매 성공! : <color=#C0F678>"+ name1Text[num] + "</color>, " + inputFieldAmount.text +"개","rescue");
            //}
            //else if(num==2||num==3){
            //    UIManager.instance.SetPopUp("구매 성공! : "+ name1Text[num] + " " + (price_check*25).ToString()  +"개","rescue");

            //}<color=#C0F678>패</color>키지

        }

        // packageCheck[0] = true;
        // contentNameText.text = packageChild[0].GetChild(0).GetComponent<Text>().text;
        // contentNameText.color = packageChild[0].GetChild(0).GetComponent<Text>().color;
        // contentsText.text = packageChild[0].GetChild(1).GetComponent<Text>().text;
        int tempPow= (int)Mathf.Pow(2,num);
        switch(code){
            case "000" :
            
                // BuffManager.instance.autoChargeFuel = true;
                // BuffManager.instance.autoGatherRP = true;
                // for(int i=0;i<BuffManager.instance.rpParent.childCount;i++){
                //     BuffManager.instance.rpParent.GetChild(i).GetComponent<SpriteButton>().buildingType = BuildingType.None;
                //     BuffManager.instance.AutoGatherRP(BuffManager.instance.rpParent.GetChild(i));
                // }

                BuffManager.instance.buffs[0].count+=10*tempPow*amount;
                BuffManager.instance.boxCount+=10*tempPow*amount;
                BuffManager.instance.buffs[4].count+=25*tempPow*amount;
                BuffManager.instance.buffs[5].count+=25*tempPow*amount;


                // UIManager.instance.ActivateLowerUIPanel(5);
                // UIManager.instance.ActivateLowerUIPanel(6);
                break;
            case "001" :
                BuffManager.instance.buffs[0].count+=10*tempPow*amount;
                BuffManager.instance.boxCount+=10*tempPow*amount;
                BuffManager.instance.buffs[4].count+=25*tempPow*amount;
                BuffManager.instance.buffs[5].count+=25*tempPow*amount;

                break;
            case "002" :
                BuffManager.instance.buffs[0].count+=10*tempPow*amount;
                BuffManager.instance.boxCount+=10*tempPow*amount;
                BuffManager.instance.buffs[4].count+=25*tempPow*amount;
                BuffManager.instance.buffs[5].count+=25*tempPow*amount;

                break;
            case "003" :
                BuffManager.instance.buffs[0].count+=10*tempPow*amount;
                BuffManager.instance.boxCount+=10*tempPow*amount;
                BuffManager.instance.buffs[4].count+=25*tempPow*amount;
                BuffManager.instance.buffs[5].count+=25*tempPow*amount;

                break;
            case "004" :
                
                BuffManager.instance.buffs[0].count+=10*tempPow*amount;
                BuffManager.instance.boxCount+=10*tempPow*amount;
                BuffManager.instance.buffs[4].count+=25*tempPow*amount;
                BuffManager.instance.buffs[5].count+=25*tempPow*amount;

                break;
            case "100" :
            
                BuffManager.instance.buffs[0].count+=amount;
                // BuffManager.instance.autoChargeFuel = true;
                // BuffManager.instance.autoGatherRP = true;
                // for(int i=0;i<BuffManager.instance.rpParent.childCount;i++){
                //     BuffManager.instance.rpParent.GetChild(i).GetComponent<SpriteButton>().buildingType = BuildingType.None;
                //     BuffManager.instance.AutoGatherRP(BuffManager.instance.rpParent.GetChild(i));
                // }
                break;
            case "101" :
                
                BuffManager.instance.boxCount+=amount;
                //BuffManager.instance.buffs[0].count+=50;
                //BuffManager.instance.buffs[1].count+=50;
                break;
            case "102" :
                BuffManager.instance.buffs[4].count+=amount;
                
                break;
            case "103" :
                BuffManager.instance.buffs[5].count+=amount;

                break;
            case "104" :
                
                BuffManager.instance.boxCount+=10;
                break;
            case "105" :
                
                BuffManager.instance.boxCount+=20;
                break;
            case "106" :
                
                BuffManager.instance.boxCount+=40;
                break;

        }

        BuffManager.instance.RefreshUICount();
        RefreshShopUI();

    }
    public void BuyCoin(int num){
        int tempPlus = 0;
        switch(num){
            case 0:
                tempPlus=10;
                break;
            case 1:
                tempPlus=50;
                break;
            case 2:
                tempPlus=100;
                break;
            case 3:
                tempPlus=500;
                break;
        }
        PlayerManager.instance.curCoin+=tempPlus;
        UIManager.instance.SetPopUp("구매 성공! : <color=yellow>코인</color> "+tempPlus.ToString(),"rescue");


    }
    string code;
    public void OpenBuyCheckPanel_Package(int num){
        if(num<10) code = "00"+num.ToString();

            nameText_check.text = packageChild[num].GetChild(0).GetComponent<Text>().text;
            nameText_check.color = packageChild[num].GetChild(0).GetComponent<Text>().color;
        desText_check.text = des0Text[num];
        itemImage_check.sprite = packageSprite;
        itemImage_check.color = Color.white;
        itemImage_check.rectTransform.sizeDelta = new Vector2(200,200);
            autoText.SetActive(false);
        // if(num==0||num==1){
        //     autoText.SetActive(false);
        // }
        // else if(num==2||num==3){
        //     autoText.SetActive(true);
        // }
        defaultPrice = int.Parse(price0[num].text);
        amount_check = 1;
        price_check = defaultPrice;

        inputFieldAmount.text = amount_check.ToString();
        priceText_check.text = price_check.ToString();
        buyCheckPanel.SetActive(true);
    }
    public void OpenBuyCheckPanel_Item(int num){
        if(num<10) code = "10"+num.ToString();

        nameText_check.text = name1Text[num];
        nameText_check.color = Color.white;
        desText_check.text = des1Text[num];
        itemImage_check.sprite = image1[num].sprite;
        itemImage_check.color = image1[num].color;
        itemImage_check.rectTransform.sizeDelta = image1[num].rectTransform.sizeDelta;
        if(num==0||num==1){
            autoText.SetActive(false);
        }
        else if(num==2||num==3){
            autoText.SetActive(true);
        }
        
        defaultPrice = int.Parse(price1[num].text);
        amount_check = 1;
        price_check = defaultPrice;

        inputFieldAmount.text = amount_check.ToString();
        priceText_check.text = price_check.ToString();
        buyCheckPanel.SetActive(true);
    }
    public void AmountAddBtn(){
        
        inputFieldAmount.text = (++amount_check).ToString();
        priceText_check.text = (amount_check * defaultPrice).ToString();

    }
    public void AmountSubBtn(){
        if(amount_check>1){

        inputFieldAmount.text = (--amount_check).ToString();
        priceText_check.text = (amount_check * defaultPrice).ToString();
        }
    }
    public void BuyCheckBtn(){
        if(PlayerManager.instance.curCoin>=amount_check * defaultPrice){
            Debug.Log(amount_check+"개 구입");
            Buy(code,amount_check);
            PlayerManager.instance.curCoin -= amount_check * defaultPrice;
        }
        else{            
            UIManager.instance.SetPopUp("코인이 부족합니다.","error1");

        }
    }
        
    public void SetInputAmount()

    {
        if (inputFieldAmount.text.Length > 0)

        {
            int inputTemp = int.Parse(inputFieldAmount.text) ;

            amount_check = inputTemp ;
            price_check = inputTemp;

            //amountText_check.text = amount_check.ToString();
            priceText_check.text = inputFieldAmount.text;

        }

    }

}
