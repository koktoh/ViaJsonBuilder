namespace ViaJsonBuilder.Models.Builder
{
    interface IJsonBuilder
    {
        public string Build(JsonBuildingContext context);
    }
}
