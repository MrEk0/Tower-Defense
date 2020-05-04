using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject settingPanel;
    [SerializeField] GameObject levelPanel;
    [SerializeField] GameObject startPanel;

    Animator animator;

    const string ClickLevelButton = "ClickLevelButton";
    const string ClickLevelButtonBack = "ClickLevelBack";
    const string ClickSettingsButton = "ClickSettingsButton";
    const string ClickSettingsButtonBack = "ClickSettingsBack";

    int[] ids;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        ids = new int[4]
        {
             Animator.StringToHash(ClickLevelButton),
             Animator.StringToHash(ClickLevelButtonBack),
             Animator.StringToHash(ClickSettingsButton),
             Animator.StringToHash(ClickSettingsButtonBack)
        };
    }

    public void PushSettingsButton()
    {
        //GameManager.Load();
        //settingPanel.SetActive(true);
        AudioManager.PlayUIButtonAudio();

        animator.ResetTrigger(ids[3]);
        animator.SetTrigger(ids[2]);
    }

    public void PushBackSettings()
    {
        AudioManager.PlayUIButtonAudio();
        //GameManager.Save();

        animator.ResetTrigger(ids[2]);
        animator.SetTrigger(ids[3]);
    }

    public void PushLevelButton()
    {
        //GameManager.Load();
        //levelPanel.SetActive(true);
        AudioManager.PlayUIButtonAudio();
        animator.ResetTrigger(ids[1]);
        animator.SetTrigger(ids[0]);
    }

    public void PushBackLevel()
    {
        AudioManager.PlayUIButtonAudio();
        animator.ResetTrigger(ids[0]);
        animator.SetTrigger(ids[1]);
    }
}

