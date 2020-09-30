using Sylver.Network.Client;
using Sylver.Network.SourceRCON.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sylver.Network.SourceRCON.Tests
{
    class Program
    {

        private SourceRCONClient _client;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var tokenSrc = new CancellationTokenSource();

            var clientConfig = new NetClientConfiguration("127.0.0.1", 27015);
            _client = new SourceRCONClient(clientConfig);
            _client.OnClientConnected += Client_OnClientConnected;
            _client.OnClientDisconnected += Client_OnClientDisconnected;
            _client.OnCommandResponse += Client_OnCommandResponse;
            _client.OnAuthenticationResult += _client_OnAuthenticationResult;

            _client.Connect();

            while (true)
            {
                var inp = Console.ReadLine();
                if (inp.Equals("exit"))
                    break;

                _client.ExecCommand(inp);
            }

            _client.Disconnect();

            Console.WriteLine("Program will exit now!");
            Console.ReadLine();
        }

        private void _client_OnAuthenticationResult(object sender, bool e)
        {
            if(e)
                Console.WriteLine("Authentication successful!");
            else
                Console.WriteLine("Authentication failed!");
        }

        private void Client_OnCommandResponse(object sender, string e)
        {
            Console.WriteLine(e);
        }

        private void Client_OnClientDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected!");
        }

        private void Client_OnClientConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Connected!");
            _client.Authenticate("lolgetout");
        }
    }
}
