using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public void Buy(string name){
        switch(name){
            case "BronzeP" :
                BuffManager.instance.autoChargeFuel = true;
                
                BuffManager.instance.autoGatherRP = true;
                for(int i=0;i<BuffManager.instance.rpParent.childCount;i++){
                    BuffManager.instance.rpParent.GetChild(i).GetComponent<SpriteButton>().buildingType = BuildingType.None;
                }

                BuffManager.instance.boxCount+=10;
                BuffManager.instance.buffs[0].count+=50;
                BuffManager.instance.buffs[1].count+=50;

                BuffManager.instance.RefreshUICount();

                UIManager.instance.ActivateLowerUIPanel(5);
                UIManager.instance.ActivateLowerUIPanel(6);
                break;
            case "AutoGatherRP" :
                
                break;

        }
    }
}
