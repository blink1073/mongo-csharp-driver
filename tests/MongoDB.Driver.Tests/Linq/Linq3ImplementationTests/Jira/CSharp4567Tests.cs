﻿/* Copyright 2010-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Linq.Expressions;
using FluentAssertions;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Core.TestHelpers.XunitExtensions;
using MongoDB.Driver.Linq;
using MongoDB.TestHelpers.XunitExtensions;
using Xunit;

namespace MongoDB.Driver.Tests.Linq.Linq3ImplementationTests.Jira
{
    public class CSharp4567Tests : Linq3IntegrationTest
    {
        [Theory]
        [ParameterAttributeData]
        public void Projection_to_derived_type_should_work(
            [Values(LinqProvider.V2, LinqProvider.V3)] LinqProvider linqProvider)
        {
            RequireServer.Check().Supports(Feature.FindProjectionExpressions);
            var collection = GetCollection(linqProvider);
            Expression<Func<C, object>> projection = x => new R { X = x.Id };

            var find = collection.Find("{}").Project(projection);

            var translatedProjection = TranslateFindProjection(collection, find);
            if (linqProvider == LinqProvider.V2)
            {
                translatedProjection.Should().Be("{ _id : 1 }");
            }
            else
            {
                translatedProjection.Should().Be("{ X : '$_id', _id : 0 }");
            }

            var result = (R)find.Single();
            result.X.Should().Be(1);
        }

        private IMongoCollection<C> GetCollection(LinqProvider linqProvider)
        {
            var collection = GetCollection<C>("test", linqProvider);
            CreateCollection(
                collection,
                new C { Id = 1 });
            return collection;
        }

        private class C
        {
            public int Id { get; set; }
        }

        private class R
        {
            public int X { get; set; }
        }
    }
}
