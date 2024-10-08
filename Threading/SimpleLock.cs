using System.Runtime.CompilerServices;

namespace SimpleUtilities.Threading{
    public static class SimpleLock{

        private static readonly Random Random = new();

        ///<summary> Locks multiple objects in a thread-safe way. If one of the locks can't be acquired, the methods will unlock all the locks and try again. </summary>
        ///<param name="maxRetries"> The maximum number of retries before throwing an exception </param>
        ///<param name="delay"> The delay between retries in milliseconds. WARNING delay will be multiplied by 2 every time it retries </param>
        ///<param name="locks"> The objects to lock </param>
        ///<remarks> If delay = -1, thread will wait indefinitely until all locks are acquired </remarks>
        public static void Lock(object[] locks, int maxRetries = 7, int delay = 25) {
            if (locks == null || locks.Length == 0) throw new ArgumentException("No locks provided");
            if (maxRetries < 1) throw new ArgumentException("Max retries must be greater than 0");
            if (delay < 1 && delay != -1) throw new ArgumentException("Delay must be greater than 0");

            locks = locks.OrderBy(obj => RuntimeHelpers.GetHashCode(obj)).ToArray();
            var lockTaken = new bool[locks.Length];

            for (var retry = 0; retry < maxRetries; retry++){
                
                var acquiredAll = true;

                int i;
                for (i = 0; i < locks.Length; i++) {
                    if (delay != -1) {
                        Monitor.TryEnter(locks[i], 50, ref lockTaken[i]);

                        if (lockTaken[i]) continue; 

                        acquiredAll = false;
                        break;
                    }
                    
                    Monitor.Enter(locks[i]);
                    lockTaken[i] = true;
                }

                if (acquiredAll) return;
                
                for (var j = 0; j <= i; j++) {
                    if (!lockTaken[j]) continue;

                    Monitor.Exit(locks[j]);
                    lockTaken[j] = false;
                }

                Thread.Sleep((int)(delay * (1 + Random.NextDouble())));
                delay = Math.Min(delay * 2, 1000);
            }

            throw new InvalidOperationException("Failed to acquire all locks after multiple retries.");
        }

        ///<summary> Locks a single object in a thread-safe way. If the lock can't be acquired, the methods will unlock the lock and try again. </summary>
        /// <param name="lockObject"> The object to lock </param>
        /// <param name="maxRetries"> The maximum number of retries before throwing an exception </param>
        /// <param name="delay"> The delay between retries in milliseconds. WARNING delay will be multiplied by 2 every time it retries </param>
        public static void Lock(object lockObject, int maxRetries = 7, int delay = 25) {

            if (maxRetries < 1) throw new ArgumentException("Max retries must be greater than 0");
            if (delay < 1 && delay != -1) throw new ArgumentException("Delay must be greater than 0");

            for (var retry = 0; retry < maxRetries; retry++) {
                
                var lockTaken = false;

                if (delay != -1) {
                    Monitor.TryEnter(lockObject, 50, ref lockTaken);
                    if(lockTaken) return;
                }
                else {
                    Monitor.Enter(lockObject);
                    return;
                }

                Thread.Sleep((int)(delay * (1 + Random.NextDouble())));
                delay = Math.Min(delay * 2, 1000);
            }

            throw new InvalidOperationException("Failed to acquire all locks after multiple retries.");
        }

        ///<summary> Locks two objects in a thread-safe way. If one of the locks can't be acquired, the methods will unlock all the locks and try again. </summary>
        ///<param name="lockObjectA"> The first object to lock </param>
        ///<param name="lockObjectB"> The second object to lock </param>
        ///<param name="maxRetries"> The maximum number of retries before throwing an exception </param>
        ///<param name="delay"> The delay between retries in milliseconds. WARNING delay will be multiplied by 2 every time it retries </param>
        public static void Lock(object lockObjectA, object lockObjectB, int maxRetries = 7, int delay = 25) {
            if (maxRetries < 1) throw new ArgumentException("Max retries must be greater than 0");
            if (delay < 1 && delay != -1) throw new ArgumentException("Delay must be greater than 0");

            object[] locks;

            if (RuntimeHelpers.GetHashCode(lockObjectA) > RuntimeHelpers.GetHashCode(lockObjectB)) locks = [lockObjectB, lockObjectA];
            else locks = [lockObjectA, lockObjectB];

            bool[] lockTaken = [false, false];

            for (var retry = 0; retry < maxRetries; retry++) {

                var acquiredAll = true;

                int i;
                for (i = 0; i < locks.Length; i++) {
                    if (delay != -1) {
                        Monitor.TryEnter(locks[i], 50, ref lockTaken[i]);
                        if (lockTaken[i]) continue;

                        acquiredAll = false;
                        break;
                    }
                    
                    Monitor.Enter(locks[i]);
                    lockTaken[i] = true;
                }

                if (acquiredAll) return;

                for (var j = 0; j <= i; j++) {
                    if (!lockTaken[j]) continue;

                    Monitor.Exit(locks[j]);
                    lockTaken[j] = false;
                }

                Thread.Sleep((int)(delay * (1 + Random.NextDouble())));
                delay = Math.Min(delay * 2, 1000);

            }

            throw new InvalidOperationException("Failed to acquire all locks after multiple retries.");
        }

        ///<summary> Unlocks multiple objects. </summary>
        ///<param name="locks"> The objects to unlock </param>
        public static void Unlock(params object[] locks) {
            foreach (var obj in locks) {
                try {
                    Monitor.Exit(obj);
                }
                catch { }
            }
        }

        ///<summary> Unlocks a single object. </summary>
        ///<param name="lockObject"> The object to unlock </param>
        public static void Unlock(object lockObject) {
            try {
                Monitor.Exit(lockObject);
            }
            catch { }
        }

        ///<summary> Unlocks two objects. </summary>
        ///<param name="lockObjectA"> The first object to unlock </param>
        ///<param name="lockObjectB"> The second object to unlock </param>
        public static void Unlock(object lockObjectA, object lockObjectB) {
            try {
                Monitor.Exit(lockObjectA);
            }
            catch { }

            try {
                Monitor.Exit(lockObjectB);
            }
            catch { }
        }
    }
}
