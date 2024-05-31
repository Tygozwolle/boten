using RoeiVerenigingLibrary.Model;

namespace RoeiVerenigingLibrary.Services;

public class TrendService
{
    public int GetDifference(int oldResult, int newResult)
    {
        return oldResult - newResult;
    }
    
}