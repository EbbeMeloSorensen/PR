using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Person = PR.Domain.Entities.Person;
using PersonAssociation = PR.Domain.Entities.PersonAssociation;

namespace PR.IO.UnitTest
{
    public class DataIOHandlerTest
    {
        [Fact]
        public void ExportDataToXML_Works()
        {
            new DataIOHandler()
                .ExportDataToXML(GenerateDataSet(), "Temp.xml");
        }

        [Fact]
        public void ImportDataFromXML_Works()
        {
            new DataIOHandler().ImportDataFromXML(@"Data/People.xml", out PRData prData);

            prData.People.Count.Should().Be(3);
            prData.People.Count(p => p.FirstName == "Ebbe").Should().Be(1);
            prData.People.Count(p => p.FirstName == "Uffe").Should().Be(1);
        }

        [Fact]
        public void ExportDataToJson_Works()
        {
            new DataIOHandler().ExportDataToJson(GenerateDataSet(), "Temp.json");
        }

        [Fact]
        public void ImportDataFromJson_Works()
        {
            var dataIOHandler = new DataIOHandler();
            dataIOHandler.ImportDataFromJson(@"Data/People.json", out var prData);

            prData.People.Count.Should().Be(3);
            prData.People.Count(p => p.FirstName == "Ebbe").Should().Be(1);
            prData.People.Count(p => p.FirstName == "Ana Tayze").Should().Be(1);
            prData.People.Count(p => p.FirstName == "Uffe").Should().Be(1);

            var ebbe = prData.People.Single(p => p.FirstName == "Ebbe");
            ebbe.ObjectId.Should().Be("00000000-0000-0000-0000-000000000001");

            var ana = prData.People.Single(p => p.FirstName == "Ana Tayze");
            ana.ObjectId.Should().Be("00000000-0000-0000-0000-000000000002");

            var uffe = prData.People.Single(p => p.FirstName == "Uffe");
            uffe.ObjectId.Should().Be("00000000-0000-0000-0000-000000000003");
        }

        // Helper
        private PRData GenerateDataSet()
        {
            var now = DateTime.UtcNow;

            var ebbe = new Person
            {
                Id = Guid.NewGuid(),
                ObjectId = new Guid("00000000-0000-0000-0000-000000000001"),
                FirstName = "Ebbe",
                Surname = "Melo Sørensen",
                Nickname = "Bebsen",
                Address = "Danshøjvej 33",
                ZipCode = "2500",
                City = "Valby",
                Birthday = new DateTime(1980, 6, 13).ToUniversalTime(),
                Category = "Familie",
                Description = "Mig selv",
                Dead = false,
                Created = new DateTime(2022, 1, 1, 3, 3, 6).ToUniversalTime()
            };

            var ana = new Person
            {
                Id = Guid.NewGuid(),
                ObjectId = new Guid("00000000-0000-0000-0000-000000000002"),
                FirstName = "Ana Tayze",
                Surname = "Melo Sørensen",
                Created = now
            };

            var uffe = new Person
            {
                Id = Guid.NewGuid(),
                ObjectId = new Guid("00000000-0000-0000-0000-000000000003"),
                FirstName = "Uffe",
                Surname = "Sørensen",
                Created = now
            };

            return new PRData
            {
                People = new List<Person>
                {
                    ebbe,
                    ana,
                    uffe
                },
                PersonAssociations = new List<PersonAssociation>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ObjectId = new Guid("00000000-0000-0000-0001-000000000000"),
                        Description = "is the brother of",
                        Created = now,
                        SubjectPersonId = uffe.Id,
                        SubjectPersonObjectId = uffe.ObjectId,
                        ObjectPersonId = ebbe.Id,
                        ObjectPersonObjectId = ebbe.ObjectId
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ObjectId = new Guid("00000000-0000-0000-0002-000000000000"),
                        Description = "is married with",
                        Created = now,
                        SubjectPersonId = ebbe.ObjectId,
                        SubjectPersonObjectId = ana.ObjectId,
                        ObjectPersonId = ana.Id,
                        ObjectPersonObjectId = ana.ObjectId
                    }
                }
            };
        }
    }
}