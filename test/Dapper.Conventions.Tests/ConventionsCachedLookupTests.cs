using AutoFixture;
using Dapper.Conventions.Attributes;
using Dapper.Conventions.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Dapper.Conventions.Tests
{
    public class ConventionsCachedLookupTests
    {
        private Fixture fixture = new Fixture();

        [Test]
        public void ConventionsCachedLookup_Ctor_Throws_InvalidOperationException_When_ClassIsNotAnnotated()
        {
            // arrage
            Action sut = () => new ConventionsCachedLookup<OrderQueriesWithoutAnnotation>(fixture.Create<string>(), fixture.Create<string>());

            // act & assert
            sut.Should().Throw<InvalidOperationException>().WithMessage("**To use this service please Mark**");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void ConventionsCachedLookup_Ctor_Throws_ArgumentException_When_BaseDirectoryNullOrEmpty(string baseDirectory)
        {
            // arrage
            Action sut = () => new ConventionsCachedLookup<OrderQueries>(baseDirectory, fixture.Create<string>());

            // act & assert
            sut.Should().Throw<ArgumentException>().WithMessage("**baseDirectory**");
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void ConventionsCachedLookup_Ctor_Throws_ArgumentException_When_fileExtensionsNullOrEmpty(string fileExtensions)
        {
            // arrage
            Action sut = () => new ConventionsCachedLookup<OrderQueries>(fixture.Create<string>(), fileExtensions);

            // act & assert
            sut.Should().Throw<ArgumentException>().WithMessage("**fileExtensions**");
        }

        [Test]
        public void ConventionsCachedLookup_Ctor_Throws_InvalidOperationException_When_DirectoryDontExists()
        {
            // arrage
            Action sut = () => new ConventionsCachedLookup<OrderQueries>(fixture.Create<string>(), fixture.Create<string>());

            // act & assert
            sut.Should().Throw<DirectoryNotFoundException>().WithMessage("**Directory**does not exists**");
        }

        [Test]
        public void ConventionsCachedLookup_Ctor_SuccessfullyMaps()
        {
            // arrage & act
            var directory = "Scripts";
            var extensions = "sql";  
            var queries = new OrderQueries(new ConventionsCachedLookup<OrderQueries>(directory, extensions));

            
            // assert - check all methods
            var sqlGetAll = queries.GetAll();
            sqlGetAll.Should().Be("GetAllContents");

            var sqlGetOne = queries.GetOne();
            sqlGetOne.Should().Be("AnotherNameContents");

            var sqlPaginated = queries.GetPaginated();
            sqlPaginated.Should().Be("GetPaginatedContents");
        }

        [Test]
        public void ConventionsCachedLookup_Ctor_ThrowsAggregateException_When_Files_NotPresent()
        {
            // arrage & act
            var directory = "Scripts";
            var extensions = "sql";
            Action sut = () => new ConventionsCachedLookup<OrderQueriesNotSuccess>(directory, extensions);


            // assert - check all methods
            var notFoundFiles = new[] { "GetPaginated", "AnotherName" };

            sut.Should()
                .Throw<AggregateException>()
                .Where(
                    ag => ag.InnerExceptions.Count == 2     // 2 files dont exists.
                )
                .And.InnerExceptions
                .All(
                    i => i.Message.Contains("**Could not find file**") && 
                    notFoundFiles.Any(fileName => i.Message.Contains(fileName))
            );
        }


        [Test]
        public void ConventionsCachedLookup_Ctor_Successfully2Maps()
        {
            // arrage & act
            var directory = "Scripts";
            var extensions = "sql";
            var queries = new OrderQueriesSuccessButWithoutNaming(new ConventionsCachedLookup<OrderQueriesSuccessButWithoutNaming>(directory, extensions));


            // assert - check all methods
            var sqlGetJustOne = queries.GetJustOne();
            sqlGetJustOne.Should().Be("GetJustOneContents");

            var sqlGetOne = queries.GetOne();
            sqlGetOne.Should().Be("AnotherName2Contents");

            var sqlTemp = queries.Temp();
            sqlTemp.Should().Be("TempContents");
        }
    }


    public class OrderQueriesWithoutAnnotation
    {
        private IConventionsLookup<OrderQueriesWithoutAnnotation> conventionsQuery;

        public OrderQueriesWithoutAnnotation(IConventionsLookup<OrderQueriesWithoutAnnotation> conventionsQuery)
        {
            this.conventionsQuery = conventionsQuery;
        }


        public object GetAll() => throw new NotImplementedException();
    }


    [UseConventions("SuccessFolder/Orders")]
    public class OrderQueries
    {
        private IConventionsLookup<OrderQueries> conventionsQuery;

        public OrderQueries(IConventionsLookup<OrderQueries> conventionsQuery)
        {
            this.conventionsQuery = conventionsQuery;
        }


        public string GetAll() => conventionsQuery.GetQuery();

        [OverrideConventions("AnotherName")]
        public string GetOne() => conventionsQuery.GetQuery();

        public string GetPaginated() => conventionsQuery.GetQuery();
    }





    [UseConventions("NotSuccessFolder1/Orders")]
    public class OrderQueriesNotSuccess
    {
        private IConventionsLookup<OrderQueriesNotSuccess> conventionsQuery;

        public OrderQueriesNotSuccess(IConventionsLookup<OrderQueriesNotSuccess> conventionsQuery)
        {
            this.conventionsQuery = conventionsQuery;
        }


        public string GetAllDifferent() => conventionsQuery.GetQuery();

        [OverrideConventions("AnotherName")]
        public string GetOne() => conventionsQuery.GetQuery();

        public string GetPaginated() => conventionsQuery.GetQuery();
    }





    [UseConventions]
    public class OrderQueriesSuccessButWithoutNaming
    {
        private IConventionsLookup<OrderQueriesSuccessButWithoutNaming> conventionsQuery;

        public OrderQueriesSuccessButWithoutNaming(IConventionsLookup<OrderQueriesSuccessButWithoutNaming> conventionsQuery)
        {
            this.conventionsQuery = conventionsQuery;
        }


        public string GetJustOne() => conventionsQuery.GetQuery();

        [OverrideConventions("AnotherName2")]
        public string GetOne() => conventionsQuery.GetQuery();

        public string Temp() => conventionsQuery.GetQuery();
    }


}