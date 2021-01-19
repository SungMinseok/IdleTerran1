using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public Animator fader;
    public Animator nuke;
    public GameObject nextBtn;

    [Space]
    public GameObject whereText;
    public GameObject vessel;
    [Header("발사 전")]
    public Text[] texts_0;

    [Header("발사 후")]
    public Text[] texts_1;

    [Space]
    public GameObject endBtn;
    bool flag;
    void Start()
    {
        vessel.SetActive(false);
        endBtn.SetActive(false);
        whereText.SetActive(false);
        ClearVessel();
        //nextBtn.SetActive(false);
        StartCoroutine(EndingCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ClearVessel(){

        for(int i=0;i<texts_0.Length;i++){
            texts_0[i].gameObject.SetActive(false);
        }
        for(int i=0;i<texts_1.Length;i++){
            texts_1[i].gameObject.SetActive(false);
        }
    }

    IEnumerator EndingCoroutine(){
        
        yield return new WaitForSeconds(1f);
        
        whereText.SetActive(true);
        SoundManager.instance.Play("transmission");
        yield return new WaitForSeconds(3f);
        whereText.SetActive(false);

        fader.gameObject.SetActive(true);
        fader.SetTrigger("in");
        
        SoundManager.instance.Play("nukelaunched");
        yield return new WaitForSeconds(3f);
        
        fader.gameObject.SetActive(false);

        vessel.SetActive(true);

        for(int i=0;i<texts_0.Length;i++){
            flag = true;
            if(i!=0) texts_0[i-1].gameObject.SetActive(false);

            texts_0[i].gameObject.SetActive(true);
            SoundManager.instance.Play("transmission");

            // float tempTime = texts_0[i].text.Length * 0.5f;
            // Debug.Log(tempTime);
            yield return new WaitForSeconds(6.5f);

            //nextBtn.SetActive(true);

            //yield return new WaitUntil(()=>!flag);
        }
        ClearVessel();
        vessel.SetActive(false);

        nuke.SetTrigger("launch");
        SoundManager.instance.Play("nukelaunched");
        yield return new WaitForSeconds(0.5f);
//핵 발사 애니메이션 후 페이드 아웃.
        fader.gameObject.SetActive(true);
        fader.SetTrigger("out");
        yield return new WaitForSeconds(3f);
        SoundManager.instance.Play("nukehit");
        yield return new WaitForSeconds(3f);

//핵 터지는 소리 3초 후 대사.

        vessel.SetActive(true);
        
        for(int i=0;i<texts_1.Length;i++){
            flag = true;
            if(i!=0) texts_1[i-1].gameObject.SetActive(false);

            texts_1[i].gameObject.SetActive(true);
            SoundManager.instance.Play("transmission");
            // float tempTime = texts_0[i].text.Length * 0.5f;
            // Debug.Log(tempTime);
            yield return new WaitForSeconds(6.5f);

            //nextBtn.SetActive(true);

            //yield return new WaitUntil(()=>!flag);
        }
        //ClearVessel();
        //vessel.SetActive(false);

//돌아가기 버튼
        endBtn.SetActive(true);
    }

    public void NextBtn(){
        flag = false;
    }

    public void EndBtn(){

        SceneManager.LoadScene("Main");
    }
}
