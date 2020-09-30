using Sylver.Network.Data;
using System;

namespace Sylver.Network.SourceRCON.Data
{
    public class SourceRCONPacketProcessor : IPacketProcessor
    {
        public int HeaderSize => 4;

        public bool IncludeHeader => true;

        public int GetMessageLength(byte[] buffer)
        {
            return BitConverter.ToInt32(buffer, 0);
        }

        public INetPacketStream CreatePacket(byte[] buffer)
        {
            return new SourceRCONPacket(buffer);
        }        
    }
}
