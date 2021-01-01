using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
    public bool testMode;
    public Text log;
    public Button frontBtn, rewardBtn;
    public GameObject rewardPanel;
    // Start is called before the first frame update
    void Start()
    {
        //LoadBannerAD();
        //LoadFrontAD();
        LoadRewardAd();
    }

    // Update is called once per frame
    void Update()
    {
        rewardBtn.interactable = rewardAd.IsLoaded();
    }
    // Test Test(){

    // }

    AdRequest GetAdRequest(){
        return new AdRequest.Builder().AddTestDevice("5A4B86ABCFEC0200").Build();
    }
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string rewardID = "ca-app-pub-5101538591248349/3169693867";
    RewardedAd rewardAd;

    void LoadRewardAd(){
        rewardAd = new RewardedAd(testMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>{
            Debug.Log("리워드 성공");
            rewardPanel.SetActive(true);
            BuffManager.instance.boxCount ++;
            
        };
    }

    public void ShowRewardAd(){
        rewardAd.Show();
        LoadRewardAd();
    }


}
