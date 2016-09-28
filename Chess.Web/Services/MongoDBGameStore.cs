using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common = Chess.Common;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;

namespace Chess.Web.Services
{
    public class MongoDBGameStore : IGameStore
    {
        private readonly MongoClient _client;

        // TODO - revisit this to get look at BsonClassMap and working with Game instead of BsonDocument. (Maybe create custom Bson serialiser for Board as that was horrific!)


        public MongoDBGameStore(MongoClient mongoClient)
        {
            _client = mongoClient;
        }


        private IMongoCollection<BsonDocument> Collection
        {
            get
            {
                // TODO check whether database/collection are thread-safe and should be cached
                var database = _client.GetDatabase("chess");
                var collection = database.GetCollection<BsonDocument>("games");
                return collection;
            }
        }

        // TODO - async :-)

        public Common.Game GetGame(string gameId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", gameId);
            var doc = Collection.Find(filter).FirstOrDefault();
            var game = doc.ToGame();
            return game;
        }


        public void Save(Common.Game game)
        {
            var doc = game.ToBsonDocument();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", game.Id);
            Collection.FindOneAndReplace(filter, doc, new FindOneAndReplaceOptions<BsonDocument, BsonDocument> { IsUpsert = true });
        }
    }

    public static class BsonDocumentGameExtensions
    {
        public static BsonDocument ToBsonDocument(this Common.Game game)
        {
            return new BsonDocument
            {
                { "_id", game.Id },
                { "currentTurn", game.CurrentTurn},
                { "board", game.Board.Dump() },
                { "moves", new BsonArray(game.Moves
                                            .Select(m=> new BsonDocument
                                            {
                                                { "moveTimeUtc", m.MoveTimeUtc },
                                                { "piece", new BsonDocument { {"color", m.Piece.Color }, { "type", m.Piece.PieceType } } },
                                                { "start", m.Start.ToString() },
                                                { "end", m.End.ToString() }
                                            })
                                        )}
            };
        }
        public static Common.Game ToGame(this BsonDocument doc)
        {
            var id = doc["_id"].AsString;
            var currentTurn = (Common.Color)doc["currentTurn"].AsInt32;
            var board = Common.Board.Parse(doc["board"].AsString);
            var moves = doc["moves"].ToMoves();

            var game = new Common.Game(id, currentTurn, board, moves);
            return game;
        }
        public static List<Common.Move> ToMoves(this BsonValue value)
        {
            return value
                    .AsBsonArray
                    .Select(item =>
                        new Common.Move(
                            item["moveTimeUtc"].ToUniversalTime(),
                            item["piece"].ToPiece(),
                            item["start"].AsString,
                            item["end"].AsString
                        )
                    )
                    .ToList();
        }
        public static Common.Piece ToPiece(this BsonValue value)
        {
            var doc = value.AsBsonDocument;
            return new Common.Piece(
                    (Common.Color)doc["color"].AsInt32,
                    (Common.PieceType)doc["type"].AsInt32
                );
        }
    }


}
