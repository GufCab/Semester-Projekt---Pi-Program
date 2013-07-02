using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PlaybackCtrl;
using Containers;

/// <summary>
/// This namespace contains tests for classes in the PlaybackCtrl namespace
/// </summary>
namespace PlaybackCtrlTests
{
    /// <summary>
    /// This testfixture tests the PlayqueueHandler
    /// </summary>
    [TestFixture]
    class PlayqueueHandlerTests
    {
        /// <summary>
        /// This test creates a new PlayqueueHandler and calls GetNumberOfTracks while the playqueue is empty.
        /// </summary>
        [Test]
        public void GetNumberOfTracks_ExpectedZero()
        {
            var myQueue = new PlayqueueHandler();
            Assert.AreEqual(0,myQueue.GetNumberOfTracks());
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler and adds a Track to it.
        /// </summary>
        [Test]
        public void AddToPLayQueue_ExpectedTrackAdded()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            myQueue.AddToPlayQueue(t1);

            Assert.AreEqual(1,myQueue.GetNumberOfTracks());
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler and adds a Track to it, then calls GetNumberOfTracks.
        /// </summary>
        [Test]
        public void GetNumberOfTracks_ExpectedOne()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            myQueue.AddToPlayQueue(t1);

            Assert.AreEqual(1, myQueue.GetNumberOfTracks());
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler, adds a Track to it, removes the Track and then calls GetNumberOfTracks.
        /// </summary>
        [Test]
        public void RemoveFromPlayQueue_OneTrackAdded_ExpectedNoTracksLeft()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            myQueue.AddToPlayQueue(t1);
            myQueue.RemoveFromPlayQueue(1);

            Assert.AreEqual(0, myQueue.GetNumberOfTracks());
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler and tries to remove a Track from it while it is empty. The lack of Exceptions is due to the data validation in function RemoveFromPlayQueue
        /// </summary>
        [Test]
        public void RemoveFromPlayQueue_NoTrackAdded_ExpectedNoExceptionThrown()
        {
            var myQueue = new PlayqueueHandler();
            myQueue.RemoveFromPlayQueue(7);
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler, adds a Track to it and then calls GetTrack.
        /// </summary>
        [Test]
        public void GetTrack_OneTrackAdded_TrackReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            t1.Title = "TestTrack";
            myQueue.AddToPlayQueue(t1);
            ITrack returnedTrack = myQueue.GetTrack(1);

            Assert.AreEqual("TestTrack",returnedTrack.Title);
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler and calls GetNextTrack while the playqueue is empty.
        /// </summary>
        [Test]
        public void GetNextTrack_NoTracksAdded_ExpectedDummyReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack returnedTrack = myQueue.GetNextTrack();
            Assert.AreEqual("",returnedTrack.Path);
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler, adds a Track and calls GetNextTrack.
        /// </summary>
        [Test]
        public void GetNextTrack_OneTracksAdded_ExpectedTrackReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            t1.Title = "TestTrack";
            myQueue.AddToPlayQueue(t1);
            ITrack returnedTrack = myQueue.GetNextTrack();
            Assert.AreEqual("TestTrack",returnedTrack.Title);
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler, adds 3 unique Tracks to it and calls GetTrack(2) to receive the middle one.
        /// </summary>
        [Test]
        public void GetTrack_ThreeTracksAdded_ExpectedMiddleTrackReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            ITrack t2 = new Track();
            ITrack t3 = new Track();
            t1.Title = "Track1";
            t2.Title = "Track2";
            t3.Title = "Track3";
            myQueue.AddToPlayQueue(t1);
            myQueue.AddToPlayQueue(t2);
            myQueue.AddToPlayQueue(t3);
            ITrack returnedTrack = myQueue.GetTrack(2);
            Assert.AreEqual("Track2",returnedTrack.Title);
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler and calls GetTrack(1) while the playqueue is empty.
        /// </summary>
        [Test]
        public void GetTrack_NoTracksAdded_ExpectedDummyReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack returnedTrack = myQueue.GetTrack(1);
            Assert.AreEqual("", returnedTrack.Path);
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler, adds 3 Tracks and calls GetTrack(4).
        /// </summary>
        [Test]
        public void GetTrack_ThreeTracksAdded_AttemptedToGetFourth_ExpectedDummyReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            ITrack t2 = new Track();
            ITrack t3 = new Track();
            myQueue.AddToPlayQueue(t1);
            myQueue.AddToPlayQueue(t2);
            myQueue.AddToPlayQueue(t3);
            ITrack returnedTrack = myQueue.GetTrack(4);
            Assert.AreEqual("", returnedTrack.Path);
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler, adds 3 unique Tracks, call GetNextTrack twice, then calls GetPrevTrack.
        /// </summary>
        [Test]
        public void GetPrevTrack_ThreeTracksAdded_IndexAtTwo_ExpectedFirstTrackReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            ITrack t2 = new Track();
            ITrack t3 = new Track();
            t1.Title = "Track1";
            t2.Title = "Track2";
            t3.Title = "Track3";
            myQueue.AddToPlayQueue(t1);
            myQueue.AddToPlayQueue(t2);
            myQueue.AddToPlayQueue(t3);
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            ITrack returnedTrack = myQueue.GetPrevTrack();
            Assert.AreEqual("Track1", returnedTrack.Title);
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler, adds 3 Tracks to it and calls GetPrevTrack while at index 0.
        /// </summary>
        [Test]
        public void GetPrevTrack_ThreeTracksAdded_IndexAtZero_ExpectedDummyReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            ITrack t2 = new Track();
            ITrack t3 = new Track();
            myQueue.AddToPlayQueue(t1);
            myQueue.AddToPlayQueue(t2);
            myQueue.AddToPlayQueue(t3);
            ITrack returnedTrack = myQueue.GetPrevTrack();
            Assert.AreEqual("", returnedTrack.Path);
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler, adds 3 Tracks, calls GetNextTrack twice and calls GetCurrentTrackIndex.
        /// </summary>
        [Test]
        public void GetCurrentTrackIndex_ThreeTracksAdded_NextTrackCalledTwice_ExpectedIndexEqualsTwo()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            ITrack t2 = new Track();
            ITrack t3 = new Track();
            myQueue.AddToPlayQueue(t1);
            myQueue.AddToPlayQueue(t2);
            myQueue.AddToPlayQueue(t3);
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            Assert.AreEqual(2,myQueue.GetCurrentTrackIndex());
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler, adds 3 Tracks, calls GetNextTrack twice, GetPrevTrack once and then calls GetCurrentTrackIndex.
        /// </summary>
        [Test]
        public void GetCurrentTrackIndex_ThreeTracksAdded_NextTrackCalledTwicePrevCalledOnce_ExpectedIndexEqualsTwo()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            ITrack t2 = new Track();
            ITrack t3 = new Track();
            myQueue.AddToPlayQueue(t1);
            myQueue.AddToPlayQueue(t2);
            myQueue.AddToPlayQueue(t3);
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetPrevTrack();
            Assert.AreEqual(1, myQueue.GetCurrentTrackIndex());
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler, adds 2 Tracks to it, calls GetNextTrack five times and then calls GetCurrentTrackIndex.
        /// </summary>
        [Test]
        public void GetCurrentTrackIndex_TwoTracksAdded_NextTrackCalledFiveTimes_ExpectedIndexEqualsTwo()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            ITrack t2 = new Track();
            myQueue.AddToPlayQueue(t1);
            myQueue.AddToPlayQueue(t2);
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            Assert.AreEqual(2, myQueue.GetCurrentTrackIndex());
        }

        /// <summary>
        /// This test creates a new PlayQueueHandler, adds 2 Tracks, calls GetNextTrack and GetPrevTrack a bunch of times and then calls GetCurrentTrackIndex.
        /// </summary>
        [Test]
        public void GetCurrentTrackIndex_TwoTracksAdded_NextTrackCalledFiveTimesPrevCalledFiveTimesNextCalledFiveTimes_ExpectedIndexEqualsTwo()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            ITrack t2 = new Track();
            myQueue.AddToPlayQueue(t1);
            myQueue.AddToPlayQueue(t2);
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetPrevTrack();
            myQueue.GetPrevTrack();
            myQueue.GetPrevTrack();
            myQueue.GetPrevTrack();
            myQueue.GetPrevTrack();
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            myQueue.GetNextTrack();
            Assert.AreEqual(2, myQueue.GetCurrentTrackIndex());
        }

        /// <summary>
        /// This test creates a new PlayqueueHandler and calls GetPrevTrack while playqueue is empty.
        /// </summary>
        [Test]
        public void GetPrevTrack_NoTracksAdded_ExpectedDummyReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack returnedTrack = myQueue.GetPrevTrack();
            Assert.AreEqual("", returnedTrack.Path);
        }
    }
}
