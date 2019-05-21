using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.Generator.Infrastructure
{
    public class EntityTypeHandler: CustomCreationConverter<Entity>
    {
        private readonly SolutionMetadata _solution;

        public EntityTypeHandler(SolutionMetadata solution)
        {
            _solution = solution;
        }

        public Entity Create(Type objectType, JObject jObject)
        {
            return new Entity();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);
            if (target.Properties == null)
            {
                target.Properties = new PropertyCollection();
            }
            foreach (var field in target.Fields.Where(w => w.Properties == null))
            {
                field.Properties = new PropertyCollection();
            }
            return target;
        }

        public override Entity Create(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
