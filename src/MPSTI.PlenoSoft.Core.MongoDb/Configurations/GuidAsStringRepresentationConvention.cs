using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using System;
using MongoDB.Bson.Serialization.Conventions;

namespace MPSTI.PlenoSoft.Core.MongoDb.Configurations
{
    public class GuidAsStringRepresentationConvention : ConventionBase, IMemberMapConvention
    {
        public void Apply(BsonMemberMap memberMap)
        {
            if (memberMap.MemberType == typeof(Guid))
                memberMap.SetSerializer(new GuidSerializer(BsonType.String));

            else if (memberMap.MemberType == typeof(Guid?))
                memberMap.SetSerializer(new NullableSerializer<Guid>(new GuidSerializer(BsonType.String)));
        }

        public static void Register()
        {
            var conventionPack = new ConventionPack { new GuidAsStringRepresentationConvention() };
            ConventionRegistry.Register("GUIDs as strings Conventions", conventionPack, type => true);
        }
    }
}