﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Data;
using Extensions.Buttons;
using System;
using UI.Dialogues;

namespace UI.Panels
{
    public sealed class SavePanel : UIPanel, UIGameObject
    {
        private const int saveIconWidth = 40;
        private int saveCount
        {
            get
            {
                int c = SaveManager.allSaveInformation.SaveCount;
                if (c <= 0) throw new Exception("Save count too small");
                return c;
            }
        }
        
        public override string name
        {
            get { return "Save Panel"; }
        }
        
        private Transform saveIconParent
        {
            get { return GameObject.Find("Save Background").transform; }
        }

        private RectTransform saveBackgroundBox
        {
            get { return saveIconParent.GetComponent<RectTransform>(); }
        }

        public Save selectedSave = null;
        
        public void SelectSave(object obj, EventArgs args)
        {
            // TODO select save action attach to button
            throw new NotImplementedException();
        }

        private List<Save> saveList
        {
            get { return SaveManager.allSaveInformation.saveList; }
        }

        private void Refresh()
        {
            saveBackgroundBox.sizeDelta = new Vector2(saveBackgroundBox.sizeDelta.x, saveCount * saveIconWidth);

            // Delete all the saves in the transform
            foreach (Transform child in saveIconParent)
                Destroy(child.gameObject);

            // Now load all the saves one by one
            int yPosition = 0;
            foreach (Save s in saveList)
            {
                // The position of the new position
                GameObject save = Instantiate(Resources.Load("UI/Save Slot")) as GameObject;
                save.transform.SetParent(saveBackgroundBox.transform);
 
                // Set the position of the save
                var rectTransform = save.GetComponent<RectTransform>();
                rectTransform.offsetMax = new Vector2(0, 0);
                rectTransform.offsetMin = new Vector2(0, 0);
                rectTransform.sizeDelta = new Vector2(0, 40);
                rectTransform.anchoredPosition = new Vector2(
                    save.GetComponent<RectTransform>().anchoredPosition.x,
                    yPosition
                );

                yPosition -= 40;

                // The name and date
                var info = save.GetComponent<SaveSlot>();
                info.name = s.name;
                info.date = s.time.ToShortDateString();
            }
        }

        public void UntoggleAll()
        {
            Toggle[] buttons = saveBackgroundBox.GetComponentsInChildren<Toggle>();
            foreach (Toggle i in buttons)
            {
                if (i.gameObject.GetComponent<SaveSlot>().save == selectedSave) continue;
                i.isOn = false;
            }
        }
        
        public void SaveSelectedSave()
        {
            if (selectedSave == null) return;
            selectedSave.SaveGame();
        }

        public void LoadSave()
        {
            if (selectedSave == null) return;
            UIManager.CurrentPanel.TurnOff();
            UIManager.CurrentPanel.TurnOff();
            SaveManager.LoadSave(selectedSave.name);
        }

        public void RenameSave()
        {
            if (selectedSave == null) return;
            throw new NotImplementedException();
        }

        public void DeleteSave()
        {
            if (selectedSave == null) return;
            
            string message = "Warning, Current Save being deleted, do you wish to continue";
            WarningDialogue.Button yes = new WarningDialogue.Button("Yes", () => { DeleteSaveConfirm(selectedSave); });
            WarningDialogue.Button no = new WarningDialogue.Button("Yes", () => {});

            WarningDialogue.Warning(message, new List<WarningDialogue.Button>() { yes, no });
        }

        private void DeleteSaveConfirm(Save s)
        {
            SaveManager.DeleteSave(s.name);
            Refresh();
        }
        
        public void Back()
        {
            this.TurnOff();
        }
        
        public override void TurnOn()
        {
            base.TurnOn();
            Refresh();
        }
    }
}
