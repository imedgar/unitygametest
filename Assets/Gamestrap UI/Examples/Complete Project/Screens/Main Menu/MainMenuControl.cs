using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gamestrap
{
    public class MainMenuControl : MonoBehaviour
    {
        private static int visibleVariable = Animator.StringToHash("Visible");
        private static int notifyVariable = Animator.StringToHash("Notify");

        public GameObject settingsPanel, aboutPanel;

        public Toggle soundToggle, musicToggle;

        public Text notificationText;
        private Animator notificationAnimator;
		
		// Canvas Ref
		GameObject canvas;
		
        public void Start()
        {
            //Adds events to the Toggle buttons through code since
            //doing it through the inspector wouldn't will give the value of the button dynamically
            soundToggle.onValueChanged.AddListener(ToggleSound);
            musicToggle.onValueChanged.AddListener(ToggleMusic);

            notificationAnimator = notificationText.GetComponent<Animator>();
			
			canvas = GameObject.FindGameObjectWithTag ("UIPanel");
        }

        #region Event Methods Called from the UI
        public void PlayClick()
        {
            //GSAppExampleControl.Instance.LoadScene(ESceneNames.Levels);
			// Activate game
            GameManager.Instance.currentState = GameManager.GameStates.Roofs;
			GameManager.Instance.currentPlayerState = GameManager.PlayerStates.Running;
			canvas.SetActive(false);
        }

        public void AchievementsClick()
        {
            notificationText.text = "Achievements Clicked...";
            notificationAnimator.SetTrigger(notifyVariable);
        }

        public void LeaderboardClick()
        {
            notificationText.text = "Leaderboard Clicked...";
            notificationAnimator.SetTrigger(notifyVariable);
        }

        public void RateClick()
        {
            notificationText.text = "Rate Clicked...";
            notificationAnimator.SetTrigger(notifyVariable);
        }

        #region Settings Events
        public void ToggleSettingsPanel()
        {
            TogglePanel(settingsPanel.GetComponent<Animator>());
        }

        public void ToggleSound(bool on)
        {
            // Change the sound
        }

        public void ToggleMusic(bool on)
        {
            // Change the music
        }

        #endregion

        #region About Events

        public void FacebookClick()
        {
            Application.OpenURL("https://es.wikipedia.org/wiki/Ornithorhynchus_anatinus");
        }

        public void TwitterClick()
        {
            Application.OpenURL("https://media.tumblr.com/tumblr_m4g45bPfrr1r0y3uu.gif");

        }

        public void YoutubeClick()
        {
            Application.OpenURL("https://coubsecure-a.akamaihd.net/get/bucket:32.11/p/coub/simple/cw_image/d85f07af7f6/f09c720f2dc6ef8db429b/cotd_email_1409151304_1381571232_00026.jpg");
        }

        public void WebsiteClick()
        {
            Application.OpenURL("http://www.google.com");
        }
        #endregion

        public void ToggleAboutPanel()
        {
            TogglePanel(aboutPanel.GetComponent<Animator>());
        }

        private void TogglePanel(Animator panelAnimator)
        {
            panelAnimator.SetBool(visibleVariable, !panelAnimator.GetBool(visibleVariable));
        }
        #endregion
    }
}