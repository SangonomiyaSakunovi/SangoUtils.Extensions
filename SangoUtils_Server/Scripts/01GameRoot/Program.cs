using SangoUtils_Server_App;


SangoServerRoot.Instance.OnInit();

Task.Run(() =>
{
    while (true)
    {
        SangoServerRoot.Instance.Update();
        Thread.Sleep(10);
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
