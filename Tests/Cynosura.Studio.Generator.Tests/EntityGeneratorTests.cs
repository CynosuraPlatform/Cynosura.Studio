using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.Models;
using Xunit;

namespace Cynosura.Studio.Generator.Tests
{
    public class EntityGeneratorTests
    {
        [Fact]
        public void MergeEntity_UpdateEntity()
        {
            var fromEntity = new Entity()
            {
                Name = "Name",
                DisplayName = "Display Name 2",
                Fields = new List<Field>(),
            };
            var toEntity = new Entity()
            {
                Name = "Name",
                DisplayName = "Display Name",
                Fields = new List<Field>(),
            };
            var mergeToEntity = new Entity()
            {
                Name = "Name Not Changed",
                DisplayName = "Display Name",
                Fields = new List<Field>(),
            };

            var entityGenerator = new EntityGenerator(new CodeGenerator(null, null));
            entityGenerator.MergeEntity(fromEntity, toEntity, mergeToEntity);

            Assert.Equal("Name Not Changed", mergeToEntity.Name);
            Assert.Equal("Display Name 2", mergeToEntity.DisplayName);
        }

        [Fact]
        public void MergeEntity_UpdateFields()
        {
            var field1Id = Guid.NewGuid();
            var field2Id = Guid.NewGuid();
            var field3Id = Guid.NewGuid();
            var field4Id = Guid.NewGuid();
            var fromEntity = new Entity()
            {
                Fields = new List<Field>()
                {
                    new Field()
                    {
                        Id = field1Id,
                        Name = "Field 1",
                    },
                    new Field()
                    {
                        Id = field2Id,
                        Name = "Field 2 Changed",
                        Properties = new Infrastructure.PropertyCollection()
                        {
                            { "View", true }
                        }
                    },
                    new Field()
                    {
                        Id = field3Id,
                        Name = "Field 3 Added",
                    }
                },
            };
            var toEntity = new Entity()
            {
                Fields = new List<Field>()
                {
                    new Field()
                    {
                        Id = field1Id,
                        Name = "Field 1",
                    },
                    new Field()
                    {
                        Id = field2Id,
                        Name = "Field 2",
                    }
                },
            };
            var mergeToEntity = new Entity()
            {
                Fields = new List<Field>()
                {
                    new Field()
                    {
                        Id = field1Id,
                        Name = "Field 1",
                    },
                    new Field()
                    {
                        Id = field2Id,
                        Name = "Field 2",
                    },
                    new Field()
                    {
                        Id = field4Id,
                        Name = "Field 4 Not Deleted",
                    }
                },
            };

            var entityGenerator = new EntityGenerator(new CodeGenerator(null, null));
            entityGenerator.MergeEntity(fromEntity, toEntity, mergeToEntity);

            Assert.Equal(4, mergeToEntity.Fields.Count);
            Assert.Equal("Field 1", mergeToEntity.Fields[0].Name);
            Assert.Equal("Field 2 Changed", mergeToEntity.Fields[1].Name);
            Assert.Equal(true, mergeToEntity.Fields[1].Properties["View"]);
            Assert.Equal("Field 4 Not Deleted", mergeToEntity.Fields[2].Name);
            Assert.Equal("Field 3 Added", mergeToEntity.Fields[3].Name);
        }

        [Fact]
        public void MergeEntity_DeleteField()
        {
            var field1Id = Guid.NewGuid();
            var field2Id = Guid.NewGuid();
            var fromEntity = new Entity()
            {
                Fields = new List<Field>()
                {
                    new Field()
                    {
                        Id = field1Id,
                        Name = "Field 1",
                    },
                },
            };
            var toEntity = new Entity()
            {
                Fields = new List<Field>()
                {
                    new Field()
                    {
                        Id = field1Id,
                        Name = "Field 1",
                    },
                    new Field()
                    {
                        Id = field2Id,
                        Name = "Field 2",
                    }
                },
            };
            var mergeToEntity = new Entity()
            {
                Fields = new List<Field>()
                {
                    new Field()
                    {
                        Id = field1Id,
                        Name = "Field 1",
                    },
                    new Field()
                    {
                        Id = field2Id,
                        Name = "Field 2",
                    },
                },
            };

            var entityGenerator = new EntityGenerator(new CodeGenerator(null, null));
            entityGenerator.MergeEntity(fromEntity, toEntity, mergeToEntity);

            Assert.Equal(1, mergeToEntity.Fields.Count);
            Assert.Equal("Field 1", mergeToEntity.Fields[0].Name);
        }

        [Fact]
        public void MergeEntity_UpdateProperties()
        {
            var fromEntity = new Entity()
            {
                Properties = new Infrastructure.PropertyCollection()
                {
                    { "View", true }
                },
                Fields = new List<Field>(),
            };
            var toEntity = new Entity()
            {
                Properties = new Infrastructure.PropertyCollection()
                {
                    { "View", false }
                },
                Fields = new List<Field>(),
            };
            var mergeToEntity = new Entity()
            {
                Properties = new Infrastructure.PropertyCollection()
                {
                    { "View", false }
                },
                Fields = new List<Field>(),
            };

            var entityGenerator = new EntityGenerator(new CodeGenerator(null, null));
            entityGenerator.MergeEntity(fromEntity, toEntity, mergeToEntity);

            Assert.Equal(true, mergeToEntity.Properties["View"]);
        }
    }
}
