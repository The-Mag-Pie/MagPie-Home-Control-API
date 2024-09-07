namespace MagPie_Home_Control_API
{
    public class EnvironmentVariableMissingException(string variableName) : Exception($"Environment variable {variableName} is missing")
    {
    }

    public class ServerErrorException(string message) : Exception(message)
    {
    }
}
