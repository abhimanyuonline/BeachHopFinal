using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingValidator : MonoBehaviour
{
    [SerializeField] private Sprite[] soundToogleImages;
    [SerializeField] private Button soundToogleButton;
    [SerializeField] private Button sfxToogleButton;

    [SerializeField] private Sprite[] charToogleImages;
    [SerializeField] private Button[] chardToogleButton;  
    [SerializeField] private Image selectedCharImage;

    [SerializeField] private Button saveSetting;

    [SerializeField] public SoundManager soundManager;
   // [SerializeField] private SoundManager soundManager;

    int _selectedCharIndex;
    float _currentVolume;
    
    void Start(){
        foreach (var item in chardToogleButton)
        {
            item.onClick.AddListener(delegate { UpdateCharSelection(); });
        }
        soundToogleButton.onClick.AddListener (delegate { UpdateMusicMuteUnmuteSettting(); });
        sfxToogleButton.onClick.AddListener (delegate { UpdateSfxMuteUnmuteSettting(); });
        saveSetting.onClick.AddListener(delegate{SaveData();});
    }
    private void OnEnable() {
        _selectedCharIndex = PlayerPrefs.GetInt("CharIndex");
        selectedCharImage.GetComponent<Image>().sprite = charToogleImages[_selectedCharIndex];
        CurrentMuteUnmuteSetting();
    }

    void UpdateCharSelection(){
        
        if (_selectedCharIndex == 0)
        {
            _selectedCharIndex++;
        }
        else
        {
            _selectedCharIndex = 0;
        }
        selectedCharImage.GetComponent<Image>().sprite = charToogleImages[_selectedCharIndex];
        
    }
    public void UpdateSfxMuteUnmuteSettting()
    {
        var currentSfx = PlayerPrefs.GetInt("SFXVolume");
        if (currentSfx > 0)
            currentSfx = -1;
        else
            currentSfx = 1;

        PlayerPrefs.SetInt("SFXVolume",currentSfx);
        CurrentMuteUnmuteSetting();
    }
    void UpdateMusicMuteUnmuteSettting()
    {
        var currentMusic = PlayerPrefs.GetInt("MusicVolume");
        if (currentMusic > 0)
            currentMusic = -1;
        else
            currentMusic = 1;

        PlayerPrefs.SetInt("MusicVolume",currentMusic);
        CurrentMuteUnmuteSetting();
    }
    public void CurrentMuteUnmuteSetting(){

        var currentMusic = PlayerPrefs.GetInt("MusicVolume");
        var currentSfx = PlayerPrefs.GetInt("SFXVolume");
        
        if (currentMusic > 0)
        {
            soundToogleButton.GetComponent<Image>().sprite = soundToogleImages[0];
            soundManager.MuteMusic(false);
        }
        else
        {
            soundToogleButton.GetComponent<Image>().sprite = soundToogleImages[1];
            soundManager.MuteMusic(true);
        }
        if (currentSfx > 0)
        {
            sfxToogleButton.GetComponent<Image>().sprite = soundToogleImages[0];
            soundManager.MuteSfx(false);
        }
        else
        {
            sfxToogleButton.GetComponent<Image>().sprite = soundToogleImages[1];
            soundManager.MuteSfx(true);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("CharIndex", _selectedCharIndex);
    }
}
