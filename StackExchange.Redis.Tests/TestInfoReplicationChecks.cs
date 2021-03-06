﻿using System;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace StackExchange.Redis.Tests
{
    public class TestInfoReplicationChecks : TestBase
    {
        protected override string GetConfiguration() => base.GetConfiguration() + ",configCheckSeconds=2";
        public TestInfoReplicationChecks(ITestOutputHelper output) : base (output) { }

        [Fact]
        public void Exec()
        {
            using(var conn = Create())
            {
                var parsed = ConfigurationOptions.Parse(conn.Configuration);
                Assert.Equal(2, parsed.ConfigCheckSeconds);
                var before = conn.GetCounters();
                Thread.Sleep(TimeSpan.FromSeconds(7));
                var after = conn.GetCounters();
                int done = (int)(after.Interactive.CompletedSynchronously - before.Interactive.CompletedSynchronously);
                Assert.True(done >= 2);
            }
        }
    }
}
