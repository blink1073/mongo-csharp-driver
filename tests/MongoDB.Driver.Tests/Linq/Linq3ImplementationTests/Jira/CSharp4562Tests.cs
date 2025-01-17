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

using System.Collections.Generic;
using MongoDB.Driver.Linq;
using MongoDB.TestHelpers.XunitExtensions;
using Xunit;

namespace MongoDB.Driver.Tests.Linq.Linq3ImplementationTests.Jira
{
    public class CSharp4562Tests : Linq3IntegrationTest
    {
        [Theory]
        [ParameterAttributeData]
        public void SortBy_using_constant_should_work(
            [Values(LinqProvider.V2, LinqProvider.V3)] LinqProvider linqProvider)
        {
            var collection = CreateCollection(linqProvider);

            var aggregate = GetTranslationsSortedByEnglish(collection);

            var stages = Translate(collection, aggregate);
            AssertStages(stages, "{ $sort : { 'Text.en' : 1 } }");
        }

        [Theory]
        [ParameterAttributeData]
        public void SortBy_using_parameter_should_work(
            [Values(LinqProvider.V2, LinqProvider.V3)] LinqProvider linqProvider)
        {
            var collection = CreateCollection(linqProvider);
            var language = "sp";

            var aggregate = GetSortedTranslations(collection, language);

            var stages = Translate(collection, aggregate);
            AssertStages(stages, "{ $sort : { 'Text.sp' : 1 } }");
        }

        private IMongoCollection<Translation> CreateCollection(LinqProvider linqProvider)
        {
            var collection = GetCollection<Translation>("test", linqProvider);
            return collection;
        }

        private class Translation
        {
            public int Id { get; set; }
            public Dictionary<string, string> Text { get; set; }
        }

        private static IAggregateFluent<Translation> GetSortedTranslations(IMongoCollection<Translation> collection, string language)
        {
            return collection.Aggregate()
                .SortBy(c => c.Text[language]);
        }

        private static IAggregateFluent<Translation> GetTranslationsSortedByEnglish(IMongoCollection<Translation> collection)
        {
            return collection.Aggregate()
                .SortBy(c => c.Text["en"]);
        }
    }
}
