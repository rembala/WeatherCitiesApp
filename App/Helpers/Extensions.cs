using Newtonsoft.Json;

namespace App.Helpers {
    public static class Extensions {
        public static T GetDesirializedObject<T>(this string value) where T:class {
            var desirializedObject = JsonConvert.DeserializeObject<T>(value);

            return desirializedObject ?? null;
        }
    }
}
