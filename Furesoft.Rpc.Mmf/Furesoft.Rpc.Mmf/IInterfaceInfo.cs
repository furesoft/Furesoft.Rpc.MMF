namespace Furesoft.Rpc.Mmf.InformationApi
{
    public interface IInterfaceInfo
    {
        InterfaceInfo GetInfo(string name);
        string[] GetInterfaceNames();
    }
}