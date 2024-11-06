using System.Threading;

namespace Boogle.Engines
{
    public class Clock
    {
        private System.Threading.Timer _timer;
        private int _remainingTimeInSeconds;
        private bool _isRunning;
        
        public Clock()
        {
            _remainingTimeInSeconds = 60; // 1 minute in seconds
            _isRunning = false;
        }
        
        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _timer = new System.Threading.Timer(UpdateClock, null, 0, 1000); // Update every 1000ms (1 second)
                //Console.WriteLine("Countdown clock started from 1 minute...");
            }
        }
        
        public void Reset()
        {
            _remainingTimeInSeconds = 60; // Reset to 1 minute
            Console.WriteLine("Countdown clock reset to 1 minute.");
        }
        
        private void UpdateClock(object state)
        {
            if (_remainingTimeInSeconds > 0)
            {
                Console.Clear();
                Console.WriteLine("Time Remaining: " + TimeSpan.FromSeconds(_remainingTimeInSeconds).ToString("mm\\:ss"));
                _remainingTimeInSeconds--;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Time's up!");
                Stop(); // Stop the timer when it reaches zero
            }
        }

        // Stop the countdown clock
        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _timer?.Dispose();
                Console.WriteLine("Countdown clock stopped.");
            }
        }
    }
}