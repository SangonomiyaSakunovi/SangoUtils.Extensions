// See https://aka.ms/new-console-template for more information

using TestConsole;

ServerPeerInstance instance = new();
instance.OpenAsServer("192.168.3.55", 52517, 100);
Console.ReadKey();
