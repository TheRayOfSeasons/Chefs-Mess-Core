using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public List<Sprite> tutorialCards;
    public Image imageComponent;
    public Button previousButton;
    public Button nextButton;
    public int page = 1;

    public virtual void OnExitEvent() {}

    void SetImage()
    {
        imageComponent.sprite = tutorialCards[this.page - 1];
    }

    void Start()
    {
        this.SetImage();
        this.previousButton.onClick.AddListener(() => {
            if(this.page <= 1)
            {
                this.page = 1;
                this.previousButton.gameObject.SetActive(false);
                return;
            }
            if(!this.nextButton.gameObject.activeSelf)
            {
                this.nextButton.gameObject.SetActive(true);
            }
            page--;
            this.SetImage();
        });
        this.nextButton.onClick.AddListener(() => {
            if(this.page >= this.tutorialCards.Count)
            {
                this.OnExitEvent();
                this.gameObject.SetActive(false);
                return;
            }
            if(!this.previousButton.gameObject.activeSelf)
            {
                this.previousButton.gameObject.SetActive(true);
            }
            page++;
            this.SetImage();
        });
    }
}
