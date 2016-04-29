using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gamestrap
{
    public class GameplayUI : MonoBehaviour
    {

        public GameObject pauseButton;
		public GameObject pausePanel;
		public GameObject coinImage;

        private bool pause;
		
		public Text highScore;
		public Text lastScore;
		public Text currentScore;
		public Text speed;
		public Text coins;
		
		void Start (){
			currentScore.enabled = false;
			speed.enabled = false;
			pauseButton.SetActive (false);
			coinImage.SetActive (false);
			coins.enabled = false;
		}
		
		void Update()
	    {
	        UpdateTextsUI ();
	    }

        /// <summary>
        /// It activates the pause animation in the pause panel
        /// </summary>
        public bool Pause
        {
            get { return pause; }
            set
            {
                pause = value;

                if (pause)
                {

					if (Time.timeScale != 0f){
						Time.timeScale = 0f;
						pausePanel.GetComponent<Animator>().SetBool("Visible", true);
					}
					else {
						Time.timeScale = 1.0f;
						pausePanel.GetComponent<Animator>().SetBool("Visible", false);
					}
					
                }
            }
        }
		
		void UpdateTextsUI (){
			if (GameManager.Instance.CanStartGameLogic() && GameManager.Instance.score > 0){
				pauseButton.SetActive (true);
				currentScore.enabled = true;
				speed.enabled = (true);
				speed.text = (Mathf.Round (GameManager.Instance.naturalWorldSpeed)) + " sp";
				currentScore.text = (Mathf.Round (GameManager.Instance.score)) + " m";
				if (GameManager.Instance.showCoins){
					coinImage.SetActive (true);
					coins.enabled = (true);
					coins.text = GameManager.Instance.totalCoins + "";
				}
				else{
					coinImage.SetActive (false);
					coins.enabled = (false);
				}
			}
			lastScore.text = PlayerPrefs.GetInt("lastscore").ToString() + " last";
			highScore.text = PlayerPrefs.GetInt("highscore").ToString() + " max";
		}
    }
}