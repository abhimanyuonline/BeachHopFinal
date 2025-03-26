using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Numerics;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.Video;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
   // [SerializeField] Image backgroundPanel;
    [SerializeField] GameObject selectedPlayer;
    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] Animator _animatorPlayer;

    [SerializeField] CanvasGroup canvasGroupStartScreen;
    [SerializeField] CanvasGroup canvasGroupGameScreen;
    [SerializeField] RawImage[] animationRaw;
    //[SerializeField] Texture[] animTexture;
    //[SerializeField] GameObject transAnimation;

    [SerializeField] VideoPlayer startAnimationVideoPlayer;


    [SerializeField] Vector2 playerBoyInitialLoc = new Vector2();
    [SerializeField]
    private GameObject pillarPrefab;

    Dictionary<float, float> _pillarSizeFactor = new();

    [SerializeField]
    Vector2 pillarStartingPosVec2 = new Vector2();

    [SerializeField] Vector2 nextPillarLocationVec2 = new Vector2();

    [SerializeField] Vector2 _nextTempPillarLoc = new Vector2();

    [SerializeField]
    GameObject stairsPrefab;
    Vector2 stairsStartingPos = new Vector2();

    [SerializeField]
    List<GameObject> pillarsList = new List<GameObject>();

    [SerializeField] bool startStairs = false;

    [SerializeField] bool startIncreaingStair = false;
    [SerializeField] Button soundToogleButton;

    [SerializeField] GameObject stairsObj;
    [SerializeField]
    float stairsMaxLength = 3.5f;
    [SerializeField]
    float growingfactor = 0.05f;
    [SerializeField] float ignoreWidth = 0.0f;
    [SerializeField] GameObject inGameMenuCanvas;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject screenTapCanvas;
    [SerializeField] GameObject gameFinishedPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject scoreUpdaterText;
    [SerializeField] GameObject reviveGamePanel;
    
    [SerializeField] Slider reviveEndGameSlider;
    [SerializeField] Button reviveEndGameButton;
    
    [SerializeField] DataManager dataManager;
    
    [SerializeField] GameObject[] clouds;
    [SerializeField] SoundManager soundManger;
    [SerializeField] Button[] buttons;
    
    [SerializeField] private GameObject conffetiGO;
    
    [SerializeField] bool playerIsGameScreen = false;
    [SerializeField] bool isIdle = false;
    [SerializeField] float currentIdelTime = 0.0f;
    [SerializeField] Animator idleAnimation;
    [SerializeField] GameObject pressAndHold;
    [SerializeField] GameObject[] waveStarting;
    [SerializeField] Renderer backGroundRenderer;
    [SerializeField] float speed = 0.25f;
    [FormerlySerializedAs("ScoreBoardCrown")] [SerializeField] GameObject scoreBoardCrown;

    [SerializeField] GoogleMobileAdsScript googleAds;
    
    Vector2 BackgroundPanelInitialPos;
    void Awake()
    {
        if (PlayerPrefs.GetInt("MusicVolume") == 0) {
            PlayerPrefs.SetInt("MusicVolume",1);
            PlayerPrefs.SetInt("SFXVolume",1);
        }
        soundManger.MuteMusic(!( PlayerPrefs.GetInt("MusicVolume") > 0));
        soundManger.MuteSfx(!(PlayerPrefs.GetInt("SFXVolume") > 0));
        soundManger.PlayMusic("Background", true);

        canvasGroupStartScreen.DOFade(1, 0.25f);
        mainMenuCanvas.gameObject.SetActive(true);
        buttons.ToList().ForEach(item =>
        {
            if (item != null)
                item.onClick.AddListener(() => PlayButtonClickSound());
        });
        int lastScore = PlayerPrefs.GetInt("Score");
        foreach (var VARIABLE in dataManager.HighestScoreText)
        {
            VARIABLE.text = lastScore.ToString();
        }
    }

    private void Update() => Input.backButtonLeavesApp = true;
    
    float _currentBackgroundMovementTime = 0.0f;
    private void FixedUpdate()
    {
        HandleStairsActivity();
        HandleBackgroundMovement();
    }

    private void HandleStairsActivity()
    {
        if (startStairs)
        {
            StairsIncreasingActivity();
            isIdle = false;
        }
    }

    private void HandleBackgroundMovement()
    {
        if (startBackgroundMovement)
        {
            _currentBackgroundMovementTime += Time.deltaTime;
            if (_currentBackgroundMovementTime > 1.80f)
            {
                startBackgroundMovement = false;
                _currentBackgroundMovementTime = 0.0f;
                return;
            }
            Vector2 offset = backGroundRenderer.material.mainTextureOffset;
            offset.x += speed * (Time.deltaTime / 10);
            offset.x = (offset.x) % 1;
            backGroundRenderer.material.mainTextureOffset = offset;
        }
    }

    void LateUpdate()
    {
        if (!playerIsGameScreen || !isIdle)
            return;

        if (!startStairs)
        {
            currentIdelTime += Time.deltaTime;
            if (currentIdelTime > 5.0f)
            {
                Debug.Log("Idle");
                StartIdleAnimation();
                currentIdelTime = 0.0f;
            }
        }
        else
        {
            idleAnimation.gameObject.SetActive(false);
            pressAndHold.gameObject.SetActive(false);
            currentIdelTime = 0.0f;
        }
    }
    public void StartIdleAnimation()
    {
        idleAnimation.gameObject.SetActive(true);
        pressAndHold.gameObject.SetActive(true);
    }

    public void OnClick_StartGameScene()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        inGameMenuCanvas.gameObject.SetActive(true);
        canvasGroupGameScreen.DOFade(1, 0.5f);
    }

    public void AnimationControl()
    {
        canvasGroupStartScreen.DOFade(0.0f, 1f).OnComplete(() => OnClick_StartGameScene());
        screenTapCanvas.gameObject.SetActive(true);
        gameFinishedPanel.gameObject.SetActive(false);
        scoreUpdaterText.gameObject.SetActive(false);
        reviveGamePanel.gameObject.SetActive(false);

        var index = PlayerPrefs.GetInt("CharIndex");
        selectedPlayer = playerPrefabs[index];
        _animatorPlayer = selectedPlayer.GetComponent<Animator>();
        SetIdleTrue();
        SetFallFalse();
        dataManager.RestCurrentScore();
        foreach (var var in clouds)
        {
            var.SetActive(true);
        }
        
        playerIsGameScreen = true;
        idleAnimation.gameObject.SetActive(true);
        pressAndHold.gameObject.SetActive(true);
        conffetiGO.SetActive(false);
        scoreBoardCrown.SetActive(false);
        ResetGameplay();
    }

    bool itsFirstTime = false;
    private void ResetGameplay()
    {
        pillarsList.ToList().ForEach(item => Destroy(item));
        pillarsList.Clear();
        itsFirstTime = true;
        selectedPlayer.transform.SetPositionAndRotation(playerBoyInitialLoc, Quaternion.identity);
        SpawnPillar(pillarPrefab, pillarStartingPosVec2);
        itsFirstTime = false;

        _nextTempPillarLoc = RandomiseNextPillarLocationVec2(pillarStartingPosVec2, nextPillarLocationVec2);
        SpawnPillar(pillarPrefab, _nextTempPillarLoc);

        startIncreaingStair = true;

        var diff = pillarStartingPosVec2.x - nextPillarLocationVec2.x;

        var newLoc = RandomiseNextPillarLocationVec2(_nextTempPillarLoc, new Vector2(_nextTempPillarLoc.x + Math.Abs(diff), _nextTempPillarLoc.y));
        Vector2 nextGridPos = new Vector2(newLoc.x, newLoc.y);
        SpawnPillar(pillarPrefab, nextGridPos);
        isIdle = true;
        pillarsList[2].gameObject.SetActive(false);
    }

    void ReliveGamePlay() {
        Destroy(stairsObj);
        reviveGamePanel.gameObject.SetActive(value:false);
        SetIdleTrue();
        SetWalkFalse();
        SetFallFalse();
        selectedPlayer.transform.SetPositionAndRotation(playerBoyInitialLoc, Quaternion.identity);
        isIdle = true;
        startIncreaingStair = true;
    }

    public Vector2 RandomiseNextPillarLocationVec2(Vector2 firstPos, Vector2 secondPos)
    {
        var diff = firstPos.x - secondPos.x;
        var ranX = Random.Range(0.5f, 1.95f);
        Vector2 newloc = new Vector2(firstPos.x + Math.Abs(diff) + ranX, secondPos.y);
        return newloc;
    }
    private void SpawnPillar(GameObject obj, Vector2 pos)
    {
        GameObject obj2 = Instantiate(obj, pos, Quaternion.identity);
        pillarsList.Add(obj2);
        if (itsFirstTime)
        {
            obj2.transform.localScale = new Vector2(1.0f, obj2.transform.localScale.y);
            obj2.transform.GetChild(2).gameObject.SetActive(true);
            return;
        }
        var rand = PillarSize();
        obj2.transform.localScale = new Vector2(rand, obj2.transform.localScale.y);
    }

    private void SpawnStairs(GameObject obj, Vector2 pos)
    {
        stairsObj = Instantiate(obj, pos, Quaternion.identity);
        SpriteRenderer spriteRenderer = stairsObj.GetComponent<SpriteRenderer>();
        stairsObj.transform.Rotate(0, 0, 90);
        spriteRenderer.size = new Vector2(0, spriteRenderer.size.y);
    }

    public void OnClick_Stairs()
    {
        
        if (!startIncreaingStair)
            return;
        //stairsStartingPos.x = pillarStartingPosVec2.x;
        var currentTowerWidth = (float)pillarsList[0].transform.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2 * pillarsList[0].transform.localScale.x;
        stairsStartingPos.x = pillarsList[0].transform.position.x + currentTowerWidth;
        // stairsStartingPos.y = pillarStartingPosVec2.y + pillarsList[0].transform.localScale.y;
        if (stairsObj == null) SpawnStairs(stairsPrefab, stairsStartingPos);

        startStairs = true;
        soundManger.PlaySfx("PlankInc", true);
    }
    // ReSharper disable Unity.PerformanceAnalysis
    void StairsIncreasingActivity()
    {
        if (stairsObj == null)
        {
            Debug.Log("object not found");
            return;
        }
        SpriteRenderer spriteRenderer = stairsObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {

            if (spriteRenderer.size.x > stairsMaxLength)
            {
                spriteRenderer.size = new Vector2(0, spriteRenderer.size.y);
            }
            else
            {
                spriteRenderer.size = new Vector2(spriteRenderer.size.x + growingfactor, spriteRenderer.size.y);
            }
        }

    }
    public void OnClick_StopGrowth()
    {
        if (!startIncreaingStair)
            return;

        if (stairsObj == null)
            return;

        startIncreaingStair = false;
        startStairs = false;
        soundManger.StopSfx("PlankInc");
        AlignStairs();
    }

    public void AlignStairs()
    {
        stairsObj.transform.DOLocalMoveX(stairsStartingPos.x - 0.05f, 1);
        stairsObj.transform.DOLocalRotate(new Vector3(0, 0, -90), 2.0f, RotateMode.LocalAxisAdd).OnComplete(StartMovingPlayer);
    }
    void StartMovingPlayer()
    {
        float length = stairsObj.GetComponent<SpriteRenderer>().size.x * stairsObj.transform.localScale.x;
        float position = stairsObj.transform.position.x + length;
        float duration = length / 1.0f;
        float dis = 0.05f;
        int steps = (int)Math.Round(length / dis);

        selectedPlayer.transform.DOMoveX(position, duration).SetEase(Ease.Linear).OnComplete(CheckPlayerStatus);
        //  selectedPlayer.transform.DOMoveX(dis, 0.1f).SetEase(Ease.Linear).SetLoops(steps).OnComplete(CheckPlayerStatus);
        SetWalkTrue();
        SetIdleFalse();
        soundManger.PlaySfx("CharWalk", true);

    }
    void CheckPlayerStatus()
    {
        SetWalkFalse();
        SetIdleTrue();
        GameObject currentTower = pillarsList[1];
        var currentTowerWidth = (float)currentTower.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2 * currentTower.transform.localScale.x;
        var startPosX = currentTower.transform.position.x - currentTowerWidth;
        var endPosX = currentTower.transform.position.x + currentTowerWidth;
        var currentPlayerPos = selectedPlayer.transform.position.x;

        soundManger.StopSfx("CharWalk");
        if (currentPlayerPos > startPosX  && currentPlayerPos < endPosX)
        {
            Debug.Log("reached");
        //    Debug.Log("Player position: " + currentPlayerPos + ", Start X: " + (startPosX + ignoreWidth) + ", End X: " + (endPosX - ignoreWidth) + ", Ignore Width: " + ignoreWidth);
            SucssefullyReached();

        }
        else
        {
            Debug.Log("Failed");
        //    Debug.Log("Player position: " + currentPlayerPos + ", Start X: " + (startPosX + ignoreWidth) + ", End X: " + (endPosX - ignoreWidth) + ", Ignore Width: " + ignoreWidth);
            UnSucssefullyReached();
        }
    }

    void SucssefullyReached()
    {
        pillarsList[2].gameObject.SetActive(true);
        Destroy(stairsObj);
        scoreUpdaterText.transform.position = new Vector2(pillarsList[1].transform.position.x, selectedPlayer.transform.position.y);
        StartScoreUpdateAnimation();
        _nextTempPillarLoc = RandomiseNextPillarLocationVec2(pillarStartingPosVec2, nextPillarLocationVec2);
        Vector2 prevGridPos = new Vector2(pillarStartingPosVec2.x - 2, pillarStartingPosVec2.y);
        pillarsList[0].transform.DOMoveX(prevGridPos.x, 2);
        pillarsList[1].transform.DOMoveX(pillarStartingPosVec2.x, 2).OnComplete(AfterSucessfullPlayerReach);
        pillarsList[2].transform.DOMoveX(_nextTempPillarLoc.x, 2);
        selectedPlayer.transform.DOMoveX(pillarStartingPosVec2.x, 2);
        UpdateBackground();
    }

    float displacement = 1.5f;

    bool startBackgroundMovement = false;
    void UpdateBackground()
    {
        startBackgroundMovement = true;
        foreach (var wavObj in waveStarting)
        {
            foreach (var item in wavObj.GetComponentsInChildren<Transform>()){
                item.transform.DOLocalMoveX(item.transform.localPosition.x - displacement, 2.0f);
            }
        }
    }
    
    void StartScoreUpdateAnimation()
    {
        scoreUpdaterText.SetActive(value: true);
        var endPos = dataManager.currentScoreText[1].transform;
        scoreUpdaterText.transform.DOMove(endPos.position, 0.75f).SetEase(Ease.Linear).OnComplete(OnEndScoreUpdateAnimation);
    }
    void OnEndScoreUpdateAnimation()
    {
        scoreUpdaterText.SetActive(value: false);
        dataManager.UpdateScore();
        soundManger.PlaySfx("ScoreBoardUpdate");
        var lastHighScore = PlayerPrefs.GetInt("Score");

        if (lastHighScore < dataManager._currentScore && lastHighScore > 0)
        {
            conffetiGO.SetActive(true);
            soundManger.PlaySfx("HighScore");
            Invoke(nameof(HighScoreAnimation),2.0f);
        }
    }

    void HighScoreAnimation()
    {
       scoreBoardCrown.gameObject.SetActive(true);
    }
       

    void UnSucssefullyReached()
    {
        selectedPlayer.transform.DOMoveY(-7.0f, 2);
        stairsObj.transform.DOLocalRotate(new Vector3(0, 0, -90), 2.0f, RotateMode.LocalAxisAdd).OnComplete(AfterUnSucessfullPlayerReach);
        SetIdleFalse();
        SetWalkFalse();
        SetFallTrue();
        soundManger.PlaySfx("CharFall", false);
    }

    void AfterSucessfullPlayerReach()
    {
        Destroy(pillarsList[0]);
        pillarsList.RemoveAt(0);
        startIncreaingStair = true;
        SetIdleTrue();
        SetWalkFalse();
        var diff = pillarStartingPosVec2.x - nextPillarLocationVec2.x;
        selectedPlayer.transform.SetParent(pillarsList[1].transform.parent);
        

        var newLoc = RandomiseNextPillarLocationVec2(_nextTempPillarLoc, new Vector2(_nextTempPillarLoc.x + Math.Abs(diff), _nextTempPillarLoc.y));
        Vector2 nextGridPos = new Vector2(newLoc.x, newLoc.y);
        SpawnPillar(pillarPrefab, nextGridPos);
        isIdle = true;
        pillarsList[2].gameObject.SetActive(false);
    }

    void AfterUnSucessfullPlayerReach()
    {
        isIdle = false;
        Destroy(stairsObj);
        reviveGamePanel.SetActive(true);
        reviveEndGameSlider.value = 0;
        reviveEndGameButton.interactable = false;
        StartCoroutine(StartFillingBar());
        googleAds.DestroyAllAds();
        googleAds.LoadRewardedAd();
    }

    private IEnumerator StartFillingBar() {
        if (reviveEndGameSlider.value > 0.99f) {
            StopCoroutine(StartFillingBar());
            reviveEndGameButton.interactable = true;
            yield break;
        }

        reviveEndGameSlider.value += 0.01f;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(StartFillingBar());
    }
    

    public void RestartGameAfterReward() {
        ReliveGamePlay();
    }

    public void onClick_StartRewardAd(){
        googleAds.ShowRewardedAd();
    }
    public void StartEndGameActivity()
    {
        googleAds.DestroyAllAds();
        googleAds.LoadInterstitialAd();
        googleAds.ShowInterstitialAd();
        
        reviveGamePanel.SetActive(false);
        dataManager.GameEndMenu();
    }
    

    public float PillarSize()
    {
        if (dataManager._currentScore < 5)
            return Random.Range(0.8f, 1.2f);

        //prob for if 30% prob for easy move
        var ran = Random.Range(1, 100);
        if (ran < 30)
        {
            return Random.Range(0.75f, 1.0f);
        }
        return Random.Range(0.25f, 0.75f);

    }

    void SetWalkTrue()
    {
        _animatorPlayer.SetBool("Walk", true);
    }

    void SetWalkFalse()
    {
        _animatorPlayer.SetBool("Walk", false);
    }

    void SetIdleTrue()
    {
        _animatorPlayer.SetBool("Idle", true);
    }

    void SetIdleFalse()
    {
        _animatorPlayer.SetBool("Idle", false);
    }

    void SetFallTrue()
    {
        _animatorPlayer.SetBool("Fall", true);
    }

    void SetFallFalse()
    {
        _animatorPlayer.SetBool("Fall", false);
    }

    public void OnPauseClick()
    {
        Time.timeScale = 0;
        soundManger.PauseMusicSfx();
        pausePanel.gameObject.SetActive(true);
        playerIsGameScreen = false;
        
        googleAds.DestroyAllAds();
        googleAds.LoadInterstitialAd();
        googleAds.ShowInterstitialAd();
    }
    public void OnResumeClick()
    {
        Time.timeScale = 1;
        soundManger.PlayMusicSfx();
        pausePanel.gameObject.SetActive(false);
        playerIsGameScreen = true;
    }

    public void PlayButtonClickSound()
    {
        soundManger.PlaySfx("Button");
    }
}
