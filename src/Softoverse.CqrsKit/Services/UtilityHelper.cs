namespace Softoverse.CqrsKit.Services;

public static class UtilityHelper
{
    public static string GetFormattedTypeName(Type type)
    {
        // Handle generic types
        if (type.IsGenericType)
        {
            // Get the base type name without the backtick (e.g., List`1 -> List)
            string typeName = type.Name.Split('`')[0];

            // Recursively process all generic arguments
            Type[] genericArguments = type.GetGenericArguments();
            string formattedGenericArguments = string.Join(",", Array.ConvertAll(genericArguments, GetFormattedTypeName));

            return $"{typeName}<{formattedGenericArguments}>";
        }

        // Handle arrays
        if (type.IsArray)
        {
            // Get the element type and append "[]" for array notation
            return $"{GetFormattedTypeName(type.GetElementType()!)}[]";
        }

        // Handle non-generic types (e.g., int, string)
        return type.Name;
    }
}