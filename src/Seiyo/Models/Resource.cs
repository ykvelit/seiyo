using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Seiyo.Models;

public class Resource
{
    [BsonId] [BsonElement("_id")] public ObjectId Id { get; set; }

    [BsonElement("type")] public string Type { get; set; } = null!;

    [BsonElement("displayName")] public string DisplayName { get; set; } = null!;

    [BsonElement("description")] public string Description { get; set; } = string.Empty;

    [BsonElement("layout")] public byte[]? Layout { get; set; } = null;

    [BsonElement("businessObject")] public BusinessObject BusinessObject { get; set; } = null!;

    [BsonElement("embedding")] public double[] Embedding { get; set; } = [];
}