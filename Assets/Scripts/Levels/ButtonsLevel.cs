using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class ButtonsLevel : MonoBehaviour, IRewardedVideoAdListener
{
    // Ссылка на параметры игры
    private Parameters parameters;

    private void Awake()
    {
        parameters = Camera.main.GetComponent<Parameters>();
    }

    private void Start()
    {
        // Активация обратных вызовов для видеорекламы
        Appodeal.setRewardedVideoCallbacks(this);
    }

    /// <summary>Переключение игровой паузы (течение времени от 0 до 1)</summary>
    public void TogglePause(int time)
    {
        Time.timeScale = time;

        // Устанавливаем режим игры в зависимости от времени
        parameters.Mode = (time == 0) ? "pause" : "play";
    }

    /// <summary>Определение типа воскрешения персонажа</summary>
    public void IdentifyTypeRespawn()
    {
        // Если монет достаточно
        if (PlayerPrefs.GetInt("coins") >= 50)
        {
            // Вычитаем стоимость воскрешения
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - 50);
            // Возобновляем уровень
            ResumeLevel();
        }
        else
        {
            // Иначе проверяем предзагрузку видеорекламы
            Appodeal.isLoaded(Appodeal.REWARDED_VIDEO);
            // Затем отображаем рекламу
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        }

        // Если доступен интернет
        if (Application.internetReachability != NetworkReachability.NotReachable)
            // Разблокируем достижение (только не сначала)
            PlayServices.UnlockingAchievement(GPGSIds.achievement_10);
    }

    /// <summary>Возобновление уровня после проигрыша</summary>
    private void ResumeLevel()
    {
        // Скрываем панель затемнения, панель подсказок, рамку для кнопок, панель поражения и панель монет
        parameters[(int)Parameters.InterfaceElements.Blackout].SetActive(false);
        parameters[(int)Parameters.InterfaceElements.HintPanel].SetActive(false);
        parameters[(int)Parameters.InterfaceElements.Frame].SetActive(false);
        parameters[(int)Parameters.InterfaceElements.LosePanel].SetActive(false);
        parameters[(int)Parameters.InterfaceElements.CoinsPanel].SetActive(false);
        // Отображаем кнопку паузы
        parameters[(int)Parameters.InterfaceElements.PauseButton].SetActive(true);

        // Активируем игровой режим
        parameters.Mode = "play";

        // Оживляем персонажа
        parameters.Character.CharacterRespawn();
        // Продолжаем отсчет времени прохождения
        parameters.StartCoroutine("CountTime");

        // Возобновляем приостановленные методы
        parameters.StartLevel.Invoke();
    }

    #region Appodeal
    // Обратные вызовы для видеорекламы с вознаграждением
    public void onRewardedVideoLoaded(bool isPrecache) { }
    public void onRewardedVideoFailedToLoad() { }
    public void onRewardedVideoShown() { }
    public void onRewardedVideoClosed(bool finished) { }
    // Если реклама полностью просмотрена, возобновляем уровень
    public void onRewardedVideoFinished(double amount, string name) { ResumeLevel(); }
    public void onRewardedVideoClicked() { }
    public void onRewardedVideoExpired() { }
    #endregion


    /// <summary>Успешное завершение уровня</summary>
    public void CompleteLevel()
    {
        // Переключаем режим игры на победу
        parameters.Mode = "victory";

        // Скрываем кнопку паузы
        parameters[(int)Parameters.InterfaceElements.PauseButton].SetActive(false);

        // Отображаем результат уровня
        parameters.Invoke("ShowResults", 0);
    }
}