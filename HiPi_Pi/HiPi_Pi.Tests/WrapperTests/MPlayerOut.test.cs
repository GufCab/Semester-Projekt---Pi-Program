using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using NUnit.Framework;
using MPlayer;

namespace HiPi_Pi.Tests.WrapperTests
{
    [TestFixture]
    public class MPlayerOutTests : AssertionHelper
    {
        private MPlayerOut testMPlayerOut;

        [Test]
        public void MPlayerOutCtorStreamSetCorr()
        {
            var streamMock = MockRepository.GenerateMock<StreamReader>();

            testMPlayerOut = new MPlayerOut(streamMock);
            Assert.AreEqual(streamMock, testMPlayerOut.GetStream());
        }

        //ToDo Test of Multithreaded systems..
    }
}
