namespace Revenant.Shared.Packets
{
    /// <summary>
    /// 실시간 서버 패킷 식별자. 클라이언트와 서버가 같은 정의를 공유한다.
    /// </summary>
    public enum PacketId : ushort
    {
        None = 0,
        Heartbeat = 1,
    }
}
