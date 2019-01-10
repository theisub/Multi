namespace Core
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Functionality extracted from the SystemUnderTestHubleObject.
    /// </summary>
    public class Functionality
    {
        private bool throwExceptionFlag;

        public void Init()
        {
            this.Message = "Init";
        }

        public void Do1()
        {
            // sometimes the asynchronous operation throws an exceptions 
            if (throwExceptionFlag) { throw new Exception();}

            this.Message += " Work";
        }

        public void Do2(TaskStatus taskStatus)
        {
            if (taskStatus == TaskStatus.RanToCompletion)
            {
                this.Message += " OK";
            }
            if (taskStatus == TaskStatus.Faulted)
            {
                this.Message += " NOK";
            }
        }

        public void DoRecurring()
        {
            if (throwExceptionFlag) { throw new Exception(); }

            this.Message += " Poll";
        }

        public string Message { get; private set; }

        public void PrepareToThrowException()
        {
            throwExceptionFlag = true;
        }
    }
}
