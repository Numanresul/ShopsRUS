# ShopsRus
####   Bu proje .Net 6 Framework ile oluşturulmuş bir Rest Web Api Projesidir.
####   Atlas Mongo üzerinde oluşturulmuş erişime açık bir MongoDB ile entegre çalışır.
####   Bağlandığı MongoDb de kullanacağı collectionları proje otomatik oluşturur ve içlerine gerekli seed dataları tek seferlik oluşturur ve yazar.
####   Projeyi localinize çektikten sonra `InvoiceServiceTest.cs` ve `Program.cs` içerisindeki **connectionString** tanımlamalarını değiştererek kendi mongo veritabanınızda işlem yapabilirsiniz.
####   Projede NUnit ile test caseleri oluşturulmuştur bu testleri çalıştırarak sonuçları inceleyebilirsiniz.
####   Proje reposu bir Sonar Cloud hesabına bağlanarak master branchında yapılan her push sonrası tetiklenen bir git workflowu ile SonarQloud da bir kalite raporu oluşturmaktadır.
####   **Son oluşmuş rapor linki** :
```http 
https://sonarcloud.io/summary/new_code?id=Numanresul_ShopsRUS 
```
####   Projeyi çektikten sonra ayağa kaldırmak için .net 6 faramework yüklü olan bir makinede "**/ShopsRus**" dizininde `dotnet run` komutunu çalıştırabilirsiniz
####   NUnit testlerini çalıştırmak için projenin "**/ShopsRusNUnitTest**" dizininde `dotnet test` komutunu kullanabilirsiniz.
 Projeyi ayağa kaldırdıktan sonra açılan Swagger ekranında 
```http
  api/GetAllGeneratedCustomerList
```
 endpointine istek attıktan sonra dönen cevaptan müşterilerden birinin id sini alabilirsiniz ve bu id yi kullanarak 
 ```http
  api/CalculateInvoice
```
  
 isimli endpoitte **CustomerId** değişkenine setleyerek sonrasında isteğinize göre **TotalAmount** ve **IsGrocery** değişkenlerini değiştirerek test yapabilirsiniz
####   Tüm Customerları aldığınızda gördüğünüz Type değişkenindeki sayıların aşağıdaki gibi bir anlamı vardır;

0 = Employee,
1 = Affiliate,
2 = Regular

| Employee | Affiliate | Regular |
| :-------- | :------- | :------ |
| `0`      | `1` | `2` |

####   Son olarak projeye .net in kendi extensionlarından olan `Microsoft.CodeAnalysis.NetAnalyzers` entegre edilmiştir böylelikle   "**/ShopsRus**" dizininde `dotnet build` komutunu çalıştırdığınızda static linting ve analiz işlemleri yapılarak sonuçlar console ekranında listelenecektir.
