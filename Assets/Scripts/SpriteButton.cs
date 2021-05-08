using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum BuildingType{
    Enterable,
    Resource,
    Drop,
    Item,
    UI,
    None,
}
[RequireComponent (typeof (BoxCollider2D))]
public class SpriteButton : MonoBehaviour
{
    [SerializeField] public BuildingType buildingType;
    public string objectName;

    Vector2 defaultScale;
    void Start(){
        defaultScale = transform.localScale;
        if(objectName == "" )objectName = gameObject.name;
        if(tag == "RP"){
            if(BuffManager.instance.autoGatherRP){
                buildingType = BuildingType.None;
            }
        }
    }
    void OnMouseEnter(){
        //Debug.Log("들옴");
        //transform.localScale = new Vector2(defaultScale.x * 1.1f,defaultScale.y * 1.1f);
    }
    // void OnMouseExit(){
        
    //     transform.localScale = defaultScale;
    // }
    void OnMouseUp(){
        //if(!CameraMovement.instance.isMoving){
        if(!UIManager.instance.OnUI() && !UIManager.instance.uiBlocked && !CameraMovement.instance.isMoving){
            Debug.Log(gameObject.name+": 클릭");
            if(buildingType == BuildingType.Enterable){
                UIManager.instance.EnterBuilding(objectName);
                //PlayerManager.instance.Order(transform,OrderType.Enter);
            }
            else if(buildingType == BuildingType.Resource){

                // PlayerManager.instance.selectedMineral = this.transform;
                // UIManager.instance.ToggleAuto();
            }
            else if(buildingType == BuildingType.Drop || buildingType == BuildingType.Item ){
               // PlayerManager.instance.Order(transform,OrderType.Get);
                
            }
            else if(buildingType == BuildingType.UI ){
                Debug.Log("UI");
                
            }
            else if(buildingType == BuildingType.None){

            }
        }
        else{
            Debug.Log(gameObject.name+": 클릭 무시"+UIManager.instance.OnUI()+UIManager.instance.uiBlocked+CameraMovement.instance.isMoving);

        }

        //}
        //Debug.Log("ㅇㅇ");
    }

    public void DestroySprite(){
        Destroy(this);
    }
}
