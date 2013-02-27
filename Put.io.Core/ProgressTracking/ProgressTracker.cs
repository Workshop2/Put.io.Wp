using System;
using System.Collections.Generic;
using System.Linq;

namespace Put.io.Core.ProgressTracking
{
    public delegate void ProgressTrackerHandler(bool isWorking);

    public class ProgressTracker
    {
        private List<int> Tracker { get; set; }
        private Random IDGenerator { get; set; }
        private bool PreviousStatus { get; set; }
        public event ProgressTrackerHandler OnProgressChanged;

        public ProgressTracker()
        {
            Tracker = new List<int>();
            IDGenerator = new Random();
            PreviousStatus = false;
        }

        /// <summary>
        /// Starts a new progress transaction. Events may raise from this call.
        /// </summary>
        /// <returns>Unique ID for the transaction</returns>
        public int StartNewTransaction()
        {
            var id = GetUniqueID();

            Tracker.Add(id);

            UpdateProgress();

            return id;
        }

        /// <summary>
        /// If id exists, then it is removed from the tracker. Events may raise from this call.
        /// </summary>
        public void CompleteTransaction(int id)
        {
            Tracker.Remove(id);
            UpdateProgress();
        }

        private int GetUniqueID()
        {
            var id = IDGenerator.Next(0, 999999);

            //Like this will ever happen, but its worth being double sure...right? :P
            while (Tracker.Any(x => x == id))
            {
                id = IDGenerator.Next(0, 999999);
            }

            return id;
        }

        private void UpdateProgress()
        {
            var currentStatus = Tracker.Any();
            if (currentStatus == PreviousStatus)
                return;

            PreviousStatus = currentStatus;

            if (OnProgressChanged != null)
            {
                OnProgressChanged(currentStatus);
            }
        }
    }
}