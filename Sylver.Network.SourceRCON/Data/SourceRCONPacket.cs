using Sylver.Network.Data;
using System.IO;

namespace Sylver.Network.SourceRCON.Data
{
    public class SourceRCONPacket : NetPacketStream
    {
        public override byte[] Buffer
        {
            get
            {
                if(State == NetPacketStateType.Write)
                {
                    long oldPosition = Position;

                    Seek(0, SeekOrigin.Begin);
                    Write((int)Length - sizeof(int) + sizeof(byte));
                    Seek(0, SeekOrigin.End);
                    Write((byte)'\0');
                    Seek((int)oldPosition, SeekOrigin.Begin);
                }

                return base.Buffer;
            }
        }

        public SourceRCONPacket(int packetId = 0, int packetType = 2)
        {
            Write(0);           // Size
            Write(packetId);    // Packet Id
            Write(packetType);  // Packet Type
        }

        public SourceRCONPacket(byte[] buffer) : base(buffer)
        {
        }
    }
}
