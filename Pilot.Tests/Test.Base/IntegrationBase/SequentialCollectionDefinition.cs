﻿namespace Test.Base.IntegrationBase;

[CollectionDefinition("SequentialCollection", DisableParallelization = true)]
public class SequentialCollectionDefinition : ICollectionFixture<TestFixture>
{
    // Этот класс просто определяет коллекцию, он остается пустым.
}

public class TestFixture
{
    // Инициализация ресурсов
}