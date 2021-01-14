using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    public Slider slider;
    public InputField inputText;

    // Start is called before the first frame update
    void Start()
    {
#if !DEV_MODE
        inputText.gameObject.SetActive(false);
        StartCoroutine(Load());
#else
        inputText.gameObject.SetActive(true);
        inputText.onEndEdit.AddListener(delegate{SetSaveNum();});
        
        //onSubmit.AddListener(delegate { SetSaveNum(); });

#endif
        //StartCoroutine(Load());
        //if(!SettingManager.instance.testMode) StartCoroutine(Load());
    }
#if DEV_MODE
    void SetSaveNum(){
        if(inputText.text != ""){
            SettingManager.instance.saveNum = int.Parse(inputText.text);
            StartCoroutine(Load());
        }
    }
#endif

    IEnumerator Load(){
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(1);
        asyncScene.allowSceneActivation = false;
        float timeC = 0;
            //yield return new WaitForSeconds(0.5f);
        while(!asyncScene.isDone){
            yield return null;
            //Debug.Log(asyncScene.progress);
            timeC += Time.deltaTime;
            if (asyncScene.progress >=0.9f)
            {
                // loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1, timeC);
                slider.value = Mathf.Lerp(slider.value, 1, timeC);
                if (slider.value == 1.0f)
                {
                    asyncScene.allowSceneActivation = true;
                }         
            }
            else
            {
                slider.value= Mathf.Lerp(slider.value, asyncScene.progress, timeC);
                if (slider.value >= asyncScene.progress){
                    timeC = 0f;
                }
            }
            //Debug.Log(timeC);
            // slider.value = Mathf.Clamp01(operation.progress);
            // Debug.Log(slider.value);
            // yield return new WaitForSeconds(0.5f);
        }

    }
}
