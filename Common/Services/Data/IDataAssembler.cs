using Common.Samples;

namespace Common.Services.Data
{
    public interface IDataAssembler
    {
        string GetMeta();
        DroneSample AssembleDroneSample(string csvLine, out double _time);
    }
}
