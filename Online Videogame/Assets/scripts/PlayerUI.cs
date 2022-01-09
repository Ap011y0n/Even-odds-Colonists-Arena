using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    #region Private Fields


    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private Text playerNameText;


    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

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
        if (playerNameText != null)
        {
            playerNameText.text = target.photonView.Owner.NickName;
        }
    }

    #endregion
}
