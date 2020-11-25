namespace Cubra
{
    public class Coin : ObjectsToCollect
    {
        public override void ActionsOnEnter(Character character)
        {
            character.SetSound(Character.Sounds.Coin);
            character.PlayingSound.PlaySound();

            GameManager.Instance.Coins++;

            InstanseObject.SetActive(false);
        }
    }
}