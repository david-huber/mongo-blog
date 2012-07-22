using System.Web.Mvc;

namespace Mongo.Tests
{
    public static class ActionResultExtensions
    {
        public static TModelType GetModelFromViewResult<TModelType>(this ActionResult result)
        {
            return (TModelType)((ViewResult)result).ViewData.Model;
        }
    }
}
