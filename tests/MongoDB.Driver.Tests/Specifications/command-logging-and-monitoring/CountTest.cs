/* Copyright 2010-present MongoDB Inc.
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
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;

namespace MongoDB.Driver.Tests.Specifications.command_logging_and_monitoring
{
    public class CountTest : CrudOperationTestBase
    {
        private BsonDocument _filter;
        private CountOptions _options = new CountOptions();

        protected override void Execute(IMongoCollection<BsonDocument> collection, bool async)
        {
            if (async)
            {
#pragma warning disable 618
                collection.CountAsync(_filter, _options).GetAwaiter().GetResult();
#pragma warning restore
            }
            else
            {
#pragma warning disable 618
                collection.Count(_filter, _options);
#pragma warning restore
            }
        }

        protected override bool TrySetArgument(string name, BsonValue value)
        {
            switch (name)
            {
                case "filter":
                    _filter = (BsonDocument)value;
                    return true;
                case "skip":
                    _options.Skip = value.ToInt64();
                    return true;
                case "limit":
                    _options.Limit = value.ToInt64();
                    return true;
            }

            return false;
        }
    }
}
