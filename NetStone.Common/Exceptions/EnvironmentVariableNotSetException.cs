namespace NetStone.Common.Exceptions;

public class EnvironmentVariableNotSetException(string environmentVariable) : Exception(environmentVariable);