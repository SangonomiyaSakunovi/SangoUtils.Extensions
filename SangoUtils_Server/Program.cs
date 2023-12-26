using SangoUtils_Server.Program;


SangoServerRoot.Instance.OnInit();

while (true)
{
    SangoServerRoot.Instance.Update();
    string? input = Console.ReadLine();
    if (input == "Quit")
    {
        SangoServerRoot.Instance.OnDispose();
    }
    Thread.Sleep(18);
}
