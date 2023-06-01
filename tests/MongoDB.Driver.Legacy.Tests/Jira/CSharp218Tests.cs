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

using MongoDB.Bson;
using Xunit;

namespace MongoDB.Driver.Tests.Jira.CSharp218
{
    public class CSharp218Tests
    {
        public class C
        {
            public ObjectId Id;
            public P P;
        }

        public struct S
        {
            public ObjectId Id;
            public P P;
        }

        public struct P
        {
            public int X;
            public int Y;
        }

        private MongoCollection<BsonDocument> _collection;

        public CSharp218Tests()
        {
            _collection = LegacyTestConfiguration.Collection;
        }

        [Fact]
        public void TestDeserializeClassWithStructPropertyWithoutConstructorSucceeds()
        {
            _collection.RemoveAll();
            var c = new C { Id = ObjectId.GenerateNewId(), P = new P { X = 1, Y = 2 } };
            _collection.Insert(c);

            var result = _collection.FindOneAs<C>();
            Assert.Equal(1, result.P.X);
            Assert.Equal(2, result.P.Y);
        }

        [Fact]
        public void TestDeserializeStructSucceeds()
        {
            _collection.RemoveAll();
            var s = new S { Id = ObjectId.GenerateNewId(), P = new P { X = 1, Y = 2 } };
            _collection.Insert(s);
            var result = _collection.FindOneAs<S>();
            Assert.Equal(1, result.P.X);
            Assert.Equal(2, result.P.Y);
        }

        [Fact]
        public void TestInsertForClassWithIdSucceeds()
        {
            _collection.RemoveAll();
            var c = new C { Id = ObjectId.GenerateNewId(), P = new P { X = 1, Y = 2 } };
            _collection.Insert(c);
            Assert.Equal(1, _collection.Count());
            var r = _collection.FindOne();
            Assert.Equal(2, r.ElementCount);
            Assert.Equal(2, r["P"].AsBsonDocument.ElementCount);
            Assert.Equal(c.Id, r["_id"].AsObjectId);
            Assert.Equal(c.P.X, r["P"]["X"].AsInt32);
            Assert.Equal(c.P.Y, r["P"]["Y"].AsInt32);
        }

        [Fact]
        public void TestInsertForClassWithoutIdSucceeds()
        {
            _collection.RemoveAll();
            var c = new C { P = new P { X = 1, Y = 2 } };
            _collection.Insert(c);
            Assert.Equal(1, _collection.Count());
            var r = _collection.FindOne();
            Assert.Equal(2, r.ElementCount);
            Assert.Equal(2, r["P"].AsBsonDocument.ElementCount);
            Assert.Equal(c.Id, r["_id"].AsObjectId);
            Assert.Equal(c.P.X, r["P"]["X"].AsInt32);
            Assert.Equal(c.P.Y, r["P"]["Y"].AsInt32);
        }

        [Fact]
        public void TestInsertForStructWithIdSucceeds()
        {
            _collection.RemoveAll();
            var s = new S { Id = ObjectId.GenerateNewId(), P = new P { X = 1, Y = 2 } };
            _collection.Insert(s);
            Assert.Equal(1, _collection.Count());
            var r = _collection.FindOne();
            Assert.Equal(2, r.ElementCount);
            Assert.Equal(2, r["P"].AsBsonDocument.ElementCount);
            Assert.Equal(s.Id, r["_id"].AsObjectId);
            Assert.Equal(s.P.X, r["P"]["X"].AsInt32);
            Assert.Equal(s.P.Y, r["P"]["Y"].AsInt32);
        }

        [Fact]
        public void TestInsertForStructWithoutIdFails()
        {
            _collection.RemoveAll();
            var s = new S { P = new P { X = 1, Y = 2 } };
            Assert.Throws<BsonSerializationException>(() => _collection.Insert(s));
        }

        [Fact]
        public void TestSaveForClassWithIdSucceeds()
        {
            _collection.RemoveAll();
            var c = new C { Id = ObjectId.GenerateNewId(), P = new P { X = 1, Y = 2 } };
            _collection.Save(c);
            Assert.Equal(1, _collection.Count());
            var r = _collection.FindOne();
            Assert.Equal(2, r.ElementCount);
            Assert.Equal(2, r["P"].AsBsonDocument.ElementCount);
            Assert.Equal(c.Id, r["_id"].AsObjectId);
            Assert.Equal(c.P.X, r["P"]["X"].AsInt32);
            Assert.Equal(c.P.Y, r["P"]["Y"].AsInt32);
        }

        [Fact]
        public void TestSaveForClassWithoutIdSucceeds()
        {
            _collection.RemoveAll();
            var c = new C { P = new P { X = 1, Y = 2 } };
            _collection.Save(c);
            Assert.Equal(1, _collection.Count());
            var r = _collection.FindOne();
            Assert.Equal(2, r.ElementCount);
            Assert.Equal(2, r["P"].AsBsonDocument.ElementCount);
            Assert.Equal(c.Id, r["_id"].AsObjectId);
            Assert.Equal(c.P.X, r["P"]["X"].AsInt32);
            Assert.Equal(c.P.Y, r["P"]["Y"].AsInt32);
        }

        [Fact]
        public void TestSaveForStructWithIdSucceeds()
        {
            _collection.RemoveAll();
            var s = new S { Id = ObjectId.GenerateNewId(), P = new P { X = 1, Y = 2 } };
            _collection.Save(s);
            Assert.Equal(1, _collection.Count());
            var r = _collection.FindOne();
            Assert.Equal(2, r.ElementCount);
            Assert.Equal(2, r["P"].AsBsonDocument.ElementCount);
            Assert.Equal(s.Id, r["_id"].AsObjectId);
            Assert.Equal(s.P.X, r["P"]["X"].AsInt32);
            Assert.Equal(s.P.Y, r["P"]["Y"].AsInt32);
        }

        [Fact]
        public void TestSaveForStructWithoutIdFails()
        {
            _collection.RemoveAll();
            var s = new S { P = new P { X = 1, Y = 2 } };
            Assert.Throws<BsonSerializationException>(() => _collection.Save(s));
        }
    }
}
