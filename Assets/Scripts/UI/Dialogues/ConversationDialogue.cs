﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Extensions;
using Objects.Movable.Characters;
using Objects.Movable.Characters.Individuals;

namespace UI.Dialogues
{
    public sealed class ConversationDialogue : UIDialogue
    {
        public string mainText
        {
            set
            {
                Text t = transform.Find("Main Text").GetComponent<Text>();
                t.text = value;
            }
        }
        public string titleText
        {
            set
            {
                Text t = transform.Find("Title Text").GetComponent<Text>();
                t.text = value;
            }
        }
        public string continueText
        {
            set
            {
                Text t = transform.Find("Continue Button").GetComponent<Text>();
                t.text = value;
            }
        }
        public string[] responseTexts
        {
            set
            {
                Debug.Assert(value.Length <= 4);
                var t = from obj in this.GetComponentsInChildren<Transform>()
                        where obj.name.Contains("Response Text")
                        select obj.GetComponent<Text>();

                Text[] text = t.ToArray();
                for (int i = 0; i < value.Length; i++)
                    text[i].text = value[i];
            }
        }

        public Sprite mainImage
        {
            set {
                Image i = transform.Find("Image").GetComponent<Image>();
                i.sprite = value;
            }
        }

        public void ResponsePressed(int i)
        {
        }

        public void ContinuePressed()
        {
            
        }

        public void Reset()
        {
            mainText = "";
            titleText = "";
            responseTexts = new string[4] { "", "", "", "" };
            mainImage = null;
        }
    }
}
