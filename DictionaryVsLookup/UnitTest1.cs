using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DictionaryVsLookup
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDictionary()
        {
            var teamProperties = GetUserJoinedTeams();
            var teamDictionary = teamProperties.ToDictionary(p => p.Id);
            var ownedGroup = GetOwnedGroups(teamProperties);

            List<Tuple<int, DirectoryTeam>> list = new List<Tuple<int, DirectoryTeam>>(ownedGroup.Count);
            list.AddRange(ownedGroup.Select((group) =>
                CreateClassFromGroupAndTeam(group, teamDictionary)
            ));

            Assert.AreEqual(10, list.Count);
        }

        [TestMethod]
        public void TestLookup()
        {
            var teamProperties = GetUserJoinedTeams();
            var teamLookup = teamProperties.ToLookup(p => p.Id);
            var ownedGroup = GetOwnedGroups(teamProperties);

            List<Tuple<int, DirectoryTeam>> list = new List<Tuple<int, DirectoryTeam>>(ownedGroup.Count);
            list.AddRange(ownedGroup.Select((group) =>
                CreateClassFromGroupAndTeam(group, teamLookup)
            ));

            Assert.AreEqual(10, list.Count);
        }

        List<int> GetOwnedGroups(List<DirectoryTeam> allTeams)
        {
            var list = new List<int>();
            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                list.Add(allTeams[Math.Abs(random.Next(9999))].Id);
            }
            return list;
        }

        List<DirectoryTeam> GetUserJoinedTeams()
        {
            var list = new List<DirectoryTeam>(10000);
            var random = new Random();
            for (var i = 0; i < 10000; i++)
            {
                list.Add(new DirectoryTeam { Id = random.Next() });
            }
            return list;
        }

        class DirectoryTeam
        {
            public int Id { get; set; }
            public bool IsArchived { get; set; }
        }

        private static Tuple<int, DirectoryTeam> CreateClassFromGroupAndTeam(int group, Dictionary<int, DirectoryTeam> teamDictionary)
        {
            DirectoryTeam team;
            Assert.IsTrue(teamDictionary.TryGetValue(group, out team));
            return Tuple.Create(group, team);
        }

        private static Tuple<int, DirectoryTeam> CreateClassFromGroupAndTeam(int group, ILookup<int, DirectoryTeam> teamLookup)
        {
            DirectoryTeam team;
            Assert.IsTrue(teamLookup.Contains(group));
            team = teamLookup[group].First();
            return Tuple.Create(group, team);
        }
    }
}
