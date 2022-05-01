using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class HumanReaction : MonoBehaviour
{
    [SerializeField]
    private GameObject ConfussionIcon;

    [SerializeField]
    private GameObject TriggerIcon;

    [SerializeField]
    private float confussionTime;
    [SerializeField]
    private float triggerTime;

    public EmotionEvent EmotionChangeEvent;

    private GameObject currentEmotion;

    private Sequence resetSequence;

    private EmotionType currentEmotionType;


    public void OnConfussion(Vector2 position)
    {
        if (currentEmotionType != EmotionType.Nutral)
        {
            OnTrigger(position);
            return;
        }
        if (currentEmotionType == EmotionType.Confussion)
            return;
        currentEmotionType = EmotionType.Confussion;
        EmotionChangeEvent?.Invoke(new EmotionChangeTrigger() { type = EmotionType.Confussion, position = position});
        DisplayEmotion(ConfussionIcon);
        StartClearEmotionProcess(confussionTime);
    }

    public void StartClearEmotionProcess(float clearTime)
    {
        if (resetSequence != null)
            resetSequence.Kill();
        resetSequence = DOTween.Sequence().AppendInterval(clearTime).AppendCallback(() => { NutraliseEmotion(); resetSequence = null; });
    }

    public void OnTrigger(Vector2 position)
    {
        if (currentEmotionType == EmotionType.Triggered)
            return;
        currentEmotionType = EmotionType.Triggered;
        EmotionChangeEvent?.Invoke(new EmotionChangeTrigger() { type = EmotionType.Triggered, position = position});
        
        DisplayEmotion(TriggerIcon);
        StartClearEmotionProcess(triggerTime);
    }

    private void NutraliseEmotion()
    {
        if (currentEmotionType == EmotionType.Nutral)
            return;
        currentEmotionType = EmotionType.Nutral;
        EmotionChangeEvent?.Invoke(new EmotionChangeTrigger() { type = EmotionType.Nutral, position = transform.position });
        
        ClearEmotion();
    }

    private void ClearEmotion() => DisplayEmotion(null);
    private void DisplayEmotion(GameObject emotion)
    {
        if (currentEmotion != null)
        {
            Destroy(currentEmotion);
            currentEmotion = null;
        }
        if (emotion != null)
        {
            currentEmotion = Instantiate(emotion, transform.position, Quaternion.identity);
            currentEmotion.GetComponent<HumanFollow>().SetTarget(transform);
        }
    }

    private void OnDestroy()
    {
        resetSequence.Kill();
    }

}

public enum EmotionType
{
    Nutral,
    Confussion,
    Triggered
}
