using UnityEngine;

namespace RR.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;//Starts as null
        public void StartAction(IAction action)
        {
            if(currentAction == action) return;//action is already the current one so nothing to cancel

            if(currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;//Set current action to the new action instead
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}