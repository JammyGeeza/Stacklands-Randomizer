using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stacklands_Randomizer_Mod.GUI
{
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager instance;
        private static CustomButton _connectionStatusElement;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Create a connection status UI element in the main menu.
        /// </summary>
        public void CreateConnectionStatusElement()
        {   
            // Ignore if already created
            if (_connectionStatusElement is not null) return;

            if (GameCanvas.instance.GetScreen<MainMenu>() is MainMenu menu)
            {
                Debug.Log($"Creating archipelago connection status UI element...");

                // Instantiate a clone of the Join Discord button
                CustomButton ele = Instantiate(menu.JoinDiscordButton, menu.transform.parent);
                ele.gameObject.SetActive(true);

                // Prevent underlines
                ele.EnableUnderline = false;

                // Set position as top-middle
                ele.RectTransform.anchorMin = new Vector2(0.5f, 1);
                ele.RectTransform.anchorMax = new Vector2(0.5f, 1);
                ele.RectTransform.pivot = new Vector2(0.5f, 1f);
                ele.RectTransform.sizeDelta = new Vector2(750, 50);
                ele.RectTransform.anchoredPosition = new Vector2(0, -10);

                // Store element
                _connectionStatusElement = ele;

                // Set connection status as false by default
                SetConnectionStatus(false);
            }
        }

        /// <summary>
        /// Set the connection status UI element text.
        /// </summary>
        /// <param name="connected">Whether or not it is currently connected.</param>
        public void SetConnectionStatus(bool connected, string reason = null)
        {
            string status = string.Format(
                "AP Connected: {0}{1}", 
                SokLoc.Translate(connected ? SokTerms.label_yes : SokTerms.label_no),
                string.IsNullOrWhiteSpace(reason) ? "" : $"\n{reason}"
            );

            StartCoroutine(
                SetConnectionStatus(status)
            );
        }

        /// <summary>
        /// Set the connection status after one frame
        /// </summary>
        /// <param name="text">The status text to display in the element.</param>
        private IEnumerator SetConnectionStatus(string text)
        {
            yield return null;

            // Update if not null
            if (_connectionStatusElement is not null)
            {
                _connectionStatusElement.TextMeshPro.text = text;
                _connectionStatusElement.TextMeshPro.alignment = TextAlignmentOptions.Center;
                _connectionStatusElement.TextMeshPro.ForceMeshUpdate();
            }
        }
    }
}
