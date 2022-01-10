using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    #region Private Fields


    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private GameObject pNameText;
    //private Text playerNameText;


    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

    [SerializeField]
    private GameObject pBulletsText;
    //private Text playerBullets;

    private PlayerManager target;


    #endregion


    #region MonoBehaviour Callbacks
    void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }
    void Update()
    {
        // Reflect the Player Health
        if (playerHealthSlider != null)
        {
            playerHealthSlider.value = target.Health;
        }
        if (pBulletsText != null)
        {
            pBulletsText.GetComponent<TMPro.TextMeshProUGUI>().text = target.currentBullets.ToString();
        }
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    #endregion


    #region Public Methods

    public void SetTarget(PlayerManager _target)
    {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
        // Cache references for efficiency
        target = _target;
        if (pNameText != null)
        {
            pNameText.GetComponent<TMPro.TextMeshProUGUI>().text = target.photonView.Owner.NickName;
        }
    }

    #endregion
}
