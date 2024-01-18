namespace SangoUtils_Server_App.Test
{
    public class ElephantExample : BaseAnimalExamlpe //Logic Scripts
    {
        public ElephantIntoRefrigeratorExample? ElephantIntoRefrigerator { get; set; }

        public Action<Type, string>? OnMessaged { get; set; }

        public override void OnMessage(string message)
        {
            OnMessaged?.Invoke(typeof(ElephantExample), message);
        }
    }
    public class ElephantIntoRefrigeratorExample //Logic Scripts
    {
        private readonly ElephantExample elephant = BoxServiceExample.Instance.GetAnimal<ElephantExample>();
        public void Main()
        {
            elephant.ElephantIntoRefrigerator = this;
            elephant.OnMessaged = OnMessaged;
        }

        private void OnMessaged(Type type, string message)
        {
            Console.WriteLine("A message type: " + type.FullName);
            Console.WriteLine("A message Received: " + message);
        }
    }
}
