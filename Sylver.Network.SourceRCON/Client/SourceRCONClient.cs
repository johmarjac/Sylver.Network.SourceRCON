using Sylver.Network.Client;
using Sylver.Network.Data;
using Sylver.Network.SourceRCON.Data;
using System;
using System.Text;

namespace Sylver.Network.SourceRCON.Client
{
    public class SourceRCONClient : NetClient
    {

        public bool IsAuthenticated { get; private set; }

        public event EventHandler OnClientConnected;
        public event EventHandler OnClientDisconnected;

        public event EventHandler<bool> OnAuthenticationResult;

        public event EventHandler<string> OnCommandResponse;

        public SourceRCONClient()
        {
            PacketProcessor = new SourceRCONPacketProcessor();
        }

        public SourceRCONClient(NetClientConfiguration clientConfiguration) : base(clientConfiguration)
        {
            PacketProcessor = new SourceRCONPacketProcessor();
        }

        public void Authenticate(string password)
        {
            using(var packet = new SourceRCONPacket(0, 3))
            {
                var data = Encoding.Default.GetBytes(password);
                packet.Write(data, 0, data.Length);

                Send(packet);
            }
        }

        public void ExecCommand(string cmd)
        {
            using (var packet = new SourceRCONPacket(1, 2))
            {
                var data = Encoding.Default.GetBytes(cmd);
                packet.Write(data, 0, data.Length);

                Send(packet);
            }
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            base.HandleMessage(packet);

            var size = packet.Read<int>();
            var id = packet.Read<int>();
            var type = packet.Read<int>();

            if(type == 2 && id == -1)
            {
                IsAuthenticated = false;
                OnAuthenticationResult?.Invoke(this, false);
            }
            else if(type == 2 && id == 0)
            {
                IsAuthenticated = true;
                OnAuthenticationResult?.Invoke(this, true);
            }

            if(type == 0)
            {
                var len = (int)(packet.Length - packet.Position - 1);
                var bData = packet.Read<byte>(len);
                var str = Encoding.Default.GetString(bData);

                OnCommandResponse?.Invoke(this, str);
            }
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            OnClientConnected?.Invoke(this, EventArgs.Empty);

        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();
            OnClientDisconnected?.Invoke(this, EventArgs.Empty);
        }
    }
}
