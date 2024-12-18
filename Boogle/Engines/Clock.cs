using System.Threading;

namespace Boogle.Engines
{
    public class Clock
    {
        private System.Threading.Timer _timer;
        private int _remainingTimeInSeconds;
        private int _seconds;
        private bool _isRunning;
        
        public Clock(int seconds)
        {
            _seconds = seconds;
            _remainingTimeInSeconds = seconds;
            _isRunning = false;
        }
        /// <summary>
        /// Start the clock
        /// </summary>
        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _timer = new System.Threading.Timer(UpdateClock, null, 0, 1000); 
            }
        }
        /// <summary>
        /// Reset the clock
        /// </summary>
        public void Reset()
        {
            _remainingTimeInSeconds = _seconds; 
            
        }
        /// <summary>
        /// Update the clock by decreasing it or reseting it
        /// </summary>
        /// <param name="state"></param>
        private void UpdateClock(object state)
        {
            if (_remainingTimeInSeconds > 0)
            {
                Console.Clear();
                
                _remainingTimeInSeconds--;
            }
            else
            {
                Console.Clear();
                
                Stop(); 
            }
        }

       /// <summary>
       /// Stop the clock
       /// </summary>
        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _timer?.Dispose();
                Console.WriteLine("Countdown clock stopped.");
            }
        }
        /// <summary>
        /// Get the time ramaining on the clock
        /// </summary>
        /// <returns></returns>
        public int GetTimeRemaining(){
            return _remainingTimeInSeconds;
        }
        /// <summary>
        /// Return a string describing the time
        /// </summary>
        /// <returns></returns>
        public string GetFormattedTime()
    {
        int minutes = _remainingTimeInSeconds / 60;
        int seconds = _remainingTimeInSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }
    }
}