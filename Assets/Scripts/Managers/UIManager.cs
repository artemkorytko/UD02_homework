using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Text rewardsText;

    private UIEffects _effects;
    private GameObject _currentPanel;

    private void Awake()
    {
        _effects = new UIEffects();
    }
    
    private async UniTask EnableCurrentPanel()
    {
        if (_currentPanel == null) return;
        
        _currentPanel.SetActive(true);
        await _effects.PanelFadeInEffect(_currentPanel);
    }
    
    public async UniTask DisableCurrentPanel()
    {
        if (_currentPanel == null) return;

        await _effects.PanelFadeOutEffect(_currentPanel);
        _currentPanel.SetActive(false);
        _currentPanel = null;
    }

    private async UniTask ShowPanel(GameObject panel)
    {
        await DisableCurrentPanel();
        _currentPanel = panel;
        await EnableCurrentPanel();
    }

    public async UniTask ShowStartPanel()
    {
        await ShowPanel(startPanel);
    }
    
    public async UniTask ShowGamePanel()
    {
        await ShowPanel(gamePanel);
    }
    
    public async UniTask ShowWinPanel(Dictionary<Viking, RewardComponent> rewards, RewardComponent playerReward)
    {
        await ShowPanel(winPanel);
        
        var tempString = "REWARDS:\n";
        tempString += $"Player get {playerReward.name}.\n";
        
        foreach (var viking in rewards)
        {
            tempString += $"{viking.Key.name} get {viking.Value}.\n";
        }

        await _effects.TextEffect(rewardsText, tempString);
    }
}
