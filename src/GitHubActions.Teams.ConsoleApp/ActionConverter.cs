using System;

using MessageCardModel;
using MessageCardModel.Actions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aliencube.GitHubActions.Teams.ConsoleApp
{
    public class ActionConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override bool CanRead => true;

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => objectType == typeof(BaseAction);

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();

        /// <inheritdoc />
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
