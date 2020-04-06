using AutoFixture;
using Dapper.Conventions.Interfaces;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Dapper.Conventions.Tests
{
    public class QueryExecutorTests
    {
        private Fixture fixture = new Fixture();

        [Test]
        public void QueryExecutor_Ctor_Should_ThrowArgumentNullException_When_ConventionsLookupIsNull()
        {
            // arrange
            Action act = () => new QueryExecutor<object>(null);

            // act & assert
            act.Should().Throw<ArgumentNullException>().WithMessage("**conventionsLookup**");
        }

        [Test]
        public void QueryExecutor_Ctor_Should_ThrowArgumentNullException_When_DbConnectionFactoryIsNull()
        {
            // arrange
            Action act = () => new QueryExecutor<object>(A.Fake<IConventionsLookup<object>>(), null);

            // act & assert
            act.Should().Throw<ArgumentNullException>().WithMessage("**dbConnectionFactory**");
        }


        [Test]
        public void QueryExecutor_Run_Callback2Parameters_Should_CallCallback()
        {
            // arrange
            var query = fixture.Create<string>();
            var fakeConventionsLookup = A.Fake<IConventionsLookup<object>>();
            A
                .CallTo(() => fakeConventionsLookup.GetQuery(A<string>._))
                .Returns(query);
                

            var sut = new QueryExecutor<object>(fakeConventionsLookup);

            // act
            var result = sut.Run(sql => sql);

            // act & assert
            A.CallTo(() => fakeConventionsLookup.GetQuery(A<string>._))
                .MustHaveHappenedOnceExactly();
            
            result.Should().Be(query);
        }

        [Test]
        public void QueryExecutor_Callback3Parameters_Should_Throw_InvalidOperationException_When_DbFactoryNotSet()
        {
            // arrange
            var fakeConventionsLookup = A.Fake<IConventionsLookup<object>>();          
            var sut = new QueryExecutor<object>(fakeConventionsLookup);

            // act
            Action act = () => sut.Run((sql, connection) => sql);

            // act & assert
            act.Should().Throw<InvalidOperationException>().WithMessage("**please call the constructor passing a connection factory**");
        }

        [Test]
        public void QueryExecutor_Callback3Parameters_Should_Run_Successfully()
        {
            // arrange
            var fakeConventionsLookup = A.Fake<IConventionsLookup<object>>();
            IDbConnection fakeConnection = A.Fake<IDbConnection>();

            var query = fixture.Create<string>();
            
            A
                .CallTo(() => fakeConventionsLookup.GetQuery(A<string>._))
                .Returns(query);

            var sut = new QueryExecutor<object>(fakeConventionsLookup, () => fakeConnection);

            // act
            var result = sut.Run((sql, connection) => new { sql, connection });

            // act & assert
            A.CallTo(() => fakeConventionsLookup.GetQuery(A<string>._))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => fakeConnection.Dispose())
                .MustHaveHappenedOnceExactly();

            result.connection.Should().Be(fakeConnection);
            result.sql.Should().Be(query);
            
        }
    }
}
