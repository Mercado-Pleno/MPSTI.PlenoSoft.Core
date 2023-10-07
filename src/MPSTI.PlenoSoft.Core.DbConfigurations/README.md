<!--
https://devblogs.microsoft.com/nuget/add-a-readme-to-your-nuget-package/
https://devblogs.microsoft.com/nuget/write-a-high-quality-readme-for-nuget-packages/
-->


# MPSTI.PlenoSoft.Core.DbConfigurations
Este pacote permite que você use a estrutura de configuração do .net (IConfiguration), com informações providas por um banco de dados relacional (SQL), de forma que as configurações podem facilmente ser alteradas de forma dinâmica diretamente no seu banco de dados, sem a necessidadade de alterar o arquivo appsettings.json, evitando acesso aos servidores e ao deploy da aplicação.


## Getting started
Para usar este pacote, você precisará de 2 arquivos no seu projeto:
1. [`appsettings.json / local.settings.json`](#appsettings.json)
   - Terá as configurações das strings de conexão com o seu banco de dados;
2. [`Program.cs`](#Program.cs)
   - Onde o Configuration builder será criado e configurado;



### Prerequisites
- .net >= 6;
- VS 2023 community (ou +);
- String de Conexão com algum Banco de Dados relacional com acesso somente leitura (não há necessidade de escrita);
  - No banco de dados, é necessário uma tabela para armazenar as configurações (ex.: Config, Configuração, Configuration);
  - Na tabela, somente 2 colunas são necessárias: uma que guarda Chave de Identificação e outra que gaurda o Valor (Key, Value);
    - Mas isso não impede que a tabela tenha outros campos, como: Id, Sistema, Tipo, Chave, Valor, Descrição, etc;
  - Flexibilidade: O nome da tabela de configuração, e os nomes das colunas podem ser configurados na dinamicamente. 

## Usage

Siga os passoa abaixo apara validar a solução:
1. Instale a última versão do pacote Nuget [`MPSTI.PlenoSoft.Core.DbConfigurations`](https://www.nuget.org/packages/MPSTI.PlenoSoft.Core.DbConfigurations/):
   - `Install-Package MPSTI.PlenoSoft.Core.DbConfigurations`
   - `<PackageReference Include="MPSTI.PlenoSoft.Core.DbConfigurations" Version="1.0.0.9" />`
2. Crie ou edite o arquivo `appsettings.json` abaixo (ao final deste passo-à-passo):
   - 2.1 Remova as strings de conexão que você não tem ou não deseja usar:
     - Dica: se você não tiver nenhum banco de dados, recomendo o uso do Sqlite para validar a solução;
   - 2.2 Configure as a(s) string(s) de conexão, colocando as informações adequadas;
   - 2.3 Verifique se a(s) string(s) de conexão estão corretas e conectando corretamente ao banco de dados;
3. Compile o programa e execute-o;


### Show me the code!


#### `appsettings.json`
```
{
    "ConnectionStrings": {
        "Sqlite": "Data Source=ConfigDb.sqlite;Cache=Shared"
    }
}
```


#### `Program.cs`
```using System;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Extensions;

namespace SmartCase
{
    public static class Program
    {
        public static void Main()
        {
            var configuration = UseConfiguration();
            var messageHello = configuration["DbKeyHello"];
            var messageWorld = configuration["DbKeyWorld"];
            Console.WriteLine($"{messageHello} {messageWorld}!");
        }

        private static IConfiguration UseConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false) // Needs to configuration.GetConnectionString
                .AddDbConfiguration(settings => // Use Sqlite With Setup Configuration Settings... but, you can use a separated file for this
                {
                    settings.CheckChangeInterval = TimeSpan.FromMinutes(1);
                    settings.CommandSelectQuerySql = "Select * From Configuration;";
                    settings.ConfigurationKeyColumn = "Key";
                    settings.ConfigurationValueColumn = "Value";
                    settings.DbConnectionFactory = configuration => new SqliteConnection(configuration.GetConnectionString("Sqlite"));
                });

            return builder.Build();
        }
    }
}
```





## Additional documentation

[View In GitHub Repository](https://github.com/Mercado-Pleno/MPSTI.PlenoSoft.Core)
\
or
\
[Inspect Source Code Directly](https://github.com/Mercado-Pleno/MPSTI.PlenoSoft.Core/tree/master/src/MPSTI.PlenoSoft.Core.DbConfigurations/Sql)


### Other implementation

#### `appsettings.json`
```
{
    "ConnectionStrings": {
        "SqlServer": "Server=your-sqlserver.database.net;Database=Config;Trusted_Connection=True;"
    }
}
```


#### `Program.cs`
```using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Extensions;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using SmartCase.DbConfigurations;

namespace SmartCase
{
    public static class Program
    {
        public static void Main()
        {
            var configuration = UseConfiguration();
            var messageHello = configuration["DbKeyHello"];
            var messageWorld = configuration["DbKeyWorld"];
            Console.WriteLine($"{messageHello} {messageWorld}!");
        }

        private static IConfiguration UseConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false) // Needs to configuration.GetConnectionString
                .AddDbConfiguration<SqlServerConfigurationSettings>(); // Use SqlServer With Configuration Setting Class

            return builder.Build();
        }
    }
}
```


### `DbSettings.cs`
```using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;


namespace SmartCase.DbConfigurations
{
    // SqlServer Configuration Setting Class needs only to implements the IDbConfigurationSettings
    public class SqlServerConfigurationSettings : IDbConfigurationSettings
    {
        public TimeSpan CheckChangeInterval => TimeSpan.FromMinutes(1);
        public string CommandSelectQuerySql => "Select Key, Value From Configuration Where (System Like '%SmartCase%');";
        public string ConfigurationKeyColumn => "Key";
        public string ConfigurationValueColumn => "Value";
        public IDbConnection CreateDbConnection(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServer");
            return new SqlConnection(connectionString);
        }
    }
}
```


## Feedback

[Conctact me](mailto:software@mercadopleno.com.br)