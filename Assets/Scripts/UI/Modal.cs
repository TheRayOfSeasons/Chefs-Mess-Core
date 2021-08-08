using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIComponents
{
    [RequireComponent(typeof(RectTransform))]
    public class Modal : MonoBehaviour
    {
        public GameObject body;
        public Button exitButtonComponent;

        public virtual void Initialize()
        {
            this.Toggle(false);
            if(this.exitButtonComponent)
            {
                this.exitButtonComponent.onClick.AddListener(() => {
                    this.Toggle(false);
                });
            }
        }

        public virtual void OnModalDisplay()
        {
            GameManager.Instance.ToggleHubMode(false);
        }

        public virtual void OnModalExit()
        {
            GameManager.Instance.ToggleHubMode(true);
        }

        public virtual void OnNonHubModalDisplay() {}

        public virtual void OnNonHubModalExit() {}

        public virtual void Toggle(bool toggle)
        {
            this.body.SetActive(toggle);
            if(toggle)
            {
                this.OnModalDisplay();
            }
            else
            {
                this.OnModalExit();
            }
        }

        public virtual void ToggleToNonHub(bool toggle)
        {
            this.body.SetActive(toggle);
            if(toggle)
            {
                this.OnNonHubModalDisplay();
            }
            else
            {
                this.OnNonHubModalExit();
            }
        }

        void Start()
        {
            this.Initialize();
        }
    }
}
