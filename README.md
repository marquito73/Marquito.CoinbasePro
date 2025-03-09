# Marquito.CoinbasePro

It's a custom C# library that use the v3 version of Coinbase Pro API / Advanced Trade API (api.coinbase.com/api/v3/brokerage).

## Endpoints

Currently supported Endpoints with this library are : 
1. Account
2. Convert
3. Order
4. Product
5. API key permission (key_permissions)

## Init the coinbase client to use Endpoints

For use this you need to create a [TradingConfiguration](Marquito.CoinbasePro/Class/Entities/File/TradingConfiguration.cs) class which these properties : 
1. SecretKey : Your secret key
2. ApiKey : Your public key
3. OrganizationName : Your organization name
4. DefaultProduct : The default product to use
5. DefaultPeriod : The default period to use (see (Marquito.CoinbasePro/Class/Enums/TradingPeriod.cs)

You have two ways to initialize the [CoinbaseProClient](Marquito.CoinbasePro/Class/Client/Coinbase/CoinbaseProClient.cs) constructor : 
1. ``` public CoinbaseProClient(string secretKey, string organizationName, string apiKey) ```
2. ``` public CoinbaseProClient(TradingConfiguration tradingConfiguration) ```

And next, you can just call Coinbase API through Endpoint client methods.
