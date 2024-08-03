namespace Pilot.Contracts.Services;

public static class ReflectionService
{
    public static bool IsSubclassOfRawGeneric(this Type generic, Type? toCheck) {
        if (toCheck == null) throw new NullReferenceException("Не нашлось(");
        
        while (toCheck != null && toCheck != typeof(object)) {
           var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
           if (generic == cur) {
               return true;
           }
           
           toCheck = toCheck.BaseType;
        }
        
        return false;
   }
}