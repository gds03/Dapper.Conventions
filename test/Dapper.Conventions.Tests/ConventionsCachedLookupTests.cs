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
        public void ConventionsCachedLookup_Ctor_Throws_AggregateExceptionWithArgumentExceptionAndFileNotFound_When_MethodNameAreEqualAndFileNotExists()
        {
            // arrage & act
            var directory = "Scripts";
            var extensions = "sql";

            // arrage
            Action sut = () => new ConventionsCachedLookup<OrderQuerieUnsucessfullyWithSameMethodNames>(directory, extensions);

            // act & assert
            sut.Should().Throw<AggregateException>()
                .And
                .InnerExceptions
                .Should()
                .HaveCount(2)
                .And
                .ContainSingle(ex => ex.GetType() == typeof(ArgumentException) && ((ArgumentException) ex).Message.Contains("GetBy"))
                .And
                .ContainSingle(ex => ex.GetType() == typeof(FileNotFoundException) && ((FileNotFoundException)ex).Message.Contains("AnotherName2"));
                
        }


        [Test]
        public void ConventionsCachedLookup_Ctor_ThrowsAggregateExceptionWithFileNotFound_When_Files_NotPresent()
        {
            // arrage & act
            var directory = "Scripts";
            var extensions = "sql";
            Action sut = () => new ConventionsCachedLookup<OrderQueriesNotSuccess>(directory, extensions);


            // assert - check all methods
            var notFoundFiles = new[] { "GetPaginated", "AnotherName" };

            sut.Should().Throw<AggregateException>()
               .And
               .InnerExceptions
               .Should()
               .HaveCount(2)
               .And
               .ContainItemsAssignableTo<FileNotFoundException>()
               .And
               .Contain(ex => notFoundFiles.Any(file => ex.Message.Contains(file)));
        }



        [Test]
        public void ConventionsCachedLookup_Ctor_SuccessfullyMaps()
        {
            // arrage & act
            var directory = "Scripts";
            var extensions = "sql";
            var sut = new ConventionsCachedLookup<OrderQueries>(directory, extensions);

            // assert - check all methods
            var sqlGetAll = sut.GetCommandFor(nameof(OrderQueries.GetAll));
            sqlGetAll.Should().Be("GetAllContents");

            var sqlGetOne = sut.GetCommandFor(nameof(OrderQueries.GetOne));
            sqlGetOne.Should().Be("AnotherNameContents");

            var sqlPaginated = sut.GetCommandFor(nameof(OrderQueries.GetPaginated));
            sqlPaginated.Should().Be("GetPaginatedContents");
        }


        [Test]
        public void ConventionsCachedLookup_Ctor_Successfully2Maps()
        {
            // arrage & act
            var directory = "Scripts";
            var extensions = "sql";
            var sut = new ConventionsCachedLookup<OrderQueriesSuccessButWithoutNaming>(directory, extensions);


            // assert - check all methods
            var sqlGetJustOne = sut.GetCommandFor(nameof(OrderQueriesSuccessButWithoutNaming.GetJustOne));
            sqlGetJustOne.Should().Be("GetJustOneContents");

            var sqlGetOne = sut.GetCommandFor(nameof(OrderQueriesSuccessButWithoutNaming.GetOne));
            sqlGetOne.Should().Be("AnotherName2Contents");

            var sqlTemp = sut.GetCommandFor(nameof(OrderQueriesSuccessButWithoutNaming.Temp));
            sqlTemp.Should().Be("TempContents");
        }


        [Test]
        public void ConventionsCachedLookup_GetQueryWithCompilerServices()
        {
            // arrage & act
            var directory = "Scripts";
            var extensions = "sql";
            var sut = new ConventionsCachedLookup<OrderQueriesSuccessButWithoutNaming>(directory, extensions);
            var orders = new OrderQueriesSuccessButWithoutNaming(sut);


            // assert - check all methods
            var sqlGetJustOne = orders.GetJustOne();
            sqlGetJustOne.Should().Be("GetJustOneContents");

            var sqlGetOne = orders.GetOne();
            sqlGetOne.Should().Be("AnotherName2Contents");

            var sqlTemp = orders.Temp();
            sqlTemp.Should().Be("TempContents");
        }
    }


    public class OrderQueriesWithoutAnnotation
    {
        private IConventionsLookup<OrderQueriesWithoutAnnotation> convetions;

        public OrderQueriesWithoutAnnotation(IConventionsLookup<OrderQueriesWithoutAnnotation> conventionsQuery)
        {
            this.convetions = conventionsQuery;
        }


        public object GetAll() => throw new NotImplementedException();
    }


    [UseConventions("SuccessFolder/Orders")]
    public class OrderQueries
    {
        private IConventionsLookup<OrderQueries> convetions;

        public OrderQueries(IConventionsLookup<OrderQueries> conventionsQuery)
        {
            this.convetions = conventionsQuery;
        }


        public string GetAll() => convetions.GetCommandFor();

        [OverrideConventions("AnotherName")]
        public string GetOne() => convetions.GetCommandFor();

        public string GetPaginated() => convetions.GetCommandFor();
    }





    [UseConventions("NotSuccessFolder1/Orders")]
    public class OrderQueriesNotSuccess
    {
        private IConventionsLookup<OrderQueriesNotSuccess> convetions;

        public OrderQueriesNotSuccess(IConventionsLookup<OrderQueriesNotSuccess> conventionsQuery)
        {
            this.convetions = conventionsQuery;
        }


        public string GetAllDifferent() => convetions.GetCommandFor();

        [OverrideConventions("AnotherName")]
        public string GetOne() => convetions.GetCommandFor();

        public string GetPaginated() => convetions.GetCommandFor();
    }





    [UseConventions]
    public class OrderQueriesSuccessButWithoutNaming
    {
        private IConventionsLookup<OrderQueriesSuccessButWithoutNaming> convetions;

        public OrderQueriesSuccessButWithoutNaming(IConventionsLookup<OrderQueriesSuccessButWithoutNaming> conventionsQuery)
        {
            this.convetions = conventionsQuery;
        }


        public string GetJustOne() => convetions.GetCommandFor();

        [OverrideConventions("AnotherName2")]
        public string GetOne() => convetions.GetCommandFor();

        public string Temp() => convetions.GetCommandFor();
    }

    [UseConventions]
    public class OrderQuerieUnsucessfullyWithSameMethodNames
    {
        private IConventionsLookup<OrderQuerieUnsucessfullyWithSameMethodNames> convetions;

        public OrderQuerieUnsucessfullyWithSameMethodNames(IConventionsLookup<OrderQuerieUnsucessfullyWithSameMethodNames> conventionsQuery)
        {
            this.convetions = conventionsQuery;
        }

        // instead call GetOneById
        public string GetBy(int id) => convetions.GetCommandFor(); 

        public string GetBy(string name) => convetions.GetCommandFor();

        [OverrideConventions("AnotherName2")]
        public string GetOne() => convetions.GetCommandFor();
    }


}