using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public enum AchvType{
    box,
}
[System.Serializable]
public class Achv{
    public AchvType achvType;
    public string name;
    public string des;
    public float[] tarAmount;
    public int[] rwdAmount;
    [Space]
    public int phase;
    public float curAmount;
}
public class AchvManager : MonoBehaviour
{
    public static AchvManager instance;
    public Achv[] achvs;
    public Sprite boxSprite;
    public Transform grid;
    Text[] nameTexts;
    Text[] desTexts;
    Slider[] sliders;
    Text[] sliderTexts;
    Button[] getBtns;
    Image[] rwdimgs;
    Text[] rwdAmountTexts;
    GameObject[] lockeds;
    [Header ("데이터 값")]
    public long totalMineral;
    public long totalRP;
    public uint totalResearch; 

    void Awake(){
        instance = this;
    }
    void Start()
    {
        nameTexts = new Text[grid.childCount];
        desTexts = new Text[grid.childCount];
        sliders = new Slider[grid.childCount];
        sliderTexts = new Text[grid.childCount];
        getBtns = new Button[grid.childCount];
        rwdimgs = new Image[grid.childCount];
        rwdAmountTexts = new Text[grid.childCount];
        lockeds = new GameObject[grid.childCount];

        for(int i=0;i<grid.childCount;i++){
            int temp = i;
            nameTexts[i] = grid.GetChild(i).GetChild(0).GetComponent<Text>();
            desTexts[i] = grid.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>();
            sliders[i] = grid.GetChild(i).GetChild(1).GetComponent<Slider>();
            sliderTexts[i] = grid.GetChild(i).GetChild(1).GetChild(2).GetComponent<Text>();
            getBtns[i] = grid.GetChild(i).GetChild(2).GetComponent<Button>();
            rwdimgs[i] = grid.GetChild(i).GetChild(2).GetChild(1).GetComponent<Image>();
            rwdAmountTexts[i] = grid.GetChild(i).GetChild(2).GetChild(2).GetComponent<Text>();
            lockeds[i] = grid.GetChild(i).GetChild(2).GetChild(3).gameObject;
            getBtns[temp].onClick.AddListener(()=>GetRewardBtn(temp));

        }

        SetAchv();
        RefreshAchv();
    }

    public void SetAchv(){
        
        for(int i=0;i<grid.childCount;i++){
            nameTexts[i].text = achvs[i].name;
            desTexts[i].text = achvs[i].des;
            switch(achvs[i].achvType){
                case AchvType.box :
                    rwdimgs[i].sprite = boxSprite;
                    break;
                default :
                    break;
            }
            // if(achvs[i].sprite!=null)
            //     rwdimgs[i].sprite = achvs[i].sprite;
            // else{
            //     rwdimgs[i].sprite = boxSprite;
            // }
        }
    }

    public void RefreshAchv(int num = -1){
        if(num == -1){
            for(int i=0;i<grid.childCount;i++){
                RefreshAchv(i);
            }
        }
        else{

            //현재 값 설정.
            switch(num){
                //미네랄 //미네랄 저장시 업데이트
                case 0 :
                    achvs[num].curAmount = (float)(totalMineral);
                    break;
                case 1 :
                    achvs[num].curAmount = (float)(totalRP);
                    break;
                case 2 :
                    achvs[num].curAmount = (float)(totalResearch);
                    break;

            }

            //보상 갯수 표시
            //if(achvs[num].rwdAmount[achvs[num].phase]!=)
            rwdAmountTexts[num].text = achvs[num].rwdAmount[achvs[num].phase].ToString();

            //업적 상황 슬라이더 비율 표시
            //K, M, B로 표시 ( 1000 넘는 경우 )
            if(achvs[num].tarAmount[achvs[num].phase]>=1000){

                sliderTexts[num].text = FloatToFloat_Comp(achvs[num].curAmount) +"/"+FloatToFloat_Comp(achvs[num].tarAmount[achvs[num].phase]);
            }
            else{
                sliderTexts[num].text = achvs[num].curAmount +"/"+achvs[num].tarAmount[achvs[num].phase];

            }
            sliders[num].value = achvs[num].curAmount / achvs[num].tarAmount[achvs[num].phase];
            
            //보상 잠금 여부
            if(achvs[num].curAmount >= achvs[num].tarAmount[achvs[num].phase]){
                lockeds[num].SetActive(false);
            }
            else{
                lockeds[num].SetActive(true);
            }
        }
        
    }

    // void Update(){
    //     Debug.Log(LongToFloat_Comp(PlayerManager.instance.curMineral));
    // }
    public string LongToFloat_Comp(long amount){
        if(amount<1000){
            return amount.ToString();
        }
        else if(amount<1000000){
            //string.Format("{0:0.##}",
            //return (amount*0.001f).ToString("N3")+"K"; 
            return string.Format("{0:0.##}", (amount*0.001f)) +"K";
        }
        else if(amount<1000000000){
            //return (amount*0.000001f).ToString("N3")+"M"; 
            return string.Format("{0:0.##}", (amount*0.000001f)) +"M";
        }
        else if(amount<1000000000000){
            //return (amount*0.000000001f).ToString("N3")+"B"; 
            return string.Format("{0:0.##}", (amount*0.000000001f)) +"B";
        }
        return "";
    }
    public string FloatToFloat_Comp(float amount){
        if(amount<1000){
            return amount.ToString();
        }
        else if(amount<1000000){
            return string.Format("{0:0.##}", (amount*0.001f)) +"K";
        }
        else if(amount<1000000000){
            return string.Format("{0:0.##}", (amount*0.000001f)) +"M";
        }
        else if(amount<1000000000000){
            return string.Format("{0:0.##}", (amount*0.000000001f)) +"B";
        }
        return "";
    }

    public void GetRewardBtn(int num){
        
        switch(achvs[num].achvType){
            case AchvType.box :
                UIManager.instance.SetRewardPop("랜덤 보급품 "+achvs[num].rwdAmount[achvs[num].phase].ToString()+"개 획득!", "Box",achvs[num].rwdAmount[achvs[num].phase]);
                break;
            default :
                break;
        }
        
        achvs[num].phase ++;

        RefreshAchv(num);

    }
    // public float LongToFloat(long amount){
    //     if(amount<1000){
    //         return amount;
    //     }
    //     else if(amount<1000000){
    //         return (amount*0.001f); 
    //     }
    //     else if(amount<1000000000){
    //         return (amount*0.000001f); 
    //     }
    //     else if(amount<1000000000000){
    //         return (amount*0.000000001f); 
    //     }
    //     return -1;
    // }
}
