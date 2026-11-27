using System;
using System.Collections.Generic;
using Cynosura.Studio.Generator.Models;
using Xunit;

namespace Cynosura.Studio.Generator.Tests
{
    public class EntityTests
    {
        private static Entity CreateBaseEntity()
        {
            return new Entity
            {
                Id = Guid.NewGuid(),
                Name = "BaseEntity",
                IsAbstract = true,
                Fields = new List<Field>
                {
                    new Field { Name = "Id", Type = FieldType.Int32, IsRequired = true, IsSystem = true },
                    new Field { Name = "CreationDate", Type = FieldType.DateTime, IsRequired = true, IsSystem = true },
                    new Field { Name = "ModificationDate", Type = FieldType.DateTime, IsRequired = true, IsSystem = true },
                },
            };
        }

        [Fact]
        public void AssignFieldNumbers_WithBaseEntity_StartsAfterBaseFields()
        {
            var baseEntity = CreateBaseEntity();
            baseEntity.AssignFieldNumbers();

            var entity = new Entity
            {
                Name = "Country",
                BaseEntity = baseEntity,
                Fields = new List<Field>
                {
                    new Field { Name = "CountryName" },
                    new Field { Name = "Currency" },
                },
            };
            entity.AssignFieldNumbers();

            Assert.Equal(1, baseEntity.Fields[0].Number);
            Assert.Equal(2, baseEntity.Fields[1].Number);
            Assert.Equal(3, baseEntity.Fields[2].Number);
            Assert.Equal(4, entity.Fields[0].Number);
            Assert.Equal(5, entity.Fields[1].Number);
        }

        [Fact]
        public void ValidateProtoFieldNumbers_WithoutBaseEntity_Throws()
        {
            var entity = new Entity
            {
                Name = "Country",
                Fields = new List<Field>
                {
                    new Field { Name = "CountryName" },
                    new Field { Name = "Currency" },
                },
            };
            entity.AssignFieldNumbers();

            var exception = Assert.Throws<Exception>(() => entity.ValidateProtoFieldNumbers());
            Assert.Contains("proto field number", exception.Message);
        }

        [Fact]
        public void ValidateProtoFieldNumbers_WithBaseEntity_DoesNotThrow()
        {
            var baseEntity = CreateBaseEntity();
            baseEntity.AssignFieldNumbers();

            var entity = new Entity
            {
                Name = "Country",
                BaseEntity = baseEntity,
                Fields = new List<Field>
                {
                    new Field { Name = "CountryName" },
                    new Field { Name = "Currency" },
                },
            };
            entity.AssignFieldNumbers();

            entity.ValidateProtoFieldNumbers();
        }
    }
}
