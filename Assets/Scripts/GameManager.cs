using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Startpanel, LeaderBoardPanel, UIPanel, PopUp, LeftControl, RightControl,GameBackButton; 
    public GameObject Player;
    public GameObject[] PlayerLife;
    public GameObject ScoreCount, CoinCount;
    public int LifeCount = 3;
    public GameObject EndlifeText;
    public static GameManager Instance;
    private int Distance,dist = 0;
    public int MyCoins = 0;
    public int AddCoin = 5;

#if UNITY_ANDROID
    private string _adUnitIdBa = "ca-app-pub-3940256099942544/6300978111";
    private string _adUnitIdIn = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
  private string _adUnitIdBa = "ca-app-pub-3940256099942544/2934735716";
  private string _adUnitIdIn = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitIdBa = "unused";
  private string _adUnitIdIn = "unused";
#endif

    BannerView _bannerView;
    InterstitialAd _interstitialAd;

    void Awake()
    {
        Instance = this;
        ScoreCount.transform.GetChild(0).gameObject.GetComponent<Text>().text = "0";
        CoinCount.transform.GetChild(0).gameObject.GetComponent<Text>().text = MyCoins.ToString();
        LeftControl.SetActive(false);
        RightControl.SetActive(false);
        GameBackButton.SetActive(false);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            LoadInterstitialAd();
        });
    }
    // Update is called once per frame
    void LateUpdate()  
    {
        if (Player.GetComponent<PlayerMovement>().isStart)
        {
            dist = (int)Player.transform.position.z;
            ScoreCount.transform.GetChild(0).gameObject.GetComponent<Text>().text = (dist - Distance).ToString();
        }       
    }

    public void StartGame()
    {
        LeftControl.SetActive(true);
        RightControl.SetActive(true);
        GameBackButton.SetActive(true);

        Startpanel.SetActive(false);
        Player.GetComponent<PlayerMovement>().isStart = true;
        CreateBannerView();
    }

    public void OpenLeaderBoard()
    {
        Startpanel.SetActive(false);
        LeaderBoardPanel.SetActive(true);
        ShowInterstitialAd();
    }

    public void BacktoMain()
    {
        Startpanel.SetActive(true);
        LeaderBoardPanel.SetActive(false);
        Player.GetComponent<PlayerMovement>().isStart = false;
        LoadInterstitialAd();
    }

    public void Replay()
    {
        if (LifeCount >= 1)
        {
            PopUp.SetActive(false);
            Player.GetComponent<PlayerMovement>().isStart = true;
            CreateBannerView();
        }
        else
        {
            EndlifeText.SetActive(true);
        }    
    }

    public void GameExit()
    {
        EndlifeText.SetActive(false);
        PopUp.SetActive(false);
        Startpanel.SetActive(true);
        LeaderBoardPanel.SetActive(false);
        if (LifeCount < 3)
        {
            LifeCount = 3;
            for (int i = 0; i < LifeCount; i++)
            {
                PlayerLife[i].SetActive(true);
            }
        }
        ScoreCount.transform.GetChild(0).gameObject.GetComponent<Text>().text = "0";
        Distance = dist;
        DestroyBannerAd();
    }
    // Banner ADs Call
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyBannerAd(); 
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_adUnitIdBa, AdSize.Banner, AdPosition.Bottom);

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }
    public void DestroyBannerAd() 
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    // Interstitial ADs Call
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitIdIn, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());

                _interstitialAd = ad;
        });
    }

    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        }
        ;
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
    }
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }
}
