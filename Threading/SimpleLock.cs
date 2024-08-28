using System;
using System.Linq;
using System.Threading;

namespace SimpleUtilities.Threading{
    public class SimpleLock : IDisposable{

        private readonly object[] locks;

        ///<summary> Locks multiple objects in a thread-safe way. If one of the locks can't be acquired, the methods will unlock all the locks and try again. </summary>
        ///<param name="maxRetries"> The maximum number of retries before throwing an exception </param>
        ///<param name="delay"> The delay between retries in milliseconds. WARNING delay will be multiplied by 2 every time it retries </param>
        ///<param name="locks"> The objects to lock </param>
        ///<remarks> If delay = -1, thread will wait indefinitely until all locks are acquired </remarks>
        public SimpleLock(int maxRetries = 7, int delay = 25, params object[] locks){

            if (locks == null || locks.Length == 0) throw new ArgumentException("No locks provided");
            if (maxRetries < 1) throw new ArgumentException("Max retries must be greater than 0");
            if (delay < 1 && delay != -1) throw new ArgumentException("Delay must be greater than 0");

            this.locks = locks.OrderBy(obj => obj.GetHashCode()).ToArray();
            bool[] lockTaken = new bool[this.locks.Length];

            for (int retry = 0; retry < maxRetries; retry++){
                try{
                    for (int i = 0; i < this.locks.Length; i++){
                        if(delay != -1) Monitor.TryEnter(this.locks[i], 50, ref lockTaken[i]);
                        else Monitor.Enter(this.locks[i]);

                        if(!lockTaken[i])
                            throw new InvalidOperationException("Failed to acquire lock " + this.locks[i].GetHashCode());
                    }
                       
                    return;
                }
                catch{

                    Console.WriteLine("Failed to acquire all locks. Retrying in " + delay + "ms");

                    for (int i = 0; i < this.locks.Length; i++){
                        if (lockTaken[i]){
                            Monitor.Exit(this.locks[i]);
                            lockTaken[i] = false;
                        }
                    }

                    Thread.Sleep(delay);
                    delay *= 2;
                }
            }

            throw new InvalidOperationException("Failed to acquire all locks after multiple retries.");
        }

        ///<summary> Locks multiple objects in a thread-safe way. If one of the locks can't be acquired, the methods will unlock all the locks and try again. </summary>
        ///<param name="waitUntilLocksObtained"> If true, thread will wait indefinitely until all locks are acquired </param>
        ///<param name="locks"> The objects to lock </param>
        ///<remarks> Default maxRetries = 7, delay = 25 </remarks>
        public SimpleLock(bool waitUntilLocksObtained = false, params object[] locks) : this(7, waitUntilLocksObtained ? -1 : 25, locks){}

        ///<summary> Locks multiple objects in a thread-safe way. If one of the locks can't be acquired, the methods will unlock all the locks and try again. </summary>
        ///<param name="locks"> The objects to lock </param>
        ///<remarks> Default maxRetries = 7, delay = 25 </remarks>
        public SimpleLock(params object[] locks) : this(7, 25, locks) { }

        public void Dispose(){
            if (locks == null || locks.Length == 0) throw new ArgumentException("No locks provided");

            foreach (var obj in locks) Monitor.Exit(obj);
        }
    }
}
