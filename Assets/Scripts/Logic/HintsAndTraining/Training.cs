using System;
using System.Collections;
using Logic.UI;
using UnityEngine;

namespace Logic.HintsAndTraining
{
    public class Training : MonoBehaviour
    {
        [Header("Панель обучения")]
        [SerializeField] private GameObject _trainingPanel;
        
        [Header("Текстовый компонент")]
        [SerializeField] private TextTranslation _textTranslation;

        [Header("Ключи подсказок")]
        [SerializeField] private string[] _hintKeys;

        public event Action TrainingCompleted;

        private int _learningStage;

        public void StartTraining() =>
            _ = StartCoroutine(StartTrainingCoroutine());

        private IEnumerator StartTrainingCoroutine()
        {
            yield return new WaitForSeconds(1f);
            _trainingPanel.SetActive(true);
            UpdateTrainingText();
        }
        
        public void UpdateTrainingText()
        {
            string key = _hintKeys[_learningStage];
            _textTranslation.ChangeKey(key);
            _learningStage++;
        }

        public bool CheckNumberOfHints() =>
            _learningStage < _hintKeys.Length;

        public void OnTrainingCompleted()
        {
            _trainingPanel.SetActive(false);
            TrainingCompleted?.Invoke();
        }
    }
}