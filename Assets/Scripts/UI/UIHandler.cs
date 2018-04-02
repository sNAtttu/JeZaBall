using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {    
        public Sprite verticalSprite;
        public Sprite horizontalSprite;

        private GameObject startGameButton;
        private Image switchDirectionImage;

        public void HideStartButton()
        {
            startGameButton = GameObject.FindGameObjectWithTag(Utilities.Constants.TagStartGameBtn);
            startGameButton.SetActive(false);
        }

        public void SetVerticalSprite()
        {
            InitSwitchDirectionImage();
            switchDirectionImage.sprite = verticalSprite;
        }

        public void SetHorizontalSprite()
        {
            InitSwitchDirectionImage();
            switchDirectionImage.sprite = horizontalSprite;
        }

        private void InitSwitchDirectionImage()
        {
            if(switchDirectionImage == null)
            {
                switchDirectionImage =
                    GameObject.FindGameObjectWithTag(Utilities.Constants.TagSwitchDirectionImage)
                    .GetComponent<Image>();
            }
        }

    }

}
