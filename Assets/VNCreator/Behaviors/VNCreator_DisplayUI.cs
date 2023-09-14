using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace VNCreator
{
    public class VNCreator_DisplayUI : DisplayBase
    {
        [Header("Text")]
        public Text characterNameTxt;
        public Text dialogueTxt;

        [Header("Visuals")]
        public Image characterImg;
        public Image backgroundImg;

        [Header("Audio")]
        public AudioSource musicSource;
        public AudioSource soundEffectSource;

        [Header("Buttons")]
        public Button nextBtn;
        public Button previousBtn;
        public Button saveBtn;
        public Button menuButton;

        [Header("Choices")]
        public Button choiceBtn1;
        public Button choiceBtn2;
        public Button choiceBtn3;

        [Header("End")]
        public GameObject endScreen;

        [Header("Main menu")]
        [Scene]
        public string mainMenu;

        [Header("My objects")]

        [Header("Audio Source's")]
        public AudioSource music;

        [Header("Game Objects")]
        public GameObject nameTextFrame;
        public GameObject endTransition;
        public GameObject tutorial;
        public GameObject transitionTwo;
        
        [Header("Animators")]
        public Animator photoAnimator;
        public Animator kidAppear;
        
        [Header("Variables")]
        public int nodesIn;
        public float timerOne;
        public float timerTwo;
        public bool canCount;
        public bool canOnlyCount;
        public bool canContinue;

        void Start()
        {
            endTransition.SetActive(true);
            
            canCount = true;
            canContinue = true;

            nextBtn.onClick.AddListener(delegate { NextNode(0); });

            if(previousBtn != null)
                previousBtn.onClick.AddListener(Previous);
            if(saveBtn != null)
                saveBtn.onClick.AddListener(Save);
            if (menuButton != null)
                menuButton.onClick.AddListener(ExitGame);

            if(choiceBtn1 != null)
                choiceBtn1.onClick.AddListener(delegate { NextNode(0); });
            if(choiceBtn2 != null)
                choiceBtn2.onClick.AddListener(delegate { NextNode(1); });
            if(choiceBtn3 != null)
                choiceBtn3.onClick.AddListener(delegate { NextNode(2); });

            endScreen.SetActive(false);

            StartCoroutine(DisplayCurrentNode());
            timerOne = timerTwo;
        }

        private void Update()
        {
            if (canCount)
            {
                timerOne -= Time.deltaTime;
                if (timerOne <= 0)
                {
                    endTransition.SetActive(false);

                    if (tutorial != null)
                        tutorial.SetActive(true);

                    canCount = false;
                    timerOne = timerTwo;
                }

                else
                {
                    if (tutorial != null)
                    {
                        tutorial.SetActive(false);

                        if (Input.GetMouseButton(0))
                            Destroy(tutorial);
                    }
                }
            }

            if (canOnlyCount)
            {
                timerOne -= Time.deltaTime;

                if (timerOne <= 0)
                {
                    photoAnimator.SetBool("Interact", true);
                    timerOne = timerTwo;
                    canOnlyCount = false;
                    canContinue = true;
                }
            }

            if (nodesIn == 1)
            {
                tutorial.SetActive(false);

                if (kidAppear != null)
                {
                    kidAppear.gameObject.SetActive(true);
                    kidAppear.SetTrigger("Appear");
                }
            }

            if (nodesIn == 7)
            {
                kidAppear.SetTrigger("Disappear");
                photoAnimator.SetBool("Angry Fast", true);
            }

            else if (nodesIn == 8)
            {
                photoAnimator.SetBool("Shake", true);
                photoAnimator.SetBool("Angry Fast", false);
                music.Stop();
            }

            else if (nodesIn == 10)
            {
                photoAnimator.SetBool("Shake", false);
                music.Play();
            }

            else if (nodesIn == 21)
            {
                photoAnimator.SetBool("Shake", true);
                music.Stop();
            }

            else if (nodesIn == 23)
            {
                photoAnimator.SetBool("Shake", false);
                music.Play();
            }
            
            if (nodesIn == 24)
            {
                if (transitionTwo != null)
                    transitionTwo.SetActive(true);

                Destroy(transitionTwo, 1.5f);

                canOnlyCount = true;
                canContinue = false;
                nodesIn += 2;
            }

            if (nodesIn == 27)
            {
                photoAnimator.SetBool("Interact", false);
            }

            else if (nodesIn == 33)
            {
                photoAnimator.SetBool("Interact", false);
                photoAnimator.SetBool("Angry", true);
            }

            else if (nodesIn == 43)
            {
                photoAnimator.SetBool("Shake", true);
                photoAnimator.SetBool("Angry", false);
                music.Stop();
            }

            else if (nodesIn == 44)
            {
                photoAnimator.SetBool("Shake", false);
                music.Play();
            }

            else if (nodesIn == 86)
            {
                canOnlyCount = true;

                photoAnimator.SetBool("Interact", true);
            }

            else if (nodesIn == 87)
            {
                photoAnimator.SetBool("Interact", false);
                photoAnimator.SetBool("Angry Fast", false);
            }
        }

        protected override void NextNode(int _choiceId)
        {
            if (canContinue)
            {
                if (lastNode)
                {
                    endScreen.SetActive(true);
                    return;
                }

                base.NextNode(_choiceId);
                StartCoroutine(DisplayCurrentNode());
                nodesIn++;
            }
        }

        IEnumerator DisplayCurrentNode()
        {
            characterNameTxt.text = currentNode.characterName;
            if (currentNode.characterSpr != null)
            {
                characterImg.sprite = currentNode.characterSpr;
                characterImg.color = Color.white;
                nameTextFrame.SetActive(true);
            }
            else
            {
                nameTextFrame.SetActive(false);
                characterImg.color = new Color(1, 1, 1, 0);
            }
            if(currentNode.backgroundSpr != null)
                backgroundImg.sprite = currentNode.backgroundSpr;

            if (currentNode.choices <= 1) 
            {
                nextBtn.gameObject.SetActive(true);

                choiceBtn1.gameObject.SetActive(false);
                choiceBtn2.gameObject.SetActive(false);
                choiceBtn3.gameObject.SetActive(false);

                previousBtn.gameObject.SetActive(loadList.Count != 1);
            }
            else
            {
                nextBtn.gameObject.SetActive(false);

                choiceBtn1.gameObject.SetActive(true);
                choiceBtn1.transform.GetChild(0).GetComponent<Text>().text = currentNode.choiceOptions[0];

                choiceBtn2.gameObject.SetActive(true);
                choiceBtn2.transform.GetChild(0).GetComponent<Text>().text = currentNode.choiceOptions[1];

                if (currentNode.choices == 3)
                {
                    choiceBtn3.gameObject.SetActive(true);
                    choiceBtn3.transform.GetChild(0).GetComponent<Text>().text = currentNode.choiceOptions[2];
                }
                else
                {
                    choiceBtn3.gameObject.SetActive(false);
                }
            }

            if (currentNode.backgroundMusic != null)
                VNCreator_MusicSource.instance.Play(currentNode.backgroundMusic);
            if (currentNode.soundEffect != null)
                VNCreator_SfxSource.instance.Play(currentNode.soundEffect);

            dialogueTxt.text = string.Empty;
            if (GameOptions.isInstantText)
            {
                dialogueTxt.text = currentNode.dialogueText;
                
            }
            else
            {
                
                char[] _chars = currentNode.dialogueText.ToCharArray();
                string fullString = string.Empty;
                for (int i = 0; i < _chars.Length; i++)
                {
                    fullString += _chars[i];
                    dialogueTxt.text = fullString;

                    yield return new WaitForSeconds(0.01f/ GameOptions.readSpeed);
                }
            }
        }

        protected override void Previous()
        {
            base.Previous();
            StartCoroutine(DisplayCurrentNode());
        }

        void ExitGame()
        {
            SceneManager.LoadScene(mainMenu, LoadSceneMode.Single);
        }
    }
}