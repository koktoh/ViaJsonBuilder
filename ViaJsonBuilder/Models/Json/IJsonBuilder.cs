namespace ViaJsonBuilder.Models.Json
{
    interface IJsonBuilder
    {
        public string Build(JsonBuildingContext context);
    }
}
