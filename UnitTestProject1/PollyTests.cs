using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Polly;

namespace UnitTestProject1
{
    /// <summary>
    /// Some basic experiments with polly
    /// </summary>
    [TestClass]
    public class PollyTests
    {
        public int AttemptCounter { get; set; }
        public PollyTests()
        {
            AttemptCounter = 0;
        }
        [TestMethod]
        public void VerySimple_WaitAndRetry()
        {
            TimeSpan spDelay = TimeSpan.FromMilliseconds(1000);
            //Polly.PolicyBuilder builder = new PolicyBuilder();
            //builder.Retry()
            //builder.exc
            //var policy = 
            var policyBuilder=Policy.Handle<System.DivideByZeroException>();
            var policy=policyBuilder.WaitAndRetry(new TimeSpan[] { spDelay, spDelay, spDelay });
            int result = 0, x1 = 100, x2 = 200;
            policy.Execute(()=> { result=this.SimpleDivideByZero(x1, x2); });
            Trace.WriteLine($"Successfully complete, result={result}");
        }

        private int SimpleDivideByZero(int x1, int x2)
        {
            Trace.WriteLine($"{nameof(SimpleDivideByZero)}. Attempts={AttemptCounter}");
            AttemptCounter++;
            if (AttemptCounter <= 2)
            {
                Trace.WriteLine($"{nameof(SimpleDivideByZero)}. Going to throw {nameof(System.DivideByZeroException)}");
                throw new System.DivideByZeroException("Haah!. Too few attempts. Keep trying");
            }
            else
            {
                //Ok
                Trace.WriteLine($"{nameof(SimpleDivideByZero)}. Success at last");
                return x1+x2;
            }
        }

        [TestMethod]
        public void RetryForEver()
        {
            var policyBuilder = Policy.Handle<System.DivideByZeroException>();
            var policy = policyBuilder.RetryForever();
            Action<int> fnDoSomeWork=delegate(int maxretries)
            {
                Trace.WriteLine($"{nameof(fnDoSomeWork)}, Current attempts={AttemptCounter} Max attempts={maxretries}");
                this.AttemptCounter++;
                if (this.AttemptCounter > maxretries)
                {
                    Trace.WriteLine($"{nameof(fnDoSomeWork)}    Success finally!!");
                    return;
                }
                else
                {
                    Trace.WriteLine($"{nameof(fnDoSomeWork)}    going to throw exception");
                    throw new System.DivideByZeroException("Haah!. Too few attempts. Keep trying");
                }
            };
            Trace.TraceInformation("Going to attempt to do some work");
            Trace.Indent();
            policy.Execute(() => fnDoSomeWork(5));
            Trace.Unindent();
            Trace.TraceInformation("All done");
        }
    }
}
