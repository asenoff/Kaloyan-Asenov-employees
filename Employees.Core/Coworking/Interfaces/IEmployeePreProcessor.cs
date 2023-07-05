namespace Employees.Core.Coworking.Interfaces
{
    public interface ICoworkersPreProcessor
    {
        public Dictionary<ulong, Dictionary<uint, List<(DateTime, DateTime)>>> PreProcess(string data);
    }
}
