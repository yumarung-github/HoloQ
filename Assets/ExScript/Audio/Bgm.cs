using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bgm : MonoBehaviour, IEquatable<Bgm>, IComparable<Bgm>, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler
{
    public string bgmName;
    public AudioClip bgmClip;
    public string artist;
    public string bgmDate;
    public string bgmTime;

    public bool Equals(Bgm other)
    {

        return bgmName.Equals(bgmName);
    }

    public int CompareTo(Bgm other)
    {
        return bgmName.CompareTo(other.bgmName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Uimanager.Instance.bgmWindow.activeSelf)
        {
            Uimanager.Instance.bgmWindow.GetComponent<BgmWindow>().title.text = "Title : " + bgmName;
            Uimanager.Instance.bgmWindow.GetComponent<BgmWindow>().artist.text = "Artist : " + artist;
            Uimanager.Instance.bgmWindow.GetComponent<BgmWindow>().playTime.text = "PlayTime : " + bgmTime;
            Uimanager.Instance.bgmWindow.GetComponent<BgmWindow>().releaseDate.text = "ReleaseDate : " + bgmDate;
        }
        //정보 띄우기
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Uimanager.Instance.bgmWindow.activeSelf)
        {
            if (AudioManager.Instance.nowBgm != null)
            {
                AudioManager.Instance.ReturnBgm(AudioManager.Instance.nowBgm);
            }
            
            AudioManager.Instance.nowBgm = AudioManager.Instance.PopBgm(bgmClip, true);
            Debug.Log(bgmName);
        }
        //오디오매니저에 접근해서 bgm 변경
    }
}
