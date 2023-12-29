using SangoUtils_Server;


SangoServerRoot.Instance.OnInit();

Task.Run(() =>
{
    while (true)
    {
        SangoServerRoot.Instance.Update();
        Thread.Sleep(18);
    }
});

while (true)
{
    string? input = Console.ReadLine();
    if (input == "Quit")
    {
        SangoServerRoot.Instance.OnDispose();
    }
}
