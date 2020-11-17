namespace RR.Core
{
    public interface IAction
    {
        //The following functions are to be implemented by any class calling IAction 
        void Cancel();
    }
}