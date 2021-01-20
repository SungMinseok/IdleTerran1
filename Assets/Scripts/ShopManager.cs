using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public GameObject shopPanel;
    public GameObject successBuyPanel;
    public Text contentNameText;
    public Text contentsText;

    public Transform packageParent;
    Transform[] packageChild;
    public Transform normalParent;
    Transform[] normalChild;
    [Header("구매여부")]
    public int[] packageCheck;
    public int[] normalCheck;

    void Awake(){
        instance = this;
    }
    void Start(){
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
        RefreshShopUI();
    }

    public void RefreshShopUI(){
        //if(packageCheck[0])
//패키지 한번만 구매 가능
        // for(int i=0;i<packageParent.childCount;i++){

        //     if(packageCheck[i]){//구매 시.
        //         if(i==0){
        //             normalChild[i].GetChild(normalChild[i].childCount-2).gameObject.SetActive(true);
        //             packageChild[i].GetChild(packageChild[i].childCount-1).gameObject.SetActive(true);
        //         }
        //         //packageChild[i].GetChild(packageChild[i].childCount-1).gameObject.SetActive(true);
        //     }
        // }
        if(packageCheck[0]==1){
            
            normalChild[0].GetChild(normalChild[0].childCount-2).gameObject.SetActive(true);
            packageChild[0].GetChild(packageChild[0].childCount-1).gameObject.SetActive(true);
        }

        if(normalCheck[0]==1){
            
            normalChild[0].GetChild(normalChild[0].childCount-1).gameObject.SetActive(true);
            packageChild[0].GetChild(packageChild[0].childCount-2).gameObject.SetActive(true);
        }
        // for(int i=0;i<normalParent.childCount;i++){
        //     if(normalCheck[i]>){//구매 시.
        //         if(i==0){
        //             normalChild[i].GetChild(packageChild[i].childCount-2).gameObject.SetActive(true);
        //         }
        //         packageChild[i].GetChild(packageChild[i].childCount-1).gameObject.SetActive(true);
        //     }
        // }
    }
    public void PurchaseFailure(){
        Debug.Log("구입 실패");
    }
    public void PurchaseSuccess(){
        Debug.Log("구입 성공");
    }

    public void Buy(string code){//000 001 002 003 004 , 100 101 102 103 104 105 106
        SoundManager.instance.Play("rescue");

        //shopPanel.SetActive(false);
        successBuyPanel.SetActive(true);
        //contentNameText.text = name;
        int type = int.Parse(code)/100;
        int num = int.Parse(code)%100;

        if(type == 0){
            
            //packageCheck[num] = true;
            packageCheck[num] +=1;
            contentNameText.text = packageChild[num].GetChild(0).GetComponent<Text>().text;
            contentNameText.color = packageChild[num].GetChild(0).GetComponent<Text>().color;
            contentsText.text = packageChild[num].GetChild(1).GetComponent<Text>().text;
        }
        else{
            normalCheck[num] +=1;
            contentNameText.text = normalChild[num].GetChild(0).GetComponent<Text>().text;
            contentNameText.color = normalChild[num].GetChild(0).GetComponent<Text>().color;
            contentsText.text = normalChild[num].GetChild(1).GetComponent<Text>().text;

        }

        // packageCheck[0] = true;
        // contentNameText.text = packageChild[0].GetChild(0).GetComponent<Text>().text;
        // contentNameText.color = packageChild[0].GetChild(0).GetComponent<Text>().color;
        // contentsText.text = packageChild[0].GetChild(1).GetComponent<Text>().text;

        switch(code){
            case "000" :
            
                BuffManager.instance.autoChargeFuel = true;
                BuffManager.instance.autoGatherRP = true;
                for(int i=0;i<BuffManager.instance.rpParent.childCount;i++){
                    BuffManager.instance.rpParent.GetChild(i).GetComponent<SpriteButton>().buildingType = BuildingType.None;
                    BuffManager.instance.AutoGatherRP(BuffManager.instance.rpParent.GetChild(i));
                }

                BuffManager.instance.boxCount+=10;
                BuffManager.instance.buffs[0].count+=50;
                BuffManager.instance.buffs[1].count+=50;


                // UIManager.instance.ActivateLowerUIPanel(5);
                // UIManager.instance.ActivateLowerUIPanel(6);
                break;
            case "001" :
                
                BuffManager.instance.boxCount+=20;
                BuffManager.instance.buffs[0].count+=100;
                BuffManager.instance.buffs[1].count+=100;
                break;
            case "002" :
                
                BuffManager.instance.boxCount+=30;
                BuffManager.instance.buffs[0].count+=150;
                BuffManager.instance.buffs[1].count+=150;
                break;
            case "003" :
                
                BuffManager.instance.boxCount+=60;
                BuffManager.instance.buffs[0].count+=300;
                BuffManager.instance.buffs[1].count+=300;
                break;
            case "004" :
                
                BuffManager.instance.boxCount+=120;
                BuffManager.instance.buffs[0].count+=600;
                BuffManager.instance.buffs[1].count+=600;
                break;
            case "100" :
            
                BuffManager.instance.autoChargeFuel = true;
                BuffManager.instance.autoGatherRP = true;
                for(int i=0;i<BuffManager.instance.rpParent.childCount;i++){
                    BuffManager.instance.rpParent.GetChild(i).GetComponent<SpriteButton>().buildingType = BuildingType.None;
                    BuffManager.instance.AutoGatherRP(BuffManager.instance.rpParent.GetChild(i));
                }
                break;
            case "101" :
                
                BuffManager.instance.buffs[0].count+=50;
                BuffManager.instance.buffs[1].count+=50;
                break;
            case "102" :
                
                BuffManager.instance.buffs[0].count+=100;
                BuffManager.instance.buffs[1].count+=100;
                break;
            case "103" :
                
                BuffManager.instance.buffs[0].count+=200;
                BuffManager.instance.buffs[1].count+=200;
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
}
