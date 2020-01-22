using MessageCardModel;
using MessageCardModel.Actions;
using MessageCardModel.Actions.OpenUri;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aliencube.GitHubActions.Teams.ConsoleApp
{
    internal class ActionConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType) => objectType == typeof(BaseAction);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var actionType = jsonObject.GetValue("@type").Value<string>();
            BaseAction target = actionType switch
            {
                "ActionCard" => new ActionCardAction(),
                "HttpPOST" => new HttpPostAction(),
                "OpenUri" => new OpenUriAction(),
                string type => throw new NotSupportedException($"Cannot deserialize action type: {type}")
            };

            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }
    }
}
