using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    MMD4MecanimModel mmd4MecanimModel;
    int morphNum;
    Coroutine motionCo = null;
    // Start is called before the first frame update
    void Start()
    {
        morphNum = 0;
        mmd4MecanimModel = GetComponent<MMD4MecanimModel>();
        //Debug.Log(mmd4MecanimModel.morphList[0].name);
        //Debug.Log(mmd4MecanimModel.morphList.Length);
        //Debug.Log(mmd4MecanimModel.morphList[1].morphCategory.ToString());
    }
    enum MORP_TYPE_Lui { 
        HokEye = 38,
        Mayuge = 1
    }
    enum MORP_TYPE_Lap
    {
        Hau = 21,
        Mayuge =1
    }
    enum MORP_TYPE_Koyo
    {
        Hau = 21,
        Mayuge = 1
    }
    enum MORP_TYPE_Saka
    {
        Hau = 21,
        Mayuge = 1
    }
    enum MORP_TYPE_Iro
    {
        Hau = 21,
        Mayuge =1
    }
    enum MORP_TYPE_Towa
    {
        Hau = 47,
        Mayuge = 1
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))//lui
        {
            if(motionCo != null)
            {
                StopCoroutine(motionCo);
            }
            Debug.Log("dd");
            mmd4MecanimModel.morphList[40].weight = 1f;
            MorphSelect((int)MORP_TYPE_Lui.Mayuge);
            //mmd4MecanimModel.morphList[38].weight = 1f; // towa 43 // laplus 49
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))//Lap
        {
            if (motionCo != null)
            {
                StopCoroutine(motionCo);
            }
            Debug.Log("dd");
            mmd4MecanimModel.morphList[21].weight = 1f;
            MorphSelect((int)MORP_TYPE_Lap.Mayuge);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))//Koyo
        {
            if (motionCo != null)
            {
                StopCoroutine(motionCo);
            }
            Debug.Log("dd");
            mmd4MecanimModel.morphList[21].weight = 1f;
            MorphSelect((int)MORP_TYPE_Koyo.Mayuge);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))//Saka
        {
            if (motionCo != null)
            {
                StopCoroutine(motionCo);
            }
            Debug.Log("dd");
            mmd4MecanimModel.morphList[21].weight = 1f;
            MorphSelect((int)MORP_TYPE_Saka.Mayuge);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))//Iro
        {
            if (motionCo != null)
            {
                StopCoroutine(motionCo);
            }
            Debug.Log("dd");
            mmd4MecanimModel.morphList[21].weight = 1f;
            MorphSelect((int)MORP_TYPE_Iro.Mayuge);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))//Towa
        {
            if (motionCo != null)
            {
                StopCoroutine(motionCo);
            }
            Debug.Log("dd");
            mmd4MecanimModel.morphList[47].weight = 1f;
            MorphSelect((int)MORP_TYPE_Towa.Mayuge);
        }
    }
    public void DmgEmotion(char_Type type)
    {
        if (motionCo != null)
        {
            StopCoroutine(motionCo);
        }
        switch ((int)type)
        {
            case 0://lap
                mmd4MecanimModel.morphList[21].weight = 1f;
                MorphSelect((int)MORP_TYPE_Lap.Mayuge);
                break;
            case 1://koyo
                mmd4MecanimModel.morphList[21].weight = 1f;
                MorphSelect((int)MORP_TYPE_Koyo.Mayuge);
                break;
            case 2://saka
                mmd4MecanimModel.morphList[21].weight = 1f;
                MorphSelect((int)MORP_TYPE_Saka.Mayuge);
                break;
            case 3://iro
                mmd4MecanimModel.morphList[21].weight = 1f;
                MorphSelect((int)MORP_TYPE_Iro.Mayuge);
                break;
            case 4://lui
                mmd4MecanimModel.morphList[40].weight = 1f;
                MorphSelect((int)MORP_TYPE_Lui.Mayuge);
                break;
            case 5://towa
                mmd4MecanimModel.morphList[47].weight = 1f;
                MorphSelect((int)MORP_TYPE_Towa.Mayuge);
                break;
            default: break;
        }
    }
    void MorphSelect(int tempMorph_Type)
    {
        morphNum = tempMorph_Type;//이로하는 12+
        mmd4MecanimModel.morphList[morphNum].weight = 1f;
        
        motionCo = StartCoroutine("MorphPlay");
        //Debug.Log((12 + (int)tempMorph_Type).ToString() + 
        //   mmd4MecanimModel.morphList[12 + (int)tempMorph_Type].weight);
    }
   IEnumerator MorphPlay()
    {
        mmd4MecanimModel.morphList[morphNum].weight = 0f;
        while (mmd4MecanimModel.morphList[morphNum].weight <= 1.0f)
        {
            mmd4MecanimModel.morphList[morphNum].weight += 0.05f;
            //Debug.Log(mmd4MecanimModel.morphList[morphNum].weight);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (mmd4MecanimModel.morphList[morphNum].weight >=0.1f)
        {
            mmd4MecanimModel.morphList[morphNum].weight -= 0.05f;
            //Debug.Log(mmd4MecanimModel.morphList[morphNum].weight);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        mmd4MecanimModel.morphList[morphNum].weight = 0;
        //Debug.Log(mmd4MecanimModel.morphList[morphNum].weight);
        mmd4MecanimModel.morphList[21].weight = 0f;
        mmd4MecanimModel.morphList[40].weight = 0f;
        StopCoroutine(motionCo);
        
        yield return null;
    }
}
