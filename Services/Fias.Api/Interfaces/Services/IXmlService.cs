﻿namespace Fias.Api.Interfaces.Services
{
    public interface IXmlService
    {
        T? DeserializeFiasXml<T>(FileStream xmlFile) where T : class;
    }
}
