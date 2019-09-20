public class SawCircular : Saw
{
    protected override void Update()
    {
        // Вращаем пилу
        transform.Rotate(0, 0, rotate);
    }
}