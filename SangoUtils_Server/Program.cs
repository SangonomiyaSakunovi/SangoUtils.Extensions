using SangoUtils_Server;

ServerRoot.Instance.Init();

while (true)
{
    ServerRoot.Instance.Update();
    Thread.Sleep(18);
}
