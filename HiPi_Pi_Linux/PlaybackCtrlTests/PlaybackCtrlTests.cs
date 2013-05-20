using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PlaybackCtrl;
using Containers;

namespace PlaybackCtrlTests
{
    [TestFixture]
    class PlayqueueHandlerTests
    {
        [Test]
        public void GetNumberOfTracks_ExpectedZero()
        {
            var myQueue = new PlayqueueHandler();
            Assert.AreEqual(0,myQueue.GetNumberOfTracks());
        }

        [Test]
        public void AddToPLayQueue_ExpectedTrackAdded()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            myQueue.AddToPlayQueue(t1);

            Assert.AreEqual(1,myQueue.GetNumberOfTracks());
        }

        [Test]
        public void GetNumberOfTracks_ExpectedOne()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            myQueue.AddToPlayQueue(t1);

            Assert.AreEqual(1, myQueue.GetNumberOfTracks());
        }

        [Test]
        public void RemoveFromPlayQueue_OneTrackAdded_ExpectedNoTracksLeft()
        {
            var myQueue = new PlayqueueHandler();
            ITrack t1 = new Track();
            myQueue.AddToPlayQueue(t1);
            myQueue.RemoveFromPlayQueue(1);

            Assert.AreEqual(0, myQueue.GetNumberOfTracks());
        }

        [Test]
        public void RemoveFromPlayQueue_NoTrackAdded_ExpectedNoExceptionThrown()
        {
            var myQueue = new PlayqueueHandler();
            myQueue.RemoveFromPlayQueue(7);
        }

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

        [Test]
        public void GetNextTrack_NoTracksAdded_ExpectedDummyReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack returnedTrack = myQueue.GetNextTrack();
            Assert.AreEqual("",returnedTrack.Path);
        }

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

        [Test]
        public void GetTrack_NoTracksAdded_ExpectedDummyReturned()
        {
            var myQueue = new PlayqueueHandler();
            ITrack returnedTrack = myQueue.GetTrack(1);
            Assert.AreEqual("", returnedTrack.Path);
        }

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
    }
}
