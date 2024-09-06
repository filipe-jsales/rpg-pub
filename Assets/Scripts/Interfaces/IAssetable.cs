namespace Interfaces
{
    public interface IAssetable<in T>
    {
        T Asset { set; }
    }   
}
