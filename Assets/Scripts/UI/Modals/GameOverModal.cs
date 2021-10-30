using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UIComponents;

public class GameOverModal : Modal
{
    public Button retryGameButtonComponent;
    public Button backToMenuButtonComponent;

    public override void Initialize()
    {
        base.Initialize();
        this.retryGameButtonComponent.onClick.AddListener(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        this.backToMenuButtonComponent.onClick.AddListener(() => {
            GameManager.Instance.ReturnToMainMenu();
        });
    }
}
