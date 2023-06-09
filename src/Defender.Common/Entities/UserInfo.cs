﻿using MongoDB.Bson.Serialization.Attributes;

namespace Defender.Common.Entities;

public class UserInfo : IBaseModel
{
    [BsonId]
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Nickname { get; set; }

    public DateTime? CreatedDate { get; set; }
}
