# SmartLighting.Web

Interface Web do protótipo SmartLighting. Permite consultar projetos, cadastrar
e atualizar dispositivos e visualizar zonas, últimos estados das luminárias e
telemetria recebida pela API. A previsão do modelo de ML ocorre nos ESP32;
esta interface não calcula nem exibe a probabilidade prevista.

## Executar localmente

Requer .NET SDK 10 e a SmartLighting.Api em execução. Configure `Api:BaseUrl`
com o endereço da API acessível ao processo Web. O valor de desenvolvimento em
`appsettings.Development.json` usa `https://localhost:7040/`. Se iniciar a API
com o perfil HTTP, ajuste a URL, por exemplo:

```powershell
$env:Api__BaseUrl = "http://localhost:5282/"
dotnet restore
dotnet run --launch-profile http
```

Abra `http://localhost:5034`. O banco da API deve conter um projeto e suas
zonas para que seja possível cadastrar dispositivos. A preparação desse banco
e os exemplos de requisição estão documentados no repositório
`SmartLighting.Api`.

O dashboard mostra o último percentual de brilho informado por cada placa e
as contagens de eventos dos sensores. Esses valores não são medição de
potência, consumo elétrico ou economia de energia. A tela atualiza os dados
ao abrir e ao usar o botão **Atualizar**.
