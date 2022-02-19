using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder().WithUrl("http://localhost:5259/Chat").Build();
connection.On<string>("ClientMessageHandler", ClientMessageHandler);

static void ClientMessageHandler(string message)
{
    Console.WriteLine("Message from server : {0}", message);
}

Console.WriteLine("Ожидание установки соединения с сервером сообщений. Нажмите любую кнопку, чтобы установить соединение");
Console.ReadKey();
await connection.StartAsync();
Console.WriteLine("Соединение с сервером сообщений установлено");

while (true)
{
    var message = Console.ReadLine();
    await connection.InvokeAsync("SendMessage", message);
}
