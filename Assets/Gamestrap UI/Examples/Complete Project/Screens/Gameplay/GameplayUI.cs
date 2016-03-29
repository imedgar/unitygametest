﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Gamestrap
{
    public class GameplayUI : MonoBehaviour
    {

        public GameObject pauseButton;
		public GameObject pausePanel;

        private bool pause;
		
		public Text highScore;
		public Text lastScore;
		public Text currentScore;
		
		void Start (){
			currentScore.enabled = false;
			pauseButton.SetActive (false);
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
                    pausePanel.GetComponent<Animator>().SetBool("Visible", true);
                }
                else
                {		
                    pausePanel.GetComponent<Animator>().SetBool("Visible", false);
                }
            }
        }
		
		void UpdateTextsUI (){
			if (GameManager.Instance.CanStartGameLogic() && GameManager.Instance.score > 0){
				pauseButton.SetActive (true);
				currentScore.enabled = true;
				currentScore.text = (Mathf.Round (GameManager.Instance.score)) + " m";
			}
			lastScore.text = PlayerPrefs.GetInt("lastscore").ToString() + " last";
			highScore.text = PlayerPrefs.GetInt("highscore").ToString() + " max";
		}
    }
}